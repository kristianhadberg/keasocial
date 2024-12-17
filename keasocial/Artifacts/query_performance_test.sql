-- Enable profiling
SET profiling = 1;

-- Query 1: Analyze comments for a specific post
SELECT * FROM Comments WHERE PostId = 1;

-- Query 2: Analyze comments for another post
SELECT * FROM Comments WHERE PostId = 2;

-- Query 3: Analyze calendars for a specific user
SELECT * FROM Calendars WHERE UserId = 3;

-- Query 4: Analyze lectures (assuming the column name was missed in your example)
SELECT * FROM Lectures WHERE LectureId = 1;

-- Show all executed queries with their query IDs
SHOW PROFILES;

-- View detailed profile for each query (replace X with the query ID)
SHOW PROFILE FOR QUERY 1;
SHOW PROFILE FOR QUERY 2;
SHOW PROFILE FOR QUERY 3;
SHOW PROFILE FOR QUERY 4;
