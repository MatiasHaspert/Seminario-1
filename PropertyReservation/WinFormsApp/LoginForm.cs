using System.Text.RegularExpressions;
using Application.DTOs.User;
using WinFormsClient;

namespace WinFormsApp
{
    public partial class LoginForm : Form
    {
        // Regex razonable para email (no estricta RFC, pero suficiente para validación de UI).
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            HideError();
            ActiveControl = txtEmail;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            HideError();

            var email = txtEmail.Text.Trim();
            var password = txtPassword.Text;

            // Validación local antes de pegarle a la API.
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Ingrese un email válido y una contraseña.");
                FocusEmpty();
                return;
            }

            if (!EmailRegex.IsMatch(email))
            {
                ShowError("El formato del email no es válido.");
                txtEmail.Focus();
                return;
            }

            SetBusy(true);

            try
            {
                var response = await Program.AuthClient.LoginAsync(new UserLoginDTO
                {
                    Email = email,
                    Password = password
                });

                if (response == null)
                {
                    // Credenciales incorrectas.
                    ShowError("Email o contraseña incorrectos.");
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // El rol debe ser exactamente Admin.
                var userRole = SessionManager.GetUserRole();
                if (!string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    Program.AuthClient.Logout();
                    ShowError("Esta aplicación es de uso exclusivo para administradores.");
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException ex)
            {
                // sin conexión / servicio caído.
                ShowError($"No se pudo contactar al servidor. {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"Error inesperado: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            btnLogin.Enabled = !busy;
            btnLogin.Text = busy ? "Iniciando sesión..." : "Iniciar sesión";
            txtEmail.Enabled = !busy;
            txtPassword.Enabled = !busy;
            chkShowPassword.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void HideError()
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }

        private void FocusEmpty()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Focus();
            }
        }
    }
}
