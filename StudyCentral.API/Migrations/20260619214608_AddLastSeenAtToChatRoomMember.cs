using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyCentral.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLastSeenAtToChatRoomMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenAt",
                table: "ChatRoomMembers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("abababab-abab-abab-abab-abababababab"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1364));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1367));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1403));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddabc"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("efefefef-efef-efef-efef-efefefefefef"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "ChatRoomMembers",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("22222222-2222-2222-2222-222222222222") },
                column: "LastSeenAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "ChatRoomMembers",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("33333333-3333-3333-3333-333333333333") },
                column: "LastSeenAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "ChatRoomMembers",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("44444444-4444-4444-4444-444444444444") },
                column: "LastSeenAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "ChatRoomMembers",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("88888888-8888-8888-8888-888888888888") },
                column: "LastSeenAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "ChatRoomMembers",
                keyColumns: new[] { "ChatRoomId", "UserId" },
                keyValues: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("99999999-9999-9999-9999-999999999999") },
                column: "LastSeenAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1329));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1336));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("56565656-5656-5656-5656-565656565656"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("67676767-6767-6767-6767-676767676767"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1500));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("78787878-7878-7878-7878-787878787878"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1509));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("89898989-8989-8989-8989-898989898989"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1511));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("90909090-9090-9090-9090-909090909090"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1513));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("abababab-abab-abab-abab-abababababab"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1485));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1481));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1483));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("12121212-1212-1212-1212-121212121212"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1461));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1313-1313-1313-131313131313"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1463));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("14141414-1414-1414-1414-141414141414"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1465));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1458));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1233));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1238));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1244));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 19, 21, 46, 7, 789, DateTimeKind.Utc).AddTicks(1245));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeenAt",
                table: "ChatRoomMembers");

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("abababab-abab-abab-abab-abababababab"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6558));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6556));

            migrationBuilder.UpdateData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: new Guid("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6557));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6591));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddabc"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: new Guid("efefefef-efef-efef-efef-efefefefefef"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6604));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6525));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("56565656-5656-5656-5656-565656565656"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("67676767-6767-6767-6767-676767676767"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6664));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("78787878-7878-7878-7878-787878787878"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6666));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("89898989-8989-8989-8989-898989898989"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "StudyFiles",
                keyColumn: "Id",
                keyValue: new Guid("90909090-9090-9090-9090-909090909090"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6688));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("abababab-abab-abab-abab-abababababab"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6647));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6644));

            migrationBuilder.UpdateData(
                table: "StudyFolders",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("12121212-1212-1212-1212-121212121212"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6626));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1313-1313-1313-131313131313"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("14141414-1414-1414-1414-141414141414"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6630));

            migrationBuilder.UpdateData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6387));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6391));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6414));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6393));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6418));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 12, 30, 13, 888, DateTimeKind.Utc).AddTicks(6420));
        }
    }
}
