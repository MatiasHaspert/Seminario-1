// Servicio de comodidades (amenities): llamadas HTTP al controlador /Amenity.
// Provee el catálogo de comodidades para elegir al crear/editar una propiedad.
import { publicApi } from "@/shared/api/axios";

const AMENITY_URL = "/Amenity";

// Obtiene la lista completa de comodidades disponibles.
export const getAmenities = async () => {
    const response = await publicApi.get(AMENITY_URL);
    return response.data;
}
