using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Forms;

public partial class BooksForm : Form
{
    private DataGridView dgvBooks;
    private TextBox txtSearch;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnRefresh;
    private Button btnClose;

    public BooksForm()
    {
        InitializeComponent();
        Text = "Справочник книг";
        LoadBooks();
    }

    private void InitializeComponent()
    {
        dgvBooks = new DataGridView();
        txtSearch = new TextBox();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();

        SuspendLayout();

        // DataGridView
        dgvBooks.Dock = DockStyle.Top;
        dgvBooks.Location = new Point(0, 40);
        dgvBooks.Size = new Size(900, 380);
        dgvBooks.AllowUserToAddRows = false;
        dgvBooks.AllowUserToDeleteRows = false;
        dgvBooks.ReadOnly = true;
        dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBooks.Columns.AddRange(
            new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 40, Visible = false },
            new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "Название", Width = 280 },
            new DataGridViewTextBoxColumn { Name = "Author", HeaderText = "Автор", Width = 160 },
            new DataGridViewTextBoxColumn { Name = "Year", HeaderText = "Год", Width = 60 },
            new DataGridViewTextBoxColumn { Name = "ISBN", HeaderText = "ISBN", Width = 130 },
            new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Всего", Width = 60 },
            new DataGridViewTextBoxColumn { Name = "Available", HeaderText = "Доступно", Width = 80 }
        );

        // Search
        txtSearch.Location = new Point(12, 12);
        txtSearch.Size = new Size(300, 23);
        txtSearch.PlaceholderText = "Поиск по названию или автору...";
        txtSearch.TextChanged += (s, e) => LoadBooks();

        // Buttons
        btnAdd.Text = "Добавить";
        btnAdd.Location = new Point(340, 10);
        btnAdd.Click += btnAdd_Click;

        btnEdit.Text = "Редактировать";
        btnEdit.Location = new Point(430, 10);
        btnEdit.Click += btnEdit_Click;

        btnDelete.Text = "Удалить";
        btnDelete.Location = new Point(540, 10);
        btnDelete.Click += btnDelete_Click;

        btnRefresh.Text = "Обновить";
        btnRefresh.Location = new Point(630, 10);
        btnRefresh.Click += (s, e) => LoadBooks();

        btnClose.Text = "Закрыть";
        btnClose.Location = new Point(720, 10);
        btnClose.Click += (s, e) => Close();

        Controls.AddRange(new Control[] { dgvBooks, txtSearch, btnAdd, btnEdit, btnDelete, btnRefresh, btnClose });
        ClientSize = new Size(900, 480);
        ResumeLayout();
    }

    private void LoadBooks()
    {
        using var db = new LibraryDbContext();
        var query = db.Books.AsQueryable();

        string search = txtSearch.Text.Trim().ToLower();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Title.ToLower().Contains(search) ||
                                     b.Author.ToLower().Contains(search));
        }

        var list = query
            .OrderBy(b => b.Title)
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Author,
                Year = b.YearPublished,
                b.ISBN,
                Total = b.TotalCopies,
                Available = b.AvailableCopies
            })
            .ToList();

        dgvBooks.Rows.Clear();
        foreach (var item in list)
        {
            dgvBooks.Rows.Add(item.Id, item.Title, item.Author, item.Year, item.ISBN, item.Total, item.Available);
        }
    }

    private int? GetSelectedBookId()
    {
        if (dgvBooks.SelectedRows.Count == 0) return null;
        return Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["Id"].Value);
    }

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        using var db = new LibraryDbContext();
        var editForm = new BookEditForm();
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            var newBook = editForm.Book;
            db.Books.Add(newBook);
            db.SaveChanges();
            LoadBooks();
        }
    }

    private void btnEdit_Click(object? sender, EventArgs e)
    {
        var id = GetSelectedBookId();
        if (id == null) return;

        using var db = new LibraryDbContext();
        var book = db.Books.Find(id.Value);
        if (book == null) return;

        var editForm = new BookEditForm(book);
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            db.SaveChanges();
            LoadBooks();
        }
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        var id = GetSelectedBookId();
        if (id == null) return;

        if (MessageBox.Show("Удалить выбранную книгу?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        using var db = new LibraryDbContext();
        var book = db.Books.Include(b => b.Loans).FirstOrDefault(b => b.Id == id);
        if (book == null) return;

        // Проверяем, есть ли активные выдачи
        if (book.Loans.Any(l => l.ReturnDate == null))
        {
            MessageBox.Show("Нельзя удалить книгу, по которой есть активные выдачи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        db.Books.Remove(book);
        db.SaveChanges();
        LoadBooks();
    }
}
