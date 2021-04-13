using Notesier_API.Models;
using Notesier_API.ViewModels;
using Notesier_API.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        public UserModel FindAndUpdateUser(int id, UpdateMeViewModel updateMeViewModel)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);


            if (user != null)
            {
                if (updateMeViewModel.Name != null)
                {
                    user.Name = updateMeViewModel.Name;
                }

                if(updateMeViewModel.Password != null)
                {
                    user.Password = Crypto.HashPassword(updateMeViewModel.Password);
                }

                db.SaveChanges();

                return user;
            }

            return null;
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
