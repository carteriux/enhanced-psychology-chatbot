-- SQL script to create test admin user
-- Run this in your MySQL database: CPC_Chatbot

-- First, add the Cohort column if it doesn't exist
ALTER TABLE Users ADD COLUMN IF NOT EXISTS Cohort VARCHAR(50) NULL;

-- Insert test admin user
INSERT INTO Users (
    Email, 
    FirstName, 
    LastName, 
    MiddleName, 
    EnrollmentNumber, 
    Password, 
    IsAdmin, 
    IsFirstTime, 
    Cohort
) VALUES (
    'admin@test.com',
    'Admin',
    'Test',
    'User',
    'ADMIN001',
    'admin123',  -- You might need to hash this password
    1,           -- IsAdmin = true
    0,           -- IsFirstTime = false
    '2024-A'     -- Test cohort
);

-- Also create a few test students with different cohorts
INSERT INTO Users (
    Email, 
    FirstName, 
    LastName, 
    MiddleName, 
    EnrollmentNumber, 
    Password, 
    IsAdmin, 
    IsFirstTime, 
    Cohort
) VALUES 
('student1@test.com', 'Juan', 'Pérez', 'Carlos', 'STU001', 'student123', 0, 1, '2024-A'),
('student2@test.com', 'María', 'González', 'Elena', 'STU002', 'student123', 0, 1, '2024-A'),
('student3@test.com', 'Pedro', 'López', 'Antonio', 'STU003', 'student123', 0, 1, '2024-B'),
('student4@test.com', 'Ana', 'Martínez', 'Sofia', 'STU004', 'student123', 0, 1, '2024-B');

-- Verify the data
SELECT IdUser, FirstName, LastName, EnrollmentNumber, Email, IsAdmin, Cohort 
FROM Users 
WHERE EnrollmentNumber IN ('ADMIN001', 'STU001', 'STU002', 'STU003', 'STU004');