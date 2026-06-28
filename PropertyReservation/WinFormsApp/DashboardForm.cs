using Application.DTOs.Admin;
using WinFormsClient;

namespace WinFormsApp
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            var email = SessionManager.GetUserEmail();
            lblUserInfo.Text = string.IsNullOrEmpty(email) ? string.Empty : $"Sesión: {email}";
            await LoadStatsAsync();
            refreshTimer.Start();
        }

        private async Task LoadStatsAsync()
        {
            lblStatus.Text = "Cargando estadísticas...";
            ResetKpis();

            try
            {
                var stats = await Program.AdminClient.GetStatsAsync();
                if (stats == null)
                {
                    lblStatus.Text = "No se pudieron cargar las estadísticas. Reintente.";
                    return;
                }

                RenderStats(stats);
                lblStatus.Text = $"Última actualización: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void ResetKpis()
        {
            lblUsersValue.Text = "--";
            lblUsersBreakdown.Text = string.Empty;
            grpUsers.Text = "Usuarios";
            lblPropertiesValue.Text = "--";
            lblReviewsValue.Text = "--";
            lblReservationsValue.Text = "--";
            lblReservationsBreakdown.Text = string.Empty;
            lblPendingPaymentsValue.Text = "--";
        }

        private void RenderStats(AdminStatsDTO stats)
        {
            var totalUsers = stats.UsersByRole?.Values.Sum() ?? 0;
            lblUsersValue.Text = totalUsers.ToString();
            grpUsers.Text = "Usuarios";
            lblUsersBreakdown.Text = FormatBreakdown(stats.UsersByRole);

            lblPropertiesValue.Text = stats.TotalProperties.ToString();
            lblReviewsValue.Text = stats.TotalReviews.ToString();

            var reservationsTotal = stats.ReservationsByStatus?.Values.Sum() ?? 0;
            lblReservationsValue.Text = reservationsTotal.ToString();
            lblReservationsBreakdown.Text = FormatBreakdown(stats.ReservationsByStatus);

            lblPendingPaymentsValue.Text = stats.PendingPayments.ToString();
        }

        private static string FormatBreakdown(Dictionary<string, int>? dict)
        {
            if (dict == null || dict.Count == 0) return string.Empty;
            return string.Join("   ", dict.Where(kv => kv.Value > 0)
                                          .Select(kv => $"{kv.Key}: {kv.Value}"));
        }

        private async void refreshTimer_Tick(object? sender, EventArgs e)
        {
            await LoadStatsAsync();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cerrar la sesión actual?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Program.AuthClient.Logout();
            DialogResult = DialogResult.Abort; // marca "logout" para Program.Main
            Close();
        }

        private void btnUsers_Click(object sender, EventArgs e) => OpenChild(new UserManagementForm());
        private void btnProperties_Click(object sender, EventArgs e) => OpenChild(new PropertyListForm());
        private void btnReservations_Click(object sender, EventArgs e) => OpenChild(new ReservationManagementForm());
        private void btnPayments_Click(object sender, EventArgs e) => OpenChild(new PaymentApprovalForm());
        private void btnReviews_Click(object sender, EventArgs e) => OpenChild(new ReviewModerationForm());
        private void btnAmenities_Click(object sender, EventArgs e) => OpenChild(new AmenityManagementForm());

        private async void OpenChild(Form form)
        {
            using (form)
            {
                form.ShowDialog(this);
            }
            await LoadStatsAsync();
        }
    }
}
