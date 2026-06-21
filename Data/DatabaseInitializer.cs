using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        using var context = new LibraryDbContext();

        // Создаём базу если её нет
        context.Database.EnsureCreated();

        // Если книг ещё нет — добавляем тестовые данные
        if (!context.Books.Any())
        {
            var books = new[]
            {
                new Book { Title = "Война и мир", Author = "Лев Толстой", YearPublished = 1869, ISBN = "978-5-17-123456-1", Publisher = "АСТ", TotalCopies = 5, AvailableCopies = 5 },
                new Book { Title = "Преступление и наказание", Author = "Фёдор Достоевский", YearPublished = 1866, ISBN = "978-5-17-123456-2", Publisher = "Эксмо", TotalCopies = 3, AvailableCopies = 3 },
                new Book { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", YearPublished = 1967, ISBN = "978-5-17-123456-3", Publisher = "АСТ", TotalCopies = 4, AvailableCopies = 4 },
                new Book { Title = "1984", Author = "Джордж Оруэлл", YearPublished = 1949, ISBN = "978-5-17-123456-4", Publisher = "Питер", TotalCopies = 2, AvailableCopies = 2 },
                new Book { Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", YearPublished = 1997, ISBN = "978-5-17-987654-1", Publisher = "Росмэн", TotalCopies = 6, AvailableCopies = 6 }
            };

            context.Books.AddRange(books);
        }

        if (!context.Readers.Any())
        {
            var readers = new[]
            {
                new Reader { FullName = "Иванов Иван Иванович", Phone = "+7 (999) 123-45-67", Email = "ivanov@example.com", RegistrationDate = DateTime.Now.AddMonths(-5) },
                new Reader { FullName = "Петрова Анна Сергеевна", Phone = "+7 (999) 765-43-21", Email = "petrova@example.com", RegistrationDate = DateTime.Now.AddMonths(-3) },
                new Reader { FullName = "Сидоров Алексей Петрович", Phone = "+7 (912) 555-12-34", Email = "sidorov@example.com", RegistrationDate = DateTime.Now.AddMonths(-1) }
            };

            context.Readers.AddRange(readers);
        }

        context.SaveChanges();
    }
}
