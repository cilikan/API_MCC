using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class role2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIK",
                table: "tb_t_accountrole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NIK",
                table: "tb_t_accountrole",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
