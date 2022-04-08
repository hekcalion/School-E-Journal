using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolGradeWebApp.Data.Migrations
{
    public partial class TeacherWorkLoad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "GradeDetails",
                type: "TEXT",
                nullable: false,               
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWorkloads_GroupID",
                table: "TeacherWorkloads",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWorkloads_SubjectID",
                table: "TeacherWorkloads",
                column: "SubjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherWorkloads_Groups_GroupID",
                table: "TeacherWorkloads",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherWorkloads_Subjects_SubjectID",
                table: "TeacherWorkloads",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherWorkloads_Groups_GroupID",
                table: "TeacherWorkloads");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherWorkloads_Subjects_SubjectID",
                table: "TeacherWorkloads");

            migrationBuilder.DropIndex(
                name: "IX_TeacherWorkloads_GroupID",
                table: "TeacherWorkloads");

            migrationBuilder.DropIndex(
                name: "IX_TeacherWorkloads_SubjectID",
                table: "TeacherWorkloads");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "GradeDetails",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
