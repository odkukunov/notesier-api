using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.ViewModels
{
    public class LoginViewModel
    {
        public string Name { get; set; }

        [Column(TypeName = "varchar(200)"), Required(ErrorMessage = "Пароль - это обязательное поле!")]
        public string Password { get; set; }
    }
}
