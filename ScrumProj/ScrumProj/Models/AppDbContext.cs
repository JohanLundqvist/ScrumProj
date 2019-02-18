using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ScrumProj.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CategoryInEntry> CategoryInEntrys { get; set; }
        public DbSet<DevelopmentProject> Projects { get; set; }
        public DbSet<PushNote> PushNotes { get; set; }
        public DbSet<DevFile> DevFiles { get; set; }
        public DbSet<WantMailOrNo> WantMailOrNoes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DevelopmentProject>()
           .HasMany(c => c.Participants).WithMany(i => i.Projects)
           .Map(t => t.MapLeftKey("ProjectId")
                 .MapRightKey("UserID")
                  .ToTable("ProjectParticipants"));

            modelBuilder.Entity<DevFile>()
            .HasRequired<DevelopmentProject>(f => f.ThisProject)
            .WithMany(p => p.Files)
            .HasForeignKey<int>(s => s.ProjectId);
        }

    }

}
