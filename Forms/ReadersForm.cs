using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Forms;

public partial class ReadersForm : Form
{
    private DataGridView dgv;
    private TextBox txtSearch;
    private Button btnAdd, btnEdit, btnDelete, btnClose;

    public ReadersForm()
    {
        InitializeComponent();
        Text = "Справочник читателей";
        LoadReaders();
    }

    private void InitializeComponent()
    {
        dgv = new DataGridView();
        txtSearch = new TextBox();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnClose = new Button();

        SuspendLayout();

        dgv.Location = new Point(0, 45);
        dgv.Size = new Size(850, 360);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.Columns.AddRange(
            new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { Name = "FullName", HeaderText = "ФИО", Width = 280 },
            new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Телефон", Width = 140 },
            new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", Width = 180 },
            new DataGridViewTextBoxColumn { Name = "RegDate", HeaderText = "Дата регистрации", Width = 130 }
        );

        txtSearch.Location = new Point(12, 12);
        txtSearch.Size = new Size(280, 23);
        txtSearch.PlaceholderText = "Поиск по ФИО...";
        txtSearch.TextChanged += (s, e) => LoadReaders();

        btnAdd.Text = "Добавить"; btnAdd.Location = new Point(320, 10); btnAdd.Click += btnAdd_Click;
        btnEdit.Text = "Редактировать"; btnEdit.Location = new Point(410, 10); btnEdit.Click += btnEdit_Click;
        btnDelete.Text = "Удалить"; btnDelete.Location = new Point(520, 10); btnDelete.Click += btnDelete_Click;
        btnClose.Text = "Закрыть"; btnClose.Location = new Point(620, 10); btnClose.Click += (s, e) => Close();

        Controls.AddRange(new Control[] { dgv, txtSearch, btnAdd, btnEdit, btnDelete, btnClose });
        ClientSize = new Size(850, 430);
        ResumeLayout();
    }

    private void LoadReaders()
    {
        using var db = new LibraryDbContext();
        var q = db.Readers.AsQueryable();
        string s = txtSearch.Text.Trim().ToLower();
        if (!string.IsNullOrEmpty(s))
            q = q.Where(r => r.FullName.ToLower().Contains(s));

        dgv.Rows.Clear();
        foreach (var r in q.OrderBy(r => r.FullName))
        {
            dgv.Rows.Add(r.Id, r.FullName, r.Phone, r.Email, r.RegistrationDate.ToShortDateString());
        }
    }

    private int? GetSelectedId()
    {
        if (dgv.SelectedRows.Count == 0) return null;
        return Convert.ToInt32(dgv.SelectedRows[0].Cells[0].Value);
    }

    private void btnAdd_Click(object? s, EventArgs e)
    {
        using var db = new LibraryDbContext();
        var f = new ReaderEditForm();
        if (f.ShowDialog() == DialogResult.OK)
        {
            db.Readers.Add(f.Reader);
            db.SaveChanges();
            LoadReaders();
        }
    }

    private void btnEdit_Click(object? s, EventArgs e)
    {
        int? id = GetSelectedId();
        if (id == null) return;
        using var db = new LibraryDbContext();
        var r = db.Readers.Find(id.Value);
        if (r == null) return;
        var f = new ReaderEditForm(r);
        if (f.ShowDialog() == DialogResult.OK)
        {
            db.SaveChanges();
            LoadReaders();
        }
    }

    private void btnDelete_Click(object? s, EventArgs e)
    {
        int? id = GetSelectedId();
        if (id == null) return;
        if (MessageBox.Show("Удалить читателя?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        using var db = new LibraryDbContext();
        var r = db.Readers.Find(id.Value);
        if (r == null) return;

        // Проверка активных выдач
        if (db.Loans.Any(l => l.ReaderId == id && l.ReturnDate == null))
        {
            MessageBox.Show("У читателя есть невозвращённые книги. Сначала оформите возврат.", "Нельзя удалить", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        db.Readers.Remove(r);
        db.SaveChanges();
        LoadReaders();
    }
}
