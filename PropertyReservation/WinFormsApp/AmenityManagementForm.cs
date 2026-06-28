using Application.DTOs.Amenity;

namespace WinFormsApp
{
    public partial class AmenityManagementForm : Form
    {
        public AmenityManagementForm()
        {
            InitializeComponent();
            UiTheme.ApplyGridStyle(dgvAmenities);
        }

        private async void AmenityManagementForm_Load(object sender, EventArgs e)
        {
            await LoadAmenitiesAsync();
        }

        private async Task LoadAmenitiesAsync()
        {
            lblStatus.Text = "Cargando amenities...";
            try
            {
                var amenities = await Program.AmenityClient.GetAllAmenitiesAsync();
                var list = amenities?.OrderBy(a => a.Name).ToList();
                dgvAmenities.DataSource = list;

                if (list == null || list.Count == 0)
                {
                    lblStatus.Text = "No hay amenities. Cree uno con \"Crear\".";
                    btnEdit.Enabled = btnDelete.Enabled = false;
                }
                else
                {
                    lblStatus.Text = $"{list.Count} amenity(ies).";
                    btnEdit.Enabled = btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

        private AmenityResponseDTO? GetSelected()
        {
            if (dgvAmenities.CurrentRow?.DataBoundItem is AmenityResponseDTO a) return a;
            MessageBox.Show("Seleccione un amenity primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using var dialog = new AmenityEditDialog(title: "Nuevo amenity", initialName: string.Empty);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                var result = await Program.AmenityClient.CreateAmenityAsync(new AmenityRequestDTO { Name = dialog.NameValue });
                if (result == null)
                {
                    MessageBox.Show("No se pudo crear el amenity. Verifique que el nombre no esté duplicado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadAmenitiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            var a = GetSelected();
            if (a == null) return;

            using var dialog = new AmenityEditDialog(title: "Editar amenity", initialName: a.Name);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            if (string.Equals(dialog.NameValue, a.Name, StringComparison.Ordinal))
            {
                MessageBox.Show("No se realizaron cambios.", "Aviso");
                return;
            }

            try
            {
                var ok = await Program.AmenityClient.UpdateAmenityAsync(a.Id, new AmenityRequestDTO { Name = dialog.NameValue });
                if (!ok)
                {
                    MessageBox.Show("No se pudo actualizar el amenity.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadAmenitiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var a = GetSelected();
            if (a == null) return;

            if (MessageBox.Show(
                    $"¿Eliminar el amenity '{a.Name}'?\r\n" +
                    "Si está asignado a propiedades, será removido de todas ellas.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                var ok = await Program.AmenityClient.DeleteAmenityAsync(a.Id);
                if (!ok)
                {
                    MessageBox.Show("No se pudo eliminar el amenity.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadAmenitiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    internal class AmenityEditDialog : Form
    {
        private readonly TextBox _txtName;
        public string NameValue => _txtName.Text.Trim();

        public AmenityEditDialog(string title, string initialName)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(360, 130);

            var lbl = new Label { Text = "Nombre:", Location = new Point(15, 18), AutoSize = true };
            _txtName = new TextBox { Location = new Point(80, 14), Width = 260, Text = initialName, MaxLength = 100 };

            var btnOk = new Button { Text = "Guardar", Location = new Point(175, 80), Size = new Size(80, 30) };
            var btnCancel = new Button { Text = "Cancelar", Location = new Point(260, 80), Size = new Size(80, 30), DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (NameValue.Length < 2)
                {
                    MessageBox.Show("El nombre debe tener al menos 2 caracteres.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.AddRange(new Control[] { lbl, _txtName, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
