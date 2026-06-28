import { useState, useEffect } from "react";
import { getAmenities } from "@/features/property/services/amenityService";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFloppyDisk, faXmark } from '@fortawesome/free-solid-svg-icons';

/**
 * Componente reutilizable de formulario de propiedad
 * @param {Object} props
 * @param {string} props.mode - 'create' o 'edit'
 * @param {Object} props.initialData - Datos iniciales del formulario (para modo edit)
 * @param {Function} props.onSubmit - Callback al enviar (recibe payload)
 * @param {Function} props.onCancel - Callback al cancelar
 * @param {boolean} props.loading - Estado de carga
 * @param {string} props.error - Mensaje de error a mostrar
 * @param {Function} props.onFormChange - Callback cuando cambia el formulario
 */
export default function PropertyForm({ 
  mode = 'create', 
  initialData = null, 
  onSubmit, 
  onCancel, 
  loading = false, 
  error = null,
  onFormChange = null
}) {
  const [form, setForm] = useState({
    title: "",
    description: "",
    nightlyPrice: "",
    maxGuests: 1,
    bedrooms: 0,
    bathrooms: 0,
    city: "",
    state: "",
    country: "",
    streetAddress: "",
    postalCode: "",
    amenityIds: [],
  });

  const [availableAmenities, setAvailableAmenities] = useState([]);
  const [loadingAmenities, setLoadingAmenities] = useState(true);
  const [amenitiesExpanded, setAmenitiesExpanded] = useState(false);

  // Cargar amenities disponibles
  useEffect(() => {
    const loadAmenities = async () => {
      try {
        const amenities = await getAmenities();
        setAvailableAmenities(amenities);
      } catch (err) {
        console.error("Error cargando amenities:", err);
      } finally {
        setLoadingAmenities(false);
      }
    };
    loadAmenities();
  }, []);

  // Pre-llenar formulario si hay datos iniciales (modo edición)
  useEffect(() => {
    if (initialData) {
      setForm({
        title: initialData.title || "",
        description: initialData.description || "",
        nightlyPrice: initialData.nightlyPrice || "",
        maxGuests: initialData.maxGuests || 1,
        bedrooms: initialData.bedrooms || 0,
        bathrooms: initialData.bathrooms || 0,
        city: initialData.city || "",
        state: initialData.state || "",
        country: initialData.country || "",
        streetAddress: initialData.streetAddress || "",
        postalCode: initialData.postalCode || "",
        amenityIds: initialData.amenities?.map(a => a.id) || [],
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
    if (onFormChange) onFormChange(true);
  };

  const handleNumberChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
    if (onFormChange) onFormChange(true);
  };

  const handleAddressChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
    if (onFormChange) onFormChange(true);
  };

  // Agrega o quita una comodidad de la selección según si ya estaba marcada.
  const handleAmenityToggle = (amenityId) => {
    setForm((f) => {
      const isSelected = f.amenityIds.includes(amenityId);
      return {
        ...f,
        amenityIds: isSelected
          ? f.amenityIds.filter(id => id !== amenityId)
          : [...f.amenityIds, amenityId]
      };
    });
    if (onFormChange) onFormChange(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validación mínima: título y dirección completa son obligatorios.
    if (!form.title) {
      return;
    }
    if (!form.city || !form.country || !form.streetAddress || !form.state || !form.postalCode) {
      return;
    }

    // Construimos el payload convirtiendo a número los campos numéricos (los
    // inputs siempre devuelven strings) y usando valores por defecto seguros.
    const payload = {
      title: form.title,
      description: form.description,
      nightlyPrice: form.nightlyPrice ? Number(form.nightlyPrice) : 0,
      maxGuests: form.maxGuests ? parseInt(form.maxGuests) : 1,
      bedrooms: form.bedrooms ? parseInt(form.bedrooms) : 0,
      bathrooms: form.bathrooms ? parseInt(form.bathrooms) : 0,
      city: form.city,
      state: form.state,
      country: form.country,
      streetAddress: form.streetAddress,
      postalCode: form.postalCode ? parseInt(form.postalCode) : 0,
      amenityIds: form.amenityIds,
    };

    // Llamar al callback pasado por el padre
    if (onSubmit) {
      onSubmit(payload);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="row g-3 text-start">
      {error && <div className="col-12"><div className="alert alert-danger">{error}</div></div>}

      <div className="col-12">
        <label className="form-label">Título</label>
        <input
          name="title"
          className="form-control"
          value={form.title}
          onChange={handleChange}
          required
        />
      </div>

      <div className="col-md-3">
        <label className="form-label">Precio por noche (USD)</label>
        <div className="input-group">
          <span className="input-group-text">$</span>
          <input
            name="nightlyPrice"
            type="number"
            step="0.01"
            min="0.1"
            className="form-control"
            placeholder="Ej: 120.00"
            value={form.nightlyPrice}
            onChange={handleNumberChange}
            required
          />
        </div>
      </div>
      <div className="col-md-3">
        <label className="form-label">Huéspedes máx.</label>
        <input
          name="maxGuests"
          type="number"
          min="1"
          className="form-control"
          value={form.maxGuests}
          onChange={handleNumberChange}
          required
        />
      </div>
      <div className="col-md-3">
        <label className="form-label">Habitaciones</label>
        <input
          name="bedrooms"
          type="number"
          min="0"
          className="form-control"
          value={form.bedrooms}
          onChange={handleNumberChange}
          required
        />
      </div>
      <div className="col-md-3">
        <label className="form-label">Baños</label>
        <input
          name="bathrooms"
          type="number"
          min="0"
          className="form-control"
          value={form.bathrooms}
          onChange={handleNumberChange}
          required
        />
      </div>

      {/* Descripción */}
      <div className="col-12 mt-2">
        <label className="form-label">Descripción</label>
        <textarea
          name="description"
          className="form-control"
          value={form.description}
          onChange={handleChange}
          rows={4}
        />
      </div>

      {/* Ubicación */}
      <div className="col-12 mt-2">
        <h6 className="text-muted">Ubicación</h6>
      </div>
      <div className="col-md-4">
        <label className="form-label">Ciudad</label>
        <input
          name="city"
          className="form-control"
          placeholder="Ej: Rosario"
          value={form.city}
          onChange={handleAddressChange}
          maxLength={100}
          autoComplete="address-level2"
          required
        />
      </div>
      <div className="col-md-4">
        <label className="form-label">Provincia/Estado</label>
        <input
          name="state"
          className="form-control"
          placeholder="Ej: Santa Fe"
          value={form.state}
          onChange={handleAddressChange}
          maxLength={100}
          autoComplete="address-level1"
          required
        />
      </div>
      <div className="col-md-4">
        <label className="form-label">País</label>
        <input
          name="country"
          className="form-control"
          list="countryOptions"
          placeholder="Selecciona o escribe tu país"
          value={form.country}
          onChange={handleAddressChange}
          maxLength={100}
          autoComplete="country-name"
          required
        />
        <datalist id="countryOptions">
          <option value="Argentina" />
          <option value="Uruguay" />
          <option value="Chile" />
          <option value="Paraguay" />
          <option value="Brasil" />
          <option value="México" />
          <option value="España" />
          <option value="Estados Unidos" />
        </datalist>
      </div>
      <div className="col-md-8">
        <label className="form-label">Dirección</label>
        <input
          name="streetAddress"
          className="form-control"
          placeholder="Calle y número, piso, depto"
          value={form.streetAddress}
          onChange={handleAddressChange}
          maxLength={200}
          autoComplete="street-address"
          required
        />
      </div>
      <div className="col-md-4">
        <label className="form-label">Código postal</label>
        <input
          name="postalCode"
          type="number"
          className="form-control"
          placeholder="Ej: 2000"
          value={form.postalCode}
          onChange={handleAddressChange}
          autoComplete="postal-code"
          required
        />
      </div>

      {/* Amenidades */}
      <div className="col-12 mt-2">
        <div 
          className="d-flex justify-content-between align-items-center" 
          style={{ cursor: 'pointer' }}
          onClick={() => setAmenitiesExpanded(!amenitiesExpanded)}
        >
          <h6 className="text-muted">
            Amenities {form.amenityIds.length > 0 && (
              <span className="badge bg-primary ms-2">{form.amenityIds.length}</span>
            )}
          </h6>
          <span className="text-muted small">
            {amenitiesExpanded ? '▼ Ocultar' : '▶ Mostrar'}
          </span>
        </div>
      </div>
      {amenitiesExpanded && (
        <>
          {loadingAmenities ? (
            <div className="col-12">
              <p className="text-muted small">Cargando amenities...</p>
            </div>
          ) : availableAmenities.length === 0 ? (
            <div className="col-12">
              <p className="text-muted small">No hay amenidades disponibles</p>
            </div>
          ) : (
            availableAmenities.map((amenity) => (
              <div key={amenity.id} className="col-md-4 col-lg-3">
                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id={`amenity-${amenity.id}`}
                    checked={form.amenityIds.includes(amenity.id)}
                    onChange={() => handleAmenityToggle(amenity.id)}
                  />
                  <label className="form-check-label" htmlFor={`amenity-${amenity.id}`}>
                    {amenity.name}
                  </label>
                </div>
              </div>
            ))
          )}
        </>
      )}

      <div className="col-12 d-flex gap-2 mt-2">
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Guardando...' : (
            <>
              <FontAwesomeIcon icon={faFloppyDisk} className="me-2" />
              Guardar
            </>
          )}
        </button>
        {mode === 'create' && (
          <button type="button" className="btn btn-secondary" onClick={onCancel} disabled={loading}>
            <FontAwesomeIcon icon={faXmark} className="me-2" />
            Cancelar
          </button>
        )}
      </div>
    </form>
  );
}
