using Microsoft.EntityFrameworkCore;
using myGameScore.Database.Mapping;
using myGameScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myGameScore.Database
{
    public class ContextLibrary : DbContext
    {
        public ContextLibrary(DbContextOptions<ContextLibrary> options) : base(options)
        {

        }
        public DbSet<Score> scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ScoreMap());
        }
    }
}
