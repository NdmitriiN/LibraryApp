using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace LibraryApp.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Локальная база SQL Server LocalDB — не требует установки сервера
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связей
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Reader)
            .WithMany(r => r.Loans)
            .HasForeignKey(l => l.ReaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Пример данных при первом запуске (Seed)
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Война и мир", Author = "Лев Толстой", YearPublished = 1869, ISBN = "978-5-17-123456-1", Publisher = "АСТ", TotalCopies = 5, AvailableCopies = 5 },
            new Book { Id = 2, Title = "Преступление и наказание", Author = "Фёдор Достоевский", YearPublished = 1866, ISBN = "978-5-17-123456-2", Publisher = "Эксмо", TotalCopies = 3, AvailableCopies = 3 },
            new Book { Id = 3, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", YearPublished = 1967, ISBN = "978-5-17-123456-3", Publisher = "АСТ", TotalCopies = 4, AvailableCopies = 4 },
            new Book { Id = 4, Title = "1984", Author = "Джордж Оруэлл", YearPublished = 1949, ISBN = "978-5-17-123456-4", Publisher = "Питер", TotalCopies = 2, AvailableCopies = 2 }
        );

        modelBuilder.Entity<Reader>().HasData(
            new Reader { Id = 1, FullName = "Иванов Иван Иванович", Phone = "+7 (999) 123-45-67", Email = "ivanov@example.com", RegistrationDate = new DateTime(2025, 1, 15) },
            new Reader { Id = 2, FullName = "Петрова Анна Сергеевна", Phone = "+7 (999) 765-43-21", Email = "petrova@example.com", RegistrationDate = new DateTime(2025, 3, 20) }
        );
    }
}
