using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Models
{
    public class NotesierContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public NotesierContext(DbContextOptions<NotesierContext> options) : base(options) {
            Database.EnsureCreated();
        }
    }
}
