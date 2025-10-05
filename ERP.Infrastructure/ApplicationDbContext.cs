using Microsoft.EntityFrameworkCore;
using ERP.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        // DbSet Here
        public DbSet<User> User { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Branch> Branch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region [User]

            modelBuilder.Entity<User>()
                .HasOne(c => c.Company)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            #endregion [User]

            #region [Company]
            #endregion [Company]

            #region [Branch]

            modelBuilder.Entity<Branch>()
                .HasOne(c => c.Company)
                .WithMany(b => b.Branches)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion [Branch]
        }
    }
}
