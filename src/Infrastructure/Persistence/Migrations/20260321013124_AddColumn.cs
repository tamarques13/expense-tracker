using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spentir.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "LastGenerated",
                table: "Subscriptions",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastGenerated",
                table: "Subscriptions");
        }
    }
}
