import { useState, useEffect } from "react";
import { getPropertyImages, deletePropertyImage, uploadPropertyImages, setMainImage } from "@/features/property/services/propertyImageService";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faStar, faUpload, faImage, faXmark } from '@fortawesome/free-solid-svg-icons';
import { useToast } from "@/shared/ui/Toast";
import { useConfirm } from "@/shared/ui/ConfirmDialog";
import { getApiErrorMessage } from "@/shared/utils/apiError";

// Gestor de imágenes de una propiedad (usado en la pestaña "Imágenes" de la
// edición). Permite previsualizar y subir nuevas fotos, eliminarlas y marcar
// cuál es la principal. Avisa al padre con onImagesUpdated tras cada cambio.
export default function PropertyImagesManager({ propertyId, onImagesUpdated }) {
    const toast = useToast();
    const confirm = useConfirm();
    const [images, setImages] = useState([]);
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [previewUrls, setPreviewUrls] = useState([]);
    const [loading, setLoading] = useState(false);
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadImages();
    }, [propertyId]);

    const loadImages = async () => {
        try {
            setLoading(true);
            const data = await getPropertyImages(propertyId);
            setImages(data);
            if (onImagesUpdated) onImagesUpdated();
        } catch (err) {
            console.error("Error al cargar imágenes:", err);
            setError(getApiErrorMessage(err, "No se pudieron cargar las imágenes."));
        } finally {
            setLoading(false);
        }
    };

    // Al elegir archivos, generamos URLs temporales en memoria (createObjectURL)
    // para previsualizarlos antes de subirlos al servidor.
    const handleFileSelect = (e) => {
        const files = Array.from(e.target.files);
        setSelectedFiles(files);
        const urls = files.map(file => URL.createObjectURL(file));
        setPreviewUrls(urls);
    };

    const handleUpload = async () => {
        if (selectedFiles.length === 0) {
            toast("Por favor seleccioná al menos una imagen.", "warning");
            return;
        }

        try {
            setUploading(true);
            setError(null);
            await uploadPropertyImages(propertyId, selectedFiles);
            // Limpiamos la selección/preview y recargamos la galería desde el servidor.
            setSelectedFiles([]);
            setPreviewUrls([]);
            await loadImages();
            toast("Imágenes subidas correctamente.", "success");
        } catch (err) {
            console.error("Error al subir imágenes:", err);
            // Mostramos el mensaje real del backend (ej. "El tipo de archivo .svg no esta permitido.").
            toast(getApiErrorMessage(err, "No se pudieron subir las imágenes."), "error");
        } finally {
            setUploading(false);
        }
    };

    const handleDelete = async (imageId) => {
        const ok = await confirm("La imagen se eliminará permanentemente.", {
            title: "Eliminar imagen",
            confirmText: "Sí, eliminar",
            variant: "danger",
        });
        if (!ok) return;

        try {
            await deletePropertyImage(imageId);
            await loadImages();
        } catch (err) {
            console.error("Error al eliminar imagen:", err);
            toast(getApiErrorMessage(err, "No se pudo eliminar la imagen."), "error");
        }
    };

    const handleSetMain = async (imageId) => {
        try {
            await setMainImage(imageId);
            await loadImages();
        } catch (err) {
            console.error("Error al establecer imagen principal:", err);
            toast(getApiErrorMessage(err, "No se pudo establecer la imagen principal."), "error");
        }
    };

    const cancelSelection = () => {
        setSelectedFiles([]);
        setPreviewUrls([]);
    };

    if (loading) {
        return <p>Cargando imágenes...</p>;
    }

    return (
        <div>
            {error && (
                <div className="alert alert-danger alert-dismissible fade show" role="alert">
                    {error}
                    <button type="button" className="btn-close" onClick={() => setError(null)}></button>
                </div>
            )}

            {/* Sección de subida */}
            <div className="card mb-4">
                <div className="card-body">
                    <h5 className="card-title mb-3">
                        <FontAwesomeIcon icon={faUpload} className="me-2" />
                        Subir nuevas imágenes
                    </h5>
                    
                    <div className="mb-3">
                        <input
                            type="file"
                            className="form-control"
                            accept="image/*"
                            multiple
                            onChange={handleFileSelect}
                            disabled={uploading}
                        />
                        <small className="text-muted d-block">
                            Formatos permitidos: JPG, JPEG, PNG, WEBP
                        </small>
                        {images.length === 0 && (
                            <small className="text-info d-block mt-1">
                                💡 La primera imagen que subas será automáticamente la imagen principal de la propiedad
                            </small>
                        )}
                    </div>

                    {previewUrls.length > 0 && (
                        <div>
                            <h6 className="mb-3">Previsualización ({previewUrls.length} archivo{previewUrls.length !== 1 ? 's' : ''})</h6>
                            <div className="row g-3 mb-3">
                                {previewUrls.map((url, index) => (
                                    <div key={index} className="col-6 col-md-4 col-lg-3">
                                        <div className="card">
                                            <img
                                                src={url}
                                                alt={`Preview ${index + 1}`}
                                                className="card-img-top"
                                                style={{ height: '150px', objectFit: 'cover' }}
                                            />
                                            <div className="card-body p-2 text-center">
                                                <small className="text-muted">{selectedFiles[index].name}</small>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                            <div className="d-flex gap-2">
                                <button
                                    className="btn btn-primary"
                                    onClick={handleUpload}
                                    disabled={uploading}
                                >
                                    {uploading ? 'Subiendo...' : (
                                        <>
                                            <FontAwesomeIcon icon={faUpload} className="me-2" />
                                            Confirmar y subir
                                        </>
                                    )}
                                </button>
                                <button
                                    className="btn btn-outline-secondary"
                                    onClick={cancelSelection}
                                    disabled={uploading}
                                >
                                    <FontAwesomeIcon icon={faXmark} className="me-2" />
                                    Cancelar
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            </div>

            {/* Galería de imágenes existentes */}
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title mb-3">
                        <FontAwesomeIcon icon={faImage} className="me-2" />
                        Galería actual ({images.length} imagen{images.length !== 1 ? 'es' : ''})
                    </h5>

                    {images.length === 0 ? (
                        <div className="text-center text-muted py-5">
                            <FontAwesomeIcon icon={faImage} size="3x" className="mb-3 opacity-25" />
                            <p>No hay imágenes cargadas aún</p>
                            <p className="small">Sube tu primera imagen usando el formulario de arriba</p>
                        </div>
                    ) : (
                        <div className="row g-3">
                            {images.map((image) => (
                                <div key={image.id} className="col-6 col-md-4 col-lg-3">
                                    <div className={`card h-100 ${image.isMainImage ? 'border-primary' : ''}`}>
                                        <div className="position-relative">
                                            <img
                                                src={image.url}
                                                alt={image.fileName}
                                                className="card-img-top"
                                                style={{ height: '200px', objectFit: 'cover' }}
                                            />
                                            {image.isMainImage && (
                                                <div className="position-absolute top-0 start-0 m-2">
                                                    <span className="badge bg-primary">
                                                        <FontAwesomeIcon icon={faStar} className="me-1" />
                                                        Principal
                                                    </span>
                                                </div>
                                            )}
                                        </div>
                                        <div className="card-body p-2">
                                            <div className="d-flex gap-1 mb-2">
                                                <button
                                                    className={`btn btn-sm flex-grow-1 ${image.isMainImage ? 'btn-warning' : 'btn-outline-warning'}`}
                                                    onClick={() => !image.isMainImage && handleSetMain(image.id)}
                                                    title={image.isMainImage ? "Imagen principal" : "Establecer como principal"}
                                                    disabled={image.isMainImage}
                                                >
                                                    <FontAwesomeIcon icon={faStar} />
                                                </button>
                                                <button
                                                    className="btn btn-sm btn-outline-danger flex-grow-1"
                                                    onClick={() => handleDelete(image.id)}
                                                    title="Eliminar imagen"
                                                >
                                                    <FontAwesomeIcon icon={faTrash} />
                                                </button>
                                            </div>
                                            <small className="text-muted d-block text-truncate" title={image.fileName}>
                                                {image.fileName}
                                            </small>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
