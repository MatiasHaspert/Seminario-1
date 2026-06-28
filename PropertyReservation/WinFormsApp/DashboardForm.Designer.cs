namespace WinFormsApp
{
    partial class DashboardForm
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
            lblTitle = new Label();
            lblUserInfo = new Label();
            btnLogout = new Button();
            refreshTimer = new System.Windows.Forms.Timer(components);

            pnlKpis = new TableLayoutPanel();
            grpUsers = new GroupBox(); lblUsersValue = new Label(); lblUsersBreakdown = new Label();
            grpProperties = new GroupBox(); lblPropertiesValue = new Label();
            grpReviews = new GroupBox(); lblReviewsValue = new Label();
            grpReservations = new GroupBox(); lblReservationsValue = new Label(); lblReservationsBreakdown = new Label();
            grpPendingPayments = new GroupBox(); lblPendingPaymentsValue = new Label();

            pnlActions = new FlowLayoutPanel();
            btnUsers = new Button();
            btnProperties = new Button();
            btnReservations = new Button();
            btnPayments = new Button();
            btnReviews = new Button();
            btnAmenities = new Button();

            lblStatus = new Label();

            pnlHeader.SuspendLayout();
            pnlKpis.SuspendLayout();
            pnlActions.SuspendLayout();
            SuspendLayout();

            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Size = new Size(1110, 80);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblUserInfo);
            pnlHeader.Controls.Add(btnLogout);

            //
            // lblTitle
            //
            lblTitle.Text = "Panel de Administración";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(24, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;

            //
            // lblUserInfo
            //
            lblUserInfo.Text = string.Empty;
            lblUserInfo.ForeColor = Color.FromArgb(189, 195, 199);
            lblUserInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblUserInfo.AutoSize = true;
            lblUserInfo.Location = new Point(26, 52);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.TabIndex = 1;

            //
            // refreshTimer
            //
            refreshTimer.Interval = 30000;
            refreshTimer.Tick += refreshTimer_Tick;

            //
            // btnLogout
            //
            btnLogout.Text = "Cerrar sesión";
            btnLogout.Size = new Size(120, 32);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Location = new Point(985, 24);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.BackColor = Color.FromArgb(192, 57, 43);
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Name = "btnLogout";
            btnLogout.TabIndex = 3;
            btnLogout.Click += btnLogout_Click;

            //
            // pnlKpis
            //
            pnlKpis.BackColor = Color.White;
            pnlKpis.Dock = DockStyle.Top;
            pnlKpis.Height = 160;
            pnlKpis.ColumnCount = 5;
            pnlKpis.RowCount = 1;
            pnlKpis.Padding = new Padding(18, 14, 18, 10);
            pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlKpis.Name = "pnlKpis";
            pnlKpis.TabIndex = 1;
            pnlKpis.Controls.Add(grpUsers, 0, 0);
            pnlKpis.Controls.Add(grpProperties, 1, 0);
            pnlKpis.Controls.Add(grpReviews, 2, 0);
            pnlKpis.Controls.Add(grpReservations, 3, 0);
            pnlKpis.Controls.Add(grpPendingPayments, 4, 0);

            //
            // grpUsers
            //
            grpUsers.Text = "Usuarios";
            grpUsers.Dock = DockStyle.Fill;
            grpUsers.Margin = new Padding(6);
            grpUsers.ForeColor = Color.FromArgb(52, 73, 94);
            grpUsers.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpUsers.Name = "grpUsers";
            grpUsers.TabIndex = 0;
            grpUsers.Controls.Add(lblUsersValue);
            grpUsers.Controls.Add(lblUsersBreakdown);

            //
            // lblUsersValue
            //
            lblUsersValue.Text = "--";
            lblUsersValue.Dock = DockStyle.Fill;
            lblUsersValue.TextAlign = ContentAlignment.MiddleCenter;
            lblUsersValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblUsersValue.ForeColor = Color.FromArgb(41, 128, 185);
            lblUsersValue.Name = "lblUsersValue";
            lblUsersValue.TabIndex = 0;

            //
            // lblUsersBreakdown
            //
            lblUsersBreakdown.Text = string.Empty;
            lblUsersBreakdown.Dock = DockStyle.Bottom;
            lblUsersBreakdown.Height = 40;
            lblUsersBreakdown.TextAlign = ContentAlignment.MiddleCenter;
            lblUsersBreakdown.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblUsersBreakdown.ForeColor = Color.FromArgb(127, 140, 141);
            lblUsersBreakdown.AutoEllipsis = true;
            lblUsersBreakdown.Name = "lblUsersBreakdown";
            lblUsersBreakdown.TabIndex = 1;

            //
            // grpProperties
            //
            grpProperties.Text = "Propiedades";
            grpProperties.Dock = DockStyle.Fill;
            grpProperties.Margin = new Padding(6);
            grpProperties.ForeColor = Color.FromArgb(52, 73, 94);
            grpProperties.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpProperties.Name = "grpProperties";
            grpProperties.TabIndex = 1;
            grpProperties.Controls.Add(lblPropertiesValue);

            //
            // lblPropertiesValue
            //
            lblPropertiesValue.Text = "--";
            lblPropertiesValue.Dock = DockStyle.Fill;
            lblPropertiesValue.TextAlign = ContentAlignment.MiddleCenter;
            lblPropertiesValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPropertiesValue.ForeColor = Color.FromArgb(41, 128, 185);
            lblPropertiesValue.Name = "lblPropertiesValue";
            lblPropertiesValue.TabIndex = 0;

            //
            // grpReviews
            //
            grpReviews.Text = "Reseñas";
            grpReviews.Dock = DockStyle.Fill;
            grpReviews.Margin = new Padding(6);
            grpReviews.ForeColor = Color.FromArgb(52, 73, 94);
            grpReviews.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpReviews.Name = "grpReviews";
            grpReviews.TabIndex = 2;
            grpReviews.Controls.Add(lblReviewsValue);

            //
            // lblReviewsValue
            //
            lblReviewsValue.Text = "--";
            lblReviewsValue.Dock = DockStyle.Fill;
            lblReviewsValue.TextAlign = ContentAlignment.MiddleCenter;
            lblReviewsValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblReviewsValue.ForeColor = Color.FromArgb(155, 89, 182);
            lblReviewsValue.Name = "lblReviewsValue";
            lblReviewsValue.TabIndex = 0;

            //
            // grpReservations
            //
            grpReservations.Text = "Reservas";
            grpReservations.Dock = DockStyle.Fill;
            grpReservations.Margin = new Padding(6);
            grpReservations.ForeColor = Color.FromArgb(52, 73, 94);
            grpReservations.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpReservations.Name = "grpReservations";
            grpReservations.TabIndex = 3;
            grpReservations.Controls.Add(lblReservationsValue);
            grpReservations.Controls.Add(lblReservationsBreakdown);

            //
            // lblReservationsValue
            //
            lblReservationsValue.Text = "--";
            lblReservationsValue.Dock = DockStyle.Fill;
            lblReservationsValue.TextAlign = ContentAlignment.MiddleCenter;
            lblReservationsValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblReservationsValue.ForeColor = Color.FromArgb(41, 128, 185);
            lblReservationsValue.Name = "lblReservationsValue";
            lblReservationsValue.TabIndex = 0;

            //
            // lblReservationsBreakdown
            //
            lblReservationsBreakdown.Text = string.Empty;
            lblReservationsBreakdown.Dock = DockStyle.Bottom;
            lblReservationsBreakdown.Height = 40;
            lblReservationsBreakdown.TextAlign = ContentAlignment.MiddleCenter;
            lblReservationsBreakdown.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblReservationsBreakdown.ForeColor = Color.FromArgb(127, 140, 141);
            lblReservationsBreakdown.AutoEllipsis = true;
            lblReservationsBreakdown.Name = "lblReservationsBreakdown";
            lblReservationsBreakdown.TabIndex = 1;

            //
            // grpPendingPayments
            //
            grpPendingPayments.Text = "Pagos pendientes";
            grpPendingPayments.Dock = DockStyle.Fill;
            grpPendingPayments.Margin = new Padding(6);
            grpPendingPayments.ForeColor = Color.FromArgb(52, 73, 94);
            grpPendingPayments.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpPendingPayments.Name = "grpPendingPayments";
            grpPendingPayments.TabIndex = 4;
            grpPendingPayments.Controls.Add(lblPendingPaymentsValue);

            //
            // lblPendingPaymentsValue
            //
            lblPendingPaymentsValue.Text = "--";
            lblPendingPaymentsValue.Dock = DockStyle.Fill;
            lblPendingPaymentsValue.TextAlign = ContentAlignment.MiddleCenter;
            lblPendingPaymentsValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPendingPaymentsValue.ForeColor = Color.FromArgb(243, 156, 18);
            lblPendingPaymentsValue.Name = "lblPendingPaymentsValue";
            lblPendingPaymentsValue.TabIndex = 0;

            //
            // pnlActions
            //
            pnlActions.BackColor = Color.FromArgb(236, 240, 241);
            pnlActions.Dock = DockStyle.Top;
            pnlActions.Height = 70;
            pnlActions.Padding = new Padding(18, 12, 18, 12);
            pnlActions.WrapContents = false;
            pnlActions.Name = "pnlActions";
            pnlActions.TabIndex = 2;
            pnlActions.Controls.Add(btnUsers);
            pnlActions.Controls.Add(btnProperties);
            pnlActions.Controls.Add(btnReservations);
            pnlActions.Controls.Add(btnPayments);
            pnlActions.Controls.Add(btnReviews);
            pnlActions.Controls.Add(btnAmenities);

            //
            // btnUsers
            //
            btnUsers.Text = "Gestionar Usuarios";
            btnUsers.Size = new Size(168, 44);
            btnUsers.Margin = new Padding(5);
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.BackColor = Color.FromArgb(41, 128, 185);
            btnUsers.ForeColor = Color.White;
            btnUsers.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnUsers.Cursor = Cursors.Hand;
            btnUsers.Name = "btnUsers";
            btnUsers.TabIndex = 0;
            btnUsers.Click += btnUsers_Click;

            //
            // btnProperties
            //
            btnProperties.Text = "Gestionar Propiedades";
            btnProperties.Size = new Size(168, 44);
            btnProperties.Margin = new Padding(5);
            btnProperties.FlatStyle = FlatStyle.Flat;
            btnProperties.FlatAppearance.BorderSize = 0;
            btnProperties.BackColor = Color.FromArgb(41, 128, 185);
            btnProperties.ForeColor = Color.White;
            btnProperties.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnProperties.Cursor = Cursors.Hand;
            btnProperties.Name = "btnProperties";
            btnProperties.TabIndex = 1;
            btnProperties.Click += btnProperties_Click;

            //
            // btnReservations
            //
            btnReservations.Text = "Gestionar Reservas";
            btnReservations.Size = new Size(168, 44);
            btnReservations.Margin = new Padding(5);
            btnReservations.FlatStyle = FlatStyle.Flat;
            btnReservations.FlatAppearance.BorderSize = 0;
            btnReservations.BackColor = Color.FromArgb(41, 128, 185);
            btnReservations.ForeColor = Color.White;
            btnReservations.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnReservations.Cursor = Cursors.Hand;
            btnReservations.Name = "btnReservations";
            btnReservations.TabIndex = 2;
            btnReservations.Click += btnReservations_Click;

            //
            // btnPayments
            //
            btnPayments.Text = "Aprobar Pagos";
            btnPayments.Size = new Size(168, 44);
            btnPayments.Margin = new Padding(5);
            btnPayments.FlatStyle = FlatStyle.Flat;
            btnPayments.FlatAppearance.BorderSize = 0;
            btnPayments.BackColor = Color.FromArgb(41, 128, 185);
            btnPayments.ForeColor = Color.White;
            btnPayments.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnPayments.Cursor = Cursors.Hand;
            btnPayments.Name = "btnPayments";
            btnPayments.TabIndex = 3;
            btnPayments.Click += btnPayments_Click;

            //
            // btnReviews
            //
            btnReviews.Text = "Gestionar Reseñas";
            btnReviews.Size = new Size(168, 44);
            btnReviews.Margin = new Padding(5);
            btnReviews.FlatStyle = FlatStyle.Flat;
            btnReviews.FlatAppearance.BorderSize = 0;
            btnReviews.BackColor = Color.FromArgb(41, 128, 185);
            btnReviews.ForeColor = Color.White;
            btnReviews.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnReviews.Cursor = Cursors.Hand;
            btnReviews.Name = "btnReviews";
            btnReviews.TabIndex = 4;
            btnReviews.Click += btnReviews_Click;

            //
            // btnAmenities
            //
            btnAmenities.Text = "Gestionar Amenities";
            btnAmenities.Size = new Size(168, 44);
            btnAmenities.Margin = new Padding(5);
            btnAmenities.FlatStyle = FlatStyle.Flat;
            btnAmenities.FlatAppearance.BorderSize = 0;
            btnAmenities.BackColor = Color.FromArgb(41, 128, 185);
            btnAmenities.ForeColor = Color.White;
            btnAmenities.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnAmenities.Cursor = Cursors.Hand;
            btnAmenities.Name = "btnAmenities";
            btnAmenities.TabIndex = 5;
            btnAmenities.Click += btnAmenities_Click;

            //
            // lblStatus
            //
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 26;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Padding = new Padding(18, 0, 18, 0);
            lblStatus.BackColor = Color.FromArgb(236, 240, 241);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Font = new Font("Segoe UI", 8.5F);
            lblStatus.Text = "Listo.";
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 4;

            //
            // DashboardForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1110, 620);
            Controls.Add(pnlActions);
            Controls.Add(pnlKpis);
            Controls.Add(pnlHeader);
            Controls.Add(lblStatus);
            Font = new Font("Segoe UI", 9F);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PropertyReservation — Panel de Administración";
            Load += DashboardForm_Load;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlKpis.ResumeLayout(false);
            pnlActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblUserInfo;
        private Button btnLogout;
        private System.Windows.Forms.Timer refreshTimer;

        private TableLayoutPanel pnlKpis;
        private GroupBox grpUsers; private Label lblUsersValue; private Label lblUsersBreakdown;
        private GroupBox grpProperties; private Label lblPropertiesValue;
        private GroupBox grpReservations; private Label lblReservationsValue; private Label lblReservationsBreakdown;
        private GroupBox grpPendingPayments; private Label lblPendingPaymentsValue;
        private GroupBox grpReviews; private Label lblReviewsValue;

        private FlowLayoutPanel pnlActions;
        private Button btnUsers;
        private Button btnProperties;
        private Button btnReservations;
        private Button btnPayments;
        private Button btnReviews;
        private Button btnAmenities;

        private Label lblStatus;
    }
}
