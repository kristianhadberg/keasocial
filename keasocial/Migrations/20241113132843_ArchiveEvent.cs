using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class ArchiveEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS `ArchivedLecture`;
            ");
            
            migrationBuilder.Sql(@"
                    CREATE TABLE `ArchivedLecture` (
                  `ArchivedLectureId` int NOT NULL AUTO_INCREMENT,
                  `LectureTitle` longtext NOT NULL,
                  `LectureDescription` longtext NOT NULL,
                  `LectureDate` datetime NOT NULL,
                  `LectureTime` time NOT NULL,
                  `LectureId` int NOT NULL,
                  PRIMARY KEY (`ArchivedLectureId`)
                ) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci
            ");

            migrationBuilder.Sql(@"
                DROP EVENT IF EXISTS delete_old_lectures;

                CREATE EVENT delete_old_lectures
                ON schedule
	                EVERY 1 YEAR
                    STARTS '2025-01-01'
                DO BEGIN
	                INSERT INTO ArchivedLecture (LectureId, LectureTitle, LectureDescription, LectureDate, LectureTime)
                    SELECT LectureId, LectureTitle, LectureDescription, LectureDate, LectureTime
                    FROM Lectures
                    WHERE LectureDate < NOW() - INTERVAL 1 YEAR;
                    
                    DELETE FROM Lectures
	                WHERE LectureDate < NOW() - INTERVAL 1 YEAR;

                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
