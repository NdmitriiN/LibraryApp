namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int YearPublished { get; set; }
    public string? ISBN { get; set; }
    public string? Publisher { get; set; }
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
