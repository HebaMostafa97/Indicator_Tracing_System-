using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace ITS.Models
{
    public class ApplicationContext : DbContext
    {
        private static ApplicationContext instance = new ApplicationContext();

        public ApplicationContext() : base("name=ConnectionString")
        {}

        public static ApplicationContext getInstance()
        {
            return instance;
        }
        public DbSet<User> Users { set; get; }
        public DbSet<Group> Groups { set; get; }
        public DbSet<GroupPermission> Groupspermission { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .Property(e => e.Email)
                .IsUnicode(false);

            //modelBuilder.Entity<Country>()
            //    .Property(e => e.IsDefault)

            // Email Unique with serverside //
              
            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.Owners)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
               .HasOptional<User>(s => s.Modifier)
               .WithMany(g => g.Modifiers)
               .HasForeignKey<int?>(s => s.ModifierID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
               .HasRequired<User>(s => s.Owner)
               .WithMany(g => g.GroupsOwner)
               .HasForeignKey<int?>(s => s.OwnerID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.GroupsModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

             //modelBuilder.Entity<Group_Permission>()
             //   .HasOptional<Group>(s => s.Group)
             //   .WithMany(g => g.Group_Permissions)
             //   .HasForeignKey<int?>(s => s.GroupID)
             //   .WillCascadeOnDelete(false);

      modelBuilder.Entity<IndicatorAttachment>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.IndicatorAttachmentsOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndicatorAttachment>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.IndicatorAttachmentsModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Indicator>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.IndicatorsOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Indicator>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.IndicatorsModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sector>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.SectorsOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sector>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.SectorsModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.IssuesOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.IssuesModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Periodicity>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.PeriodicitiesOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Periodicity>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.PeriodicitiesModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.CountriesOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.CountriesModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publisher>()
                .HasRequired<User>(s => s.Owner)
                .WithMany(g => g.PublishersOwner)
                .HasForeignKey<int?>(s => s.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publisher>()
                .HasOptional<User>(s => s.Modifier)
                .WithMany(g => g.PublishersModifier)
                .HasForeignKey<int?>(s => s.ModifierID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sector>()
                .HasOptional<Sector>(s => s._Sector)
                .WithMany(g => g.Sectors)
                .HasForeignKey<int?>(s => s.ParentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasOptional<Issue>(s => s._Issue)
                .WithMany(g => g.Issues)
                .HasForeignKey<int?>(s => s.ParentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Indicator>()
                .HasMany<Sector>(s => s.Sectors)
                .WithMany(c => c.Indicators)
                .Map(cs => {
                    cs.MapLeftKey("IndicatorID");
                    cs.MapRightKey("SectorID");
                    cs.ToTable("ITS_LNK_Indicator_Sector");
                });

            modelBuilder.Entity<Indicator>()
                .HasMany<Issue>(s => s.Issues)
                .WithMany(c => c.Indicators)
                .Map(cs => {
                    cs.MapLeftKey("IndicatorID");
                    cs.MapRightKey("IssueID");
                    cs.ToTable("ITS_LNK_Indicator_Issue");
                });

            modelBuilder.Entity<User>()
                .HasMany<Group>(s => s.Groups)
                .WithMany(c => c.Users)
                .Map(cs => {
                    cs.MapLeftKey("UserID");
                    cs.MapRightKey("GroupID");
                    cs.ToTable("ITS_LNK_User_Group");
                });
        }

        public System.Data.Entity.DbSet<ITS.Models.Publisher> Publishers { get; set; }

        public System.Data.Entity.DbSet<ITS.Models.Country> Countries { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Periodicity> Periodicities { get; set; }

        public System.Data.Entity.DbSet<ITS.Models.IndicatorAttachment> IndicatorAttachments { get; set; }
    }
}