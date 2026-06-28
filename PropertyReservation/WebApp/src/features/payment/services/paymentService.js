// Servicio de pagos: llamadas HTTP al controlador /payments.
// Maneja la subida del comprobante (multipart), su descarga como imagen,
// y la revisión/aprobación por parte del dueño. Todo requiere autenticación.
import { privateApi } from "@/shared/api/axios";

// Crear un nuevo pago (Subir comprobante)
export const createPayment = async (paymentData) => {
    try {
        const response = await privateApi.post("/payments", paymentData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

// Obtener la imagen del comprobante (Como requiere Auth, la pedimos como Blob)
export const getPaymentProof = async (paymentId) => {
    try {
        const response = await privateApi.get(`/payments/${paymentId}/proof`, {
            responseType: 'blob' // Importante para manejar archivos
        });
        // Creamos una URL temporal para mostrar la imagen en el navegador
        return URL.createObjectURL(response.data);
    } catch (error) {
        throw error;
    }
};

// Obtener pagos pendientes (Para el Owner)
export const getPaymentsUnderReview = async () => {
    try {
        const response = await privateApi.get("/payments/underReview");
        return response.data;
    } catch (error) {
        throw error;
    }
};

// Cambiar estado del pago (Para el Owner)
export const changePaymentStatus = async (paymentId, paymentStatus) => {
    try {
        const response = await privateApi.patch(`/payments/${paymentId}/status`, null, {
            params: paymentStatus
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

// Eliminar pago (User)
export const deletePayment = async (paymentId) => {
    try {
        await privateApi.delete(`/payments/${paymentId}`);
    } catch (error) {
        throw error;
    }
};