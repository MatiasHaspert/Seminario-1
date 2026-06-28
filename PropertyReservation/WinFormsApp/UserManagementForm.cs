using Application.DTOs.User;
using Domain.Enums;
using WinFormsClient;

namespace WinFormsApp
{
    public partial class UserManagementForm : Form
    {
        public UserManagementForm()
        {
            InitializeComponent();
            UiTheme.ApplyGridStyle(dgvUsers);
        }

        private async void UserManagementForm_Load(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                string? roleFilter = cboRoleFilter.SelectedIndex > 0 ? cboRoleFilter.SelectedItem?.ToString() : null;
                bool? activeFilter = cboStatusFilter.SelectedIndex switch
                {
                    1 => true,
                    2 => false,
                    _ => null
                };
                var email = string.IsNullOrWhiteSpace(txtEmailFilter.Text) ? null : txtEmailFilter.Text.Trim();

                var users = await Program.UsersClient.GetUsersAsync(email, roleFilter, activeFilter);
                var list = users?.OrderBy(u => u.Email).ToList();

                dgvUsers.DataSource = list;

                if (list == null || list.Count == 0)
                {
                    lblHint.Text = email != null || roleFilter != null || activeFilter != null
                        ? "Ningún usuario coincide con los criterios."
                        : "No hay usuarios registrados.";
                    SetActionsEnabled(false);
                }
                else
                {
                    lblHint.Text = $"{list.Count} usuario(s) encontrado(s).";
                    SetActionsEnabled(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo obtener la lista de usuarios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetActionsEnabled(false);
            }
        }

        private void SetActionsEnabled(bool enabled)
        {
            btnDetail.Enabled = enabled;
            btnChangeRole.Enabled = enabled;
            btnToggleStatus.Enabled = enabled;
        }

        private UserListDTO? GetSelected()
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserListDTO user) return user;
            MessageBox.Show("Seleccione un usuario primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private bool IsSelf(UserListDTO user)
        {
            var currentId = SessionManager.GetUserId();
            return currentId != null && currentId == user.Id.ToString();
        }

        private async void btnApplyFilter_Click(object sender, EventArgs e) => await LoadUsersAsync();

        private async void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtEmailFilter.Clear();
            cboRoleFilter.SelectedIndex = 0;
            cboStatusFilter.SelectedIndex = 0;
            await LoadUsersAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e) => await LoadUsersAsync();

        private async void btnDetail_Click(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;

            try
            {
                var detail = await Program.UsersClient.GetUserByIdAsync(user.Id);
                if (detail == null)
                {
                    MessageBox.Show("No se pudo obtener el detalle del usuario.", "Aviso");
                    return;
                }

                var msg = $"Id: {detail.Id}\r\n" +
                          $"Email: {detail.Email}\r\n" +
                          $"Nombre: {detail.Name} {detail.LastName}\r\n" +
                          $"Teléfono: {detail.Phone}\r\n" +
                          $"Rol: {detail.Role}\r\n" +
                          $"Estado: {(detail.IsActive ? "Activo" : "Deshabilitado")}\r\n" +
                          $"Fecha de alta: {detail.CreatedAt:dd/MM/yyyy HH:mm}\r\n\r\n" +
                          $"Reservas: {detail.ReservationsCount}\r\n" +
                          $"Propiedades publicadas: {detail.PropertiesCount}";

                MessageBox.Show(msg, "Detalle de usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnChangeRole_Click(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;

            if (IsSelf(user))
            {
                MessageBox.Show("No puede cambiar su propio rol.", "Operación no permitida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new ChangeRoleDialog(user.Email, user.Role);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            if (MessageBox.Show(
                    $"¿Confirma cambiar el rol de '{user.Email}' de {user.Role} a {dialog.SelectedRole}?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                await Program.UsersClient.UpdateUserRoleAsync(user.Id, new UpdateUserRoleDTO { Role = dialog.SelectedRole });
                MessageBox.Show("Rol actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnToggleStatus_Click(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;

            if (IsSelf(user))
            {
                MessageBox.Show("No puede deshabilitarse a sí mismo.", "Operación no permitida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var action = user.IsActive ? "deshabilitar" : "habilitar";
            if (MessageBox.Show($"¿Confirma {action} al usuario '{user.Email}'?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                await Program.UsersClient.UpdateUserStatusAsync(user.Id, new UpdateUserStatusDTO { IsActive = !user.IsActive });
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

    internal class ChangeRoleDialog : Form
    {
        private readonly ComboBox _cboRole;
        public Role SelectedRole { get; private set; }

        public ChangeRoleDialog(string email, string currentRole)
        {
            Text = "Cambiar rol";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(360, 150);

            var lbl = new Label
            {
                Text = $"Usuario: {email}\r\nRol actual: {currentRole}",
                Location = new Point(15, 15),
                AutoSize = true
            };

            _cboRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(15, 65),
                Width = 320
            };
            foreach (var r in Enum.GetNames(typeof(Role)))
            {
                if (!string.Equals(r, currentRole, StringComparison.OrdinalIgnoreCase))
                    _cboRole.Items.Add(r);
            }
            if (_cboRole.Items.Count > 0) _cboRole.SelectedIndex = 0;

            var btnOk = new Button { Text = "Aceptar", Location = new Point(175, 105), Size = new Size(75, 28), DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Location = new Point(260, 105), Size = new Size(75, 28), DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_cboRole.SelectedItem is string s && Enum.TryParse<Role>(s, out var role))
                {
                    SelectedRole = role;
                }
            };

            Controls.AddRange(new Control[] { lbl, _cboRole, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
