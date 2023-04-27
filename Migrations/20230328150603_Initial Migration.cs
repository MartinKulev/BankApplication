using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditBooleanInfos",
                columns: table => new
                {
                    Card_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    Has_taken_credit = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditBooleanInfos", x => x.Card_number);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditDateInfos",
                columns: table => new
                {
                    Card_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    Credit_taken_date = table.Column<string>(type: "longtext", nullable: false),
                    Credit_toReturn_date = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditDateInfos", x => x.Card_number);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditMoneyInfos",
                columns: table => new
                {
                    Card_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    Credit_amount = table.Column<double>(type: "double", nullable: false),
                    Credit_interest = table.Column<double>(type: "double", nullable: false),
                    Credit_ToBePaid = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditMoneyInfos", x => x.Card_number);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserBankInfos",
                columns: table => new
                {
                    Card_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    PIN = table.Column<string>(type: "longtext", nullable: false),
                    IBAN = table.Column<string>(type: "longtext", nullable: false),
                    EGN = table.Column<string>(type: "longtext", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankInfos", x => x.Card_number);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserIBANInfos",
                columns: table => new
                {
                    IBAN = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIBANInfos", x => x.IBAN);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    EGN = table.Column<string>(type: "varchar(255)", nullable: false),
                    First_name = table.Column<string>(type: "longtext", nullable: false),
                    Last_name = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.EGN);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditBooleanInfos");

            migrationBuilder.DropTable(
                name: "CreditDateInfos");

            migrationBuilder.DropTable(
                name: "CreditMoneyInfos");

            migrationBuilder.DropTable(
                name: "UserBankInfos");

            migrationBuilder.DropTable(
                name: "UserIBANInfos");

            migrationBuilder.DropTable(
                name: "UserInfos");
        }
    }
}
