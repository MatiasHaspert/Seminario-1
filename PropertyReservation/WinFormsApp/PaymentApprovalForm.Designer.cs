namespace WinFormsApp
{
    partial class PaymentApprovalForm
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

            splitMain = new SplitContainer();

            dgvPayments = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colProperty = new DataGridViewTextBoxColumn();
            colGuest = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colMethod = new DataGridViewTextBoxColumn();
            colUploaded = new DataGridViewTextBoxColumn();

            pnlActions = new Panel();
            btnLoadProof = new Button();
            btnApprove = new Button();
            btnReject = new Button();
            btnRefresh = new Button();
            lblNotice = new Label();

            grpProof = new GroupBox();
            pbProof = new PictureBox();
            lblProofInfo = new Label();
            btnOpenExternal = new Button();

            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbProof).BeginInit();
            pnlActions.SuspendLayout();
            grpProof.SuspendLayout();
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
            lblFormTitle.Text = "Aprobar / Rechazar Pagos";
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(22, 12);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.TabIndex = 0;

            //
            // lblFormSubtitle
            //
            lblFormSubtitle.Text = "Revise el comprobante y decida sobre los pagos bajo revisión";
            lblFormSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Location = new Point(24, 43);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.TabIndex = 1;

            //
            // splitMain
            //
            splitMain.Dock = DockStyle.Fill;
            splitMain.Orientation = Orientation.Vertical;
            splitMain.SplitterDistance = 600;
            splitMain.SplitterWidth = 4;
            splitMain.BackColor = Color.FromArgb(236, 240, 241);
            splitMain.Name = "splitMain";
            splitMain.TabIndex = 1;

            //
            // splitMain.Panel1
            //
            splitMain.Panel1.Controls.Add(dgvPayments);
            splitMain.Panel1.Controls.Add(pnlActions);

            //
            // dgvPayments
            //
            dgvPayments.Dock = DockStyle.Fill;
            dgvPayments.AutoGenerateColumns = false;
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.AllowUserToDeleteRows = false;
            dgvPayments.ReadOnly = true;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.MultiSelect = false;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.BackgroundColor = Color.White;
            dgvPayments.BorderStyle = BorderStyle.None;
            dgvPayments.Font = new Font("Segoe UI", 9F);
            dgvPayments.Name = "dgvPayments";
            dgvPayments.TabIndex = 0;
            dgvPayments.SelectionChanged += dgvPayments_SelectionChanged;
            dgvPayments.Columns.AddRange(new DataGridViewColumn[] { colId, colProperty, colGuest, colAmount, colMethod, colUploaded });

            colId.HeaderText = "Pago"; colId.DataPropertyName = "paymentId"; colId.FillWeight = 100; colId.Name = "colId";
            colProperty.HeaderText = "Propiedad"; colProperty.DataPropertyName = "propertyName"; colProperty.FillWeight = 150; colProperty.Name = "colProperty";
            colGuest.HeaderText = "Huésped"; colGuest.DataPropertyName = "guestEmail"; colGuest.FillWeight = 130; colGuest.Name = "colGuest";
            colAmount.HeaderText = "Monto"; colAmount.DataPropertyName = "amount"; colAmount.FillWeight = 80; colAmount.DefaultCellStyle.Format = "C2"; colAmount.Name = "colAmount";
            colMethod.HeaderText = "Método"; colMethod.DataPropertyName = "paymentMethod"; colMethod.FillWeight = 80; colMethod.Name = "colMethod";
            colUploaded.HeaderText = "Subido"; colUploaded.DataPropertyName = "uploadedAt"; colUploaded.FillWeight = 110; colUploaded.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; colUploaded.Name = "colUploaded";

            //
            // pnlActions
            //
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 82;
            pnlActions.Padding = new Padding(12, 14, 12, 14);
            pnlActions.BackColor = Color.White;
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 1;
            pnlActions.Controls.AddRange(new Control[] { btnLoadProof, btnApprove, btnReject, btnRefresh, lblNotice });

            //
            // btnLoadProof
            //
            btnLoadProof.Text = "Ver comprobante";
            btnLoadProof.Location = new Point(12, 17);
            btnLoadProof.Size = new Size(150, 34);
            btnLoadProof.FlatStyle = FlatStyle.Flat;
            btnLoadProof.FlatAppearance.BorderSize = 1;
            btnLoadProof.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnLoadProof.BackColor = Color.White;
            btnLoadProof.ForeColor = Color.FromArgb(52, 73, 94);
            btnLoadProof.Cursor = Cursors.Hand;
            btnLoadProof.Name = "btnLoadProof";
            btnLoadProof.TabIndex = 0;
            btnLoadProof.Click += btnLoadProof_Click;

            //
            // btnApprove
            //
            btnApprove.Text = "Aprobar";
            btnApprove.Location = new Point(170, 17);
            btnApprove.Size = new Size(110, 34);
            btnApprove.FlatStyle = FlatStyle.Flat;
            btnApprove.FlatAppearance.BorderSize = 0;
            btnApprove.BackColor = Color.FromArgb(39, 174, 96);
            btnApprove.ForeColor = Color.White;
            btnApprove.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnApprove.Cursor = Cursors.Hand;
            btnApprove.Name = "btnApprove";
            btnApprove.TabIndex = 1;
            btnApprove.Click += btnApprove_Click;

            //
            // btnReject
            //
            btnReject.Text = "Rechazar";
            btnReject.Location = new Point(285, 17);
            btnReject.Size = new Size(110, 34);
            btnReject.FlatStyle = FlatStyle.Flat;
            btnReject.FlatAppearance.BorderSize = 0;
            btnReject.BackColor = Color.FromArgb(192, 57, 43);
            btnReject.ForeColor = Color.White;
            btnReject.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnReject.Cursor = Cursors.Hand;
            btnReject.Name = "btnReject";
            btnReject.TabIndex = 2;
            btnReject.Click += btnReject_Click;

            //
            // btnRefresh
            //
            btnRefresh.Text = "Refrescar";
            btnRefresh.Location = new Point(480, 17);
            btnRefresh.Size = new Size(105, 34);
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
            // lblNotice
            //
            lblNotice.Text = "ℹ Solo se pueden aprobar o rechazar pagos con tarjeta de crédito o débito.";
            lblNotice.Location = new Point(12, 55);
            lblNotice.AutoSize = true;
            lblNotice.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblNotice.ForeColor = Color.FromArgb(127, 140, 141);
            lblNotice.Name = "lblNotice";
            lblNotice.TabIndex = 4;

            //
            // splitMain.Panel2
            //
            splitMain.Panel2.Controls.Add(grpProof);
            splitMain.Panel2.BackColor = Color.White;

            //
            // grpProof
            //
            grpProof.Text = "Comprobante de pago";
            grpProof.Dock = DockStyle.Fill;
            grpProof.Padding = new Padding(10);
            grpProof.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            grpProof.ForeColor = Color.FromArgb(52, 73, 94);
            grpProof.BackColor = Color.White;
            grpProof.Name = "grpProof";
            grpProof.TabIndex = 0;
            grpProof.Controls.AddRange(new Control[] { pbProof, lblProofInfo, btnOpenExternal });

            //
            // pbProof
            //
            pbProof.Dock = DockStyle.Fill;
            pbProof.SizeMode = PictureBoxSizeMode.Zoom;
            pbProof.BackColor = Color.FromArgb(236, 240, 241);
            pbProof.Name = "pbProof";
            pbProof.TabIndex = 0;

            //
            // lblProofInfo
            //
            lblProofInfo.Dock = DockStyle.Top;
            lblProofInfo.Height = 30;
            lblProofInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblProofInfo.Padding = new Padding(8, 0, 8, 0);
            lblProofInfo.Font = new Font("Segoe UI", 9F);
            lblProofInfo.ForeColor = Color.FromArgb(52, 73, 94);
            lblProofInfo.Text = "Seleccione un pago y presione \"Ver comprobante\".";
            lblProofInfo.Name = "lblProofInfo";
            lblProofInfo.TabIndex = 1;

            //
            // btnOpenExternal
            //
            btnOpenExternal.Dock = DockStyle.Bottom;
            btnOpenExternal.Text = "Abrir comprobante en visor externo";
            btnOpenExternal.Height = 36;
            btnOpenExternal.FlatStyle = FlatStyle.Flat;
            btnOpenExternal.FlatAppearance.BorderSize = 1;
            btnOpenExternal.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnOpenExternal.BackColor = Color.White;
            btnOpenExternal.ForeColor = Color.FromArgb(52, 73, 94);
            btnOpenExternal.Font = new Font("Segoe UI", 9F);
            btnOpenExternal.Cursor = Cursors.Hand;
            btnOpenExternal.Enabled = false;
            btnOpenExternal.Name = "btnOpenExternal";
            btnOpenExternal.TabIndex = 2;
            btnOpenExternal.Click += btnOpenExternal_Click;

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
            lblStatus.TabIndex = 2;

            //
            // PaymentApprovalForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1120, 620);
            Controls.Add(splitMain);
            Controls.Add(lblStatus);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            Name = "PaymentApprovalForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Aprobar / Rechazar Pagos";
            Load += PaymentApprovalForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbProof).EndInit();
            pnlActions.ResumeLayout(false);
            grpProof.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;

        private SplitContainer splitMain;
        private DataGridView dgvPayments;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colProperty;
        private DataGridViewTextBoxColumn colGuest;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colMethod;
        private DataGridViewTextBoxColumn colUploaded;

        private Panel pnlActions;
        private Button btnLoadProof;
        private Button btnApprove;
        private Button btnReject;
        private Button btnRefresh;
        private Label lblNotice;

        private GroupBox grpProof;
        private PictureBox pbProof;
        private Label lblProofInfo;
        private Button btnOpenExternal;

        private Label lblStatus;
    }
}
