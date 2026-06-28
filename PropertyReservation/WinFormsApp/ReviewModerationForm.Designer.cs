namespace WinFormsApp
{
    partial class ReviewModerationForm
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

            pnlPropertySelector = new Panel();
            lblPropertySelector = new Label();
            txtPropertySearch = new TextBox();
            cboProperty = new ComboBox();
            btnApplyFilter = new Button();

            dgvReviews = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colProperty = new DataGridViewTextBoxColumn();
            colUser = new DataGridViewTextBoxColumn();
            colRating = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();

            pnlActions = new Panel();
            btnDelete = new Button();
            btnRefresh = new Button();

            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            pnlPropertySelector.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReviews).BeginInit();
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
            lblFormTitle.Text = "Moderación de Reseñas";
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(22, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.TabIndex = 0;

            //
            // lblFormSubtitle
            //
            lblFormSubtitle.Text = "Revisión y eliminación de reseñas de propiedades";
            lblFormSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Location = new Point(24, 43);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.TabIndex = 1;

            //
            // pnlPropertySelector
            //
            pnlPropertySelector.Dock = DockStyle.Top;
            pnlPropertySelector.Height = 65;
            pnlPropertySelector.BackColor = Color.FromArgb(236, 240, 241);
            pnlPropertySelector.Name = "pnlPropertySelector";
            pnlPropertySelector.TabIndex = 1;
            pnlPropertySelector.Controls.AddRange(new Control[] { lblPropertySelector, txtPropertySearch, cboProperty, btnApplyFilter });

            //
            // lblPropertySelector
            //
            lblPropertySelector.Text = "Filtrar por propiedad:";
            lblPropertySelector.AutoSize = true;
            lblPropertySelector.Location = new Point(18, 22);
            lblPropertySelector.Width = 140;
            lblPropertySelector.Font = new Font("Segoe UI", 9F);
            lblPropertySelector.Name = "lblPropertySelector";
            lblPropertySelector.TabIndex = 0;

            //
            // txtPropertySearch
            //
            txtPropertySearch.Location = new Point(165, 18);
            txtPropertySearch.Width = 200;
            txtPropertySearch.PlaceholderText = "Buscar propiedad...";
            txtPropertySearch.Name = "txtPropertySearch";
            txtPropertySearch.TabIndex = 1;
            txtPropertySearch.TextChanged += txtPropertySearch_TextChanged;

            //
            // cboProperty
            //
            cboProperty.Location = new Point(375, 18);
            cboProperty.Width = 280;
            cboProperty.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProperty.DisplayMember = "Title";
            cboProperty.ValueMember = "Id";
            cboProperty.Name = "cboProperty";
            cboProperty.TabIndex = 2;
            cboProperty.SelectedIndexChanged += cboProperty_SelectedIndexChanged;

            //
            // btnApplyFilter
            //
            btnApplyFilter.Text = "Aplicar filtro";
            btnApplyFilter.Location = new Point(665, 16);
            btnApplyFilter.Size = new Size(130, 34);
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.BackColor = Color.FromArgb(41, 128, 185);
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnApplyFilter.Cursor = Cursors.Hand;
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.TabIndex = 3;
            btnApplyFilter.Click += btnApplyFilter_Click;

            //
            // dgvReviews
            //
            dgvReviews.Dock = DockStyle.Fill;
            dgvReviews.AutoGenerateColumns = false;
            dgvReviews.AllowUserToAddRows = false;
            dgvReviews.AllowUserToDeleteRows = false;
            dgvReviews.ReadOnly = true;
            dgvReviews.RowHeadersVisible = false;
            dgvReviews.MultiSelect = false;
            dgvReviews.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReviews.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReviews.BackgroundColor = Color.White;
            dgvReviews.BorderStyle = BorderStyle.None;
            dgvReviews.Font = new Font("Segoe UI", 9F);
            dgvReviews.Name = "dgvReviews";
            dgvReviews.TabIndex = 2;
            dgvReviews.Columns.AddRange(new DataGridViewColumn[] { colId, colProperty, colUser, colRating, colDate, colComment });

            colId.HeaderText = "Id"; colId.DataPropertyName = "id"; colId.FillWeight = 30; colId.Name = "colId";
            colProperty.HeaderText = "Propiedad"; colProperty.DataPropertyName = "propertyName"; colProperty.FillWeight = 130; colProperty.Name = "colProperty";
            colUser.HeaderText = "Autor (email)"; colUser.DataPropertyName = "userEmail"; colUser.FillWeight = 90; colUser.Name = "colUser";
            colRating.HeaderText = "Rating"; colRating.DataPropertyName = "rating"; colRating.FillWeight = 50; colRating.Name = "colRating";
            colDate.HeaderText = "Fecha"; colDate.DataPropertyName = "date"; colDate.FillWeight = 80; colDate.DefaultCellStyle.Format = "dd/MM/yyyy"; colDate.Name = "colDate";
            colComment.HeaderText = "Comentario"; colComment.DataPropertyName = "comment"; colComment.FillWeight = 250; colComment.Name = "colComment";

            //
            // pnlActions
            //
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 64;
            pnlActions.BackColor = Color.White;
            pnlActions.Padding = new Padding(18, 14, 18, 14);
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 3;
            pnlActions.Controls.AddRange(new Control[] { btnDelete, btnRefresh });

            //
            // btnDelete
            //
            btnDelete.Text = "Eliminar reseña";
            btnDelete.Location = new Point(18, 15);
            btnDelete.Size = new Size(140, 34);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Enabled = false;
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 0;
            btnDelete.Click += btnDelete_Click;

            //
            // btnRefresh
            //
            btnRefresh.Text = "Refrescar";
            btnRefresh.Location = new Point(730, 15);
            btnRefresh.Size = new Size(110, 34);
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnRefresh.BackColor = Color.FromArgb(236, 240, 241);
            btnRefresh.ForeColor = Color.FromArgb(52, 73, 94);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 1;
            btnRefresh.Click += btnRefresh_Click;

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
            // ReviewModerationForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(870, 610);
            Controls.Add(dgvReviews);
            Controls.Add(pnlActions);
            Controls.Add(lblStatus);
            Controls.Add(pnlPropertySelector);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "ReviewModerationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Moderación de Reseñas";
            Load += ReviewModerationForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlPropertySelector.ResumeLayout(false);
            pnlPropertySelector.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReviews).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        private Panel pnlPropertySelector;
        private Label lblPropertySelector;
        private TextBox txtPropertySearch;
        private ComboBox cboProperty;
        private Button btnApplyFilter;

        private DataGridView dgvReviews;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colProperty;
        private DataGridViewTextBoxColumn colUser;
        private DataGridViewTextBoxColumn colRating;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colComment;

        private Panel pnlActions;
        private Button btnDelete;
        private Button btnRefresh;

        private Label lblStatus;
    }
}
