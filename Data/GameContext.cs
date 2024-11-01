using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniGameV4.Model;

namespace MiniGameV4.Data
{
    internal class GameContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connection string
            options.UseSqlServer("Data Source=L-6BKGP34;Initial Catalog=MiniGameV4EF;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }

        // properties correspond to tables to create in the database
        public DbSet<User> User { get; set; }
        public DbSet<GameRecord> GameRecord { get; set; }
    }
}
