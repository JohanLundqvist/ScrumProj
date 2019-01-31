using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ScrumProj.Models
{
    public class AppDbContext : DbContext
    {
        DbSet<Entry> Entries { get; set; }
        DbSet<File> Files { get; set; }
        //dbsets

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          //  modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //public System.Data.Entity.DbSet<ScrumProj.Models.File> Files { get; set; }
    }

}
