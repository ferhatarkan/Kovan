using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kovan.Infrastructure.Migrations;

public partial class AddInvoiceWarehouse : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "WarehouseId",
            table: "Invoices",
            type: "uuid",
            nullable: false,
            defaultValue: Guid.Empty);

        migrationBuilder.CreateIndex(
            name: "IX_Invoices_WarehouseId",
            table: "Invoices",
            column: "WarehouseId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_Invoices_WarehouseId", table: "Invoices");
        migrationBuilder.DropColumn(name: "WarehouseId", table: "Invoices");
    }
}
