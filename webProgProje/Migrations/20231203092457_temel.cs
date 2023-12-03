using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webProgProje.Migrations
{
    /// <inheritdoc />
    public partial class temel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anadallar",
                columns: table => new
                {
                    AnadalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnadalAd = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anadallar", x => x.AnadalID);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    TC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    KullaniciTipi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.TC);
                });

            migrationBuilder.CreateTable(
                name: "Adminler",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TC = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adminler", x => x.AdminID);
                    table.ForeignKey(
                        name: "FK_Adminler_Kullanicilar_TC",
                        column: x => x.TC,
                        principalTable: "Kullanicilar",
                        principalColumn: "TC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doktorlar",
                columns: table => new
                {
                    DoktorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorDerece = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TC = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    AnadalID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktorlar", x => x.DoktorID);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Anadallar_AnadalID",
                        column: x => x.AnadalID,
                        principalTable: "Anadallar",
                        principalColumn: "AnadalID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Kullanicilar_TC",
                        column: x => x.TC,
                        principalTable: "Kullanicilar",
                        principalColumn: "TC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hastalar",
                columns: table => new
                {
                    HastaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TC = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hastalar", x => x.HastaID);
                    table.ForeignKey(
                        name: "FK_Hastalar_Kullanicilar_TC",
                        column: x => x.TC,
                        principalTable: "Kullanicilar",
                        principalColumn: "TC",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    RandevuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RandevuDate = table.Column<DateTime>(type: "date", nullable: false),
                    RandevuTime = table.Column<DateTime>(type: "time", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    DoktorID = table.Column<int>(type: "int", nullable: false),
                    AnadalID = table.Column<int>(type: "int", nullable: false),
                    HastaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.RandevuID);
                    table.ForeignKey(
                        name: "FK_Randevular_Anadallar_AnadalID",
                        column: x => x.AnadalID,
                        principalTable: "Anadallar",
                        principalColumn: "AnadalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_Doktorlar_DoktorID",
                        column: x => x.DoktorID,
                        principalTable: "Doktorlar",
                        principalColumn: "DoktorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_Hastalar_HastaID",
                        column: x => x.HastaID,
                        principalTable: "Hastalar",
                        principalColumn: "HastaID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adminler_TC",
                table: "Adminler",
                column: "TC");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_AnadalID",
                table: "Doktorlar",
                column: "AnadalID");

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_TC",
                table: "Doktorlar",
                column: "TC");

            migrationBuilder.CreateIndex(
                name: "IX_Hastalar_TC",
                table: "Hastalar",
                column: "TC");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_AnadalID",
                table: "Randevular",
                column: "AnadalID");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_DoktorID",
                table: "Randevular",
                column: "DoktorID");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_HastaID",
                table: "Randevular",
                column: "HastaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adminler");

            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "Doktorlar");

            migrationBuilder.DropTable(
                name: "Hastalar");

            migrationBuilder.DropTable(
                name: "Anadallar");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
