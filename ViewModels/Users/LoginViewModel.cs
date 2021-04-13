using Microsoft.EntityFrameworkCore;
using Notesier_API.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.ViewModels
{
    public class LoginViewModel : UserBaseViewModel
    {
        [Required(ErrorMessage = "Имя - обязательно поле"), StringLength(16, MinimumLength = 3, ErrorMessage = "Длина имени должна составлять от 3 до 16 символов!")]
        public override string Name { get; set; }

        [Column(TypeName = "varchar(200)"), Required(ErrorMessage = "Пароль - это обязательное поле!")]
        public override string Password { get; set; }
    }
}
