namespace WinFormsApp
{
    partial class PropertyListForm
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
            pnlHeader = new Panel();
            lblFormTitle = new Label();
            lblFormSubtitle = new Label();

            dgvProperties = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colTitle = new DataGridViewTextBoxColumn();
            colNightlyPrice = new DataGridViewTextBoxColumn();
            colMaxGuests = new DataGridViewTextBoxColumn();
            colBedrooms = new DataGridViewTextBoxColumn();
            colBathrooms = new DataGridViewTextBoxColumn();
            colCountry = new DataGridViewTextBoxColumn();
            colState = new DataGridViewTextBoxColumn();
            colCity = new DataGridViewTextBoxColumn();

            pnlToolbar = new Panel();
            chkIncludeDeleted = new CheckBox();
            btnRefresh = new Button();

            pnlActions = new Panel();
            btnDetail = new Button();
            btnDelete = new Button();
            btnRestore = new Button();

            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProperties).BeginInit();
            pnlToolbar.SuspendLayout();
            pnlActions.SuspendLayout();
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
            lblFormTitle.Text = "Gestionar Propiedades";
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(22, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.TabIndex = 0;

            //
            // lblFormSubtitle
            //
            lblFormSubtitle.Text = "Ver detalle, eliminar o restaurar propiedades";
            lblFormSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Location = new Point(24, 43);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.TabIndex = 1;

            //
            // pnlToolbar
            //
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Height = 60;
            pnlToolbar.BackColor = Color.FromArgb(236, 240, 241);
            pnlToolbar.Padding = new Padding(18, 14, 18, 14);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.TabIndex = 1;
            pnlToolbar.Controls.Add(chkIncludeDeleted);
            pnlToolbar.Controls.Add(btnRefresh);

            //
            // chkIncludeDeleted
            //
            chkIncludeDeleted.Text = "Incluir eliminadas (soft-deleted)";
            chkIncludeDeleted.AutoSize = true;
            chkIncludeDeleted.Location = new Point(22, 20);
            chkIncludeDeleted.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            chkIncludeDeleted.ForeColor = Color.FromArgb(52, 73, 94);
            chkIncludeDeleted.Name = "chkIncludeDeleted";
            chkIncludeDeleted.TabIndex = 0;
            chkIncludeDeleted.CheckedChanged += chkIncludeDeleted_CheckedChanged;

            //
            // btnRefresh
            //
            btnRefresh.Text = "Refrescar";
            btnRefresh.Location = new Point(880, 14);
            btnRefresh.Size = new Size(110, 32);
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnRefresh.BackColor = Color.White;
            btnRefresh.ForeColor = Color.FromArgb(52, 73, 94);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 1;
            btnRefresh.Click += btnRefresh_Click;

            //
            // dgvProperties
            //
            dgvProperties.AllowUserToAddRows = false;
            dgvProperties.AllowUserToDeleteRows = false;
            dgvProperties.AutoGenerateColumns = false;
            dgvProperties.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProperties.Columns.AddRange(new DataGridViewColumn[] { colId, colTitle, colNightlyPrice, colMaxGuests, colBedrooms, colBathrooms, colCountry, colState, colCity });
            dgvProperties.Dock = DockStyle.Fill;
            dgvProperties.MultiSelect = false;
            dgvProperties.Name = "dgvProperties";
            dgvProperties.TabIndex = 2;
            dgvProperties.ReadOnly = true;
            dgvProperties.RowHeadersVisible = false;
            dgvProperties.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProperties.BackgroundColor = Color.White;
            dgvProperties.BorderStyle = BorderStyle.None;
            dgvProperties.Font = new Font("Segoe UI", 9F);
            dgvProperties.SelectionChanged += dgvProperties_SelectionChanged;
            dgvProperties.CellFormatting += dgvProperties_CellFormatting;

            colId.DataPropertyName = "id"; colId.HeaderText = "Id"; colId.Name = "colId"; colId.ReadOnly = true; colId.Visible = false;
            colTitle.DataPropertyName = "title"; colTitle.HeaderText = "Título"; colTitle.Name = "colTitle"; colTitle.ReadOnly = true;
            colNightlyPrice.DataPropertyName = "nightlyPrice"; colNightlyPrice.HeaderText = "Precio/noche"; colNightlyPrice.Name = "colNightlyPrice"; colNightlyPrice.ReadOnly = true; colNightlyPrice.DefaultCellStyle.Format = "C2";
            colMaxGuests.DataPropertyName = "maxGuests"; colMaxGuests.HeaderText = "Huéspedes máx."; colMaxGuests.Name = "colMaxGuests"; colMaxGuests.ReadOnly = true;
            colBedrooms.DataPropertyName = "bedrooms"; colBedrooms.HeaderText = "Habitaciones"; colBedrooms.Name = "colBedrooms"; colBedrooms.ReadOnly = true;
            colBathrooms.DataPropertyName = "bathrooms"; colBathrooms.HeaderText = "Baños"; colBathrooms.Name = "colBathrooms"; colBathrooms.ReadOnly = true;
            colCountry.DataPropertyName = "country"; colCountry.HeaderText = "País"; colCountry.Name = "colCountry"; colCountry.ReadOnly = true;
            colState.DataPropertyName = "state"; colState.HeaderText = "Provincia"; colState.Name = "colState"; colState.ReadOnly = true;
            colCity.DataPropertyName = "city"; colCity.HeaderText = "Ciudad"; colCity.Name = "colCity"; colCity.ReadOnly = true;

            //
            // pnlActions
            //
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 64;
            pnlActions.BackColor = Color.White;
            pnlActions.Padding = new Padding(18, 14, 18, 14);
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 3;
            pnlActions.Controls.AddRange(new Control[] { btnDetail, btnDelete, btnRestore });

            //
            // btnDetail
            //
            btnDetail.Text = "Ver detalle";
            btnDetail.Location = new Point(18, 17);
            btnDetail.Size = new Size(105, 34);
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
            // btnDelete
            //
            btnDelete.Text = "Eliminar";
            btnDelete.Location = new Point(130, 17);
            btnDelete.Size = new Size(125, 34);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.FromArgb(243, 156, 18);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Visible = false;
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 1;
            btnDelete.Click += btnDelete_Click;

            //
            // btnRestore
            //
            btnRestore.Text = "Restaurar";
            btnRestore.Location = new Point(130, 17);
            btnRestore.Size = new Size(125, 34);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.BackColor = Color.FromArgb(52, 73, 94);
            btnRestore.ForeColor = Color.White;
            btnRestore.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnRestore.Cursor = Cursors.Hand;
            btnRestore.Visible = false;
            btnRestore.Name = "btnRestore";
            btnRestore.TabIndex = 2;
            btnRestore.Click += btnRestore_Click;

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
            lblStatus.TabIndex = 4;

            //
            // PropertyListForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1010, 600);
            Controls.Add(dgvProperties);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Controls.Add(pnlActions);
            Controls.Add(lblStatus);
            Font = new Font("Segoe UI", 9F);
            Name = "PropertyListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestionar Propiedades";
            Load += PropertyListForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProperties).EndInit();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        private Panel pnlToolbar;
        private CheckBox chkIncludeDeleted;
        private Button btnRefresh;

        private DataGridView dgvProperties;
        private Panel pnlActions;
        private Button btnDetail;
        private Button btnDelete;
        private Button btnRestore;
        private Label lblStatus;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colTitle;
        private DataGridViewTextBoxColumn colNightlyPrice;
        private DataGridViewTextBoxColumn colMaxGuests;
        private DataGridViewTextBoxColumn colBedrooms;
        private DataGridViewTextBoxColumn colBathrooms;
        private DataGridViewTextBoxColumn colCountry;
        private DataGridViewTextBoxColumn colState;
        private DataGridViewTextBoxColumn colCity;
    }
}
