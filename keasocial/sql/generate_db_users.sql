-- ADMIN USER
CREATE USER 'superadmin'@'%' IDENTIFIED BY 'admin_password';

GRANT ALL PRIVILEGES ON *.* TO 'superadmin'@'%' WITH GRANT OPTION;

-- APPLICATION USER
CREATE USER 'application_user'@'%' IDENTIFIED BY 'application_password';

GRANT SELECT, INSERT, UPDATE, DELETE ON ArchivedLecture TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON CalendarLecture TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON Calendars TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON CommentLikes TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON Comments TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON __EFMigrationsHistory TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON Lectures TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON PostLikes TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON Posts TO 'application_user'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE ON Users TO 'application_user'@'%';

-- READ ONLY USER
CREATE USER 'readonly'@'%' IDENTIFIED BY 'readonly_password';

GRANT SELECT ON ArchivedLecture TO 'readonly'@'%';
GRANT SELECT ON CalendarLecture TO 'readonly'@'%';
GRANT SELECT ON Calendars TO 'readonly'@'%';
GRANT SELECT ON CommentLikes TO 'readonly'@'%';
GRANT SELECT ON Comments TO 'readonly'@'%';
GRANT SELECT ON __EFMigrationsHistory TO 'readonly'@'%';
GRANT SELECT ON Lectures TO 'readonly'@'%';
GRANT SELECT ON PostLikes TO 'readonly'@'%';
GRANT SELECT ON Posts TO 'readonly'@'%';
GRANT SELECT ON Users TO 'readonly'@'%';

-- RESTRICTED USER
CREATE USER 'restricted'@'%' IDENTIFIED BY 'restricted_password';

GRANT SELECT ON CommentLikes TO 'restricted'@'%';
GRANT SELECT ON Comments TO 'restricted'@'%';
GRANT SELECT ON Lectures TO 'restricted'@'%';
GRANT SELECT ON PostLikes TO 'restricted'@'%';
GRANT SELECT ON Posts TO 'restricted'@'%';

FLUSH PRIVILEGES;