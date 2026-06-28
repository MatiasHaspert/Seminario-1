// Sistema de notificaciones "toast" global. El provider mantiene la lista de
// toasts activos y expone la función toast() a través de useToast() para que
// cualquier componente pueda mostrar mensajes sin pasar props manualmente.
import { createContext, useContext, useState, useCallback } from "react";
import ToastContainer from "./ToastContainer";

const ToastContext = createContext(null);

export function ToastProvider({ children }) {
    const [toasts, setToasts] = useState([]);

    // Agrega un toast a la pila y programa su autodescarte a los ~4.2s.
    // El id combina timestamp + aleatorio para evitar choques si se disparan juntos.
    const toast = useCallback((message, type = "info") => {
        const id = Date.now() + Math.random();
        setToasts(prev => [...prev, { id, message, type }]);
        setTimeout(() => setToasts(prev => prev.filter(t => t.id !== id)), 4200);
    }, []);

    // Quita un toast manualmente (al hacer clic en cerrar).
    const dismiss = useCallback((id) => {
        setToasts(prev => prev.filter(t => t.id !== id));
    }, []);

    return (
        <ToastContext.Provider value={toast}>
            {children}
            <ToastContainer toasts={toasts} onDismiss={dismiss} />
        </ToastContext.Provider>
    );
}

// Hook para disparar notificaciones: const toast = useToast(); toast("Hola", "success").
export function useToast() {
    return useContext(ToastContext);
}
