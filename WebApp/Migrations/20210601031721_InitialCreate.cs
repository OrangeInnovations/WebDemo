using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blogging");

            migrationBuilder.CreateSequence(
                name: "postseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "myusers",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myusers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blogs",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blogs_myusers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "blogging",
                        principalTable: "myusers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PosterId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_posts_blogs_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blogging",
                        principalTable: "blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_posts_myusers_PosterId",
                        column: x => x.PosterId,
                        principalSchema: "blogging",
                        principalTable: "myusers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogs_OwnerId",
                schema: "blogging",
                table: "blogs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_posts_BlogId",
                schema: "blogging",
                table: "posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_posts_PosterId",
                schema: "blogging",
                table: "posts",
                column: "PosterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "posts",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "blogs",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "myusers",
                schema: "blogging");

            migrationBuilder.DropSequence(
                name: "postseq");
        }
    }
}
