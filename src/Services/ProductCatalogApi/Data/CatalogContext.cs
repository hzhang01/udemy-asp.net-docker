using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductCatalogApi.Domain
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
        }

        public DbSet<CatalogType> CatalogTypes {get; set;}
        public DbSet<CatalogBrand> CatalogBrands {get; set;}
        public DbSet<CatalogItem> CatalogItems {get; set;}

    }
}
