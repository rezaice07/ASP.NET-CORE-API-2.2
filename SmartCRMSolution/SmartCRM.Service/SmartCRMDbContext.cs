using Microsoft.EntityFrameworkCore;
using SamrtCRM.Data.Models;
using System;

namespace SmartCRM.Service
{
    public class SmartCRMDbContext : DbContext
    {
        public SmartCRMDbContext(DbContextOptions<SmartCRMDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}
