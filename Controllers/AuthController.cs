using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notesier_API.Models;
using Notesier_API.Utils;
using Notesier_API.Utils.Filters;
using Notesier_API.Utils.Responses;
using Notesier_API.Utils.Services;
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

        private NotesierContext db;
        private JWTHandler JWTHandler;
        private ModelStateSerializer modelStateSerializer;

        public AuthController(NotesierContext context, JWTHandler _JWTCreator, ModelStateSerializer _modelStateSerializer)
        {
            db = context;
            JWTHandler = _JWTCreator;
            modelStateSerializer = _modelStateSerializer;
        }

        [Auth, HttpPost, Route("/api/auth")]
        public async Task<IActionResult> Auth()
        {
            object token = HttpContext.Items["token"];
            if(token != null)
            {
                try
                {
                    ClaimsPrincipal claims = JWTHandler.Validate(token.ToString());
                    UserModel user = db.Users.FirstOrDefault(user => user.Name == claims.Claims.First().Value);

                    return Json(new SuccessResponse(new { Id = user.Id, Name = user.Name }));
                }
                catch(Exception e)
                {
                    return BadRequest(new ErrorResponse("Неккоректный токен!"));
                }
                
                
            }
            return BadRequest(new ErrorResponse("Токен не найден!"));
        }

        [HttpPost, Route("/api/register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (db.Users.FirstOrDefault(u => u.Name == user.Name) != null)
            {
                ModelState.AddModelError("name", "Пользователь с таким именем уже существует!");
            }

            if (ModelState.IsValid)
            {
                
                string password = user.Password;

                user.Password = Crypto.HashPassword(password);
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

               
                var identity = GetIdentity(user.Name, password, out user);

                AddJWT(identity.Claims);
                return Json(new SuccessResponse(new { Id = user.Id, Name = user.Name  }));
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
                return Json(new SuccessResponse(new { Id = _user.Id, Name = _user.Name }));
            }

            return BadRequest(new ErrorResponse(modelStateSerializer.Serialize(ModelState)));
           
        }

        private ClaimsIdentity GetIdentity(string name, string password, out UserModel user)
        {
            user = db.Users.FirstOrDefault(user => user.Name == name);
            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                    new Claim("Password", user.Password)
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
