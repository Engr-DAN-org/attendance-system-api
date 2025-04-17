using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class FixSubjectTeacherRelationshipPart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubjectTeachers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers",
                columns: new[] { "SubjectId", "TeacherId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubjectTeachers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTeachers",
                table: "SubjectTeachers",
                column: "Id");
        }
    }
}
