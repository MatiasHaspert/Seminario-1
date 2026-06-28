import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import AppProviders from "@/app/providers/AppProviders";
import AppRouter from "@/app/router/AppRouter";

// Componente raíz de la aplicación.
// Envuelve el sistema de rutas (AppRouter) con los proveedores de contexto
// globales (AppProviders) para que estén disponibles en toda la app.
function App() {
    return (
        <AppProviders>
            <AppRouter />
        </AppProviders>
    );
}

export default App;
