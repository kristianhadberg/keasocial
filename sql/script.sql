-- Inserting Users


INSERT INTO "Users"
("UserId", "Name", "Email", "Password")
VALUES('1', 'John Doe', 'john@example.com', 'password123'),
      ('2', 'Jane Smith', 'jane@example.com', 'password123'),
      ('3', 'Alice Brown', 'alice@example.com', 'password123'),
      ('4', 'Bob Johnson', 'bob@example.com', 'password123');


-- Inserting Calendars (assuming 1 calendar per user)
INSERT INTO "Calendars" ("IsPublic", "UserId") VALUES
                                             (true, 1),  -- John Doe's calendar (public)
                                             (false, 2), -- Jane Smith's calendar (private)
                                             (true, 3),  -- Alice Brown's calendar (public)
                                             (true, 4);  -- Bob Johnson's calendar (public)

-- Inserting Lectures
INSERT INTO "Lectures" ("LectureTitle", "LectureDescription", "LectureDate", "LectureTime") VALUES
                                                                                      ('Introduction to C#', 'A beginner-friendly introduction to C# and .NET.', '2024-09-21', '10:00:00'),
                                                                                      ('Advanced Database Design', 'Deep dive into database design principles.', '2024-09-22', '14:00:00'),
                                                                                      ('Game Development 101', 'Learn the basics of game development with Unity.', '2024-09-23', '09:00:00'),
                                                                                      ('Outdoor Survival Skills', 'Learn the essential skills for outdoor survival.', '2024-09-24', '11:00:00');

-- Associating Lectures with Calendars
INSERT INTO "CalendarLecture" ("CalendarsCalendarId", "LecturesLectureId") VALUES
                                                                           (1, 1), -- John Doe has 'Introduction to C#' in his calendar
                                                                           (1, 2), -- John Doe has 'Advanced Database Design' in his calendar
                                                                           (2, 3), -- Jane Smith has 'Game Development 101' in her calendar
                                                                           (3, 4), -- Alice Brown has 'Outdoor Survival Skills' in her calendar
                                                                           (4, 1); -- Bob Johnson has 'Introduction to C#' in his calendar

-- Inserting Posts
INSERT INTO "Posts" ("Content", "CreatedAt", "LikeCount", "UserId") VALUES
                                                              ('Hello, World! This is my first post.', CURRENT_TIMESTAMP, 0, 1), -- John Doe's post
                                                              ('Just finished a great book on software architecture!', CURRENT_TIMESTAMP, 0, 2), -- Jane Smith's post
                                                              ('Anyone up for a hike this weekend?', CURRENT_TIMESTAMP, 0, 3), -- Alice Brown's post
                                                              ('Learning how to make video games is awesome!', CURRENT_TIMESTAMP, 0, 4); -- Bob Johnson's post

-- Inserting Comments on Posts
INSERT INTO "Comments" ("Content", "CreatedAt", "LikeCount", "PostId", "UserId") VALUES
                                                                         ('Welcome to the platform, John!', CURRENT_TIMESTAMP, 0, 1, 2), -- Jane commenting on John's post
                                                                         ('What book did you read?', CURRENT_TIMESTAMP, 0, 2, 3), -- Alice commenting on Jane's post
                                                                         ('Count me in for the hike!', CURRENT_TIMESTAMP, 0, 3, 4), -- Bob commenting on Alice's post
                                                                         ('Can you share some game development resources?', CURRENT_TIMESTAMP, 0, 4, 1); -- John commenting on Bob's post

-- Inserting Post Likes
INSERT INTO "PostLikes" ("PostId", "UserId") VALUES
                                           (1, 2), -- Jane likes John's post
                                           (2, 3), -- Alice likes Jane's post
                                           (3, 4), -- Bob likes Alice's post
                                           (4, 1); -- John likes Bob's post

-- Updating LikeCount for Posts
UPDATE "Posts" SET "LikeCount" = (SELECT COUNT(*) FROM "PostLikes" WHERE "PostLikes"."PostId" = "Posts"."PostId");

-- Inserting Comment Likes
INSERT INTO "CommentLikes" ("CommentId", "UserId") VALUES
                                                 (1, 1), -- John likes Jane's comment on his post
                                                 (2, 2), -- Jane likes Alice's comment on her post
                                                 (3, 3), -- Alice likes Bob's comment on her post
                                                 (4, 4); -- Bob likes John's comment on his post

-- Updating LikeCount for Comments
UPDATE "Comments" SET "LikeCount" = (SELECT COUNT(*) FROM "CommentLikes" WHERE "CommentLikes"."CommentId" = "Comments"."CommentId");
