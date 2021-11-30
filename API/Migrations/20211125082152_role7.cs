using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class role7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIK",
                table: "tb_t_accountrole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NIK",
                table: "tb_t_accountrole",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
