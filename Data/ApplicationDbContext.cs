﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntexBrickwell.Models;

namespace IntexBrickwell.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<LineItem> LineItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ProductRecommendation> ProductRecommendation { get; set; }
    public virtual DbSet<CustomerRecommendations> CustomerRecommendations { get; set; }
   //  public virtual DbSet<FraudPrediction> FraudPredictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Custom model configurations here
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_ID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence)
                .HasMaxLength(50)
                .HasColumnName("country_of_residence");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasKey(e => e.Pk);

            entity.Property(e => e.Pk)
                .ValueGeneratedNever()
                .HasColumnName("pk");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            modelBuilder.Entity<Order>().HasKey(o => o.TransactionId);
            
            modelBuilder.Entity<Order>()
                .Property(o => o.TransactionId)
                .ValueGeneratedOnAdd(); 

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank)
                .HasMaxLength(50)
                .HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode)
                .HasMaxLength(50)
                .HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(50)
                .HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.TypeOfCard)
                .HasMaxLength(50)
                .HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasMaxLength(2800)
                .HasColumnName("description");
            entity.Property(e => e.ImgLink)
                .HasMaxLength(150)
                .HasColumnName("img_link");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor)
                .HasMaxLength(50)
                .HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor)
                .HasMaxLength(50)
                .HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        
        modelBuilder.Entity<ProductRecommendation>(entity =>
        {
            entity.Property(e => e.ProductID).HasColumnName("Product_ID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("Product_Name");
            entity.Property(e => e.Rec1ID)
                .HasMaxLength(2800)
                .HasColumnName("Recommendation_1_ID");
            entity.Property(e => e.Rec1Name)
                .HasMaxLength(150)
                .HasColumnName("Recommendation_1");    
            entity.Property(e => e.Rec2ID)
                .HasMaxLength(2800)
                .HasColumnName("Recommendation_2_ID");
            entity.Property(e => e.Rec2Name)
                .HasMaxLength(150)
                .HasColumnName("Recommendation_2"); 
            entity.Property(e => e.Rec3ID)
                .HasMaxLength(2800)
                .HasColumnName("Recommendation_3_ID");
            entity.Property(e => e.Rec3Name)
                .HasMaxLength(150)
                .HasColumnName("Recommendation_3"); 
            entity.Property(e => e.Rec4ID)
                .HasMaxLength(2800)
                .HasColumnName("Recommendation_4_ID");
            entity.Property(e => e.Rec4Name)
                .HasMaxLength(150)
                .HasColumnName("Recommendation_4"); 
            entity.Property(e => e.Rec5ID)
                .HasMaxLength(2800)
                .HasColumnName("Recommendation_5_ID");
            entity.Property(e => e.Rec5Name)
                .HasMaxLength(150)
                .HasColumnName("Recommendation_5"); 
        });

    }
}

