using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raccord.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectAccountManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "account_manager_id",
                schema: "projects",
                table: "projects",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_projects_account_manager_id",
                schema: "projects",
                table: "projects",
                column: "account_manager_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_projects_users_account_manager_id",
                schema: "projects",
                table: "projects",
                column: "account_manager_id",
                principalSchema: "workspace",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projects_users_account_manager_id",
                schema: "projects",
                table: "projects"
            );

            migrationBuilder.DropIndex(
                name: "ix_projects_account_manager_id",
                schema: "projects",
                table: "projects"
            );

            migrationBuilder.DropColumn(
                name: "account_manager_id",
                schema: "projects",
                table: "projects"
            );
        }
    }
}
