// Servicio de autenticación: encapsula las llamadas HTTP al controlador /Auth
// del backend (login, registro y obtención del perfil del usuario logueado).
import { publicApi, privateApi } from "@/shared/api/axios";
import { getApiErrorMessage } from "@/shared/utils/apiError";

const AUTH_URL = "/Auth";

// Inicia sesión con email y contraseña; devuelve el token y los datos del usuario.
export const loginUser = async (email, password) => {
    try {
        const response = await publicApi.post(`${AUTH_URL}/login`, { email, password });
        return response.data; 
    } catch (error) {
        throw handleAuthError(error);
    }
};

// Registra un nuevo usuario a partir de los datos del formulario.
export const registerUser = async (userData) => {
    try {
        const response = await publicApi.post(`${AUTH_URL}/register`, userData);
        return response.data;
    } catch (error) {
        throw handleAuthError(error);
    }
};

// Obtiene el perfil del usuario autenticado (usa el token, endpoint privado /me).
export const getUserProfile = async () => {
    try {
        const response = await privateApi.get(`${AUTH_URL}/me`); 
        return response.data;
    } catch (error) {
        throw handleAuthError(error);
    }
};

// Helper para el manejo de errores: reusa getApiErrorMessage para extraer el
// mensaje del backend (incluyendo el { message } de AuthController) y lo envuelve
// en un Error, ya que LoginPage/RegisterPage leen err.message.
const handleAuthError = (error) => {
    return new Error(getApiErrorMessage(error, "Ha ocurrido un error inesperado."));
};