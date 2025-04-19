using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ClassScheduleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecord_ClassSchedule_ClassScheduleId",
                table: "AttendanceRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_AspNetUsers_TeacherId",
                table: "ClassSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Sections_SectionId",
                table: "ClassSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Subjects_SubjectId",
                table: "ClassSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSession_ClassSchedule_ClassScheduleId",
                table: "ClassSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSchedule",
                table: "ClassSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedule_SectionId",
                table: "ClassSchedule");

            migrationBuilder.RenameTable(
                name: "ClassSchedule",
                newName: "ClassSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedule_TeacherId",
                table: "ClassSchedules",
                newName: "IX_ClassSchedules_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedule_SubjectId",
                table: "ClassSchedules",
                newName: "IX_ClassSchedules_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_SectionId_SubjectId_StartTime_EndTime",
                table: "ClassSchedules",
                columns: new[] { "SectionId", "SubjectId", "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecord_ClassSchedules_ClassScheduleId",
                table: "AttendanceRecord",
                column: "ClassScheduleId",
                principalTable: "ClassSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_TeacherId",
                table: "ClassSchedules",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Sections_SectionId",
                table: "ClassSchedules",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSession_ClassSchedules_ClassScheduleId",
                table: "ClassSession",
                column: "ClassScheduleId",
                principalTable: "ClassSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRecord_ClassSchedules_ClassScheduleId",
                table: "AttendanceRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_AspNetUsers_TeacherId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Sections_SectionId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Subjects_SubjectId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSession_ClassSchedules_ClassScheduleId",
                table: "ClassSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedules_SectionId_SubjectId_StartTime_EndTime",
                table: "ClassSchedules");

            migrationBuilder.RenameTable(
                name: "ClassSchedules",
                newName: "ClassSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedules_TeacherId",
                table: "ClassSchedule",
                newName: "IX_ClassSchedule_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSchedules_SubjectId",
                table: "ClassSchedule",
                newName: "IX_ClassSchedule_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSchedule",
                table: "ClassSchedule",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedule_SectionId",
                table: "ClassSchedule",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRecord_ClassSchedule_ClassScheduleId",
                table: "AttendanceRecord",
                column: "ClassScheduleId",
                principalTable: "ClassSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_AspNetUsers_TeacherId",
                table: "ClassSchedule",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Sections_SectionId",
                table: "ClassSchedule",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Subjects_SubjectId",
                table: "ClassSchedule",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSession_ClassSchedule_ClassScheduleId",
                table: "ClassSession",
                column: "ClassScheduleId",
                principalTable: "ClassSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
