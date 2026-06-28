using WinFormsClient;

namespace WinFormsApp
{
    internal static class Program
    {
        // HttpClient compartido para todas las API clients.
        private static readonly HttpClient httpClient = new HttpClient(new AuthenticationHandler
        {
            // Encargado de agregar el token JWT a cada solicitud después de un login exitoso.
            InnerHandler = new HttpClientHandler()
        })
        {
            BaseAddress = new Uri("https://localhost:7099")
        };

        public static AuthApiClient AuthClient { get; private set; } = null!;
        public static PropertyApiClient PropertyClient { get; private set; } = null!;
        public static ReservationApiClient ReservationClient { get; private set; } = null!;
        public static AmenityApiClient AmenityClient { get; private set; } = null!;
        public static ReviewApiClient ReviewClient { get; private set; } = null!;
        public static PropertyAvailabilityApiClient PropertyAvailabilityClient { get; private set; } = null!;
        public static PropertyImageApiClient PropertyImageClient { get; private set; } = null!;
        public static UsersApiClient UsersClient { get; private set; } = null!;
        public static PaymentsApiClient PaymentsClient { get; private set; } = null!;
        public static AdminApiClient AdminClient { get; private set; } = null!;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            InitializeApiClients();

            // Bucle login → dashboard → logout. Permite reingresar sin reiniciar el proceso.
            while (true)
            {
                using var loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK) break;

                using var dashboard = new DashboardForm();
                var result = dashboard.ShowDialog();

                // Si el usuario cerró sesión (DialogResult.Abort), volver al login.
                if (result == DialogResult.Abort) continue;

                break;
            }
        }

        private static void InitializeApiClients()
        {
            AuthClient = new AuthApiClient(httpClient);
            PropertyClient = new PropertyApiClient(httpClient);
            ReservationClient = new ReservationApiClient(httpClient);
            AmenityClient = new AmenityApiClient(httpClient);
            ReviewClient = new ReviewApiClient(httpClient);
            PropertyAvailabilityClient = new PropertyAvailabilityApiClient(httpClient);
            PropertyImageClient = new PropertyImageApiClient(httpClient);
            UsersClient = new UsersApiClient(httpClient);
            PaymentsClient = new PaymentsApiClient(httpClient);
            AdminClient = new AdminApiClient(httpClient);
        }
    }
}
