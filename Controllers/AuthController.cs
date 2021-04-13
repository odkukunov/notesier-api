using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notesier_API.Models;
using Notesier_API.Utils;
using Notesier_API.Utils.Responses;
using Notesier_API.Utils.Services;
using Notesier_API.Utils.Services.ModelServices;
using Notesier_API.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Notesier_API
{
    [ApiController]
    public class AuthController : Controller
    {

        private JWTHandler JWTHandler;
        private ModelStateSerializer modelStateSerializer;
        private UserModelService userModelService;

        public AuthController(UserModelService _userModelService, JWTHandler _JWTCreator, ModelStateSerializer _modelStateSerializer)
        {
            JWTHandler = _JWTCreator;
            modelStateSerializer = _modelStateSerializer;
            userModelService = _userModelService;
        }

        [HttpPost, Route("/api/auth")]
        public IActionResult Auth()
        {
            UserModel user = (UserModel)HttpContext.Items["User"];

            if(user != null)
            {
                return Json(new SuccessResponse(new { Id = user.Id, Name = user.Name}));
            }

            return Unauthorized(new ErrorResponse("Вы не авторизованы!"));
        }

        [HttpPost, Route("/api/register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (userModelService.GetUserByName(user.Name) != null)
            {
                ModelState.AddModelError("name", "Пользователь с таким именем уже существует!");
            }

            if (ModelState.IsValid)
            {
                string password = user.Password;
                await userModelService.CreateUser(user);
               
                var identity = GetIdentity(user.Name, password, out user);

                AddJWT(identity.Claims);
                return Json(new SuccessResponse(user.Only("Id", "Name")));
            }

            return BadRequest(new ErrorResponse(modelStateSerializer.Serialize(ModelState)));
        }

        [HttpPost, Route("/api/login")]
        public IActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserModel _user;
                var identity = GetIdentity(user.Name, user.Password, out _user);

                if (identity == null)
                {
                    return BadRequest(new ErrorResponse("Неверный логин / пароль!"));
                }

                AddJWT(identity.Claims);
                return Json(new SuccessResponse(user.Only("Id", "Name")));
            }

            return BadRequest(new ErrorResponse(modelStateSerializer.Serialize(ModelState)));
           
        }

        [HttpPost, Route("/api/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            return NoContent();
        }

        private ClaimsIdentity GetIdentity(string name, string password, out UserModel user)
        {
            user = userModelService.GetUserByName(name);

            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                    new Claim("Password", password)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        private void AddJWT(IEnumerable<Claim> claims)
        {
            
            DateTime expiresIn = DateTime.Now.AddMinutes(AuthOptions.LIFETIME).ToLocalTime();
            string jwt = JWTHandler.Generate(claims, expiresIn);

            Response.Cookies.Append("token", $"Bearer {jwt}", new CookieOptions()
            {
                HttpOnly = true,
                Expires = expiresIn,
                Path = "/",
                Secure = true
            });
        }
    }
}
