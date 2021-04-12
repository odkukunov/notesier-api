using Notesier_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Notesier_API.Utils.Services.ModelServices
{
    public class UserModelService : ModelService
    {
        private NotesierContext db;
        public UserModelService(NotesierContext _db)
        {
            db = _db;
        }

        public UserModel GetUserByName(string name)
        {
            return db.Users.FirstOrDefault(u => u.Name == name);
        }

        public async Task CreateUser(UserModel user)
        {
            string password = user.Password;

            user.Password = Crypto.HashPassword(password);
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
