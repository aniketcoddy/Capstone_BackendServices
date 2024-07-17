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

