// Contenedor visual de los toasts. Los renderiza con un portal directamente en
// <body> para que floten por encima de todo, sin verse afectados por el
// overflow o el z-index de los componentes donde se dispararon.
import { createPortal } from "react-dom";
import ToastItem from "./ToastItem";
import "./Toast.css";

export default function ToastContainer({ toasts, onDismiss }) {
    // Si no hay toasts, no renderizamos nada (ni el contenedor).
    if (!toasts.length) return null;

    return createPortal(
        <div className="toast-stack" role="region" aria-label="Notificaciones">
            {toasts.map(t => (
                <ToastItem key={t.id} toast={t} onDismiss={onDismiss} />
            ))}
        </div>,
        document.body
    );
}
