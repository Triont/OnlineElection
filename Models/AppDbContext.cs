using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OnlineElection.Models
{
    public class AppDbContext:DbContext
    {
       public DbSet<Person> People { get; set; } 
        public DbSet<Election> Elections { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<ConfirmToken> ConfirmTokens { get; set; }
        //     public DbSet<PersonSecureData> personSecures { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
           // Database.EnsureCreated();

        }
    }
}
