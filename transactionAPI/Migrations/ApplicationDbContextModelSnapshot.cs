﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using transactionAPI.DataAccess.Data;

#nullable disable

namespace transactionAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("transactionAPI.Entities.Transaction", b =>
                {
                    b.Property<string>("TransactionId")
                        .HasColumnType("text")
                        .HasColumnName("transaction_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<string>("ClientLocation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_location");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("TimeZoneId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("time_zone_id");

                    b.Property<string>("TimeZoneRules")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("time_zone_rules");

                    b.Property<LocalDateTime>("TransactionDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("transaction_date_local");

                    b.Property<Instant>("TransactionDateUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("transaction_date_utc");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
