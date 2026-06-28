// Servicio de propiedades: llamadas HTTP al controlador /Property.
// Las consultas públicas (listar, ver, buscar) usan publicApi; las acciones del
// dueño (crear, editar, borrar, listar las propias) usan privateApi con token.
import { publicApi, privateApi } from "@/shared/api/axios";

const PROPERTY_URL = "/Property";

// Lista todas las propiedades publicadas.
export const getProperties = async () => {
    const response = await publicApi.get(PROPERTY_URL);
    return response.data;
}

// Obtiene el detalle completo de una propiedad por su id.
export const getPropertyDetails = async (id) => {
    const response = await publicApi.get(`${PROPERTY_URL}/${id}`);
    return response.data;
}

// Busca propiedades según filtros (ciudad, fechas, huéspedes, etc.),
// enviados como query params (?city=...&guests=...).
export async function searchProperties(filters) {
    const response = await publicApi.get(`${PROPERTY_URL}/search`, { params: filters });
    return response.data;
}

// Lista las propiedades del dueño autenticado.
export const getPropertiesByOwner = async () => {
    const response = await privateApi.get(`${PROPERTY_URL}/my`);
    return response.data;
}

// Crea una nueva propiedad.
export const createProperty = async (propertyData) => {
    const response = await privateApi.post(PROPERTY_URL, propertyData);
    return response.data;
}

// Actualiza una propiedad existente.
export const updateProperty = async (id, propertyData) => {
    const response = await privateApi.put(`${PROPERTY_URL}/${id}`, propertyData);
    return response.data;
}

// Elimina una propiedad por su id.
export const deleteProperty = async (id) => {
    const response = await privateApi.delete(`${PROPERTY_URL}/${id}`);
    return response.data;
}