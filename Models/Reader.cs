namespace LibraryApp.Models;

public class Reader
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
