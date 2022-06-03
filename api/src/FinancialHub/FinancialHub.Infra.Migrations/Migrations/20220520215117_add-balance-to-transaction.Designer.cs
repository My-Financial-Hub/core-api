﻿// <auto-generated />
using System;
using FinancialHub.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinancialHub.Infra.Migrations.Migrations
{
    [DbContext(typeof(FinancialHubContext))]
    [Migration("20220520215117_add-balance-to-transaction")]
    partial class addbalancetotransaction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FinancialHub.Domain.Entities.AccountEntity", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("active");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("UpdateTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.BalanceEntity", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money")
                        .HasColumnName("amount");

                    b.Property<DateTimeOffset?>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<string>("Currency")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("currency");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("UpdateTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("balances");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.CategoryEntity", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("active");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("UpdateTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.TransactionEntity", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money")
                        .HasColumnName("amount");

                    b.Property<Guid>("BalanceId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("balance_id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("category_id");

                    b.Property<DateTimeOffset?>("CreationTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("creation_time");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("FinishDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("finish_date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("active");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("TargetDate")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("target_date");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset?>("UpdateTime")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("update_time");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.BalanceEntity", b =>
                {
                    b.HasOne("FinancialHub.Domain.Entities.AccountEntity", "Account")
                        .WithMany("Balances")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.TransactionEntity", b =>
                {
                    b.HasOne("FinancialHub.Domain.Entities.BalanceEntity", "Balance")
                        .WithMany("Transactions")
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinancialHub.Domain.Entities.CategoryEntity", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Balance");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.AccountEntity", b =>
                {
                    b.Navigation("Balances");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.BalanceEntity", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FinancialHub.Domain.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
