﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("Complement")
                        .HasColumnType("varchar(40)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Street")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("ZipCode")
                        .HasColumnType("varchar(8)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Domain.Models.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("text");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Observations")
                        .HasColumnType("text");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("UpdatedByUserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Domain.Models.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("UpdatedByUserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Domain.Models.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("Category")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("UpdatedByUserName")
                        .HasColumnType("text");

                    b.Property<string>("UrlImage")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("text");

                    b.Property<string>("ExternalReference")
                        .HasColumnType("text");

                    b.Property<string>("FailureReason")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Observations")
                        .HasColumnType("text");

                    b.Property<string>("PaymentId")
                        .HasColumnType("text");

                    b.Property<int>("RequestNumber")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal?>("TotalValue")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("UpdatedByUserName")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Models.OrderItems", b =>
                {
                    b.Property<Guid>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("Discount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("FoodId", "OrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Domain.Models.Pay", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<long>("CollectorId")
                        .HasColumnType("bigint");

                    b.Property<string>("CurrencyId")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateLastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpirationDateTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ExternalReference")
                        .HasColumnType("text");

                    b.Property<int?>("Installments")
                        .HasColumnType("integer");

                    b.Property<string>("IssuerId")
                        .HasColumnType("text");

                    b.Property<string>("NotificationUrl")
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("PaymentMethodId")
                        .HasColumnType("text");

                    b.Property<string>("PaymentTypeId")
                        .HasColumnType("text");

                    b.Property<string>("QrCode")
                        .HasColumnType("text");

                    b.Property<string>("QrCodeBase64")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<decimal?>("TransactionAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Pays");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(120)");

                    b.Property<string>("FirebaseUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(11)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedByUserId")
                        .HasColumnType("varchar(35)");

                    b.Property<string>("UpdatedByUserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Models.Address", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("Domain.Models.Address", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Cart", b =>
                {
                    b.HasOne("Domain.Models.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.OrderItems", b =>
                {
                    b.HasOne("Domain.Models.Food", "Food")
                        .WithMany("Items")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Food");

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Pay", b =>
                {
                    b.HasOne("Domain.Models.Order", "Order")
                        .WithOne("Pay")
                        .HasForeignKey("Domain.Models.Pay", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Domain.Models.Food", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Models.Order", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Pay")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
