
SET SQL_SAFE_UPDATES = 0;

CREATE TABLE IF NOT EXISTS `Users` (
                                       `UserId` INT AUTO_INCREMENT PRIMARY KEY,
                                       `Name` VARCHAR(255) NOT NULL,
    `Email` VARCHAR(255) NOT NULL UNIQUE,
    `Password` VARCHAR(255) NOT NULL
    );

INSERT INTO `Users` (`UserId`, `Name`, `Email`, `Password`) VALUES
                                                                (1, 'John Doe', 'john@example.com', 'password123'),
                                                                (2, 'Jane Smith', 'jane@example.com', 'password123'),
                                                                (3, 'Alice Brown', 'alice@example.com', 'password123'),
                                                                (4, 'Bob Johnson', 'bob@example.com', 'password123');


CREATE TABLE IF NOT EXISTS `Calendars` (
                                           `CalendarId` INT AUTO_INCREMENT PRIMARY KEY,
                                           `IsPublic` BOOLEAN NOT NULL,
                                           `UserId` INT NOT NULL,
                                           FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    );

INSERT INTO `Calendars` (`IsPublic`, `UserId`) VALUES
                                                   (TRUE, 1),
                                                   (FALSE, 2),
                                                   (TRUE, 3),
                                                   (TRUE, 4);


CREATE TABLE IF NOT EXISTS `Lectures` (
                                          `LectureId` INT AUTO_INCREMENT PRIMARY KEY,
                                          `LectureTitle` VARCHAR(255) NOT NULL,
    `LectureDescription` TEXT,
    `LectureDate` DATE NOT NULL,
    `LectureTime` TIME NOT NULL
    );

INSERT INTO `Lectures` (`LectureTitle`, `LectureDescription`, `LectureDate`, `LectureTime`) VALUES
                                                                                                ('Introduction to C#', 'A beginner-friendly introduction to C# and .NET.', '2024-09-21', '10:00:00'),
                                                                                                ('Advanced Database Design', 'Deep dive into database design principles.', '2024-09-22', '14:00:00'),
                                                                                                ('Game Development 101', 'Learn the basics of game development with Unity.', '2024-09-23', '09:00:00'),
                                                                                                ('Outdoor Survival Skills', 'Learn the essential skills for outdoor survival.', '2024-09-24', '11:00:00');


CREATE TABLE IF NOT EXISTS `CalendarLecture` (
                                                 `CalendarsCalendarId` INT NOT NULL,
                                                 `LecturesLectureId` INT NOT NULL,
                                                 PRIMARY KEY (`CalendarsCalendarId`, `LecturesLectureId`),
    FOREIGN KEY (`CalendarsCalendarId`) REFERENCES `Calendars` (`CalendarId`),
    FOREIGN KEY (`LecturesLectureId`) REFERENCES `Lectures` (`LectureId`)
    );

INSERT INTO `CalendarLecture` (`CalendarsCalendarId`, `LecturesLectureId`) VALUES
                                                                               (1, 1),
                                                                               (1, 2),
                                                                               (2, 3),
                                                                               (3, 4),
                                                                               (4, 1);


CREATE TABLE IF NOT EXISTS `Posts` (
                                       `PostId` INT AUTO_INCREMENT PRIMARY KEY,
                                       `Content` TEXT NOT NULL,
                                       `CreatedAt` TIMESTAMP NOT NULL,
                                       `LikeCount` INT NOT NULL,
                                       `UserId` INT NOT NULL,
                                       FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    );

INSERT INTO `Posts` (`Content`, `CreatedAt`, `LikeCount`, `UserId`) VALUES
                                                                        ('Hello, World! This is my first post.', CURRENT_TIMESTAMP, 0, 1),
                                                                        ('Just finished a great book on software architecture!', CURRENT_TIMESTAMP, 0, 2),
                                                                        ('Anyone up for a hike this weekend?', CURRENT_TIMESTAMP, 0, 3),
                                                                        ('Learning how to make video games is awesome!', CURRENT_TIMESTAMP, 0, 4);


CREATE TABLE IF NOT EXISTS `Comments` (
                                          `CommentId` INT AUTO_INCREMENT PRIMARY KEY,
                                          `Content` TEXT NOT NULL,
                                          `CreatedAt` TIMESTAMP NOT NULL,
                                          `LikeCount` INT NOT NULL,
                                          `PostId` INT NOT NULL,
                                          `UserId` INT NOT NULL,
                                          FOREIGN KEY (`PostId`) REFERENCES `Posts` (`PostId`),
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    );

INSERT INTO `Comments` (`Content`, `CreatedAt`, `LikeCount`, `PostId`, `UserId`) VALUES
                                                                                     ('Welcome to the platform, John!', CURRENT_TIMESTAMP, 0, 1, 2),
                                                                                     ('What book did you read?', CURRENT_TIMESTAMP, 0, 2, 3),
                                                                                     ('Count me in for the hike!', CURRENT_TIMESTAMP, 0, 3, 4),
                                                                                     ('Can you share some game development resources?', CURRENT_TIMESTAMP, 0, 4, 1);


CREATE TABLE IF NOT EXISTS `PostLikes` (
                                           `PostId` INT NOT NULL,
                                           `UserId` INT NOT NULL,
                                           PRIMARY KEY (`PostId`, `UserId`),
    FOREIGN KEY (`PostId`) REFERENCES `Posts` (`PostId`),
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    );

INSERT INTO `PostLikes` (`PostId`, `UserId`) VALUES
                                                 (1, 2),
                                                 (2, 3),
                                                 (3, 4);

-- Updating the LikeCount for Posts
UPDATE `Posts`
SET `LikeCount` = (SELECT COUNT(*) FROM `PostLikes` WHERE `PostLikes`.`PostId` = `Posts`.`PostId`);


CREATE TABLE IF NOT EXISTS `CommentLikes` (
                                              `CommentId` INT NOT NULL,
                                              `UserId` INT NOT NULL,
                                              PRIMARY KEY (`CommentId`, `UserId`),
    FOREIGN KEY (`CommentId`) REFERENCES `Comments` (`CommentId`),
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    );

INSERT INTO `CommentLikes` (`CommentId`, `UserId`) VALUES
                                                       (1, 1),
                                                       (2, 2),
                                                       (3, 3),
                                                       (4, 4);

-- Updating the LikeCount for Comments
UPDATE `Comments`
SET `LikeCount` = (SELECT COUNT(*) FROM `CommentLikes` WHERE `CommentLikes`.`CommentId` = `Comments`.`CommentId`);

SET SQL_SAFE_UPDATES = 1;