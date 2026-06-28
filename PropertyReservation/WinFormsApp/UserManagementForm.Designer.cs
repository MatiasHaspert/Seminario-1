namespace WinFormsApp
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            pnlHeader = new Panel();
            lblFormTitle = new Label();
            lblFormSubtitle = new Label();

            pnlFilters = new Panel();
            lblEmail = new Label();
            txtEmailFilter = new TextBox();
            lblRole = new Label();
            cboRoleFilter = new ComboBox();
            lblStatusLbl = new Label();
            cboStatusFilter = new ComboBox();
            btnApplyFilter = new Button();
            btnClearFilter = new Button();

            dgvUsers = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colLastName = new DataGridViewTextBoxColumn();
            colPhone = new DataGridViewTextBoxColumn();
            colRole = new DataGridViewTextBoxColumn();
            colActive = new DataGridViewCheckBoxColumn();
            colCreatedAt = new DataGridViewTextBoxColumn();

            pnlActions = new Panel();
            btnDetail = new Button();
            btnChangeRole = new Button();
            btnToggleStatus = new Button();
            btnRefresh = new Button();

            lblHint = new Label();
            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();

            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 70;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Controls.Add(lblFormSubtitle);

            //
            // lblFormTitle
            //
            lblFormTitle.Text = "Gestionar Usuarios";
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(22, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.TabIndex = 0;

            //
            // lblFormSubtitle
            //
            lblFormSubtitle.Text = "Listar, cambiar rol y habilitar/deshabilitar usuarios del sistema";
            lblFormSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Location = new Point(24, 43);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.TabIndex = 1;

            //
            // pnlFilters
            //
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 64;
            pnlFilters.Padding = new Padding(18, 12, 18, 12);
            pnlFilters.BackColor = Color.FromArgb(236, 240, 241);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.TabIndex = 1;
            pnlFilters.Controls.AddRange(new Control[] { lblEmail, txtEmailFilter, lblRole, cboRoleFilter, lblStatusLbl, cboStatusFilter, btnApplyFilter, btnClearFilter });

            //
            // lblEmail
            //
            lblEmail.Text = "Email:";
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(22, 22);
            lblEmail.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(52, 73, 94);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 0;

            //
            // txtEmailFilter
            //
            txtEmailFilter.Location = new Point(70, 18);
            txtEmailFilter.Width = 210;
            txtEmailFilter.Font = new Font("Segoe UI", 9F);
            txtEmailFilter.BorderStyle = BorderStyle.FixedSingle;
            txtEmailFilter.Name = "txtEmailFilter";
            txtEmailFilter.TabIndex = 1;

            //
            // lblRole
            //
            lblRole.Text = "Rol:";
            lblRole.AutoSize = true;
            lblRole.Location = new Point(295, 22);
            lblRole.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblRole.ForeColor = Color.FromArgb(52, 73, 94);
            lblRole.Name = "lblRole";
            lblRole.TabIndex = 2;

            //
            // cboRoleFilter
            //
            cboRoleFilter.Location = new Point(325, 18);
            cboRoleFilter.Width = 120;
            cboRoleFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRoleFilter.FlatStyle = FlatStyle.Flat;
            cboRoleFilter.Items.AddRange(new object[] { "(Todos)", "User", "Owner", "Admin" });
            cboRoleFilter.SelectedIndex = 0;
            cboRoleFilter.Name = "cboRoleFilter";
            cboRoleFilter.TabIndex = 3;

            //
            // lblStatusLbl
            //
            lblStatusLbl.Text = "Estado:";
            lblStatusLbl.AutoSize = true;
            lblStatusLbl.Location = new Point(460, 22);
            lblStatusLbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatusLbl.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatusLbl.Name = "lblStatusLbl";
            lblStatusLbl.TabIndex = 4;

            //
            // cboStatusFilter
            //
            cboStatusFilter.Location = new Point(510, 18);
            cboStatusFilter.Width = 130;
            cboStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatusFilter.FlatStyle = FlatStyle.Flat;
            cboStatusFilter.Items.AddRange(new object[] { "(Todos)", "Activos", "Deshabilitados" });
            cboStatusFilter.SelectedIndex = 0;
            cboStatusFilter.Name = "cboStatusFilter";
            cboStatusFilter.TabIndex = 5;

            //
            // btnApplyFilter
            //
            btnApplyFilter.Text = "Filtrar";
            btnApplyFilter.Location = new Point(660, 16);
            btnApplyFilter.Size = new Size(90, 30);
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.BackColor = Color.FromArgb(41, 128, 185);
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnApplyFilter.Cursor = Cursors.Hand;
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.TabIndex = 6;
            btnApplyFilter.Click += btnApplyFilter_Click;

            //
            // btnClearFilter
            //
            btnClearFilter.Text = "Limpiar";
            btnClearFilter.Location = new Point(755, 16);
            btnClearFilter.Size = new Size(90, 30);
            btnClearFilter.FlatStyle = FlatStyle.Flat;
            btnClearFilter.FlatAppearance.BorderSize = 1;
            btnClearFilter.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnClearFilter.BackColor = Color.White;
            btnClearFilter.ForeColor = Color.FromArgb(52, 73, 94);
            btnClearFilter.Cursor = Cursors.Hand;
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.TabIndex = 7;
            btnClearFilter.Click += btnClearFilter_Click;

            //
            // dgvUsers
            //
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.MultiSelect = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.Font = new Font("Segoe UI", 9F);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.TabIndex = 2;
            dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colId, colEmail, colName, colLastName, colPhone, colRole, colActive, colCreatedAt });

            colId.HeaderText = "Id"; colId.DataPropertyName = "id"; colId.Width = 50; colId.FillWeight = 30; colId.Name = "colId";
            colEmail.HeaderText = "Email"; colEmail.DataPropertyName = "email"; colEmail.FillWeight = 130; colEmail.Name = "colEmail";
            colName.HeaderText = "Nombre"; colName.DataPropertyName = "name"; colName.FillWeight = 80; colName.Name = "colName";
            colLastName.HeaderText = "Apellido"; colLastName.DataPropertyName = "lastName"; colLastName.FillWeight = 80; colLastName.Name = "colLastName";
            colPhone.HeaderText = "Teléfono"; colPhone.DataPropertyName = "phone"; colPhone.FillWeight = 80; colPhone.Name = "colPhone";
            colRole.HeaderText = "Rol"; colRole.DataPropertyName = "role"; colRole.FillWeight = 60; colRole.Name = "colRole";
            colActive.HeaderText = "Activo"; colActive.DataPropertyName = "isActive"; colActive.FillWeight = 50; colActive.Name = "colActive";
            colCreatedAt.HeaderText = "Fecha alta"; colCreatedAt.DataPropertyName = "createdAt"; colCreatedAt.FillWeight = 90; colCreatedAt.DefaultCellStyle.Format = "dd/MM/yyyy"; colCreatedAt.Name = "colCreatedAt";

            //
            // pnlActions
            //
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 64;
            pnlActions.Padding = new Padding(18, 14, 18, 14);
            pnlActions.BackColor = Color.White;
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 3;
            pnlActions.Controls.AddRange(new Control[] { btnDetail, btnChangeRole, btnToggleStatus, btnRefresh });

            //
            // btnDetail
            //
            btnDetail.Text = "Ver detalle";
            btnDetail.Location = new Point(18, 17);
            btnDetail.Size = new Size(115, 34);
            btnDetail.FlatStyle = FlatStyle.Flat;
            btnDetail.FlatAppearance.BorderSize = 1;
            btnDetail.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnDetail.BackColor = Color.White;
            btnDetail.ForeColor = Color.FromArgb(52, 73, 94);
            btnDetail.Cursor = Cursors.Hand;
            btnDetail.Name = "btnDetail";
            btnDetail.TabIndex = 0;
            btnDetail.Click += btnDetail_Click;

            //
            // btnChangeRole
            //
            btnChangeRole.Text = "Cambiar rol";
            btnChangeRole.Location = new Point(140, 17);
            btnChangeRole.Size = new Size(115, 34);
            btnChangeRole.FlatStyle = FlatStyle.Flat;
            btnChangeRole.FlatAppearance.BorderSize = 0;
            btnChangeRole.BackColor = Color.FromArgb(41, 128, 185);
            btnChangeRole.ForeColor = Color.White;
            btnChangeRole.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnChangeRole.Cursor = Cursors.Hand;
            btnChangeRole.Name = "btnChangeRole";
            btnChangeRole.TabIndex = 1;
            btnChangeRole.Click += btnChangeRole_Click;

            //
            // btnToggleStatus
            //
            btnToggleStatus.Text = "Habilitar / Deshabilitar";
            btnToggleStatus.Location = new Point(262, 17);
            btnToggleStatus.Size = new Size(180, 34);
            btnToggleStatus.FlatStyle = FlatStyle.Flat;
            btnToggleStatus.FlatAppearance.BorderSize = 0;
            btnToggleStatus.BackColor = Color.FromArgb(243, 156, 18);
            btnToggleStatus.ForeColor = Color.White;
            btnToggleStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnToggleStatus.Cursor = Cursors.Hand;
            btnToggleStatus.Name = "btnToggleStatus";
            btnToggleStatus.TabIndex = 2;
            btnToggleStatus.Click += btnToggleStatus_Click;

            //
            // btnRefresh
            //
            btnRefresh.Text = "Refrescar";
            btnRefresh.Location = new Point(890, 17);
            btnRefresh.Size = new Size(110, 34);
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnRefresh.BackColor = Color.White;
            btnRefresh.ForeColor = Color.FromArgb(52, 73, 94);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 3;
            btnRefresh.Click += btnRefresh_Click;

            //
            // lblHint
            //
            lblHint.Dock = DockStyle.Top;
            lblHint.Height = 26;
            lblHint.TextAlign = ContentAlignment.MiddleLeft;
            lblHint.Padding = new Padding(20, 0, 20, 0);
            lblHint.BackColor = Color.FromArgb(232, 244, 253);
            lblHint.ForeColor = Color.FromArgb(41, 128, 185);
            lblHint.Font = new Font("Segoe UI", 8.5F);
            lblHint.Text = "Seleccione un usuario y elija una acción.";
            lblHint.Name = "lblHint";
            lblHint.TabIndex = 4;

            //
            // lblStatus
            //
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 26;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Padding = new Padding(20, 0, 20, 0);
            lblStatus.BackColor = Color.FromArgb(236, 240, 241);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Font = new Font("Segoe UI", 8.5F);
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 5;

            //
            // UserManagementForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1020, 600);
            Controls.Add(dgvUsers);
            Controls.Add(lblHint);
            Controls.Add(pnlFilters);
            Controls.Add(pnlHeader);
            Controls.Add(pnlActions);
            Controls.Add(lblStatus);
            Font = new Font("Segoe UI", 9F);
            Name = "UserManagementForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestionar Usuarios";
            Load += UserManagementForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        private Panel pnlFilters;
        private Label lblEmail;
        private TextBox txtEmailFilter;
        private Label lblRole;
        private ComboBox cboRoleFilter;
        private Label lblStatusLbl;
        private ComboBox cboStatusFilter;
        private Button btnApplyFilter;
        private Button btnClearFilter;

        private DataGridView dgvUsers;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colLastName;
        private DataGridViewTextBoxColumn colPhone;
        private DataGridViewTextBoxColumn colRole;
        private DataGridViewCheckBoxColumn colActive;
        private DataGridViewTextBoxColumn colCreatedAt;

        private Panel pnlActions;
        private Button btnDetail;
        private Button btnChangeRole;
        private Button btnToggleStatus;
        private Button btnRefresh;

        private Label lblHint;
        private Label lblStatus;
    }
}
