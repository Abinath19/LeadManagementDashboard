using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadManagementDashboard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeadActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadId = table.Column<int>(type: "int", nullable: false),
                    FromStatusId = table.Column<int>(type: "int", nullable: true),
                    ToStatusId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadActivities_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadActivities_Statuses_FromStatusId",
                        column: x => x.FromStatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadActivities_Statuses_ToStatusId",
                        column: x => x.ToStatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "ColorCode", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "#0d6efd", 1, true, "New" },
                    { 2, "#ffc107", 2, true, "Contacted" },
                    { 3, "#198754", 3, true, "Qualified" },
                    { 4, "#6c757d", 4, true, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "Id", "Company", "CreatedAt", "Email", "FirstName", "LastName", "Phone", "StatusId" },
                values: new object[,]
                {
                    { 1, "Acme Corp", new DateTime(2026, 7, 1, 9, 0, 0, 0, DateTimeKind.Utc), "alice.johnson@example.com", "Alice", "Johnson", "+1-555-0101", 1 },
                    { 2, "Globex Ltd", new DateTime(2026, 7, 2, 9, 0, 0, 0, DateTimeKind.Utc), "bob.martinez@example.com", "Bob", "Martinez", "+1-555-0102", 2 },
                    { 3, "Initech", new DateTime(2026, 7, 3, 9, 0, 0, 0, DateTimeKind.Utc), "carla.nguyen@example.com", "Carla", "Nguyen", "+1-555-0103", 3 },
                    { 4, "Umbrella Inc", new DateTime(2026, 7, 4, 9, 0, 0, 0, DateTimeKind.Utc), "david.okafor@example.com", "David", "Okafor", "+1-555-0104", 4 },
                    { 5, "Stark Industries", new DateTime(2026, 7, 5, 9, 0, 0, 0, DateTimeKind.Utc), "elena.petrova@example.com", "Elena", "Petrova", "+1-555-0105", 1 }
                });

            migrationBuilder.InsertData(
                table: "LeadActivities",
                columns: new[] { "Id", "ChangedAt", "FromStatusId", "LeadId", "Note", "ToStatusId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, 2, "Lead created", 1 },
                    { 2, new DateTime(2026, 7, 3, 9, 0, 0, 0, DateTimeKind.Utc), 1, 2, "Status changed by user", 2 },
                    { 3, new DateTime(2026, 7, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, 3, "Lead created", 1 },
                    { 4, new DateTime(2026, 7, 4, 9, 0, 0, 0, DateTimeKind.Utc), 1, 3, "Status changed by user", 2 },
                    { 5, new DateTime(2026, 7, 5, 9, 0, 0, 0, DateTimeKind.Utc), 2, 3, "Status changed by user", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadActivities_FromStatusId",
                table: "LeadActivities",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadActivities_LeadId",
                table: "LeadActivities",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadActivities_ToStatusId",
                table: "LeadActivities",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_StatusId",
                table: "Leads",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadActivities");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
