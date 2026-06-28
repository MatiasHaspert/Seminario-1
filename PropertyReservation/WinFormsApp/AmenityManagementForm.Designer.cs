namespace WinFormsApp
{
    partial class AmenityManagementForm
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

            dgvAmenities = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();

            pnlActions = new Panel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();

            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAmenities).BeginInit();
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
            lblFormTitle.Text = "Gestionar Amenities";
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(22, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.TabIndex = 0;

            //
            // lblFormSubtitle
            //
            lblFormSubtitle.Text = "Catálogo de servicios asignables a propiedades";
            lblFormSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Location = new Point(24, 43);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.TabIndex = 1;

            //
            // dgvAmenities
            //
            dgvAmenities.Dock = DockStyle.Fill;
            dgvAmenities.AutoGenerateColumns = false;
            dgvAmenities.AllowUserToAddRows = false;
            dgvAmenities.AllowUserToDeleteRows = false;
            dgvAmenities.ReadOnly = true;
            dgvAmenities.RowHeadersVisible = false;
            dgvAmenities.MultiSelect = false;
            dgvAmenities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAmenities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAmenities.BackgroundColor = Color.White;
            dgvAmenities.BorderStyle = BorderStyle.None;
            dgvAmenities.Font = new Font("Segoe UI", 9F);
            dgvAmenities.Name = "dgvAmenities";
            dgvAmenities.TabIndex = 1;
            dgvAmenities.Columns.AddRange(new DataGridViewColumn[] { colId, colName });

            colId.HeaderText = "Id"; colId.DataPropertyName = "id"; colId.FillWeight = 30; colId.Name = "colId";
            colName.HeaderText = "Nombre"; colName.DataPropertyName = "name"; colName.FillWeight = 200; colName.Name = "colName";

            //
            // pnlActions
            //
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 64;
            pnlActions.BackColor = Color.White;
            pnlActions.Padding = new Padding(18, 14, 18, 14);
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 2;
            pnlActions.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });

            //
            // btnAdd
            //
            btnAdd.Text = "Crear";
            btnAdd.Location = new Point(18, 17);
            btnAdd.Size = new Size(105, 34);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.BackColor = Color.FromArgb(39, 174, 96);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 0;
            btnAdd.Click += btnAdd_Click;

            //
            // btnEdit
            //
            btnEdit.Text = "Editar";
            btnEdit.Location = new Point(130, 17);
            btnEdit.Size = new Size(105, 34);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.BackColor = Color.FromArgb(41, 128, 185);
            btnEdit.ForeColor = Color.White;
            btnEdit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Name = "btnEdit";
            btnEdit.TabIndex = 1;
            btnEdit.Click += btnEdit_Click;

            //
            // btnDelete
            //
            btnDelete.Text = "Eliminar";
            btnDelete.Location = new Point(242, 17);
            btnDelete.Size = new Size(105, 34);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 2;
            btnDelete.Click += btnDelete_Click;

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
            lblStatus.TabIndex = 3;

            //
            // AmenityManagementForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 520);
            Controls.Add(dgvAmenities);
            Controls.Add(pnlActions);
            Controls.Add(lblStatus);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "AmenityManagementForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gestionar Amenities";
            Load += AmenityManagementForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAmenities).EndInit();
            pnlActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        private DataGridView dgvAmenities;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;

        private Panel pnlActions;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        private Label lblStatus;
    }
}
