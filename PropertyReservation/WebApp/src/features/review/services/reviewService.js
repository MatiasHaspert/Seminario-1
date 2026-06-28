// Servicio de reseñas: llamadas HTTP al controlador /Review.
// La lectura es pública; crear o editar una reseña requiere autenticación.
import { publicApi, privateApi } from "@/shared/api/axios";

const REVIEW_URL = "/Review";

// Obtiene las reseñas de una propiedad concreta.
export const getPropertyReviews = async (propertyId) => {
    const response = await publicApi.get(REVIEW_URL, { params: { propertyId } });
    return response.data;
};

// Obtiene una reseña puntual por su id.
export const getReviewById = async (id) => {
    const response = await publicApi.get(`${REVIEW_URL}/${id}`);
    return response.data;
};

// Crea una reseña (puntaje + comentario) asociada a una reserva completada.
export const createReview = async (reviewData) => {
    const response = await privateApi.post(REVIEW_URL, reviewData);
    return response.data;
};

// Edita una reseña existente del usuario.
export const updateReview = async (id, reviewData) => {
    const response = await privateApi.put(`${REVIEW_URL}/${id}`, reviewData);
    return response.data;
};
