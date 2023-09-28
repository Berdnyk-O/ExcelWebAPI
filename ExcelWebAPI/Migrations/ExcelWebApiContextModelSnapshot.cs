﻿// <auto-generated />
using ExcelWebAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExcelWebAPI.Migrations
{
    [DbContext(typeof(ExcelWebApiContext))]
    partial class ExcelWebApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("ExcelWebAPI.Models.Cell", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("SheetId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SheetId");

                    b.ToTable("Cells");
                });

            modelBuilder.Entity("ExcelWebAPI.Models.Sheet", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Sheets");
                });

            modelBuilder.Entity("ExcelWebAPI.Models.Cell", b =>
                {
                    b.HasOne("ExcelWebAPI.Models.Sheet", "Sheet")
                        .WithMany("Cells")
                        .HasForeignKey("SheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sheet");
                });

            modelBuilder.Entity("ExcelWebAPI.Models.Sheet", b =>
                {
                    b.Navigation("Cells");
                });
#pragma warning restore 612, 618
        }
    }
}