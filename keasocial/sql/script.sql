

CREATE TABLE IF NOT EXISTS "Users" (
    "UserId" serial primary key,
    "Name" varchar(255) not null,
    "Email" varchar(255) not null unique,
    "Password" varchar(255) not null
);

INSERT INTO "Users"
("UserId", "Name", "Email", "Password")
VALUES('1', 'John Doe', 'john@example.com', 'password123'),
      ('2', 'Jane Smith', 'jane@example.com', 'password123'),
      ('3', 'Alice Brown', 'alice@example.com', 'password123'),
      ('4', 'Bob Johnson', 'bob@example.com', 'password123');


CREATE TABLE IF NOT EXISTS "Calendars" (
    "CalendarId" serial primary key,
    "IsPublic" boolean not null,
    "UserId" integer not null,
    foreign key ("UserId") references "Users" ("UserId")
);


INSERT INTO "Calendars" ("IsPublic", "UserId") VALUES
                                             (true, 1),  
                                             (false, 2), 
                                             (true, 3), 
                                             (true, 4); 
    
    
CREATE TABLE IF NOT EXISTS "Lectures" (
    "LectureId" serial primary key,
    "LectureTitle" varchar(255) not null,
    "LectureDescription" text,
    "LectureDate" date not null,
    "LectureTime" time not null
);   


INSERT INTO "Lectures" ("LectureTitle", "LectureDescription", "LectureDate", "LectureTime") VALUES
                                                                                      ('Introduction to C#', 'A beginner-friendly introduction to C# and .NET.', '2024-09-21', '10:00:00'),
                                                                                      ('Advanced Database Design', 'Deep dive into database design principles.', '2024-09-22', '14:00:00'),
                                                                                      ('Game Development 101', 'Learn the basics of game development with Unity.', '2024-09-23', '09:00:00'),
                                                                                      ('Outdoor Survival Skills', 'Learn the essential skills for outdoor survival.', '2024-09-24', '11:00:00');

CREATE TABLE IF NOT EXISTS "CalendarLecture" (
    "CalendarsCalendarId" integer not null,
    "LecturesLectureId" integer not null,
    primary key ("CalendarsCalendarId", "LecturesLectureId"),
    foreign key ("CalendarsCalendarId") references "Calendars" ("CalendarId"),
    foreign key ("LecturesLectureId") references "Lectures" ("LectureId")
);

INSERT INTO "CalendarLecture" ("CalendarsCalendarId", "LecturesLectureId") VALUES
                                                                           (1, 1), 
                                                                           (1, 2), 
                                                                           (2, 3), 
                                                                           (3, 4), 
                                                                           (4, 1);

CREATE TABLE IF NOT EXISTS  "Posts" (
    "PostId" serial primary key,
    "Content" text not null,
    "CreatedAt" timestamp not null,
    "LikeCount" integer not null,
    "UserId" integer not null,
    foreign key ("UserId") references "Users" ("UserId")
);

INSERT INTO "Posts" ("Content", "CreatedAt", "LikeCount", "UserId") VALUES
                                                              ('Hello, World! This is my first post.', CURRENT_TIMESTAMP, 0, 1), 
                                                              ('Just finished a great book on software architecture!', CURRENT_TIMESTAMP, 0, 2),
                                                              ('Anyone up for a hike this weekend?', CURRENT_TIMESTAMP, 0, 3),
                                                              ('Learning how to make video games is awesome!', CURRENT_TIMESTAMP, 0, 4);

CREATE TABLE IF NOT EXISTS "Comments" (
    "CommentId" serial primary key,
    "Content" text not null,
    "CreatedAt" timestamp not null,
    "LikeCount" integer not null,
    "PostId" integer not null,
    "UserId" integer not null,
    foreign key ("PostId") references "Posts" ("PostId"),
    foreign key ("UserId") references "Users" ("UserId")
);

INSERT INTO "Comments" ("Content", "CreatedAt", "LikeCount", "PostId", "UserId") VALUES
                                                                         ('Welcome to the platform, John!', CURRENT_TIMESTAMP, 0, 1, 2),
                                                                         ('What book did you read?', CURRENT_TIMESTAMP, 0, 2, 3),
                                                                         ('Count me in for the hike!', CURRENT_TIMESTAMP, 0, 3, 4),
                                                                         ('Can you share some game development resources?', CURRENT_TIMESTAMP, 0, 4, 1);

CREATE TABLE IF NOT EXISTS "PostLikes" (
    "PostId" integer not null,
    "UserId" integer not null,
    primary key ("PostId", "UserId"),
    foreign key ("PostId") references "Posts" ("PostId"),
    foreign key ("UserId") references "Users" ("UserId")
);

INSERT INTO "PostLikes" ("PostId", "UserId") VALUES
                                           (1, 2), 
                                           (2, 3), 
                                           (3, 4); 


UPDATE "Posts" SET "LikeCount" = (SELECT COUNT(*) FROM "PostLikes" WHERE "PostLikes"."PostId" = "Posts"."PostId");


CREATE TABLE IF NOT EXISTS "CommentLikes" (
    "CommentId" integer not null,
    "UserId" integer not null,
    primary key ("CommentId", "UserId"),
    foreign key ("CommentId") references "Comments" ("CommentId"),
    foreign key ("UserId") references "Users" ("UserId")
);

INSERT INTO "CommentLikes" ("CommentId", "UserId") VALUES
                                                 (1, 1),
                                                 (2, 2),
                                                 (3, 3),
                                                 (4, 4);
    

UPDATE "Comments" SET "LikeCount" = (SELECT COUNT(*) FROM "CommentLikes" WHERE "CommentLikes"."CommentId" = "Comments"."CommentId");
