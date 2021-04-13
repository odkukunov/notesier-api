using Microsoft.EntityFrameworkCore;
using Notesier_API.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Models
{
    [Table("users"), Index("Name", IsUnique = true)]
    public class UserModel : LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Почта - это обязательное поле!"), RegularExpression(@"\w+@\w+\.\w+", ErrorMessage = "Неправильный формат почты")]
        public string Email { get; set; }

    }
}
