using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Forms;

public partial class LoansForm : Form
{
    private DataGridView dgv;
    private Button btnReturn, btnRefresh, btnClose;
    private CheckBox chkOnlyActive;
    private Label lblStats;

    public LoansForm()
    {
        InitializeComponent();
        Text = "Текущие выдачи книг";
        LoadLoans();
    }

    private void InitializeComponent()
    {
        dgv = new DataGridView();
        btnReturn = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        chkOnlyActive = new CheckBox();
        lblStats = new Label();

        SuspendLayout();

        dgv.Dock = DockStyle.Top;
        dgv.Location = new Point(0, 50);
        dgv.Size = new Size(980, 340);
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.Columns.AddRange(
            new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { Name = "Book", HeaderText = "Книга", Width = 260 },
            new DataGridViewTextBoxColumn { Name = "Reader", HeaderText = "Читатель", Width = 180 },
            new DataGridViewTextBoxColumn { Name = "Issue", HeaderText = "Выдано", Width = 90 },
            new DataGridViewTextBoxColumn { Name = "Due", HeaderText = "Вернуть до", Width = 90 },
            new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Статус", Width = 110 }
        );

        chkOnlyActive.Text = "Только активные (невозвращённые)";
        chkOnlyActive.Location = new Point(15, 18);
        chkOnlyActive.Checked = true;
        chkOnlyActive.CheckedChanged += (s, e) => LoadLoans();

        btnReturn.Text = "Оформить возврат";
        btnReturn.Location = new Point(280, 15);
        btnReturn.Size = new Size(160, 28);
        btnReturn.Click += btnReturn_Click;

        btnRefresh.Text = "Обновить";
        btnRefresh.Location = new Point(460, 15);
        btnRefresh.Click += (s, e) => LoadLoans();

        btnClose.Text = "Закрыть";
        btnClose.Location = new Point(570, 15);
        btnClose.Click += (s, e) => Close();

        lblStats.Location = new Point(15, 410);
        lblStats.AutoSize = true;

        Controls.AddRange(new Control[] { dgv, chkOnlyActive, btnReturn, btnRefresh, btnClose, lblStats });
        ClientSize = new Size(980, 450);
        ResumeLayout();
    }

    private void LoadLoans()
    {
        using var db = new LibraryDbContext();

        var query = db.Loans
            .Include(l => l.Book)
            .Include(l => l.Reader)
            .AsQueryable();

        if (chkOnlyActive.Checked)
            query = query.Where(l => l.ReturnDate == null);

        var data = query
            .OrderByDescending(l => l.IssueDate)
            .Select(l => new
            {
                l.Id,
                Book = l.Book!.Title,
                Reader = l.Reader!.FullName,
                Issue = l.IssueDate.ToShortDateString(),
                Due = l.DueDate.ToShortDateString(),
                Status = l.ReturnDate.HasValue ? "Возвращена" :
                         l.DueDate < DateTime.Today ? "ПРОСРОЧЕНА" : "На руках"
            })
            .ToList();

        dgv.Rows.Clear();
        foreach (var item in data)
        {
            int row = dgv.Rows.Add(item.Id, item.Book, item.Reader, item.Issue, item.Due, item.Status);
            // Подсветка просрочек
            if (item.Status == "ПРОСРОЧЕНА")
            {
                dgv.Rows[row].DefaultCellStyle.ForeColor = Color.Red;
                dgv.Rows[row].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            }
        }

        int active = db.Loans.Count(l => l.ReturnDate == null);
        int overdue = db.Loans.Count(l => l.ReturnDate == null && l.DueDate < DateTime.Today);
        lblStats.Text = $"Активных выдач: {active}   |   Просрочено: {overdue}";
    }

    private int? GetSelectedLoanId()
    {
        if (dgv.SelectedRows.Count == 0) return null;
        return Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);
    }

    private void btnReturn_Click(object? sender, EventArgs e)
    {
        int? loanId = GetSelectedLoanId();
        if (loanId == null) return;

        using var db = new LibraryDbContext();
        var loan = db.Loans.Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
        if (loan == null || loan.ReturnDate.HasValue)
        {
            MessageBox.Show("Эта выдача уже закрыта.");
            return;
        }

        loan.ReturnDate = DateTime.Today;
        if (loan.Book != null)
        {
            loan.Book.AvailableCopies++;
        }

        db.SaveChanges();

        MessageBox.Show("Книга успешно возвращена.", "Возврат оформлен", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LoadLoans();
    }
}
