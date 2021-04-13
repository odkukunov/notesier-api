using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notesier_API.Models;
using Notesier_API.Utils.Responses;
using Notesier_API.Utils.Services;
using Notesier_API.Utils.Services.ModelServices;
using Notesier_API.ViewModels;
using Notesier_API.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Notesier_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : Controller
    {
        private ModelStateSerializer modelStateSerializer;
        private UserModelService userModelService;

        public MeController(ModelStateSerializer _modelStateSerializer, UserModelService _userModelService)
        {
            modelStateSerializer = _modelStateSerializer;
            userModelService = _userModelService;
        }

        [HttpPatch()]
        public async Task<IActionResult> Update(UpdateMeViewModel updateMeViewModel)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"];

            if (currentUser != null)
            {
                if(!Crypto.VerifyHashedPassword(currentUser.Password, updateMeViewModel.CurrentPassword))
                {
                    ModelState.AddModelError("currentPassword", "Вы должны ввести ваш текущий пароль!");
                }

                if (ModelState.IsValid)
                {
                    UserModel user = await userModelService.FindAndUpdateUser(currentUser.Id, updateMeViewModel);

                    if (user == null)
                    {
                        return NotFound(new ErrorResponse("Пользователь не найден!"));
                    }

                    return Json(new SuccessResponse(user.Only("Id", "Name")));
                }

                return BadRequest(new ErrorResponse(modelStateSerializer.Serialize(ModelState)));
            }


            return Unauthorized(new ErrorResponse("Вы должны быть авторизованы для выполнения этого дествия"));
        }
    }
}
