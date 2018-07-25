using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogApi.Domain;

namespace ProductCatalogApi.Data
{
    public class CatalogContext:DbContext
    {
        // Constructor
        // Overiding the base class
        // Options based in through Injection
        // Used for testing purposes
        // Without changing data access
        public CatalogContext(DbContextOptions options):base(options) 
        {   
        }
        // Entity configuration using FluentAPI
        // Handler method from DbContext
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CatalogBrand>(ConfigureCatalogBrand);
            builder.Entity<CatalogType>(ConfigureCatalogType);
            builder.Entity<CatalogItem>(ConfigureCatalogItem);
        }

        private void ConfigureCatalogItem(EntityTypeBuilder<CatalogItem> builder)
        {
            // Coresponding table should be called Catalog
            builder.ToTable("Catalog");
            // Id/Primary Key 
            // High portion - created by the DB and given by the Context
            // Low portion - added by the memory (0-100)
            // Advantage - having Id before sending to the database, persistent ID
            builder.Property(c=>c.Id)
                .ForSqlServerUseSequenceHiLo("catalog_hilo")
                .IsRequired(true);
            builder.Property(c=>c.Name)
                .IsRequired(true)
                .HasMaxLength(50);
            builder.Property(c=>c.Price)
                .IsRequired(true);
            builder.Property(c=>c.PictureUrl)
                .IsRequired(false);
            builder.HasOne(c=>c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c=>c.CatalogBrandId);
            builder.HasOne(c=>c.CatalogType)
                .WithMany()
                .HasForeignKey(c=>c.CatalogTypeId);
            builder.HasData(
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=3, Description = "Shoes for next century", Name = "World Star", Price = 199.5M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/1" },
                new Domain.CatalogItem { CatalogTypeId=1,CatalogBrandId=2, Description = "will make you world champions", Name = "White Line", Price= 88.50M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/2" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=3, Description = "You have already won gold medal", Name = "Prism White Shoes", Price = 129, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/3" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=2, Description = "Olympic runner", Name = "Foundation Hitech", Price = 12, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/4" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=1, Description = "Roslyn Red Sheet", Name = "Roslyn White", Price = 188.5M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/5" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=2, Description = "Lala Land", Name = "Blue Star", Price = 112, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/6" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=1, Description = "High in the sky", Name = "Roslyn Green", Price = 212, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/7"  },
                new Domain.CatalogItem { CatalogTypeId=1,CatalogBrandId=1, Description = "Light as carbon", Name = "Deep Purple", Price = 238.5M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/8" },
                new Domain.CatalogItem { CatalogTypeId=1,CatalogBrandId=2, Description = "High Jumper", Name = "Addidas<White> Running", Price = 129, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/9" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=3, Description = "Dunker", Name = "Elequent", Price = 12, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/10" },
                new Domain.CatalogItem { CatalogTypeId=1,CatalogBrandId=2, Description = "All round", Name = "Inredeible", Price = 248.5M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/11" },
                new Domain.CatalogItem { CatalogTypeId=2,CatalogBrandId=1, Description = "Pricesless", Name = "London Sky", Price = 412, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/12" },
                new Domain.CatalogItem { CatalogTypeId=3,CatalogBrandId=3, Description = "Tennis Star", Name = "Elequent", Price = 123, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/13" },
                new Domain.CatalogItem { CatalogTypeId=3,CatalogBrandId=2, Description = "Wimbeldon", Name = "London Star", Price = 218.5M, PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/14" }
            );
        }

        private void ConfigureCatalogType(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");
            builder.Property(c=>c.id)
                .ForSqlServerUseSequenceHiLo("catalog_type_hilo")
                .IsRequired();
            builder.Property(c=>c.Type)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasData(
                new Domain.CatalogType { Type = "Running" },
                new Domain.CatalogType { Type = "Basketball" },
                new Domain.CatalogType { Type = "Tennis" }
            );
        }

        private void ConfigureCatalogBrand(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.Property(c=>c.id)
                .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
                .IsRequired();
            builder.Property(c=>c.Brand)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasData(
                new Domain.CatalogBrand { Brand = "Addidas" },
                new Domain.CatalogBrand { Brand = "Puma" },
                new Domain.CatalogBrand { Brand = "Slazenger" }
            );
        }

        public DbSet<CatalogType> CatalogTypes {get; set;}
        public DbSet<CatalogBrand> CatalogBrands {get; set;}
        public DbSet<CatalogItem> CatalogItems {get; set;}

    }
}
