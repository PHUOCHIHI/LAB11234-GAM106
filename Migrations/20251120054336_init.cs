using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 1,
                column: "Description",
                value: "Cấp độ dễ cho người mới bắt đầu");

            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 2,
                column: "Description",
                value: "Cấp độ trung bình");

            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 3,
                column: "Description",
                value: "Cấp độ khó");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 1,
                columns: new[] { "Answer", "ContentQuestion", "Option1", "Option2", "Option3", "Option4" },
                values: new object[] { "Hà Nội", "Thủ đô của Việt Nam là gì?", "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Huế" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 2,
                columns: new[] { "Answer", "ContentQuestion", "Option1", "Option2", "Option3", "Option4", "levelId" },
                values: new object[] { "Sông Đồng Nai", "Sông nào dài nhất Việt Nam?", "Sông Hồng", "Sông Đồng Nai", "Sông Cửu Long", "Sông Mê Kông", 1 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "Answer", "ContentQuestion", "Option1", "Option2", "Option3", "Option4", "levelId" },
                values: new object[,]
                {
                    { 3, "Fansipan", "Núi nào cao nhất Việt Nam?", "Bà Đen", "Fansipan", "Bạch Mã", "Lang Biang", 2 },
                    { 4, "63", "Việt Nam có bao nhiêu tỉnh thành?", "58", "63", "64", "65", 2 },
                    { 5, "Quần thể di tích Cố đô Huế", "Di sản văn hóa nào của Việt Nam được UNESCO công nhận đầu tiên?", "Vịnh Hạ Long", "Phố cổ Hội An", "Quần thể di tích Cố đô Huế", "Thánh địa Mỹ Sơn", 3 },
                    { 6, "1975", "Năm nào Việt Nam chính thức thống nhất?", "1973", "1974", "1975", "1976", 3 }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionId", "Name" },
                values: new object[,]
                {
                    { 3, "Bắc Trung Bộ" },
                    { 4, "Nam Trung Bộ" },
                    { 5, "Tây Nguyên" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "roleId", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Moderator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "roleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "roleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "roleId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 1,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 2,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "GameLevels",
                keyColumn: "LevelId",
                keyValue: 3,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 1,
                columns: new[] { "Answer", "ContentQuestion", "Option1", "Option2", "Option3", "Option4" },
                values: new object[] { "Đáp án 1", "Câu hỏi 1", "Đáp án 1", "Đáp án 2", "Đáp án 3", "Đáp án 4" });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "QuestionId",
                keyValue: 2,
                columns: new[] { "Answer", "ContentQuestion", "Option1", "Option2", "Option3", "Option4", "levelId" },
                values: new object[] { "Đáp án 2", "Câu hỏi 2", "Đáp án 1", "Đáp án 2", "Đáp án 3", "Đáp án 4", 2 });
        }
    }
}
