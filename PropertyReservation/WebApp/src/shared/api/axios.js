// src/services/api.js
// Configuración central de Axios. Define dos instancias HTTP reutilizables:
// 'publicApi' para endpoints abiertos y 'privateApi' para endpoints que
// requieren autenticación (inyecta el token JWT automáticamente).
import axios from "axios";

// URL base del backend. Todas las rutas de los servicios cuelgan de aquí.
const API_URL = "https://localhost:7099/api";

// Instancia para endpoints PÚBLICOS
// No lleva interceptor, nunca envía token.
export const publicApi = axios.create({
    baseURL: API_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

// Instancia para endpoints PRIVADOS
// Lleva el interceptor para inyectar el token JWT.
export const privateApi = axios.create({
    baseURL: API_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

// Solo configuramos el interceptor en la instancia privada
privateApi.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
