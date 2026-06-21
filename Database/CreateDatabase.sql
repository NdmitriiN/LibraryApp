-- =============================================
-- Скрипт создания базы данных для курсовой
-- "Учёт выданных книг в библиотеке"
-- =============================================

-- Создать базу 
-- CREATE DATABASE LibraryDb;
-- GO
-- USE LibraryDb;
-- GO

-- Книги
CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    YearPublished INT NOT NULL,
    ISBN NVARCHAR(20),
    Publisher NVARCHAR(100),
    TotalCopies INT NOT NULL DEFAULT 1,
    AvailableCopies INT NOT NULL DEFAULT 1
);

-- Читатели
CREATE TABLE Readers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    RegistrationDate DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Выдачи
CREATE TABLE Loans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BookId INT NOT NULL,
    ReaderId INT NOT NULL,
    IssueDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2 NULL,

    CONSTRAINT FK_Loans_Books FOREIGN KEY (BookId) REFERENCES Books(Id),
    CONSTRAINT FK_Loans_Readers FOREIGN KEY (ReaderId) REFERENCES Readers(Id)
);

-- Пример данных
INSERT INTO Books (Title, Author, YearPublished, ISBN, Publisher, TotalCopies, AvailableCopies)
VALUES 
('Война и мир', 'Лев Толстой', 1869, '978-5-17-123456-1', 'АСТ', 5, 5),
('Преступление и наказание', 'Фёдор Достоевский', 1866, '978-5-17-123456-2', 'Эксмо', 3, 3),
('Мастер и Маргарита', 'Михаил Булгаков', 1967, '978-5-17-123456-3', 'АСТ', 4, 4);

INSERT INTO Readers (FullName, Phone, Email, RegistrationDate)
VALUES 
('Иванов Иван Иванович', '+7 (999) 123-45-67', 'ivanov@example.com', '2025-01-15'),
('Петрова Анна Сергеевна', '+7 (999) 765-43-21', 'petrova@example.com', '2025-03-20');

-- Пример выдачи (закомментировано)
-- INSERT INTO Loans (BookId, ReaderId, IssueDate, DueDate)
-- VALUES (1, 1, '2025-06-01', '2025-06-15');
