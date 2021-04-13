using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.ViewModels.Users
{
    public class UserBaseViewModel
    {
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Длина имени должна составлять от 3 до 16 символов!")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Password { get; set; }
    }
}
