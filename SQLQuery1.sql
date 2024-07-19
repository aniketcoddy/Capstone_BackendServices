-- Create CourseDB Database
CREATE DATABASE CourseDB;
GO

-- Use CourseDB Database
USE CourseDB;
GO

-- Create Courses Table
CREATE TABLE Courses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Status BIT NOT NULL
);
GO

-- Insert Sample Data into Courses Table
INSERT INTO Courses (Name, Description, StartDate, EndDate, Status)
VALUES 
('Introduction to Programming', 'Basic programming concepts and syntax.', '2024-08-01', '2024-12-31', 1),
('Data Structures and Algorithms', 'Advanced data structures and algorithms.', '2024-09-01', '2025-01-31', 1),
('Web Development', 'Full stack web development.', '2024-10-01', '2025-02-28', 1),
('Database Management Systems', 'Relational database management.', '2024-11-01', '2025-03-31', 1),
('Machine Learning', 'Introduction to machine learning and AI.', '2024-08-01', '2024-12-31', 0);
GO


-- Create BatchDB Database
CREATE DATABASE BatchDB;
GO

-- Use BatchDB Database
USE BatchDB;
GO

-- Create Batches Table
CREATE TABLE Batches (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Status BIT NOT NULL,
    CourseId INT NOT NULL
);
GO

-- Insert Sample Data into Batches Table
INSERT INTO Batches (Name, Description, StartDate, EndDate, Status, CourseId)
VALUES 
('Batch A', 'Morning batch for Introduction to Programming', '2024-08-01', '2024-08-31', 1, 1),
('Batch B', 'Evening batch for Data Structures and Algorithms', '2024-09-01', '2024-09-30', 1, 2),
('Batch C', 'Weekend batch for Web Development', '2024-10-01', '2024-10-31', 1, 3),
('Batch D', 'Morning batch for Database Management Systems', '2024-11-01', '2024-11-30', 1, 4),
('Batch E', 'Evening batch for Machine Learning', '2024-08-01', '2024-08-31', 0, 5);
GO



-- Create UserDB Database
CREATE DATABASE UserDB;
GO

-- Use UserDB Database
USE UserDB;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,
    Role INT,
    Status BIT,
    PasswordHash NVARCHAR(500)  -- Adjust size as per your hashing algorithm
);

GO

select * from Users;
GO

-- Use UserDB Database
USE UserDB;
GO

-- Insert data into Users table
SET IDENTITY_INSERT Users ON;  -- Allow manual insertion of values into the identity column

INSERT INTO [Users] ([Id], [Name], [Email], [Role], [Status], [PasswordHash]) 
VALUES (1, N'John Doe', N'john.doe@example.com', 1, 1, N'DRDMzQogC4rw09DLKDIWqA==:V9Hs+7hwUE/FpZ0k0pgfecs4cgVDyNjGyOunNstk7M0=');

INSERT INTO [Users] ([Id], [Name], [Email], [Role], [Status], [PasswordHash]) 
VALUES (2, N'Sathesh', N'sam@gmail.com', 2, 1, N'sg/U6P4ZOr3iZbE6pRbCKg==:GCtllhghkLEnrD9Ff/2tVsO0maI1trxPEjOLzFIEZyU=');

INSERT INTO [Users] ([Id], [Name], [Email], [Role], [Status], [PasswordHash]) 
VALUES (3, N'sam', N'sample@gmail.com', 3, 0, N'/QWJBTI+RfzikhHaxzCTYw==:22w/cmAoHR1sxBbgTGe4N0+SoFmilJPFnDzrQ0N/5nU=');

INSERT INTO [Users] ([Id], [Name], [Email], [Role], [Status], [PasswordHash]) 
VALUES (4, N'dinesh', N'dinesh@gmail.com', 3, 1, N'3QALZrBVC3FE/XloUjqyqw==:wlWODS8FqFuer2NavSLShsuMjpljlBVc46V/MnZyrOM=');

SET IDENTITY_INSERT Users OFF;  -- Disable manual insertion of values into the identity column
GO

select * from Users;
GO


-- Create the database
CREATE DATABASE EnrollmentDB;
GO

-- Switch to the new database
USE EnrollmentDB;
GO

-- Create the Enrollments table
CREATE TABLE Enrollments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CourseId INT NOT NULL,
    Status BIT NOT NULL
);
GO

ALTER TABLE Enrollments
ALTER COLUMN Status BIT NULL;

-- Verify the table creation
SELECT * FROM Enrollments;
GO


CREATE DATABASE FeedbackDB;
GO

USE FeedbackDB;
GO


CREATE TABLE Feedbacks (
    Id INT PRIMARY KEY IDENTITY,
    CourseId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(MAX),
);

select * from Feedbacks;