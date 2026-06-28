// Servicio de reservas: llamadas HTTP al controlador /Reservation.
// Todas requieren autenticación (un huésped reserva, un dueño gestiona).
import { publicApi, privateApi } from "@/shared/api/axios";

const RESERVATION_URL = "/Reservation";

// Crea una nueva reserva para una propiedad y un rango de fechas.
export const createReservation = async (reservationData) => {
    const response = await privateApi.post(RESERVATION_URL, reservationData);
    return response.data;
}

// Obtiene el detalle de una reserva puntual por su id.
export const getReservationById = async (reservationId) => {
    const response = await privateApi.get(`${RESERVATION_URL}/${reservationId}`);
    return response.data;
}

// Obtener todas las reservas del usuario actual (vista del huésped).
export const getMyReservations = async () => {
    const response = await privateApi.get(`${RESERVATION_URL}/my-reservations`);
    return response.data;
};


// Lista las reservas de una propiedad concreta (para mostrar fechas ocupadas).
export const getReservationsBypropertyId = async (propertyId) => {
    const response = await privateApi.get(`${RESERVATION_URL}/by-property/${propertyId}`);
    return response.data;
};

// Cambia el estado de una reserva (ver ReservationStatus en shared/constants/enums).
export const changeReservationStatus = async (reservationId, status) => {
    const response = await privateApi.patch(`${RESERVATION_URL}/${reservationId}/status`, { status });
    return response.data;
}

// Lista todas las reservas de las propiedades del dueño autenticado.
export const getReservationsByOwner = async () => {
    const response = await privateApi.get(`${RESERVATION_URL}/owner`);
    return response.data;
}

/* Metodo para cancelar una reserva 
export const cancelReservation = async (id) => {
    try {
        await privateApi.put(`/reservations/${id}/cancel`);
    } catch (error) {
        throw error;
    }
};
*/