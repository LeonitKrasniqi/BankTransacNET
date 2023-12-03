﻿// <auto-generated />
using System;
using AccountManagmentSystemAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountManagmentSystemAPI.Migrations
{
    [DbContext(typeof(FinancialDbContext))]
    partial class FinancialDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountManagmentSystemAPI.Model.Domain.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CardNumber")
                        .HasColumnType("int");

                    b.Property<bool>("IsDebit")
                        .HasColumnType("bit");

                    b.HasKey("AccountId");

                    b.HasIndex("CardNumber")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("AccountManagmentSystemAPI.Model.Domain.Transaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDebit")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TransactionId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AccountManagmentSystemAPI.Model.Domain.Transaction", b =>
                {
                    b.HasOne("AccountManagmentSystemAPI.Model.Domain.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("AccountManagmentSystemAPI.Model.Domain.Account", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}