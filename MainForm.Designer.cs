using System.Drawing;

namespace LibraryApp;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem справочникиToolStripMenuItem;
    private ToolStripMenuItem книгиToolStripMenuItem;
    private ToolStripMenuItem читателиToolStripMenuItem;
    private ToolStripMenuItem операцииToolStripMenuItem;
    private ToolStripMenuItem выдатьКнигуToolStripMenuItem;
    private ToolStripMenuItem текущиеВыдачиToolStripMenuItem;
    private Button btnBooks;
    private Button btnReaders;
    private Button btnIssueBook;
    private Button btnLoans;
    private Button btnExit;
    private Label lblTitle;
    private Label lblStatus;
    private Panel panel1;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip1 = new MenuStrip();
        справочникиToolStripMenuItem = new ToolStripMenuItem();
        книгиToolStripMenuItem = new ToolStripMenuItem();
        читателиToolStripMenuItem = new ToolStripMenuItem();
        операцииToolStripMenuItem = new ToolStripMenuItem();
        выдатьКнигуToolStripMenuItem = new ToolStripMenuItem();
        текущиеВыдачиToolStripMenuItem = new ToolStripMenuItem();
        panel1 = new Panel();
        lblTitle = new Label();
        btnBooks = new Button();
        btnReaders = new Button();
        btnIssueBook = new Button();
        btnLoans = new Button();
        btnExit = new Button();
        lblStatus = new Label();

        menuStrip1.SuspendLayout();
        panel1.SuspendLayout();
        SuspendLayout();

        // MenuStrip
        menuStrip1.Items.AddRange(new ToolStripItem[] {
            справочникиToolStripMenuItem,
            операцииToolStripMenuItem
        });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(720, 24);
        menuStrip1.TabIndex = 0;

        // Справочники
        справочникиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            книгиToolStripMenuItem,
            читателиToolStripMenuItem
        });
        справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
        справочникиToolStripMenuItem.Size = new Size(94, 20);
        справочникиToolStripMenuItem.Text = "Справочники";

        книгиToolStripMenuItem.Name = "книгиToolStripMenuItem";
        книгиToolStripMenuItem.Size = new Size(180, 22);
        книгиToolStripMenuItem.Text = "Книги";
        книгиToolStripMenuItem.Click += MenuBooks_Click;

        читателиToolStripMenuItem.Name = "читателиToolStripMenuItem";
        читателиToolStripMenuItem.Size = new Size(180, 22);
        читателиToolStripMenuItem.Text = "Читатели";
        читателиToolStripMenuItem.Click += MenuReaders_Click;

        // Операции
        операцииToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            выдатьКнигуToolStripMenuItem,
            текущиеВыдачиToolStripMenuItem
        });
        операцииToolStripMenuItem.Name = "операцииToolStripMenuItem";
        операцииToolStripMenuItem.Size = new Size(72, 20);
        операцииToolStripMenuItem.Text = "Операции";

        выдатьКнигуToolStripMenuItem.Name = "выдатьКнигуToolStripMenuItem";
        выдатьКнигуToolStripMenuItem.Size = new Size(180, 22);
        выдатьКнигуToolStripMenuItem.Text = "Выдать книгу";
        выдатьКнигуToolStripMenuItem.Click += MenuIssue_Click;

        текущиеВыдачиToolStripMenuItem.Name = "текущиеВыдачиToolStripMenuItem";
        текущиеВыдачиToolStripMenuItem.Size = new Size(180, 22);
        текущиеВыдачиToolStripMenuItem.Text = "Текущие выдачи";
        текущиеВыдачиToolStripMenuItem.Click += MenuLoans_Click;

        // Panel + Title
        panel1.BackColor = Color.FromArgb(45, 52, 71);
        panel1.Controls.Add(lblTitle);
        panel1.Dock = DockStyle.Top;
        panel1.Location = new Point(0, 24);
        panel1.Name = "panel1";
        panel1.Size = new Size(720, 80);
        panel1.TabIndex = 1;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(20, 22);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(450, 32);
        lblTitle.Text = "Учёт выданных книг в библиотеке";

        // Buttons
        int btnWidth = 220;
        int btnHeight = 55;
        int startY = 130;

        btnBooks.Location = new Point(40, startY);
        btnBooks.Size = new Size(btnWidth, btnHeight);
        btnBooks.Text = "📚 Книги";
        btnBooks.Font = new Font("Segoe UI", 12F);
        btnBooks.Click += btnBooks_Click;

        btnReaders.Location = new Point(40, startY + 70);
        btnReaders.Size = new Size(btnWidth, btnHeight);
        btnReaders.Text = "👥 Читатели";
        btnReaders.Font = new Font("Segoe UI", 12F);
        btnReaders.Click += btnReaders_Click;

        btnIssueBook.Location = new Point(40, startY + 140);
        btnIssueBook.Size = new Size(btnWidth, btnHeight);
        btnIssueBook.Text = "📖 Выдать книгу";
        btnIssueBook.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnIssueBook.BackColor = Color.FromArgb(76, 175, 80);
        btnIssueBook.ForeColor = Color.White;
        btnIssueBook.Click += btnIssueBook_Click;

        btnLoans.Location = new Point(40, startY + 210);
        btnLoans.Size = new Size(btnWidth, btnHeight);
        btnLoans.Text = "📋 Текущие выдачи";
        btnLoans.Font = new Font("Segoe UI", 12F);
        btnLoans.Click += btnLoans_Click;

        btnExit.Location = new Point(40, startY + 300);
        btnExit.Size = new Size(btnWidth, 40);
        btnExit.Text = "Выход";
        btnExit.Click += btnExit_Click;

        // Status label
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(20, 500);
        lblStatus.Name = "lblStatus";
        lblStatus.ForeColor = Color.Gray;
        lblStatus.Text = "Инициализация...";

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 560);
        Controls.Add(panel1);
        Controls.Add(btnBooks);
        Controls.Add(btnReaders);
        Controls.Add(btnIssueBook);
        Controls.Add(btnLoans);
        Controls.Add(btnExit);
        Controls.Add(lblStatus);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "MainForm";
        Text = "Учёт книг в библиотеке";
        Load += MainForm_Load;

        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
