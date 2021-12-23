using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LawsuitTypeDictionary",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawsuitTypeDictionary", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    identityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    processorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    administrationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cityId = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Courts_Cities_cityId",
                        column: x => x.cityId,
                        principalSchema: "dbo",
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LawsuitTypes",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lawsuitTypeDictionaryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawsuitTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LawsuitTypes_LawsuitTypeDictionary_lawsuitTypeDictionaryID",
                        column: x => x.lawsuitTypeDictionaryID,
                        principalSchema: "dbo",
                        principalTable: "LawsuitTypeDictionary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourtAttributes",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    courtID = table.Column<int>(type: "int", nullable: false),
                    courtID1 = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtAttributes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourtAttributes_Courts_courtID1",
                        column: x => x.courtID1,
                        principalTable: "Courts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourtTypes",
                columns: table => new
                {
                    courtId = table.Column<int>(type: "int", nullable: false),
                    typeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtTypes", x => new { x.courtId, x.typeID });
                    table.ForeignKey(
                        name: "FK_CourtTypes_Courts_courtId",
                        column: x => x.courtId,
                        principalTable: "Courts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourtTypes_Types_typeID",
                        column: x => x.typeID,
                        principalTable: "Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lawsuits",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lawsuitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lawsuitEntryNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typeID = table.Column<int>(type: "int", nullable: false),
                    courtID = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lawsuits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lawsuits_Courts_courtID",
                        column: x => x.courtID,
                        principalTable: "Courts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lawsuits_LawsuitTypes_typeID",
                        column: x => x.typeID,
                        principalSchema: "dbo",
                        principalTable: "LawsuitTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LawsuitData",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courtAttributeID = table.Column<int>(type: "int", nullable: false),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lawsuitID = table.Column<int>(type: "int", nullable: true),
                    changeNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawsuitData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LawsuitData_CourtAttributes_courtAttributeID",
                        column: x => x.courtAttributeID,
                        principalSchema: "dbo",
                        principalTable: "CourtAttributes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LawsuitData_Lawsuits_lawsuitID",
                        column: x => x.lawsuitID,
                        principalSchema: "dbo",
                        principalTable: "Lawsuits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLawsuits",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lawsuitID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLawsuits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserLawsuits_Lawsuits_lawsuitID",
                        column: x => x.lawsuitID,
                        principalSchema: "dbo",
                        principalTable: "Lawsuits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLawsuits_Users_userID",
                        column: x => x.userID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 184, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9473), "Специализиран наказателен съд", "Специализиран наказателен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 183, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9469), "Софийски районен съд", "Софийски районен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 182, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9466), "Софийски окръжен съд", "Софийски окръжен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 181, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9462), "Софийски градски съд", "Софийски градски съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8829), "Върховен касационен съд", "Върховен касационен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8825), "Върховен административен съд", "Върховен административен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, null, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8759), "Апелативен специализиран наказателен съд", "Апелативен специализиран наказателен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Cities",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 1, "Благоевград" },
                    { 78, "Никопол" },
                    { 79, "Нова Загора" },
                    { 81, "Омуртаг" },
                    { 82, "Оряхово" },
                    { 86, "Пещера" },
                    { 84, "Панагюрище" },
                    { 85, "Петрич" },
                    { 77, "Несебър" },
                    { 87, "Пирдоп" },
                    { 83, "Павликени" },
                    { 76, "Момчилград" },
                    { 72, "Луковит" },
                    { 74, "Малко Търново" },
                    { 73, "Мадан" },
                    { 88, "Поморие" },
                    { 71, "Лом" },
                    { 70, "Левски" },
                    { 69, "Кула" },
                    { 68, "Кубрат" },
                    { 67, "Крумовград" },
                    { 66, "Котел" },
                    { 65, "Костинброд" },
                    { 64, "Козлодуй" },
                    { 75, "Мездра" },
                    { 89, "Попово" },
                    { 93, "Радомир" },
                    { 91, "Първомай" },
                    { 115, "Чирпан" },
                    { 114, "Червен бряг" },
                    { 113, "Чепеларе" },
                    { 112, "Царево" },
                    { 111, "Харманли" },
                    { 110, "Тутракан" },
                    { 109, "Трявна" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Cities",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 108, "Трън" },
                    { 107, "Троян" },
                    { 106, "Тополовград" },
                    { 105, "Тетевен" },
                    { 90, "Провадия" },
                    { 104, "Тервел" },
                    { 102, "Средец" },
                    { 101, "Сливница" },
                    { 100, "Севлиево" },
                    { 99, "Своге" },
                    { 98, "Свищов" },
                    { 97, "Свиленград" },
                    { 96, "Сандански" },
                    { 95, "Самоков" },
                    { 94, "Разлог" },
                    { 63, "Кнежа" },
                    { 92, "Раднево" },
                    { 103, "Ст.Загора" },
                    { 62, "Карнобат" },
                    { 80, "Нови пазар" },
                    { 60, "Казанлък" },
                    { 28, "пелативен съд" },
                    { 27, "Ямбол" },
                    { 26, "Шумен" },
                    { 25, "Хасково" },
                    { 24, "Търговище" },
                    { 23, "Стара Загора" },
                    { 22, "София" },
                    { 21, "Смолян" },
                    { 20, "Сливен" },
                    { 19, "Силистра" },
                    { 18, "Русе" },
                    { 17, "Разград" },
                    { 16, "Пловдив" },
                    { 15, "Плевен" },
                    { 14, "Перник" },
                    { 61, "Карлово" },
                    { 12, "Монтана" },
                    { 11, "Ловеч" },
                    { 10, "Кюстендил" },
                    { 9, "Кърджали" },
                    { 8, "Добрич" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Cities",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 7, "Габрово" },
                    { 6, "Враца" },
                    { 5, "Видин" },
                    { 4, "Велико Търново" },
                    { 3, "Варна" },
                    { 2, "Бургас" },
                    { 29, "Айтос" },
                    { 30, "Ардино" },
                    { 13, "Пазарджик" },
                    { 32, "Балчик" },
                    { 31, "Асеновград" },
                    { 59, "Каварна" },
                    { 58, "Ихтиман" },
                    { 57, "Исперих" },
                    { 56, "Ивайловград" },
                    { 55, "Златоград" },
                    { 54, "Етрополе" },
                    { 52, "Елин Пелин" },
                    { 51, "Елена" },
                    { 50, "Дупница" },
                    { 49, "Дулово" },
                    { 48, "Дряново" },
                    { 47, "Димитровград" },
                    { 46, "Девня" },
                    { 53, "Елхово" },
                    { 44, "Гълъбово" },
                    { 33, "Белоградчик" },
                    { 45, "Девин" },
                    { 34, "Берковица" },
                    { 35, "Ботевград" },
                    { 36, "Брезник" },
                    { 38, "Бяла Слатина" },
                    { 37, "Бяла" },
                    { 40, "Велинград" },
                    { 41, "Ген. Тошево" },
                    { 42, "Горна Оряховица" },
                    { 43, "Гоце Делчев" },
                    { 39, "Велики Преслав" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LawsuitTypeDictionary",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 22, "КНАХД" },
                    { 26, "НОХД" },
                    { 23, "КЧАНД" },
                    { 24, "Наказателно дело за възобновяване" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LawsuitTypeDictionary",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 21, "Касационно частно търговско дело" },
                    { 25, "НАХД" },
                    { 27, "НЧХД" },
                    { 33, "Частно административно дело" },
                    { 29, "Търговско дело по несъстоятелност" },
                    { 30, "Търговско дело по несъстоятелност (В)" },
                    { 31, "Фирмено дело" },
                    { 32, "ЧАНД" },
                    { 34, "Частно гражданско дело" },
                    { 20, "Касационно частно наказателно дело" },
                    { 35, "Частно търговско дело" },
                    { 28, "Търговско дело" },
                    { 19, "Касационно частно гражданско дело" },
                    { 4, "ВНОХД" },
                    { 17, "Касационно търговско дело" },
                    { 36, "ЧНД" },
                    { 1, "Административно дело" },
                    { 2, "Брачно дело" },
                    { 3, "ВНАХД" },
                    { 5, "ВНЧХД" },
                    { 6, "ВЧНД" },
                    { 7, "Въззивно гражданско дело" },
                    { 18, "Касационно частно административно дело" },
                    { 8, "Въззивно търговско дело" },
                    { 10, "Въззивно частно търговско дело" },
                    { 11, "Гражданско дело" },
                    { 12, "Гражданско дело по несъстоятелност" },
                    { 13, "Гражданско дело по несъстоятелност (В)" },
                    { 14, "Касационно административно дело" },
                    { 15, "Касационно гражданско дело" },
                    { 16, "Касационно наказателно дело" },
                    { 9, "Въззивно частно гражданско дело" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Users",
                columns: new[] { "UUID", "administrationName", "createdAt", "deletedAt", "email", "identityID", "name", "processorID", "updatedAt" },
                values: new object[] { new Guid("71967346-b744-469b-b8d7-159530990028"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("71967346-b744-469b-b8d7-159530990028"), "test", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2021, 12, 21, 14, 50, 18, 874, DateTimeKind.Local).AddTicks(8129), "Административен съд - Благоевград", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 99, 51, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9061), "Районен съд - Елена", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 100, 52, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9065), "Районен съд - Елин Пелин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 101, 53, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9069), "Районен съд - Елхово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 54, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9073), "Районен съд - Етрополе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 55, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9077), "Районен съд - Златоград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 104, 56, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9081), "Районен съд - Ивайловград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 57, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9085), "Районен съд - Исперих", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 106, 58, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9089), "Районен съд - Ихтиман", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 98, 50, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9057), "Районен съд - Дупница", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 107, 59, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9093), "Районен съд - Каварна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 109, 61, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9101), "Районен съд - Карлово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 110, 62, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9105), "Районен съд - Карнобат", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 111, 63, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9109), "Районен съд - Кнежа", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 112, 64, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9112), "Районен съд - Козлодуй", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 113, 65, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9116), "Районен съд - Костинброд", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 114, 66, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9120), "Районен съд - Котел", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 115, 67, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9124), "Районен съд - Крумовград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 116, 68, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9129), "Районен съд - Кубрат", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 108, 60, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9097), "Районен съд - Казанлък", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 117, 69, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9133), "Районен съд - Кула", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 97, 49, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9053), "Районен съд - Дулово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 94, 47, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9041), "Районен съд - Димитровград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 69, 29, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8939), "Районен съд - Айтос", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 70, 30, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8943), "Районен съд - Ардино", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, 31, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8947), "Районен съд - Асеновград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 72, 32, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8951), "Районен съд - Балчик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, 33, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8955), "Районен съд - Белоградчик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 74, 34, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8959), "Районен съд - Берковица", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 76, 35, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8967), "Районен съд - Ботевград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 77, 36, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8971), "Районен съд - Брезник", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 96, 48, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9049), "Районен съд - Дряново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 79, 37, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8979), "Районен съд - Бяла", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 82, 39, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8993), "Районен съд - Велики Преслав", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 84, 40, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9001), "Районен съд - Велинград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 88, 41, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9017), "Районен съд - Ген. Тошево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 89, 42, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9021), "Районен съд - Горна Оряховица", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 90, 43, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9025), "Районен съд - Гоце Делчев", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 91, 44, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9029), "Районен съд - Гълъбово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 92, 45, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9033), "Районен съд - Девин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 93, 46, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9037), "Районен съд - Девня", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 80, 38, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8984), "Районен съд - Бяла Слатина", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 120, 70, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9145), "Районен съд - Левски", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 122, 71, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9153), "Районен съд - Лом", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 123, 72, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9165), "Районен съд - Луковит", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 154, 96, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9290), "Районен съд - Сандански", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 155, 97, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9294), "Районен съд - Свиленград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 156, 98, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9298), "Районен съд - Свищов", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 157, 99, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9302), "Районен съд - Своге", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 158, 100, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9306), "Районен съд - Севлиево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 161, 101, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9318), "Районен съд - Сливница", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 163, 102, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9326), "Районен съд - Средец", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 164, 103, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9329), "Районен съд - Ст.Загора", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 153, 95, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9286), "Районен съд - Самоков", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 165, 104, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9333), "Районен съд - Тервел", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 167, 106, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9341), "Районен съд - Тополовград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 168, 107, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9345), "Районен съд - Троян", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 169, 108, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9349), "Районен съд - Трън", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 170, 109, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9356), "Районен съд - Трявна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 171, 110, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9360), "Районен съд - Тутракан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 173, 111, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9368), "Районен съд - Харманли", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 175, 112, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9376), "Районен съд - Царево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 176, 113, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9380), "Районен съд - Чепеларе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 166, 105, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9337), "Районен съд - Тетевен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 151, 94, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9277), "Районен съд - Разлог", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 149, 93, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9269), "Районен съд - Радомир", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 148, 92, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9265), "Районен съд - Раднево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 124, 73, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9170), "Районен съд - Мадан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 125, 74, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9174), "Районен съд - Малко Търново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 126, 75, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9178), "Районен съд - Мездра", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 127, 76, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9182), "Районен съд - Момчилград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 129, 77, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9189), "Районен съд - Несебър", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 130, 78, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9193), "Районен съд - Никопол", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 131, 79, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9197), "Районен съд - Нова Загора", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 132, 80, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9201), "Районен съд - Нови пазар", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 133, 81, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9205), "Районен съд - Омуртаг", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 134, 82, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9210), "Районен съд - Оряхово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 135, 83, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9214), "Районен съд - Павликени", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 137, 84, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9222), "Районен съд - Панагюрище", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 139, 85, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9229), "Районен съд - Петрич", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 140, 86, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9233), "Районен съд - Пещера", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 141, 87, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9237), "Районен съд - Пирдоп", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 144, 88, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9249), "Районен съд - Поморие", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 145, 89, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9253), "Районен съд - Попово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 146, 90, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9257), "Районен съд - Провадия", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 147, 91, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9261), "Районен съд - Първомай", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 28, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8821), "Военно-апелативен съд", "Военно", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 177, 114, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9444), "Районен съд - Червен бряг", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 180, 27, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9458), "Районен съд - Ямбол", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 27, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8755), "Административен съд - Ямбол", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 7, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8858), "Окръжен съд - Габрово", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 87, 7, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9013), "Районен съд - Габрово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 8, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8675), "Административен съд - Добрич", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 50, 8, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8862), "Окръжен съд - Добрич", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 95, 8, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9045), "Районен съд - Добрич", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 9, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8680), "Административен съд - Кърджали", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, 9, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8866), "Окръжен съд - Кърджали", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 118, 9, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9137), "Районен съд - Кърджали", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8671), "Административен съд - Габрово", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 10, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8683), "Административен съд - Кюстендил", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 119, 10, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9141), "Районен съд - Кюстендил", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 11, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8687), "Административен съд - Ловеч", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 53, 11, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8874), "Окръжен съд - Ловеч", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 121, 11, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9149), "Районен съд - Ловеч", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 12, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8691), "Административен съд - Монтана", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 54, 12, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8878), "Окръжен съд - Монтана", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 128, 12, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9185), "Районен съд - Монтана", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 13, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8695), "Административен съд - Пазарджик", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 52, 10, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8870), "Окръжен съд - Кюстендил", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 55, 13, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8882), "Окръжен съд - Пазарджик", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 86, 6, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9009), "Районен съд - Враца", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8666), "Административен съд - Враца", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 1, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8833), "Окръжен съд - Благоевград", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 75, 1, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8963), "Районен съд - Благоевград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8577), "Административен съд - Бургас", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 2, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8763), "Апелативен съд - Бургас", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 2, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8838), "Окръжен съд - Бургас", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 78, 2, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8975), "Районен съд - Бургас", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8650), "Административен съд - Варна", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 3, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8767), "Апелативен съд - Варна", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 6, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8854), "Окръжен съд - Враца", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 3, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8783), "Военен съд - Варна", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 81, 3, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8989), "Районен съд - Варна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8657), "Административен съд - Велико Търново", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 4, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8771), "Апелативен съд - Велико Търново", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 4, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8845), "Окръжен съд - Велико Търново", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 83, 4, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8997), "Районен съд - Велико Търново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8661), "Административен съд - Видин", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 5, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8850), "Окръжен съд - Видин", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 85, 5, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9005), "Районен съд - Видин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 3, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8841), "Окръжен съд - Варна", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 136, 13, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9218), "Районен съд - Пазарджик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 14, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8699), "Административен съд - Перник", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 56, 14, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8886), "Окръжен съд - Перник", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 21, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8727), "Административен съд - Смолян", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 63, 21, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8915), "Окръжен съд - Смолян", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 162, 21, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9322), "Районен съд - Смолян", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 22, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8731), "Административен съд - София-град", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 22, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8735), "Административен съд - София-област", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 22, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8779), "Апелативен съд - София", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 22, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8817), "Военен съд - София", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 23, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8739), "Административен съд - Стара Загора", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 160, 20, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9314), "Районен съд - Сливен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 64, 23, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8919), "Окръжен съд - Стара Загора", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 65, 24, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8923), "Окръжен съд - Търговище", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 172, 24, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9364), "Районен съд - Търговище", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 25, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8748), "Административен съд - Хасково", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 66, 25, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8927), "Окръжен съд - Хасково", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 174, 25, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9372), "Районен съд - Хасково", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 26, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8752), "Административен съд - Шумен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 67, 26, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8931), "Окръжен съд - Шумен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 179, 26, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9454), "Районен съд - Шумен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 24, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8744), "Административен съд - Търговище", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 62, 20, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8912), "Окръжен съд - Сливен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 20, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8813), "Военен съд - Сливен", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 20, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8723), "Административен съд - Сливен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 138, 14, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9226), "Районен съд - Перник", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 15, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8703), "Административен съд - Плевен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 15, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8787), "Военен съд - Плевен", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 57, 15, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8889), "Окръжен съд - Плевен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 142, 15, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9241), "Районен съд - Плевен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 16, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8707), "Административен съд - Пловдив", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 16, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8775), "Апелативен съд - Пловдив", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 16, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8809), "Военен съд - Пловдив", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 58, 16, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8893), "Окръжен съд - Пловдив", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 143, 16, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9245), "Районен съд - Пловдив", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 17, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8711), "Административен съд - Разград", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 59, 17, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8897), "Окръжен съд - Разград", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 150, 17, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9273), "Районен съд - Разград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 18, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8715), "Административен съд - Русе", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 60, 18, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8901), "Окръжен съд - Русе", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 152, 18, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9282), "Районен съд - Русе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 19, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8719), "Административен съд - Силистра", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 61, 19, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8905), "Окръжен съд - Силистра", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 159, 19, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9310), "Районен съд - Силистра", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 68, 27, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(8935), "Окръжен съд - Ямбол", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 178, 115, new DateTime(2021, 12, 21, 14, 50, 18, 878, DateTimeKind.Local).AddTicks(9450), "Районен съд - Чирпан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourtAttributes_courtID1",
                schema: "dbo",
                table: "CourtAttributes",
                column: "courtID1");

            migrationBuilder.CreateIndex(
                name: "IX_Courts_cityId",
                table: "Courts",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtTypes_typeID",
                table: "CourtTypes",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_LawsuitData_courtAttributeID",
                schema: "dbo",
                table: "LawsuitData",
                column: "courtAttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_LawsuitData_lawsuitID",
                schema: "dbo",
                table: "LawsuitData",
                column: "lawsuitID");

            migrationBuilder.CreateIndex(
                name: "IX_Lawsuits_courtID",
                schema: "dbo",
                table: "Lawsuits",
                column: "courtID");

            migrationBuilder.CreateIndex(
                name: "IX_Lawsuits_typeID",
                schema: "dbo",
                table: "Lawsuits",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_LawsuitTypes_lawsuitTypeDictionaryID",
                schema: "dbo",
                table: "LawsuitTypes",
                column: "lawsuitTypeDictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLawsuits_lawsuitID",
                schema: "dbo",
                table: "UserLawsuits",
                column: "lawsuitID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLawsuits_userID",
                schema: "dbo",
                table: "UserLawsuits",
                column: "userID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourtTypes");

            migrationBuilder.DropTable(
                name: "LawsuitData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserLawsuits",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "CourtAttributes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Lawsuits",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "LawsuitTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LawsuitTypeDictionary",
                schema: "dbo");
        }
    }
}
