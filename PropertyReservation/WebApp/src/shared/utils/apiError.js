// Extrae un mensaje de error legible y amigable a partir de un error de Axios.
//
// El backend devuelve los errores en distintas formas y antes la UI mostraba
// siempre un texto genérico ("Error al subir las imágenes"), descartando el
// detalle que manda el servidor. Esta función centraliza la lógica para mostrar
// SIEMPRE el mensaje real del backend cuando existe. Contempla:
//   - string plano:             "El tipo de archivo .svg no esta permitido."   (ej. Conflict(ex.Message))
//   - objeto { message }:       { message: "Credenciales inválidas" }          (ej. AuthController)
//   - ValidationProblemDetails: { title, status, errors: { Campo: ["..."] } }  (validación de ModelState)
//   - ProblemDetails:           { title, detail, status }
//   - sin respuesta:            red caída, CORS, timeout, servidor apagado
//
// 'fallback' es el texto a mostrar cuando no se puede determinar nada mejor.

// Textos por defecto para códigos HTTP comunes cuando la respuesta no trae cuerpo.
const STATUS_FALLBACKS = {
    401: "Tu sesión expiró o no tenés autorización. Iniciá sesión nuevamente.",
    403: "No tenés permiso para realizar esta acción.",
    404: "No se encontró el recurso solicitado.",
    500: "Ocurrió un error en el servidor. Intentá nuevamente más tarde.",
};

export const getApiErrorMessage = (
    error,
    fallback = "Ocurrió un error inesperado. Intentá nuevamente."
) => {
    // Sin respuesta del servidor: problema de red, CORS, timeout o servidor caído.
    if (error?.response == null) {
        if (error?.code === "ERR_NETWORK") {
            return "No se pudo conectar con el servidor. Verificá tu conexión e intentá de nuevo.";
        }
        if (error?.code === "ECONNABORTED") {
            return "La solicitud tardó demasiado. Intentá nuevamente.";
        }
        return error?.message || fallback;
    }

    const data = error.response.data;

    // Cuerpo de texto plano: es la forma más común en esta API (BadRequest/Conflict/NotFound(ex.Message)).
    if (typeof data === "string" && data.trim() !== "") {
        return data.trim();
    }

    if (data && typeof data === "object") {
        // { message: "..." }
        if (typeof data.message === "string" && data.message.trim() !== "") {
            return data.message.trim();
        }
        // ValidationProblemDetails: { errors: { Campo: ["msg1", "msg2"], ... } }
        if (data.errors && typeof data.errors === "object") {
            const mensajes = Object.values(data.errors).flat().filter(Boolean);
            if (mensajes.length > 0) return mensajes.join(" ");
        }
        // ProblemDetails: { title, detail }
        if (typeof data.detail === "string" && data.detail.trim() !== "") {
            return data.detail.trim();
        }
        if (typeof data.title === "string" && data.title.trim() !== "") {
            return data.title.trim();
        }
    }

    // Sin mensaje en el cuerpo (ej. 403 Forbid sin texto): usamos un texto según el código HTTP.
    return STATUS_FALLBACKS[error.response.status] || fallback;
};
