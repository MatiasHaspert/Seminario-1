using Application.DTOs.Property;
using Application.DTOs.Review;

namespace WinFormsApp
{
    public partial class ReviewModerationForm : Form
    {
        private List<PropertyListResponseDTO> _allProperties = new();
        private List<ReviewResponseDTO> _allReviews = new();

        // Sentinel para la opción "sin filtro" en el combo.
        private static readonly PropertyListResponseDTO AllPropertiesItem =
            new() { Id = 0, Title = "— Todas las propiedades —" };

        public ReviewModerationForm()
        {
            InitializeComponent();
            UiTheme.ApplyGridStyle(dgvReviews);
        }

        private async void ReviewModerationForm_Load(object sender, EventArgs e)
        {
            await LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            lblStatus.Text = "Cargando reseñas...";
            btnDelete.Enabled = false;

            try
            {
                var reviewsTask = Program.ReviewClient.GetAllReviewsAsync();
                var propertiesTask = Program.PropertyClient.GetPropertiesAsync(includeDeleted: true);

                await Task.WhenAll(reviewsTask, propertiesTask);

                _allReviews = reviewsTask.Result?.ToList() ?? new List<ReviewResponseDTO>();
                _allProperties = propertiesTask.Result?.OrderBy(p => p.Title).ToList() ?? new List<PropertyListResponseDTO>();

                RebuildPropertyCombo();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private void RebuildPropertyCombo(string filter = "")
        {
            var previousId = (cboProperty.SelectedItem as PropertyListResponseDTO)?.Id ?? 0;

            var filtered = string.IsNullOrWhiteSpace(filter)
                ? _allProperties
                : _allProperties.Where(p => p.Title.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

            var items = new List<PropertyListResponseDTO> { AllPropertiesItem };
            items.AddRange(filtered);

            cboProperty.SelectedIndexChanged -= cboProperty_SelectedIndexChanged;
            cboProperty.DataSource = null;
            cboProperty.DisplayMember = nameof(PropertyListResponseDTO.Title);
            cboProperty.ValueMember = nameof(PropertyListResponseDTO.Id);
            cboProperty.DataSource = items;
            var restoreIdx = previousId != 0 ? items.FindIndex(p => p.Id == previousId) : 0;
            cboProperty.SelectedIndex = restoreIdx >= 0 ? restoreIdx : 0;
            cboProperty.SelectedIndexChanged += cboProperty_SelectedIndexChanged;
        }

        private void ApplyFilter()
        {
            var selected = cboProperty.SelectedItem as PropertyListResponseDTO;
            var list = selected == null || selected.Id == 0
                ? _allReviews
                : _allReviews.Where(r => r.PropertyId == selected.Id).ToList();

            dgvReviews.DataSource = list;

            if (list.Count == 0)
            {
                lblStatus.Text = selected?.Id == 0
                    ? "No hay reseñas registradas."
                    : $"La propiedad '{selected?.Title}' no tiene reseñas.";
                btnDelete.Enabled = false;
            }
            else
            {
                lblStatus.Text = selected?.Id == 0
                    ? $"{list.Count} reseña(s) en total."
                    : $"{list.Count} reseña(s) para '{selected?.Title}'.";
                btnDelete.Enabled = true;
            }
        }

        private void txtPropertySearch_TextChanged(object sender, EventArgs e)
        {
            RebuildPropertyCombo(txtPropertySearch.Text);
        }

        private void cboProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private ReviewResponseDTO? GetSelected()
        {
            if (dgvReviews.CurrentRow?.DataBoundItem is ReviewResponseDTO r) return r;
            MessageBox.Show("Seleccione una reseña primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var r = GetSelected();
            if (r == null) return;

            var preview = r.Comment.Length > 120 ? r.Comment.Substring(0, 120) + "..." : r.Comment;
            if (MessageBox.Show(
                    $"¿Eliminar la reseña #{r.Id} de {r.UserName} (rating {r.Rating}/5)?\r\n\r\n\"{preview}\"\r\n\r\nEsta acción es irreversible.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                var ok = await Program.ReviewClient.DeleteReviewAsync(r.Id);
                if (!ok)
                {
                    MessageBox.Show("No se pudo eliminar la reseña.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadAllAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e) => await LoadAllAsync();
    }
}
