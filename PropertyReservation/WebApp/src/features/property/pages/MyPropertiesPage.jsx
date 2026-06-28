// Página del dueño: lista sus propiedades y permite crear, editar y eliminar.
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getPropertiesByOwner, deleteProperty } from "@/features/property/services/propertyService";
import PropertyCard from "@/features/property/components/PropertyCard";
import { useConfirm } from "@/shared/ui/ConfirmDialog";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

export default function MyPropertiesPage() {
  const navigate = useNavigate();
  const confirm = useConfirm();
  const [properties, setProperties] = useState([]);
  const [errorMessage, setErrorMessage] = useState(false);

  useEffect(() => {
      loadProperties();
  }, []);

  const loadProperties = async () => {
      try {
          const data = await getPropertiesByOwner();
          setProperties(data);
      } catch (error) {
          console.error("Error cargando propiedades:", error);
          setErrorMessage("Error al cargar tus propiedades. Intenta nuevamente más tarde.");
      }
  };

  const handleEdit = (id) => {
      navigate(`/owner/properties/edit/${id}`);
  };

  const handleDelete = async (id) => {
    // Pedimos confirmación antes de borrar (acción destructiva e irreversible).
    const ok = await confirm("Esta acción no se puede deshacer.", {
      title: "Eliminar propiedad",
      confirmText: "Sí, eliminar",
      variant: "danger",
    });
    if (!ok) return;
    try {
      await deleteProperty(id);
      // Quitamos la propiedad del estado local para reflejar el borrado sin recargar.
      setProperties(properties.filter((p) => p.id !== id));
    } catch (error) {
      console.error("Error al eliminar propiedad:", error);
      setErrorMessage(getApiErrorMessage(error, "No se pudo eliminar la propiedad. Intentá nuevamente más tarde."));
    }
  };

  return (
    <div className="container-fluid mt-4">
      {errorMessage && (
        <div className="alert alert-danger alert-dismissible" role="alert">
          {errorMessage}
          <button type="button" className="btn-close" onClick={() => setErrorMessage(null)} />
        </div>
      )}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h3 className="mb-0">Mis propiedades</h3>
        <button
          className="btn btn-primary"
          onClick={() => navigate('/owner/properties/create')}
        >
          <FontAwesomeIcon icon={faPlus} className="me-2" />
          Crear propiedad
        </button>
      </div>

      <div className="d-flex flex-wrap gap-3 justify-content-center">
        {properties.length === 0 ? (
          <p>No tienes propiedades publicadas.</p>
        ) : (
          properties.map((p) => (
            <PropertyCard
              key={p.id}
              property={p}
              showActions={true}
              onDelete={handleDelete}
              onEdit={handleEdit}
            />
          ))
        )}
      </div>
    </div>
  );
}
