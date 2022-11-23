using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

#nullable disable

namespace IceVault.Persistence.Read.Migrations
{
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public partial class v10_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = migrationBuilder.ReadSql($"{this.GetId()}_Up");
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = migrationBuilder.ReadSql($"{this.GetId()}_Down");
            migrationBuilder.Sql(sql);
        }
    }
}
