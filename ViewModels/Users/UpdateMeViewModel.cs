using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notesier_API.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.ViewModels
{
    public class UpdateMeViewModel : UserBaseViewModel
    {
        [Required(ErrorMessage = "Текущий пароль - обязательное поле")]
        public string CurrentPassword { get; set; }
    }
}
