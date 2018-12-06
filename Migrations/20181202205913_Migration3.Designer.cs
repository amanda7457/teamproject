﻿// <auto-generated />
using System;
using Group14_BevoBooks.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Group14_BevoBooks.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20181202205913_Migration3")]
    partial class Migration3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Group14_BevoBooks.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("ActiveUser");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("Zipcode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<int?>("GenreID");

                    b.Property<int>("Inventory");

                    b.Property<DateTime>("PublishedDate");

                    b.Property<int>("Reorder");

                    b.Property<decimal>("SellingPrice");

                    b.Property<decimal>("SupplierPrice");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("UniqueID");

                    b.Property<int>("UnqiueID");

                    b.HasKey("BookID");

                    b.HasIndex("GenreID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.BookOrder", b =>
                {
                    b.Property<int>("BookOrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId");

                    b.Property<int?>("BookID");

                    b.Property<int?>("ReorderQuantityDefaultReorderID");

                    b.Property<int>("Status");

                    b.HasKey("BookOrderID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("BookID");

                    b.HasIndex("ReorderQuantityDefaultReorderID");

                    b.ToTable("BookOrders");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.BookOrderDetail", b =>
                {
                    b.Property<int>("BookOrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookID");

                    b.Property<int?>("OrderBookOrderID");

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("BookOrderDetailID");

                    b.HasIndex("BookID");

                    b.HasIndex("OrderBookOrderID");

                    b.ToTable("BookOrderDetails");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.CreditCard", b =>
                {
                    b.Property<int>("CreditCardID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("CreditCardID");

                    b.HasIndex("AppUserId");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.DefaultReorder", b =>
                {
                    b.Property<int>("DefaultReorderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DefaultQuantity");

                    b.Property<string>("ManagerSetId");

                    b.HasKey("DefaultReorderID");

                    b.HasIndex("ManagerSetId");

                    b.ToTable("ReorderQuantity");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Discount", b =>
                {
                    b.Property<int>("DiscountID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscountAmount");

                    b.Property<DateTime>("DiscountEndDate");

                    b.Property<DateTime>("DiscountStartDate");

                    b.Property<int>("DiscountType");

                    b.Property<string>("PromoCode")
                        .HasMaxLength(20);

                    b.HasKey("DiscountID");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Genre", b =>
                {
                    b.Property<int>("GenreID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GenreName");

                    b.HasKey("GenreID");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId");

                    b.Property<int?>("CreditCardID");

                    b.Property<int?>("DefaultReorderID");

                    b.Property<int?>("DiscountID");

                    b.Property<DateTime>("OrderDate");

                    b.Property<bool>("OrderPlaced");

                    b.Property<decimal>("OrderShipping");

                    b.Property<int?>("ShippingID");

                    b.HasKey("OrderID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CreditCardID");

                    b.HasIndex("DefaultReorderID");

                    b.HasIndex("DiscountID");

                    b.HasIndex("ShippingID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookID");

                    b.Property<int?>("OrderID");

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderDetailID");

                    b.HasIndex("BookID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApproverId");

                    b.Property<string>("AuthorId");

                    b.Property<int?>("BookID");

                    b.Property<int>("Rating");

                    b.Property<string>("ReviewText");

                    b.HasKey("ReviewID");

                    b.HasIndex("ApproverId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Shipping", b =>
                {
                    b.Property<int>("ShippingID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ManagerSetId");

                    b.Property<decimal>("ShippingAdditional");

                    b.Property<decimal>("ShippingFirst");

                    b.HasKey("ShippingID");

                    b.HasIndex("ManagerSetId");

                    b.ToTable("ShippingPrices");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Book", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.Genre", "Genre")
                        .WithMany("Books")
                        .HasForeignKey("GenreID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.BookOrder", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("Group14_BevoBooks.Models.Book")
                        .WithMany("BookOrders")
                        .HasForeignKey("BookID");

                    b.HasOne("Group14_BevoBooks.Models.DefaultReorder", "ReorderQuantity")
                        .WithMany()
                        .HasForeignKey("ReorderQuantityDefaultReorderID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.BookOrderDetail", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookID");

                    b.HasOne("Group14_BevoBooks.Models.BookOrder", "Order")
                        .WithMany("BookOrderDetails")
                        .HasForeignKey("OrderBookOrderID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.CreditCard", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "AppUser")
                        .WithMany("CreditCards")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.DefaultReorder", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "ManagerSet")
                        .WithMany()
                        .HasForeignKey("ManagerSetId");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Order", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Group14_BevoBooks.Models.CreditCard", "CreditCard")
                        .WithMany("Orders")
                        .HasForeignKey("CreditCardID");

                    b.HasOne("Group14_BevoBooks.Models.DefaultReorder")
                        .WithMany("Orders")
                        .HasForeignKey("DefaultReorderID");

                    b.HasOne("Group14_BevoBooks.Models.Discount", "Discount")
                        .WithMany("Orders")
                        .HasForeignKey("DiscountID");

                    b.HasOne("Group14_BevoBooks.Models.Shipping")
                        .WithMany("Orders")
                        .HasForeignKey("ShippingID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.OrderDetail", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.Book", "Book")
                        .WithMany("OrderDetails")
                        .HasForeignKey("BookID");

                    b.HasOne("Group14_BevoBooks.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Review", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "Approver")
                        .WithMany("ReviewsApproved")
                        .HasForeignKey("ApproverId");

                    b.HasOne("Group14_BevoBooks.Models.AppUser", "Author")
                        .WithMany("ReviewsWritten")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Group14_BevoBooks.Models.Book", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookID");
                });

            modelBuilder.Entity("Group14_BevoBooks.Models.Shipping", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser", "ManagerSet")
                        .WithMany()
                        .HasForeignKey("ManagerSetId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Group14_BevoBooks.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Group14_BevoBooks.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
