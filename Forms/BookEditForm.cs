using LibraryApp.Models;

namespace LibraryApp.Forms;

public partial class BookEditForm : Form
{
    public Book Book { get; private set; }

    private TextBox txtTitle, txtAuthor, txtYear, txtISBN, txtPublisher, txtTotal, txtAvailable;
    private Button btnOk, btnCancel;

    public BookEditForm(Book? existing = null)
    {
        Book = existing ?? new Book { TotalCopies = 1, AvailableCopies = 1 };
        InitializeComponent();
        LoadData();
        Text = existing == null ? "Новая книга" : "Редактирование книги";
    }

    private void InitializeComponent()
    {
        var labels = new[] { "Название:", "Автор:", "Год издания:", "ISBN:", "Издательство:", "Всего экземпляров:", "Доступно:" };
        var textboxes = new List<TextBox>();

        SuspendLayout();

        txtTitle = CreateTextBox();
        txtAuthor = CreateTextBox();
        txtYear = CreateTextBox();
        txtISBN = CreateTextBox();
        txtPublisher = CreateTextBox();
        txtTotal = CreateTextBox();
        txtAvailable = CreateTextBox();

        btnOk = new Button { Text = "Сохранить", Location = new Point(140, 280), Size = new Size(100, 30) };
        btnOk.Click += btnOk_Click;

        btnCancel = new Button { Text = "Отмена", Location = new Point(260, 280), Size = new Size(100, 30), DialogResult = DialogResult.Cancel };

        var y = 20;
        for (int i = 0; i < labels.Length; i++)
        {
            Controls.Add(new Label { Text = labels[i], Location = new Point(20, y), AutoSize = true });
            var tb = i == 0 ? txtTitle : i == 1 ? txtAuthor : i == 2 ? txtYear : i == 3 ? txtISBN :
                     i == 4 ? txtPublisher : i == 5 ? txtTotal : txtAvailable;
            tb.Location = new Point(170, y);
            tb.Width = 260;
            Controls.Add(tb);
            y += 35;
        }

        Controls.Add(btnOk);
        Controls.Add(btnCancel);

        ClientSize = new Size(470, 340);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = btnOk;
        CancelButton = btnCancel;

        ResumeLayout();
    }

    private TextBox CreateTextBox() => new TextBox();

    private void LoadData()
    {
        txtTitle.Text = Book.Title;
        txtAuthor.Text = Book.Author;
        txtYear.Text = Book.YearPublished.ToString();
        txtISBN.Text = Book.ISBN ?? "";
        txtPublisher.Text = Book.Publisher ?? "";
        txtTotal.Text = Book.TotalCopies.ToString();
        txtAvailable.Text = Book.AvailableCopies.ToString();
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            MessageBox.Show("Название книги обязательно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Book.Title = txtTitle.Text.Trim();
        Book.Author = txtAuthor.Text.Trim();
        Book.YearPublished = int.TryParse(txtYear.Text, out int y) ? y : 0;
        Book.ISBN = string.IsNullOrWhiteSpace(txtISBN.Text) ? null : txtISBN.Text.Trim();
        Book.Publisher = string.IsNullOrWhiteSpace(txtPublisher.Text) ? null : txtPublisher.Text.Trim();
        Book.TotalCopies = int.TryParse(txtTotal.Text, out int t) ? t : 1;
        Book.AvailableCopies = int.TryParse(txtAvailable.Text, out int a) ? a : Book.TotalCopies;

        if (Book.AvailableCopies > Book.TotalCopies) Book.AvailableCopies = Book.TotalCopies;

        DialogResult = DialogResult.OK;
        Close();
    }
}
