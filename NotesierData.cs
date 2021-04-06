using Notesier_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Notesier_API
{
    public static class NotesierData
    {


        public static void Init(NotesierContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                new UserModel() { Name = "Xren", Email = "xren@mail.ru", Password = Crypto.HashPassword("2286713337") }
            );
                context.SaveChanges();
            }
        }
    }
}
