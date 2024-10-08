﻿using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace transactionAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    transaction_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    transaction_date_local = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    time_zone_id = table.Column<string>(type: "text", nullable: false),
                    client_location = table.Column<string>(type: "text", nullable: false),
                    transaction_date_utc = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    time_zone_rules = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.transaction_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
