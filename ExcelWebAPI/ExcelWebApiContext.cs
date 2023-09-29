using ExcelWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection.Metadata;

namespace ExcelWebAPI
{
    public class ExcelWebApiContext:DbContext
    {
        public ExcelWebApiContext(DbContextOptions<ExcelWebApiContext> options)
        : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Cell> Cells { get; set; }
        public DbSet<Sheet> Sheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sheet>()
                 .HasMany(x => x.Cells)
                 .WithOne(x => x.Sheet)
                 .HasForeignKey(x => x.SheetId)
                 .IsRequired();
        }
    }
}
