// Servicio de disponibilidad: llamadas HTTP al controlador /PropertyAvailability.
// Gestiona los rangos de fechas en los que el dueño habilita su propiedad.
import { publicApi, privateApi } from "@/shared/api/axios";

const PROPERTYAVAILABILITY_URL = "/PropertyAvailability";

// Obtiene los rangos de disponibilidad de una propiedad (consulta pública).
export const getPropertyAvailabilities = async (propertyId) => {
    const response = await publicApi.get(`${PROPERTYAVAILABILITY_URL}/${propertyId}`);
    return response.data;
}

// Agrega un nuevo rango de disponibilidad.
export const createPropertyAvailability = async (data) => {
    const response = await privateApi.post(PROPERTYAVAILABILITY_URL, data);
    return response.data;
}

// Modifica un rango de disponibilidad existente.
export const updatePropertyAvailability = async (id, data) => {
    const response = await privateApi.put(`${PROPERTYAVAILABILITY_URL}/${id}`, data);
    return response.data;
}

// Elimina un rango de disponibilidad.
export const deletePropertyAvailability = async (id) => {
    const response = await privateApi.delete(`${PROPERTYAVAILABILITY_URL}/${id}`);
    return response.data;
}
