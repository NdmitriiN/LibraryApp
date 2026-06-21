using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Forms;

public partial class IssueForm : Form
{
    private ComboBox cmbBooks;
    private ComboBox cmbReaders;
    private DateTimePicker dtpIssue;
    private DateTimePicker dtpDue;
    private Button btnIssue, btnCancel;
    private Label lblInfo;

    public IssueForm()
    {
        InitializeComponent();
        Text = "Выдача книги";
        LoadData();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        Controls.Add(new Label { Text = "Книга:", Location = new Point(20, 25) });
        cmbBooks = new ComboBox { Location = new Point(120, 22), Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(cmbBooks);

        Controls.Add(new Label { Text = "Читатель:", Location = new Point(20, 60) });
        cmbReaders = new ComboBox { Location = new Point(120, 57), Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(cmbReaders);

        Controls.Add(new Label { Text = "Дата выдачи:", Location = new Point(20, 100) });
        dtpIssue = new DateTimePicker { Location = new Point(120, 97), Width = 150, Value = DateTime.Today };
        Controls.Add(dtpIssue);

        Controls.Add(new Label { Text = "Вернуть до:", Location = new Point(20, 135) });
        dtpDue = new DateTimePicker { Location = new Point(120, 132), Width = 150, Value = DateTime.Today.AddDays(14) };
        Controls.Add(dtpDue);

        lblInfo = new Label { Location = new Point(20, 175), AutoSize = true, ForeColor = Color.DarkGreen };
        Controls.Add(lblInfo);

        btnIssue = new Button { Text = "Выдать книгу", Location = new Point(120, 220), Size = new Size(140, 40), BackColor = Color.FromArgb(76, 175, 80), ForeColor = Color.White };
        btnIssue.Click += btnIssue_Click;

        btnCancel = new Button { Text = "Отмена", Location = new Point(280, 220), Size = new Size(140, 40), DialogResult = DialogResult.Cancel };

        Controls.Add(btnIssue);
        Controls.Add(btnCancel);

        ClientSize = new Size(550, 290);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout();
    }

    private void LoadData()
    {
        using var db = new LibraryDbContext();

        // Только книги с доступными экземплярами
        var books = db.Books
            .Where(b => b.AvailableCopies > 0)
            .OrderBy(b => b.Title)
            .ToList();

        cmbBooks.DataSource = books;
        cmbBooks.DisplayMember = "Title";
        cmbBooks.ValueMember = "Id";

        var readers = db.Readers.OrderBy(r => r.FullName).ToList();
        cmbReaders.DataSource = readers;
        cmbReaders.DisplayMember = "FullName";
        cmbReaders.ValueMember = "Id";

        cmbBooks.SelectedIndexChanged += (s, e) => UpdateInfo();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (cmbBooks.SelectedItem is Book book)
        {
            lblInfo.Text = $"Доступно экземпляров: {book.AvailableCopies}";
        }
    }

    private void btnIssue_Click(object? sender, EventArgs e)
    {
        if (cmbBooks.SelectedItem is not Book book || cmbReaders.SelectedItem is not Reader reader)
        {
            MessageBox.Show("Выберите книгу и читателя.");
            return;
        }

        using var db = new LibraryDbContext();

        // Перечитываем книгу (чтобы взять актуальное количество)
        var dbBook = db.Books.First(b => b.Id == book.Id);
        if (dbBook.AvailableCopies <= 0)
        {
            MessageBox.Show("Нет свободных экземпляров этой книги.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var loan = new Loan
        {
            BookId = dbBook.Id,
            ReaderId = reader.Id,
            IssueDate = dtpIssue.Value.Date,
            DueDate = dtpDue.Value.Date
        };

        dbBook.AvailableCopies--;
        db.Loans.Add(loan);
        db.SaveChanges();

        MessageBox.Show($"Книга \"{dbBook.Title}\" выдана читателю {reader.FullName}.\nСрок возврата: {loan.DueDate:d}", 
            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }
}
