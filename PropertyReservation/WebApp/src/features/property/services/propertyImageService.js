// Servicio de imágenes de propiedad: llamadas HTTP al controlador /PropertyImage.
// Permite ver, subir, marcar como principal y eliminar las fotos de una propiedad.
import { publicApi, privateApi } from "@/shared/api/axios";

const PROPERTYIMAGE_URL = "/PropertyImage";

// Obtiene las imágenes de una propiedad (consulta pública).
export const getPropertyImages = async (propertyId) => {
    const response = await publicApi.get(`${PROPERTYIMAGE_URL}/${propertyId}`);
    return response.data;
}

// Sube una o varias imágenes. Se empaquetan en un FormData (multipart) porque
// son archivos binarios, no JSON.
export const uploadPropertyImages = async (propertyId, files) => {
    const formData = new FormData();
    files.forEach(file => {
        formData.append('files', file);
    });
    const response = await privateApi.post(`${PROPERTYIMAGE_URL}/${propertyId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
    return response.data;
}

// Marca una imagen como la principal (la que se muestra en las tarjetas/listados).
export const setMainImage = async (id) => {
    const response = await privateApi.put(`${PROPERTYIMAGE_URL}/main/${id}`);
    return response.data;
}

// Elimina una imagen por su id.
export const deletePropertyImage = async (id) => {
    const response = await privateApi.delete(`${PROPERTYIMAGE_URL}/${id}`);
    return response.data;
}
