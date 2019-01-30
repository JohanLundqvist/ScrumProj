using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ScrumProj.Models
{
    public class AppDbContext : DbContext
    {
        //dbsets
        //dbsets
        //dbsets
        public DbSet<ProfileModel> Profiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          //  modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

}
