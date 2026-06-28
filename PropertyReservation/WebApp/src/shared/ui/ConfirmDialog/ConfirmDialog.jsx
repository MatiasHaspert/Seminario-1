// Componente visual del diálogo de confirmación (la ventana modal en sí).
// Lo orquesta ConfirmContext; aquí solo se dibuja y se reciben onConfirm/onCancel.
// Incluye accesibilidad: foco inicial, cierre con Escape y roles ARIA.
import { useEffect, useRef } from "react";
import { createPortal } from "react-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faExclamationTriangle,
    faExclamationCircle,
    faQuestionCircle,
} from "@fortawesome/free-solid-svg-icons";
import "./ConfirmDialog.css";

// Estilos según la gravedad de la acción a confirmar (ícono, color y título).
const VARIANTS = {
    danger:  { icon: faExclamationTriangle, iconMod: "danger",  btnMod: "danger",  title: "Confirmar acción" },
    warning: { icon: faExclamationCircle,   iconMod: "warning", btnMod: "warning", title: "Atención"         },
    default: { icon: faQuestionCircle,      iconMod: "default", btnMod: "default", title: "Confirmar acción" },
};

export default function ConfirmDialog({
    message,
    title,
    confirmText = "Confirmar",
    cancelText  = "Cancelar",
    variant     = "default",
    onConfirm,
    onCancel,
}) {
    const cancelRef = useRef(null);
    const cfg = VARIANTS[variant] ?? VARIANTS.default;

    // Focus the cancel button on open for keyboard accessibility
    useEffect(() => { cancelRef.current?.focus(); }, []);

    // Close on Escape
    useEffect(() => {
        const handler = (e) => { if (e.key === "Escape") onCancel(); };
        document.addEventListener("keydown", handler);
        return () => document.removeEventListener("keydown", handler);
    }, [onCancel]);

    return createPortal(
        <div className="cd-backdrop" onClick={onCancel} role="presentation">
            <div
                className="cd-dialog"
                role="alertdialog"
                aria-modal="true"
                aria-labelledby="cd-title"
                aria-describedby="cd-message"
                onClick={(e) => e.stopPropagation()}
            >
                {/* Icon */}
                <div className={`cd-icon-wrap cd-icon-${cfg.iconMod}`}>
                    <FontAwesomeIcon icon={cfg.icon} />
                </div>

                {/* Text */}
                <h5 id="cd-title" className="cd-title">
                    {title ?? cfg.title}
                </h5>
                <p id="cd-message" className="cd-message">
                    {message}
                </p>

                {/* Actions */}
                <div className="cd-actions">
                    <button
                        ref={cancelRef}
                        className="cd-btn cd-btn-cancel"
                        onClick={onCancel}
                    >
                        {cancelText}
                    </button>
                    <button
                        className={`cd-btn cd-btn-${cfg.btnMod}`}
                        onClick={onConfirm}
                    >
                        {confirmText}
                    </button>
                </div>
            </div>
        </div>,
        document.body
    );
}
