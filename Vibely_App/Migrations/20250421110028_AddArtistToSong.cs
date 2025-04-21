using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vibely_App.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistToSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "artist",
                table: "songs",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "artist",
                table: "songs");
        }
    }
}
