// Toast individual: muestra ícono, título y mensaje según el tipo, y maneja las
// animaciones de entrada y salida antes de que el contenedor lo elimine.
import { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faCheckCircle,
    faTimesCircle,
    faExclamationTriangle,
    faInfoCircle,
    faTimes,
} from "@fortawesome/free-solid-svg-icons";

// Ícono y etiqueta asociados a cada tipo de toast.
const CONFIG = {
    success: { icon: faCheckCircle,        label: "Éxito"      },
    error:   { icon: faTimesCircle,        label: "Error"      },
    warning: { icon: faExclamationTriangle, label: "Atención"  },
    info:    { icon: faInfoCircle,         label: "Info"       },
};

export default function ToastItem({ toast, onDismiss }) {
    const [visible, setVisible] = useState(false);
    const [leaving, setLeaving] = useState(false);

    useEffect(() => {
        // Activamos la clase de entrada en el siguiente frame para que la
        // transición CSS se dispare, y programamos la salida automática a los 3.8s.
        const enter = requestAnimationFrame(() => setVisible(true));
        const leave = setTimeout(() => handleDismiss(), 3800);
        // Limpiamos timers/frames si el componente se desmonta antes.
        return () => { cancelAnimationFrame(enter); clearTimeout(leave); };
    }, []);

    // Lanza la animación de salida y, una vez terminada (350ms), avisa al padre
    // para que lo quite definitivamente de la lista.
    const handleDismiss = () => {
        setLeaving(true);
        setTimeout(() => onDismiss(toast.id), 350);
    };

    const { icon, label } = CONFIG[toast.type] || CONFIG.info;

    return (
        <div
            className={`toast-item toast-${toast.type} ${visible ? "toast-enter" : ""} ${leaving ? "toast-leave" : ""}`}
            role="alert"
            aria-live="polite"
        >
            <span className="toast-icon">
                <FontAwesomeIcon icon={icon} />
            </span>
            <div className="toast-body">
                <span className="toast-label">{label}</span>
                <span className="toast-message">{toast.message}</span>
            </div>
            <button
                className="toast-close"
                onClick={handleDismiss}
                aria-label="Cerrar notificación"
            >
                <FontAwesomeIcon icon={faTimes} />
            </button>
            <span className="toast-progress" />
        </div>
    );
}
