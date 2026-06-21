using LibraryApp.Models;

namespace LibraryApp.Forms;

public partial class ReaderEditForm : Form
{
    public Reader Reader { get; private set; }

    private TextBox txtName, txtPhone, txtEmail;
    private Button btnSave, btnCancel;

    public ReaderEditForm(Reader? existing = null)
    {
        Reader = existing ?? new Reader();
        InitializeComponent();
        LoadData();
        Text = existing == null ? "Новый читатель" : "Редактирование читателя";
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        txtName = new TextBox { Location = new Point(150, 25), Width = 280 };
        txtPhone = new TextBox { Location = new Point(150, 65), Width = 280 };
        txtEmail = new TextBox { Location = new Point(150, 105), Width = 280 };

        Controls.Add(new Label { Text = "ФИО:", Location = new Point(20, 28) });
        Controls.Add(txtName);
        Controls.Add(new Label { Text = "Телефон:", Location = new Point(20, 68) });
        Controls.Add(txtPhone);
        Controls.Add(new Label { Text = "Email:", Location = new Point(20, 108) });
        Controls.Add(txtEmail);

        btnSave = new Button { Text = "Сохранить", Location = new Point(150, 160), Size = new Size(110, 32) };
        btnSave.Click += btnSave_Click;
        btnCancel = new Button { Text = "Отмена", Location = new Point(280, 160), Size = new Size(110, 32), DialogResult = DialogResult.Cancel };

        Controls.Add(btnSave);
        Controls.Add(btnCancel);

        ClientSize = new Size(470, 220);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = btnSave;
        CancelButton = btnCancel;

        ResumeLayout();
    }

    private void LoadData()
    {
        txtName.Text = Reader.FullName;
        txtPhone.Text = Reader.Phone ?? "";
        txtEmail.Text = Reader.Email ?? "";
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("ФИО обязательно для заполнения.");
            return;
        }

        Reader.FullName = txtName.Text.Trim();
        Reader.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
        Reader.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();

        if (Reader.RegistrationDate == default)
            Reader.RegistrationDate = DateTime.Now;

        DialogResult = DialogResult.OK;
        Close();
    }
}
