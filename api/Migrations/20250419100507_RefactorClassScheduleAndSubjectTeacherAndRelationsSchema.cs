using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorClassScheduleAndSubjectTeacherAndRelationsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_TeacherId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedules_SectionId_SubjectId_StartTime_EndTime",
                table: "ClassSchedules");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ClassSchedules",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedules_TeacherId",
                table: "ClassSchedules",
                newName: "IX_ClassSchedules_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ClassSchedules",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "SubjectTeacherId",
                table: "ClassSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_SectionId_SubjectTeacherId_StartTime_EndTime",
                table: "ClassSchedules",
                columns: new[] { "SectionId", "SubjectTeacherId", "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_SubjectTeacherId",
                table: "ClassSchedules",
                column: "SubjectTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_UserId",
                table: "ClassSchedules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_SubjectTeachers_SubjectTeacherId",
                table: "ClassSchedules",
                column: "SubjectTeacherId",
                principalTable: "SubjectTeachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_UserId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_SubjectTeachers_SubjectTeacherId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedules_SectionId_SubjectTeacherId_StartTime_EndTime",
                table: "ClassSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedules_SubjectTeacherId",
                table: "ClassSchedules");

            migrationBuilder.DropColumn(
                name: "SubjectTeacherId",
                table: "ClassSchedules");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClassSchedules",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedules_UserId",
                table: "ClassSchedules",
                newName: "IX_ClassSchedules_TeacherId");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ClassSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_SectionId_SubjectId_StartTime_EndTime",
                table: "ClassSchedules",
                columns: new[] { "SectionId", "SubjectId", "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_TeacherId",
                table: "ClassSchedules",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
