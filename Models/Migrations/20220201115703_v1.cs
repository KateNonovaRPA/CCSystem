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
                    ClientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtAttributes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourtAttributes_Courts_courtID",
                        column: x => x.courtID,
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
                    lawsuitEntryNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    typeID = table.Column<int>(type: "int", nullable: false),
                    courtID = table.Column<int>(type: "int", nullable: false),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    { 184, null, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(566), "Специализиран наказателен съд", "Специализиран наказателен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 183, null, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(562), "Софийски районен съд", "Софийски районен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 182, null, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(558), "Софийски окръжен съд", "Софийски окръжен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 181, null, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(554), "Софийски градски съд", "Софийски градски съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, null, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9890), "Върховен касационен съд", "Върховен касационен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, null, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9886), "Върховен административен съд", "Върховен административен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, null, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9824), "Апелативен специализиран наказателен съд", "Апелативен специализиран наказателен съд", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                    { 80, "Нови пазар" },
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
                    { 81, "Омуртаг" },
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
                    { 13, "Пазарджик" },
                    { 61, "Карлово" },
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
                    { 12, "Монтана" },
                    { 32, "Балчик" },
                    { 31, "Асеновград" },
                    { 59, "Каварна" },
                    { 58, "Ихтиман" },
                    { 57, "Исперих" },
                    { 56, "Ивайловград" },
                    { 55, "Златоград" },
                    { 53, "Елхово" },
                    { 52, "Елин Пелин" },
                    { 51, "Елена" },
                    { 50, "Дупница" },
                    { 49, "Дулово" },
                    { 48, "Дряново" },
                    { 47, "Димитровград" },
                    { 46, "Девня" },
                    { 54, "Етрополе" },
                    { 44, "Гълъбово" },
                    { 33, "Белоградчик" },
                    { 45, "Девин" },
                    { 34, "Берковица" },
                    { 35, "Ботевград" },
                    { 37, "Бяла" },
                    { 38, "Бяла Слатина" },
                    { 36, "Брезник" },
                    { 39, "Велики Преслав" },
                    { 40, "Велинград" },
                    { 41, "Ген. Тошево" },
                    { 42, "Горна Оряховица" },
                    { 43, "Гоце Делчев" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LawsuitTypeDictionary",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 27, "НЧХД" },
                    { 23, "КЧАНД" },
                    { 24, "Наказателно дело за възобновяване" },
                    { 21, "Касационно частно търговско дело" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LawsuitTypeDictionary",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { 25, "НАХД" },
                    { 26, "НОХД" },
                    { 22, "КНАХД" },
                    { 28, "Търговско дело" },
                    { 33, "Частно административно дело" },
                    { 30, "Търговско дело по несъстоятелност (В)" },
                    { 31, "Фирмено дело" },
                    { 32, "ЧАНД" },
                    { 34, "Частно гражданско дело" },
                    { 35, "Частно търговско дело" },
                    { 20, "Касационно частно наказателно дело" },
                    { 36, "ЧНД" },
                    { 29, "Търговско дело по несъстоятелност" },
                    { 19, "Касационно частно гражданско дело" },
                    { 5, "ВНЧХД" },
                    { 17, "Касационно търговско дело" },
                    { 1, "Административно дело" },
                    { 2, "Брачно дело" },
                    { 3, "ВНАХД" },
                    { 4, "ВНОХД" },
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
                columns: new[] { "UUID", "AccessToken", "ClientID", "ClientSecret", "Email", "FullName", "Type", "createdAt", "deletedAt", "updatedAt" },
                values: new object[,]
                {
                    { new Guid("dea1a827-a355-4c8a-fdb0-08d9dfe8b8ae"), "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDEyOTYzNjcsImV4cCI6MzIxOTEzMzE2NywiaWF0IjoxNjQxMjk2MzY3fQ.MNqujdRSkbH9tHS5U5mmO0pJzVobnsfIzOTM5XOrJqw", new Guid("253c12aa-8fbe-4036-8ac0-ede4f69fa215"), "dd44d976-dc58-4472-a7e4-aa07e4f1874a", null, "Софийски районен съд", "robot", new DateTime(2022, 2, 1, 13, 57, 1, 974, DateTimeKind.Local).AddTicks(932), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("114f0249-effd-45fb-fdaf-08d9dfe8b8ae"), "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDEyOTYzNDEsImV4cCI6MzIxOTEzMzE0MSwiaWF0IjoxNjQxMjk2MzQxfQ.vfzrTvXJxPMJlgERvBB8ZJAoYsXoDBnQCLnIWbnh_mA", new Guid("8af1bcbf-f10e-4cc0-88da-4ec81da7ee95"), "7572f72f-3ec4-466f-ac3a-b2f7f19f13c4", null, "Върховен касационен съд", "robot", new DateTime(2022, 2, 1, 13, 57, 1, 965, DateTimeKind.Local).AddTicks(9154), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("39d2b5c4-3d64-4bdd-fdb1-08d9dfe8b8ae"), "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDEyOTYzODcsImV4cCI6MzIxOTEzMzE4NywiaWF0IjoxNjQxMjk2Mzg3fQ.6HtapfkWYwi4I0cxQgIdK9mCMd2qJswGMgpR8MaO61g", new Guid("2cf70dc5-5e2e-4580-9e0b-e52eb88ac1ae"), "2bc3eac4-cf99-4d8f-b549-1b1c82cf9ac1", null, "justiceBG", "robot", new DateTime(2022, 2, 1, 13, 57, 1, 974, DateTimeKind.Local).AddTicks(2724), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(8958), "Административен съд - Благоевград", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 99, 51, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(195), "Районен съд - Елена", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 100, 52, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(199), "Районен съд - Елин Пелин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 101, 53, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(203), "Районен съд - Елхово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 54, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(207), "Районен съд - Етрополе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 55, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(210), "Районен съд - Златоград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 104, 56, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(215), "Районен съд - Ивайловград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 57, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(219), "Районен съд - Исперих", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 106, 58, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(222), "Районен съд - Ихтиман", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 98, 50, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(189), "Районен съд - Дупница", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 107, 59, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(229), "Районен съд - Каварна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 109, 61, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(237), "Районен съд - Карлово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 110, 62, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(241), "Районен съд - Карнобат", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 111, 63, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(245), "Районен съд - Кнежа", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 112, 64, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(249), "Районен съд - Козлодуй", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 113, 65, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(253), "Районен съд - Костинброд", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 114, 66, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(257), "Районен съд - Котел", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 115, 67, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(261), "Районен съд - Крумовград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 116, 68, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(265), "Районен съд - Кубрат", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 108, 60, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(233), "Районен съд - Казанлък", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 117, 69, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(269), "Районен съд - Кула", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 97, 49, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(182), "Районен съд - Дулово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 94, 47, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(147), "Районен съд - Димитровград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 69, 29, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9998), "Районен съд - Айтос", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 70, 30, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(2), "Районен съд - Ардино", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, 31, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(7), "Районен съд - Асеновград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 72, 32, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(11), "Районен съд - Балчик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, 33, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(15), "Районен съд - Белоградчик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 74, 34, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(19), "Районен съд - Берковица", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 76, 35, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(27), "Районен съд - Ботевград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 77, 36, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(32), "Районен съд - Брезник", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 96, 48, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(174), "Районен съд - Дряново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 79, 37, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(45), "Районен съд - Бяла", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 82, 39, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(66), "Районен съд - Велики Преслав", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 84, 40, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(79), "Районен съд - Велинград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 88, 41, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(105), "Районен съд - Ген. Тошево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 89, 42, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(113), "Районен съд - Горна Оряховица", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 90, 43, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(119), "Районен съд - Гоце Делчев", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 91, 44, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(126), "Районен съд - Гълъбово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 92, 45, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(133), "Районен съд - Девин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 93, 46, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(140), "Районен съд - Девня", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 80, 38, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(52), "Районен съд - Бяла Слатина", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 120, 70, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(280), "Районен съд - Левски", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 122, 71, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(288), "Районен съд - Лом", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 123, 72, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(293), "Районен съд - Луковит", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 154, 96, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(438), "Районен съд - Сандански", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 155, 97, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(442), "Районен съд - Свиленград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 156, 98, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(446), "Районен съд - Свищов", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 157, 99, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(450), "Районен съд - Своге", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 158, 100, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(454), "Районен съд - Севлиево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 161, 101, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(467), "Районен съд - Сливница", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 163, 102, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(475), "Районен съд - Средец", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 164, 103, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(479), "Районен съд - Ст.Загора", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 153, 95, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(434), "Районен съд - Самоков", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 165, 104, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(483), "Районен съд - Тервел", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 167, 106, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(491), "Районен съд - Тополовград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 168, 107, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(495), "Районен съд - Троян", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 169, 108, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(499), "Районен съд - Трън", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 170, 109, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(503), "Районен съд - Трявна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 171, 110, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(507), "Районен съд - Тутракан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 173, 111, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(515), "Районен съд - Харманли", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 175, 112, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(523), "Районен съд - Царево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 176, 113, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(527), "Районен съд - Чепеларе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 166, 105, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(487), "Районен съд - Тетевен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 151, 94, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(427), "Районен съд - Разлог", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 149, 93, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(418), "Районен съд - Радомир", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 148, 92, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(414), "Районен съд - Раднево", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 124, 73, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(296), "Районен съд - Мадан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 125, 74, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(301), "Районен съд - Малко Търново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 126, 75, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(305), "Районен съд - Мездра", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 127, 76, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(309), "Районен съд - Момчилград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 129, 77, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(337), "Районен съд - Несебър", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 130, 78, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(341), "Районен съд - Никопол", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 131, 79, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(345), "Районен съд - Нова Загора", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 132, 80, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(349), "Районен съд - Нови пазар", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 133, 81, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(353), "Районен съд - Омуртаг", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 134, 82, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(357), "Районен съд - Оряхово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 135, 83, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(361), "Районен съд - Павликени", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 137, 84, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(369), "Районен съд - Панагюрище", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 139, 85, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(377), "Районен съд - Петрич", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 140, 86, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(381), "Районен съд - Пещера", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 141, 87, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(385), "Районен съд - Пирдоп", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 144, 88, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(398), "Районен съд - Поморие", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 145, 89, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(402), "Районен съд - Попово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 146, 90, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(406), "Районен съд - Провадия", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 147, 91, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(410), "Районен съд - Първомай", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 28, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9882), "Военно-апелативен съд", "Военно", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 177, 114, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(531), "Районен съд - Червен бряг", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 180, 27, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(543), "Районен съд - Ямбол", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 27, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9817), "Административен съд - Ямбол", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 7, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9918), "Окръжен съд - Габрово", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 87, 7, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(98), "Районен съд - Габрово", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 8, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9664), "Административен съд - Добрич", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 50, 8, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9922), "Окръжен съд - Добрич", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 95, 8, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(154), "Районен съд - Добрич", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 9, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9671), "Административен съд - Кърджали", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, 9, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9926), "Окръжен съд - Кърджали", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 118, 9, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(272), "Районен съд - Кърджали", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9659), "Административен съд - Габрово", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 10, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9677), "Административен съд - Кюстендил", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 119, 10, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(276), "Районен съд - Кюстендил", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 11, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9703), "Административен съд - Ловеч", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 53, 11, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9935), "Окръжен съд - Ловеч", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 121, 11, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(284), "Районен съд - Ловеч", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 12, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9709), "Административен съд - Монтана", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 54, 12, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9939), "Окръжен съд - Монтана", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 128, 12, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(332), "Районен съд - Монтана", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 13, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9715), "Административен съд - Пазарджик", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 52, 10, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9930), "Окръжен съд - Кюстендил", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 55, 13, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9943), "Окръжен съд - Пазарджик", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 86, 6, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(92), "Районен съд - Враца", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9652), "Административен съд - Враца", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 1, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9894), "Окръжен съд - Благоевград", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 75, 1, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(23), "Районен съд - Благоевград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9571), "Административен съд - Бургас", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 2, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9830), "Апелативен съд - Бургас", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 2, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9898), "Окръжен съд - Бургас", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 78, 2, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(38), "Районен съд - Бургас", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9629), "Административен съд - Варна", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 3, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9836), "Апелативен съд - Варна", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 6, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9914), "Окръжен съд - Враца", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 3, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9862), "Военен съд - Варна", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 81, 3, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(59), "Районен съд - Варна", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9638), "Административен съд - Велико Търново", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 4, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9843), "Апелативен съд - Велико Търново", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 4, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9906), "Окръжен съд - Велико Търново", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 83, 4, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(72), "Районен съд - Велико Търново", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9645), "Административен съд - Видин", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 5, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9910), "Окръжен съд - Видин", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 85, 5, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(86), "Районен съд - Видин", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 3, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9902), "Окръжен съд - Варна", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 136, 13, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(365), "Районен съд - Пазарджик", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 14, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9721), "Административен съд - Перник", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 56, 14, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9947), "Окръжен съд - Перник", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 21, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9769), "Административен съд - Смолян", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 63, 21, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9975), "Окръжен съд - Смолян", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 162, 21, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(471), "Районен съд - Смолян", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 22, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9776), "Административен съд - София-град", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 22, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9783), "Административен съд - София-област", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 22, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9857), "Апелативен съд - София", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 22, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9878), "Военен съд - София", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 23, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9789), "Административен съд - Стара Загора", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 160, 20, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(462), "Районен съд - Сливен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 64, 23, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9978), "Окръжен съд - Стара Загора", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 65, 24, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9982), "Окръжен съд - Търговище", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 172, 24, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(511), "Районен съд - Търговище", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 25, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9802), "Административен съд - Хасково", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 66, 25, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9986), "Окръжен съд - Хасково", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 174, 25, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(519), "Районен съд - Хасково", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 26, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9809), "Административен съд - Шумен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 67, 26, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9990), "Окръжен съд - Шумен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 179, 26, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(539), "Районен съд - Шумен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 24, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9795), "Административен съд - Търговище", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 62, 20, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9971), "Окръжен съд - Сливен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 20, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9874), "Военен съд - Сливен", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 20, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9760), "Административен съд - Сливен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 138, 14, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(373), "Районен съд - Перник", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 15, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9727), "Административен съд - Плевен", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 15, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9866), "Военен съд - Плевен", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 57, 15, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9951), "Окръжен съд - Плевен", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 142, 15, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(389), "Районен съд - Плевен", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 16, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9734), "Административен съд - Пловдив", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 16, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9849), "Апелативен съд - Пловдив", "Апелативен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 16, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9870), "Военен съд - Пловдив", "Военен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 58, 16, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9955), "Окръжен съд - Пловдив", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 143, 16, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(394), "Районен съд - Пловдив", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 17, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9740), "Административен съд - Разград", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 59, 17, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9959), "Окръжен съд - Разград", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Courts",
                columns: new[] { "ID", "cityId", "createdAt", "fullName", "name", "updatedAt" },
                values: new object[,]
                {
                    { 150, 17, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(422), "Районен съд - Разград", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 18, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9747), "Административен съд - Русе", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 60, 18, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9963), "Окръжен съд - Русе", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 152, 18, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(430), "Районен съд - Русе", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 19, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9753), "Административен съд - Силистра", "Административен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 61, 19, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9967), "Окръжен съд - Силистра", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 159, 19, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(458), "Районен съд - Силистра", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 68, 27, new DateTime(2022, 2, 1, 13, 57, 1, 977, DateTimeKind.Local).AddTicks(9994), "Окръжен съд - Ямбол", "Окръжен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 178, 115, new DateTime(2022, 2, 1, 13, 57, 1, 978, DateTimeKind.Local).AddTicks(535), "Районен съд - Чирпан", "Районен съд ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourtAttributes_courtID",
                schema: "dbo",
                table: "CourtAttributes",
                column: "courtID");

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
