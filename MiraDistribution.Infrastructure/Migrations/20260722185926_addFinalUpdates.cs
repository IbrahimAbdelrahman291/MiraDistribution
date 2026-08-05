using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiraDistribution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFinalUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "Books");
        }
    }
}
