import { useState, useEffect } from "react";
import DateRangePicker from "@/shared/ui/DateRangePicker";
import {
    getPropertyAvailabilities,
    createPropertyAvailability,
    deletePropertyAvailability
} from "@/features/property/services/propertyAvailabilityService";
import { useConfirm } from "@/shared/ui/ConfirmDialog";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarPlus, faXmark, faFloppyDisk, faTrash } from '@fortawesome/free-solid-svg-icons';

// Gestor de disponibilidad (pestaña "Disponibilidades" de la edición). El dueño
// agrega o elimina rangos de fechas en los que su propiedad puede reservarse.
export default function PropertyAvailabilityManager({ propertyId}) {
    const confirm = useConfirm();
    const [availabilities, setAvailabilities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [formData, setFormData] = useState({
        startDate: "",
        endDate: ""
    });
    const [range, setRange] = useState();

    useEffect(() => {
        loadAvailabilities();
    }, [propertyId]);

    const loadAvailabilities = async () => {
        try {
            setLoading(true);
            const data = await getPropertyAvailabilities(propertyId);
            setAvailabilities(data);
        } catch (err) {
            console.error("Error cargando disponibilidades:", err);
            setError(getApiErrorMessage(err, "Error al cargar las disponibilidades."));
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        // Validaciones: ambas fechas presentes y el inicio anterior al fin.
        // Al estar en formato "YYYY-MM-DD" se pueden comparar como strings.
        if (!formData.startDate || !formData.endDate) {
            setError("Por favor completa todas las fechas");
            return;
        }

        if (formData.startDate >= formData.endDate) {
            setError("La fecha de inicio debe ser anterior a la fecha de fin");
            return;
        }

        try {
            setLoading(true);
            await createPropertyAvailability({
                startDate: formData.startDate,
                endDate: formData.endDate,
                propertyId: parseInt(propertyId)
            });
            setFormData({ startDate: "", endDate: "" });
            setShowForm(false);
            await loadAvailabilities();
        } catch (err) {
            console.error("Error creando disponibilidad:", err);
            setError(getApiErrorMessage(err, "Error al crear la disponibilidad."));
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        const ok = await confirm("Esta acción no se puede deshacer.", {
            title: "Eliminar disponibilidad",
            confirmText: "Sí, eliminar",
            variant: "danger",
        });
        if (!ok) return;

        try {
            setLoading(true);
            await deletePropertyAvailability(id);
            await loadAvailabilities();
        } catch (err) {
            console.error("Error eliminando disponibilidad:", err);
            setError(getApiErrorMessage(err, "No se pudo eliminar la disponibilidad."));
        } finally {
            setLoading(false);
        }
    };

    // Formatea "YYYY-MM-DD" a texto legible construyendo la fecha por componentes
    // (en hora local) para evitar el corrimiento de día por zona horaria.
    const formatDate = (dateString) => {
        const [y, m, d] = dateString.split("-").map(Number);
        return new Date(y, m - 1, d).toLocaleDateString("es-ES", {
            year: "numeric",
            month: "long",
            day: "numeric"
        });
    };

    // Calcula la cantidad de noches entre dos fechas (diferencia en días).
    const calculateDays = (start, end) => {
        const [sy, sm, sd] = start.split("-").map(Number);
        const [ey, em, ed] = end.split("-").map(Number);

        const startDate = new Date(sy, sm - 1, sd);
        const endDate = new Date(ey, em - 1, ed);

        return Math.round((endDate - startDate) / (1000 * 60 * 60 * 24));
    };


    const handleRangeSelect = (r) => {
        setRange(r);
        if (r?.from) setFormData(prev => ({ ...prev, startDate: r.from.toISOString().split("T")[0] }));
        else setFormData(prev => ({ ...prev, startDate: "" }));
        if (r?.to) setFormData(prev => ({ ...prev, endDate: r.to.toISOString().split("T")[0] }));
        else setFormData(prev => ({ ...prev, endDate: "" }));
    };

    return (
        <div className="property-availability-manager">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h5 className="mb-0">Gestión de Disponibilidad</h5>
                <button 
                    className="btn btn-primary"
                    onClick={() => setShowForm(!showForm)}
                    disabled={loading}
                >
                    {showForm ? (
                        <>
                            <FontAwesomeIcon icon={faXmark} className="me-2" />
                            Cancelar
                        </>
                    ) : (
                        <>
                            <FontAwesomeIcon icon={faCalendarPlus} className="me-2" />
                            Agregar Disponibilidad
                        </>
                    )}
                </button>
            </div>

            {error && (
                <div className="alert alert-danger alert-dismissible fade show" role="alert">
                    {error}
                    <button 
                        type="button" 
                        className="btn-close" 
                        onClick={() => setError(null)}
                    ></button>
                </div>
            )}

            {showForm && (
                <div className="card mb-4">
                    <div className="card-body">
                        <h6 className="card-title">Nueva Disponibilidad</h6>
                        <form onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label className="form-label">Seleccionar Fechas:</label>
                                <DateRangePicker
                                    range={range}
                                    onRangeChange={handleRangeSelect}
                                    placeholderStart="Llegada"
                                    placeholderEnd="Salida"
                                    minDate={new Date()}
                                />
                            </div>
                            <div className="mt-3">
                                <button 
                                    type="submit" 
                                    className="btn btn-success"
                                    disabled={loading}
                                >
                                    {loading ? "Guardando..." : (
                                        <>
                                            <FontAwesomeIcon icon={faFloppyDisk} className="me-2" />
                                            Guardar
                                        </>
                                    )}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {loading && !showForm ? (
                <div className="text-center py-4">
                    <div className="spinner-border text-primary" role="status">
                        <span className="visually-hidden">Cargando...</span>
                    </div>
                </div>
            ) : availabilities.length === 0 ? (
                <div className="alert alert-info">
                    <p className="mb-0">
                        📅 No hay disponibilidades configuradas. 
                        Agrega períodos en los que tu propiedad estará disponible para reservar.
                    </p>
                </div>
            ) : (
                <div className="list-group">
                    {availabilities.map((availability) => (
                        <div key={availability.id} className="list-group-item">
                            <div className="d-flex justify-content-between align-items-center">
                                <div>
                                    <div className="fw-bold mb-1">
                                        {formatDate(availability.startDate)} → {formatDate(availability.endDate)}
                                    </div>
                                    <small className="text-muted">
                                        Duración: {calculateDays(availability.startDate, availability.endDate)} días
                                    </small>
                                </div>
                                <button
                                    className="btn btn-sm btn-outline-danger"
                                    onClick={() => handleDelete(availability.id)}
                                    disabled={loading}
                                >
                                    <FontAwesomeIcon icon={faTrash} className="me-2" />
                                    Eliminar
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
