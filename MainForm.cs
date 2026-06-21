using LibraryApp.Data;
using LibraryApp.Forms;

namespace LibraryApp;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        Text = "Учёт выданных книг в библиотеке";
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        try
        {
            DatabaseInitializer.Initialize();
            lblStatus.Text = "База данных подключена (LocalDB)";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}\n\nУбедитесь, что установлен SQL Server LocalDB.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Ошибка подключения к БД";
        }
    }

    private void btnBooks_Click(object sender, EventArgs e)
    {
        var form = new BooksForm();
        form.ShowDialog();
    }

    private void btnReaders_Click(object sender, EventArgs e)
    {
        var form = new ReadersForm();
        form.ShowDialog();
    }

    private void btnIssueBook_Click(object sender, EventArgs e)
    {
        var form = new IssueForm();
        form.ShowDialog();
    }

    private void btnLoans_Click(object sender, EventArgs e)
    {
        var form = new LoansForm();
        form.ShowDialog();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    // Для меню (если нужно)
    private void MenuBooks_Click(object sender, EventArgs e) => btnBooks_Click(sender, e);
    private void MenuReaders_Click(object sender, EventArgs e) => btnReaders_Click(sender, e);
    private void MenuIssue_Click(object sender, EventArgs e) => btnIssueBook_Click(sender, e);
    private void MenuLoans_Click(object sender, EventArgs e) => btnLoans_Click(sender, e);
}
