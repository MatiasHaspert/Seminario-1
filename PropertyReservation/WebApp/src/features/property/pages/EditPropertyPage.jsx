import { useState, useEffect } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { getPropertyDetails, updateProperty } from "@/features/property/services/propertyService";
import { useToast } from "@/shared/ui/Toast";
import { useConfirm } from "@/shared/ui/ConfirmDialog";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { getPropertyImages } from "@/features/property/services/propertyImageService";
import PropertyForm from "@/features/property/components/PropertyForm";
import PropertyImagesManager from "@/features/property/components/PropertyImagesManager";
import PropertyAvailabilityManager from "@/features/property/components/PropertyAvailabilityManager";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faCircleInfo, faImage, faImages, faCalendarDays } from '@fortawesome/free-solid-svg-icons';

// Página de edición de propiedad, organizada en pestañas: Información, Imágenes y
// Disponibilidades. La pestaña inicial puede venir indicada en la URL (?tab=...),
// lo que permite enlazar directo a una sección desde otras pantallas.
export default function PropertyEditPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const toast = useToast();
  const confirm = useConfirm();
  const [searchParams] = useSearchParams();
  // Pestaña activa; arranca en la indicada por la URL o en "info" por defecto.
  const [activeTab, setActiveTab] = useState(searchParams.get("tab") || "info");
  const [loading, setLoading] = useState(false);
  const [loadingData, setLoadingData] = useState(true);
  const [error, setError] = useState(null);
  const [propertyData, setPropertyData] = useState(null);
  const [imageCount, setImageCount] = useState(0);
  // Marca si el formulario tiene cambios sin guardar, para avisar antes de salir.
  const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);

  // Cargar datos de la propiedad al montar
  useEffect(() => {
    const loadProperty = async () => {
      try {
        const data = await getPropertyDetails(id);
        setPropertyData(data);
        
        // Cargar cantidad de imágenes
        try {
          const images = await getPropertyImages(id);
          setImageCount(images.length);
        } catch (err) {
          console.error("Error loading images:", err);
        }
      } catch (err) {
        console.error(err);
        setError("No se pudo cargar la propiedad.");
      } finally {
        setLoadingData(false);
      }
    };

    if (id) {
      loadProperty();
    }
  }, [id]);

  const handleSubmit = async (payload) => {
    setError(null);
    setLoading(true);

    try {
      await updateProperty(id, payload);
      setHasUnsavedChanges(false);
      toast("Propiedad actualizada exitosamente.", "success");
      // Recargar datos
      const data = await getPropertyDetails(id);
      setPropertyData(data);
    } catch (err) {
      console.error(err);
      setError(getApiErrorMessage(err, "Error actualizando la propiedad. Intentá nuevamente más tarde."));
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = async () => {
    // Si hay cambios pendientes, pedimos confirmación para no perderlos sin querer.
    if (hasUnsavedChanges) {
      const ok = await confirm("Tenés cambios sin guardar que se perderán.", {
        title: "Cambios sin guardar",
        confirmText: "Salir de todas formas",
        cancelText: "Seguir editando",
        variant: "warning",
      });
      if (!ok) return;
    }
    navigate("/owner/properties");
  };

  const handleImagesUpdated = async () => {
    // Actualizar contador de imágenes
    try {
      const images = await getPropertyImages(id);
      setImageCount(images.length);
    } catch (err) {
      console.error("Error loading images:", err);
    }
  };

  if (loadingData) {
    return (
      <div className="container mt-4">
        <p>Cargando propiedad...</p>
      </div>
    );
  }

  if (!propertyData) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">No se encontró la propiedad.</div>
        <button className="btn btn-secondary" onClick={() => navigate('/owner/properties')}>
          <FontAwesomeIcon icon={faArrowLeft} className="me-2" />
          Volver a Mis propiedades
        </button>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h3 className="mb-0">Editar propiedad</h3>
        <button className="btn btn-outline-secondary" onClick={handleCancel}>
          <FontAwesomeIcon icon={faArrowLeft} className="me-2" />
          Volver a Mis propiedades
        </button>
      </div>

      {/* Alert si faltan cosas por completar */}
      {imageCount === 0 && (
        <div className="alert alert-warning d-flex align-items-center" role="alert">
          <span className="me-2">⚠️</span>
          <div>
            <strong>¡Atención!</strong> Esta propiedad no tiene imágenes.{" "}
            <button 
              className="btn btn-link p-0 align-baseline"
              onClick={() => setActiveTab("images")}
            >
              <FontAwesomeIcon icon={faImage} className="me-1" />
              Agregar imágenes ahora
            </button>
          </div>
        </div>
      )}

      {/* Tabs Navigation */}
      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button
            className={`nav-link ${activeTab === "info" ? "active" : ""}`}
            onClick={() => setActiveTab("info")}
          >
            <FontAwesomeIcon icon={faCircleInfo} className="me-2" />
            Información
          </button>
        </li>
        <li className="nav-item">
          <button
            className={`nav-link ${activeTab === "images" ? "active" : ""}`}
            onClick={() => setActiveTab("images")}
          >
            <FontAwesomeIcon icon={faImages} className="me-2" />
            Imágenes {imageCount > 0 && <span className="badge bg-primary ms-1">{imageCount}</span>}
          </button>
        </li>
        <li className="nav-item">
          <button
            className={`nav-link ${activeTab === "availability" ? "active" : ""}`}
            onClick={() => setActiveTab("availability")}
          >
            <FontAwesomeIcon icon={faCalendarDays} className="me-2" />
            Disponibilidades
          </button>
        </li>
      </ul>

      {/* Tab Content */}
      {activeTab === "info" && (
        <PropertyForm
          mode="edit"
          initialData={propertyData}
          onSubmit={handleSubmit}
          onCancel={handleCancel}
          onFormChange={setHasUnsavedChanges}
          loading={loading}
          error={error}
        />
      )}

      {activeTab === "images" && (
        <PropertyImagesManager 
          propertyId={id}
          onImagesUpdated={handleImagesUpdated}
        />
      )}

      {activeTab === "availability" && (
        <PropertyAvailabilityManager 
          propertyId={id}
        />
      )}
    </div>
  );
}
