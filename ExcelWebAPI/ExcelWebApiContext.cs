using ExcelWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExcelWebAPI
{
    public class ExcelWebApiContext:DbContext
    {
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public ExcelWebApiContext(DbContextOptions<ExcelWebApiContext> options)
        : base(options) { }
    }
}
