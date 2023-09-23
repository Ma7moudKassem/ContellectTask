using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContellectTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreationDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "42934bb7-8f61-4b11-945e-748f8991a675");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89b96209-fdab-4ad2-8fec-c79e0affef53");

            migrationBuilder.RenameColumn(
                name: "CreationTimeDate",
                table: "Contact",
                newName: "CreationDateTime");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1df1b4f9-4adf-473c-8408-b90cc3d80b84", 0, "637c2c57-af0b-4048-b139-bdad42be1bcf", null, false, false, null, null, "USER1", "AQAAAAIAAYagAAAAEMJ2mupD9ukR4UutnVeeaaqGQD38EN67A2C8dhsdMVjrbS3UqVYcE/5nV/T6jgPtoA==", null, false, "3a9cab46-c8ae-4e16-b21a-ec735568bcce", false, "user1" },
                    { "4440d0f8-e7c5-41d8-b637-6c7c108103d3", 0, "062dd06a-a78f-4109-b50e-e3a9e2fe3d20", null, false, false, null, null, "USER2", "AQAAAAIAAYagAAAAEKsoQa8pHbl7XUXqxZWmr4nI/WtYeg9bd0uXcQFThDczxDgXSqvpR8wP/bL3j61R4w==", null, false, "65ae02c1-4433-4ba6-9148-7a7e3e146295", false, "user2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1df1b4f9-4adf-473c-8408-b90cc3d80b84");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4440d0f8-e7c5-41d8-b637-6c7c108103d3");

            migrationBuilder.RenameColumn(
                name: "CreationDateTime",
                table: "Contact",
                newName: "CreationTimeDate");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "42934bb7-8f61-4b11-945e-748f8991a675", 0, "b2cba42e-cabc-48c7-91bb-7d9ce9cb2cc6", null, false, false, null, null, "USER1", "AQAAAAIAAYagAAAAEAClWNaG7k7OIgW6eak+AXGK1Ve0MR1vf9DRwve0F3Hway6MlihzJrpEwfxb6koM3w==", null, false, "7f1ec25f-784b-4bf7-b233-9166e7ada8d2", false, "user1" },
                    { "89b96209-fdab-4ad2-8fec-c79e0affef53", 0, "ca73ae5f-bf63-40f5-be5f-ed57a425bd86", null, false, false, null, null, "USER2", "AQAAAAIAAYagAAAAEKc2OLIPQc5yENVrtUL4oavlCN5P0PlgjDp/Kv4tpxu0PURUP2PsqUPDxx9mCLmPUg==", null, false, "d3954e6c-28e0-4e20-acf1-48abe5968fec", false, "user2" }
                });
        }
    }
}
