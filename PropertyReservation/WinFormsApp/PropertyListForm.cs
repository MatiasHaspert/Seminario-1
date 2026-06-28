using Application.DTOs.Property;

namespace WinFormsApp
{
    public partial class PropertyListForm : Form
    {
        public PropertyListForm()
        {
            InitializeComponent();
            UiTheme.ApplyGridStyle(dgvProperties);
        }

        private async void PropertyListForm_Load(object sender, EventArgs e)
        {
            await LoadPropertiesAsync();
        }

        private async Task LoadPropertiesAsync()
        {
            lblStatus.Text = "Cargando propiedades...";
            try
            {
                var properties = await Program.PropertyClient.GetPropertiesAsync(chkIncludeDeleted.Checked);
                var list = properties?.ToList();
                dgvProperties.DataSource = list;
                lblStatus.Text = list == null || list.Count == 0
                    ? "No hay propiedades registradas."
                    : $"{list.Count} propiedad(es).";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private PropertyListResponseDTO? GetSelected()
        {
            if (dgvProperties.CurrentRow?.DataBoundItem is PropertyListResponseDTO p) return p;
            MessageBox.Show("Seleccione una propiedad primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void dgvProperties_SelectionChanged(object sender, EventArgs e)
        {
            var p = dgvProperties.CurrentRow?.DataBoundItem as PropertyListResponseDTO;
            btnDelete.Visible = p != null && !p.IsDeleted;
            btnRestore.Visible = p?.IsDeleted == true;
        }

        private void dgvProperties_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var p = dgvProperties.Rows[e.RowIndex].DataBoundItem as PropertyListResponseDTO;
            if (p?.IsDeleted == true)
            {
                e.CellStyle!.BackColor = Color.FromArgb(245, 245, 245);
                e.CellStyle.ForeColor = Color.FromArgb(160, 160, 160);
                e.CellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                e.CellStyle.SelectionForeColor = Color.FromArgb(100, 100, 100);
            }
        }

        private async void chkIncludeDeleted_CheckedChanged(object sender, EventArgs e) => await LoadPropertiesAsync();
        private async void btnRefresh_Click(object sender, EventArgs e) => await LoadPropertiesAsync();

        private async void btnDetail_Click(object sender, EventArgs e)
        {
            var p = GetSelected();
            if (p == null) return;

            try
            {
                var detail = await Program.PropertyClient.GetPropertyByIdAsync(p.Id);
                if (detail == null)
                {
                    MessageBox.Show("No se pudo obtener el detalle.", "Aviso");
                    return;
                }

                var msg = $"Id: {detail.Id}\r\n" +
                          $"Título: {detail.Title}\r\n" +
                          $"Descripción: {detail.Description}\r\n" +
                          $"Precio/noche: {detail.NightlyPrice:C}\r\n" +
                          $"Capacidad: {detail.MaxGuests} huéspedes, {detail.Bedrooms} hab, {detail.Bathrooms} baños\r\n" +
                          $"Dirección: {detail.StreetAddress}, {detail.City}, {detail.State}, {detail.Country} ({detail.PostalCode})";

                MessageBox.Show(msg, "Detalle de propiedad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var p = GetSelected();
            if (p == null) return;

            if (MessageBox.Show($"¿Eliminar la propiedad '{p.Title}'?\r\nDejará de aparecer en búsquedas públicas pero se conservan sus reservas.",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                await Program.PropertyClient.DeletePropertyAsync(p.Id);
                await LoadPropertiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            var p = GetSelected();
            if (p == null) return;

            if (MessageBox.Show($"¿Restaurar la propiedad '{p.Title}'?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                await Program.PropertyClient.RestorePropertyAsync(p.Id);
                await LoadPropertiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
