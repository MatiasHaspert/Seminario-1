using Application.DTOs.Reservation;
using Domain.Enums;

namespace WinFormsApp
{
    public partial class ReservationManagementForm : Form
    {
        // Etiquetas amigables (ES) para el filtro de estado, sin acoplar el texto mostrado al nombre del enum.
        private static readonly (ReservationStatus Status, string Label)[] StatusFilterOptions =
        {
            (ReservationStatus.PendingPayment,  "Pago pendiente"),
            (ReservationStatus.PaymentUploaded, "Comprobante subido"),
            (ReservationStatus.Confirmed,       "Confirmada"),
            (ReservationStatus.Cancelled,       "Cancelada"),
            (ReservationStatus.Completed,       "Completada"),
        };

        public ReservationManagementForm()
        {
            InitializeComponent();
            UiTheme.ApplyGridStyle(dgvReservations);
            InitStatusFilter();
        }

        // Carga el filtro de estado con etiquetas amigables; cada item conserva el enum real que se envía al backend.
        private void InitStatusFilter()
        {
            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.Add(new StatusOption { Status = null, Display = "(Todos)" });
            foreach (var (status, label) in StatusFilterOptions)
                cboStatusFilter.Items.Add(new StatusOption { Status = status, Display = label });
            cboStatusFilter.SelectedIndex = 0;
        }

        private async void ReservationManagementForm_Load(object sender, EventArgs e)
        {
            await LoadFilterOptionsAsync();
            await LoadReservationsAsync();
        }

        // Carga los combos de Propiedad y Huésped con las opciones disponibles (búsqueda por nombre).
        // Un item centinela con Id null encabeza cada combo y representa "sin filtro".
        private async Task LoadFilterOptionsAsync()
        {
            // Sembramos el centinela primero para que, aun si falla la API, el combo no quede vacío.
            cboPropertyFilter.Items.Clear();
            cboPropertyFilter.Items.Add(new FilterOption { Id = null, Display = "(Todas las propiedades)" });
            cboPropertyFilter.SelectedIndex = 0;

            cboGuestFilter.Items.Clear();
            cboGuestFilter.Items.Add(new FilterOption { Id = null, Display = "(Todos los huéspedes)" });
            cboGuestFilter.SelectedIndex = 0;

            try
            {
                // Las opciones salen del listado de reservas por defecto (sin filtros): así solo se
                // ofrecen propiedades y huéspedes que efectivamente tienen reservas, evitando filtros vacíos.
                var reservations = await Program.ReservationClient.GetAllForAdminAsync();
                var list = reservations?.ToList() ?? new List<ReservationResponseDTO>();

                foreach (var r in list.DistinctBy(r => r.PropertyId).OrderBy(r => r.PropertyTitle))
                    cboPropertyFilter.Items.Add(new FilterOption { Id = r.PropertyId, Display = r.PropertyTitle });

                foreach (var r in list.DistinctBy(r => r.GuestId).OrderBy(r => r.GuestName))
                    cboGuestFilter.Items.Add(new FilterOption { Id = r.GuestId, Display = r.GuestName });
            }
            catch (Exception ex)
            {
                // Si falla la carga, los combos quedan solo con el centinela y el form sigue usable.
                lblStatus.Text = $"No se pudieron cargar las opciones de filtro: {ex.Message}";
            }
        }

        private async Task LoadReservationsAsync()
        {
            lblStatus.Text = "Cargando reservas...";
            try
            {
                string? statusFilter = (cboStatusFilter.SelectedItem as StatusOption)?.Status?.ToString();
                int? propertyId = (cboPropertyFilter.SelectedItem as FilterOption)?.Id;
                int? guestId = (cboGuestFilter.SelectedItem as FilterOption)?.Id;
                DateOnly? from = dtpFrom.Checked ? DateOnly.FromDateTime(dtpFrom.Value.Date) : null;
                DateOnly? to = dtpTo.Checked ? DateOnly.FromDateTime(dtpTo.Value.Date) : null;

                var reservations = await Program.ReservationClient.GetAllForAdminAsync(statusFilter, propertyId, guestId, from, to);
                var list = reservations?.ToList();
                dgvReservations.DataSource = list;

                lblStatus.Text = list == null || list.Count == 0
                    ? "No hay reservas que coincidan con los criterios."
                    : $"{list.Count} reserva(s).";

                btnDetail.Enabled = list != null && list.Count > 0;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private ReservationResponseDTO? GetSelected()
        {
            if (dgvReservations.CurrentRow?.DataBoundItem is ReservationResponseDTO r) return r;
            MessageBox.Show("Seleccione una reserva primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        // Item de combo para los filtros por nombre. Id null representa "sin filtro".
        private sealed class FilterOption
        {
            public int? Id { get; init; }
            public string Display { get; init; } = string.Empty;
            public override string ToString() => Display;
        }

        // Item del filtro de estado: muestra una etiqueta amigable pero conserva el enum (Status null = sin filtro).
        private sealed class StatusOption
        {
            public ReservationStatus? Status { get; init; }
            public string Display { get; init; } = string.Empty;
            public override string ToString() => Display;
        }

        private async void btnApplyFilter_Click(object sender, EventArgs e) => await LoadReservationsAsync();
        private async void btnRefresh_Click(object sender, EventArgs e) => await LoadReservationsAsync();

        private async void btnClearFilter_Click(object sender, EventArgs e)
        {
            cboStatusFilter.SelectedIndex = 0;
            if (cboPropertyFilter.Items.Count > 0) cboPropertyFilter.SelectedIndex = 0;
            if (cboGuestFilter.Items.Count > 0) cboGuestFilter.SelectedIndex = 0;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
            await LoadReservationsAsync();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            var r = GetSelected();
            if (r == null) return;

            var msg = $"Id: {r.Id}\r\n" +
                      $"Propiedad ({r.PropertyId}): {r.PropertyTitle}\r\n" +
                      $"Huésped ({r.GuestId}): {r.GuestName}\r\n" +
                      $"Fechas: {r.StartDate:dd/MM/yyyy} → {r.EndDate:dd/MM/yyyy}\r\n" +
                      $"Total: {r.TotalPrice:C}\r\n" +
                      $"Estado: {r.Status}\r\n" +
                      $"Tiene reseña: {(r.HasReview ? "Sí" : "No")}";

            MessageBox.Show(msg, "Detalle de reserva", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
