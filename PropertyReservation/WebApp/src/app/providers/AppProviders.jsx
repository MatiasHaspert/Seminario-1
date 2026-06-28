import { AuthProvider } from "@/shared/auth/AuthContext";
import { ToastProvider } from "@/shared/ui/Toast";
import { ConfirmProvider } from "@/shared/ui/ConfirmDialog";

// Agrupa todos los proveedores de contexto globales en un solo lugar.
// El orden importa: AuthProvider queda más adentro porque puede llegar a usar
// las notificaciones (Toast) y los diálogos de confirmación (Confirm).
export default function AppProviders({ children }) {
    return (
        <ToastProvider>
            <ConfirmProvider>
                <AuthProvider>{children}</AuthProvider>
            </ConfirmProvider>
        </ToastProvider>
    );
}
