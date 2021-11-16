using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Data {

    public class ApplicationDbContext : DbContext {

        #region Properties (tables in the database)
        public DbSet<URLs> URLs { get; set; }
        
        public DbSet<BookedEvents> BookedEvents { get; set; }

        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        #endregion Properties (tables in the database)

        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {

        }
        #endregion Constructor



        //configures the database first time when ApplicationDbContext is used
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=DESKTOP-URBEC52\\SQLEXPRESS;Database=CalendarManager;Trusted_Connection=True");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
            

        }

    }
}
