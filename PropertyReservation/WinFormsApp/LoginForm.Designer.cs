namespace WinFormsApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblBrand = new Label();
            lblSubtitle = new Label();

            pnlBody = new Panel();
            lblWelcome = new Label();
            lblWelcomeHint = new Label();

            lblEmail = new Label();
            txtEmail = new TextBox();

            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();

            btnLogin = new Button();

            lblError = new Label();
            lblFooter = new Label();

            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            SuspendLayout();

            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 110;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(lblBrand);
            pnlHeader.Controls.Add(lblSubtitle);

            //
            // lblBrand
            //
            lblBrand.Text = "PropertyReservation";
            lblBrand.ForeColor = Color.White;
            lblBrand.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblBrand.AutoSize = true;
            lblBrand.Location = new Point(30, 25);
            lblBrand.Name = "lblBrand";
            lblBrand.TabIndex = 0;

            //
            // lblSubtitle
            //
            lblSubtitle.Text = "Panel de Administración";
            lblSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(32, 65);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.TabIndex = 1;

            //
            // pnlBody
            //
            pnlBody.BackColor = Color.White;
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(40, 25, 40, 25);
            pnlBody.Name = "pnlBody";
            pnlBody.TabIndex = 1;
            pnlBody.Controls.Add(lblWelcome);
            pnlBody.Controls.Add(lblWelcomeHint);
            pnlBody.Controls.Add(lblEmail);
            pnlBody.Controls.Add(txtEmail);
            pnlBody.Controls.Add(lblPassword);
            pnlBody.Controls.Add(txtPassword);
            pnlBody.Controls.Add(chkShowPassword);
            pnlBody.Controls.Add(btnLogin);
            pnlBody.Controls.Add(lblError);
            pnlBody.Controls.Add(lblFooter);

            //
            // lblWelcome
            //
            lblWelcome.Text = "Iniciar sesión";
            lblWelcome.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(44, 62, 80);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(40, 30);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.TabIndex = 0;

            //
            // lblWelcomeHint
            //
            lblWelcomeHint.Text = "Acceso exclusivo para administradores del sistema.";
            lblWelcomeHint.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblWelcomeHint.ForeColor = Color.FromArgb(127, 140, 141);
            lblWelcomeHint.AutoSize = true;
            lblWelcomeHint.Location = new Point(40, 65);
            lblWelcomeHint.Name = "lblWelcomeHint";
            lblWelcomeHint.TabIndex = 1;

            //
            // lblEmail
            //
            lblEmail.Text = "Email";
            lblEmail.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(52, 73, 94);
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(40, 110);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 2;

            //
            // txtEmail
            //
            txtEmail.Location = new Point(40, 132);
            txtEmail.Size = new Size(380, 28);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.PlaceholderText = "admin@empresa.com";
            txtEmail.Name = "txtEmail";
            txtEmail.TabIndex = 3;

            //
            // lblPassword
            //
            lblPassword.Text = "Contraseña";
            lblPassword.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(52, 73, 94);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(40, 180);
            lblPassword.Name = "lblPassword";
            lblPassword.TabIndex = 4;

            //
            // txtPassword
            //
            txtPassword.Location = new Point(40, 202);
            txtPassword.Size = new Size(380, 28);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.Name = "txtPassword";
            txtPassword.TabIndex = 5;

            //
            // chkShowPassword
            //
            chkShowPassword.Text = "Mostrar contraseña";
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(40, 240);
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.ForeColor = Color.FromArgb(52, 73, 94);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.TabIndex = 6;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;

            //
            // btnLogin
            //
            btnLogin.Text = "Iniciar sesión";
            btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(41, 128, 185);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Location = new Point(40, 290);
            btnLogin.Size = new Size(380, 44);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Name = "btnLogin";
            btnLogin.TabIndex = 7;
            btnLogin.Click += btnLogin_Click;

            //
            // lblError
            //
            lblError.AutoSize = false;
            lblError.Location = new Point(40, 350);
            lblError.Size = new Size(380, 50);
            lblError.Font = new Font("Segoe UI", 9F);
            lblError.ForeColor = Color.FromArgb(192, 57, 43);
            lblError.BackColor = Color.FromArgb(252, 235, 234);
            lblError.TextAlign = ContentAlignment.MiddleLeft;
            lblError.Padding = new Padding(10, 0, 10, 0);
            lblError.Visible = false;
            lblError.Name = "lblError";
            lblError.TabIndex = 8;

            //
            // lblFooter
            //
            lblFooter.Text = "© PropertyReservation — WinFormsApp";
            lblFooter.Font = new Font("Segoe UI", 8F);
            lblFooter.ForeColor = Color.FromArgb(149, 165, 166);
            lblFooter.AutoSize = false;
            lblFooter.Size = new Size(380, 20);
            lblFooter.Location = new Point(40, 415);
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            lblFooter.Name = "lblFooter";
            lblFooter.TabIndex = 9;

            //
            // LoginForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(460, 560);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
            AcceptButton = btnLogin;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Name = "LoginForm";
            Text = "PropertyReservation — Iniciar sesión";
            Load += LoginForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblBrand;
        private Label lblSubtitle;

        private Panel pnlBody;
        private Label lblWelcome;
        private Label lblWelcomeHint;

        private Label lblEmail;
        private TextBox txtEmail;

        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;

        private Button btnLogin;
        private Label lblError;
        private Label lblFooter;
    }
}
