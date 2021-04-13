using Microsoft.EntityFrameworkCore;
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

        public async Task<UserModel> FindAndUpdateUser(int id, UpdateMeViewModel updateMeViewModel)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);


            if (user != null)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Entry(user).CurrentValues.SetValues(updateMeViewModel.ExceptNull());

                if(updateMeViewModel.Password != null)
                {
                    user.Password = Crypto.HashPassword(updateMeViewModel.Password);
                }

                await db.SaveChangesAsync();

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

            Console.WriteLine(228);
        }
    }
}
