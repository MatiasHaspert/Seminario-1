import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createProperty } from "@/features/property/services/propertyService";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import PropertyForm from "@/features/property/components/PropertyForm";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheckCircle, faImages, faCalendarAlt, faList, faArrowRight, faInfoCircle } from '@fortawesome/free-solid-svg-icons';

// Alta de propiedad en dos pasos. Paso 1 (esta página): se cargan los datos
// básicos. Tras crearla, se muestra una pantalla de éxito con accesos directos
// al paso 2 (imágenes / disponibilidad) en la página de edición.
export default function CreatePropertyPage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  // Guarda el id devuelto por el backend; su presencia alterna a la vista de éxito.
  const [createdPropertyId, setCreatedPropertyId] = useState(null);

  const handleSubmit = async (payload) => {
    setError(null);
    setLoading(true);

    try {
      const created = await createProperty({ ...payload});
      console.log("Propiedad creada:", created);
      setCreatedPropertyId(created?.id);
    } catch (err) {
      console.error(err);
      setError(getApiErrorMessage(err, "Error creando la propiedad. Intentá nuevamente más tarde."));
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    navigate(-1);
  };

  return (
    <div className="container mt-4">
      {/* Sin id => formulario (paso 1). Con id => pantalla de éxito (paso 2). */}
      {!createdPropertyId ? (
        <>
          <div className="d-flex justify-content-between align-items-center mb-3">
            <h3 className="mb-0">Nueva propiedad — Paso 1 de 2</h3>
          </div>

          <PropertyForm
            mode="create"
            onSubmit={handleSubmit}
            onCancel={handleCancel}
            loading={loading}
            error={error}
          />
        </>
      ) : (
        <>
          <div className="row justify-content-center">
            <div className="col-lg-10">
              <div className="card shadow-lg border-0">
                <div className="card-body p-5 text-center">
                  {/* Success Icon and Message */}
                  <div className="mb-4">
                    <div className="d-inline-flex align-items-center justify-content-center bg-success bg-opacity-10 rounded-circle p-3 mb-3">
                      <FontAwesomeIcon icon={faCheckCircle} className="text-success" style={{ fontSize: '2rem' }} />
                    </div>
                    <h4 className="fw-bold text-success mb-2">¡Propiedad creada exitosamente!</h4>
                    <p className="text-muted">
                      Ahora puedes completar la información de tu propiedad
                    </p>
                  </div>

                  <hr className="my-4" />

                  {/* Action Cards */}
                  <div className="row g-4 mt-3">
                    <div className="col-md-4">
                      <div 
                        className="card h-100 border-primary shadow-sm hover-card" 
                        style={{ cursor: 'pointer', transition: 'all 0.3s ease' }}
                        onClick={() => navigate(`/owner/properties/edit/${createdPropertyId}?tab=images`)}
                        onMouseEnter={(e) => {
                          e.currentTarget.style.transform = 'translateY(-5px)';
                          e.currentTarget.style.boxShadow = '0 0.5rem 1rem rgba(0,123,255,0.3)';
                        }}
                        onMouseLeave={(e) => {
                          e.currentTarget.style.transform = 'translateY(0)';
                          e.currentTarget.style.boxShadow = '';
                        }}
                      >
                        <div className="card-body d-flex flex-column align-items-center p-4">
                          <div className="bg-primary bg-opacity-10 rounded-circle p-3 mb-3">
                            <FontAwesomeIcon icon={faImages} className="text-primary" style={{ fontSize: '2rem' }} />
                          </div>
                          <h6 className="card-title fw-bold mb-2">Agregar Imágenes</h6>
                          <p className="card-text text-muted small mb-3">
                            Sube fotos atractivas de tu propiedad
                          </p>
                          <button className="btn btn-primary mt-auto px-4">
                            Comenzar <FontAwesomeIcon icon={faArrowRight} className="ms-2" />
                          </button>
                        </div>
                      </div>
                    </div>

                    <div className="col-md-4">
                      <div 
                        className="card h-100 border-info shadow-sm hover-card" 
                        style={{ cursor: 'pointer', transition: 'all 0.3s ease' }}
                        onClick={() => navigate(`/owner/properties/edit/${createdPropertyId}?tab=availability`)}
                        onMouseEnter={(e) => {
                          e.currentTarget.style.transform = 'translateY(-5px)';
                          e.currentTarget.style.boxShadow = '0 0.5rem 1rem rgba(23,162,184,0.3)';
                        }}
                        onMouseLeave={(e) => {
                          e.currentTarget.style.transform = 'translateY(0)';
                          e.currentTarget.style.boxShadow = '';
                        }}
                      >
                        <div className="card-body d-flex flex-column align-items-center p-4">
                          <div className="bg-info bg-opacity-10 rounded-circle p-3 mb-3">
                            <FontAwesomeIcon icon={faCalendarAlt} className="text-info" style={{ fontSize: '2rem' }} />
                          </div>
                          <h6 className="card-title fw-bold mb-2">Configurar Disponibilidad</h6>
                          <p className="card-text text-muted small mb-3">
                            Define fechas disponibles y precios
                          </p>
                          <button className="btn btn-info mt-auto px-4">
                            Configurar <FontAwesomeIcon icon={faArrowRight} className="ms-2" />
                          </button>
                        </div>
                      </div>
                    </div>

                    <div className="col-md-4">
                      <div 
                        className="card h-100 border-success shadow-sm hover-card" 
                        style={{ cursor: 'pointer', transition: 'all 0.3s ease' }}
                        onClick={() => navigate("/owner/properties")}
                        onMouseEnter={(e) => {
                          e.currentTarget.style.transform = 'translateY(-5px)';
                          e.currentTarget.style.boxShadow = '0 0.5rem 1rem rgba(40,167,69,0.3)';
                        }}
                        onMouseLeave={(e) => {
                          e.currentTarget.style.transform = 'translateY(0)';
                          e.currentTarget.style.boxShadow = '';
                        }}
                      >
                        <div className="card-body d-flex flex-column align-items-center p-4">
                          <div className="bg-success bg-opacity-10 rounded-circle p-3 mb-3">
                            <FontAwesomeIcon icon={faList} className="text-success" style={{ fontSize: '2rem' }} />
                          </div>
                          <h6 className="card-title fw-bold mb-2">Ir a Mis Propiedades</h6>
                          <p className="card-text text-muted small mb-3">
                            Ver y gestionar todas tus propiedades
                          </p>
                          <button className="btn btn-success mt-auto px-4">
                            Ver lista <FontAwesomeIcon icon={faArrowRight} className="ms-2" />
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>

                  {/* Footer Note */}
                  <div className="mt-5 pt-4 border-top">
                    <p className="text-muted small mb-0">
                      <FontAwesomeIcon icon={faInfoCircle} className="me-2" />
                      Puedes completar esta información en cualquier momento desde tu panel de propiedades
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
