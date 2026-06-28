// Diálogo de confirmación global con API basada en promesas. Permite reemplazar
// el window.confirm() nativo por algo como:
//   if (await confirm("¿Borrar?")) { ... }
import { createContext, useContext, useState, useCallback } from "react";
import ConfirmDialog from "./ConfirmDialog";

const ConfirmContext = createContext(null);

export function ConfirmProvider({ children }) {
    const [dialog, setDialog] = useState(null);

    // Muestra el diálogo y devuelve una promesa que se resolverá cuando el
    // usuario elija. Guardamos 'resolve' en el estado para llamarlo después.
    const confirm = useCallback((message, options = {}) => {
        return new Promise((resolve) => {
            setDialog({ message, options, resolve });
        });
    }, []);

    // Resuelve la promesa con la respuesta (true=confirmar / false=cancelar)
    // y cierra el diálogo.
    const handleResponse = (answer) => {
        dialog?.resolve(answer);
        setDialog(null);
    };

    return (
        <ConfirmContext.Provider value={confirm}>
            {children}
            {dialog && (
                <ConfirmDialog
                    message={dialog.message}
                    {...dialog.options}
                    onConfirm={() => handleResponse(true)}
                    onCancel={() => handleResponse(false)}
                />
            )}
        </ConfirmContext.Provider>
    );
}

// Hook para pedir confirmación: const confirm = useConfirm(); await confirm("...").
export function useConfirm() {
    return useContext(ConfirmContext);
}
