import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getPropertyDetails } from "@/features/property/services/propertyService";
import { createReservation } from "@/features/reservation/services/reservationService";
import { useToast } from "@/shared/ui/Toast";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import AvailabilityCalendar from "@/features/property/components/AvailabilityCalendar/AvailabilityCalendar";
import ReservationSummary from "@/features/reservation/components/ReservationSummary";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faBed, faBath, faArrowLeft } from '@fortawesome/free-solid-svg-icons';

// Página para reservar una propiedad: muestra el calendario de disponibilidad a
// la izquierda y el resumen/confirmación a la derecha. Al confirmar, crea la
// reserva y deriva al checkout para subir el comprobante de pago.
export default function ReservationPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const toast = useToast();
    const [property, setProperty] = useState(null);
    const [selectedRange, setSelectedRange] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadPropertyDetails();
    }, [id]);

    const loadPropertyDetails = async () => {
        try {
            const data = await getPropertyDetails(id);
            setProperty(data);
        } catch (error) {
            console.error("Error al cargar el detalle de la propiedad:", error);
            setError("No se pudo cargar la propiedad");
        }
    };

    const handleReserve = async ({ guests, notes }) => {
        // No se puede reservar sin un rango de fechas completo (llegada y salida).
        if (!selectedRange?.from || !selectedRange?.to) {
            toast("Por favor seleccioná las fechas de tu reserva.", "warning");
            return;
        }

        setLoading(true);
        setError(null);

        try {
            // Creamos la reserva base
            const response = await createReservation({
                propertyId: parseInt(id),
                startDate: selectedRange.from.toISOString().split("T")[0], // Eliminamos la parte de tiempo porque el backend espera solo la fecha
                endDate: selectedRange.to.toISOString().split("T")[0],
                totalGuests: guests
                //notes: notes
            });

            // Extraemos el ID de la reserva creada
            const reservationId = response.id;

            // Con el id confirmado, derivamos al checkout para subir el comprobante.
            if (reservationId) {
                toast("¡Reserva creada! Subí el comprobante para confirmarla.", "success");
                navigate(`/reservation/${reservationId}/checkout`);
            } else {
                throw new Error("No se recibió el ID de la reserva");
            }

        } catch (error) {
            console.error(error);
            setError(getApiErrorMessage(error, "Error al crear la reserva. Por favor intentá nuevamente."));
        } finally {
            setLoading(false);
        }
    };

    if (!property) {
        return (
            <div className="container mt-4">
                <p>Cargando...</p>
            </div>
        );
    }

    return (
        <div className="container mt-4">
            <h3 className="mb-4">Reservar propiedad</h3>

            <div className="row">
                {/* Columna izquierda: Calendario */}
                <div className="col-lg-8">
                    {/* Info básica de la propiedad */}
                    <div className="card mb-3">
                        <div className="card-body p-0">
                            <div className="justify-content-between align-items-start">
                                <h6 className="mb-1">{property.title}</h6>
                                <p className="text-muted small mb-0">
                                    <FontAwesomeIcon icon={faUser} className="me-1" />
                                    {property.maxGuests} · 
                                    <FontAwesomeIcon icon={faBed} className="ms-2 me-1" />
                                    {property.bedrooms} 
                                    <FontAwesomeIcon icon={faBath} className="ms-2 me-1" />
                                    {property.bathrooms} 
                                </p>
                                <p className="fw-bold text-primary mb-0">
                                    ${property.nightlyPrice.toLocaleString()} <span className="text-muted small">/ noche</span>
                                </p>                                
                            </div>
                            <div className="text-end mt-1">
                                <button 
                                    className="btn btn-link text-decoration-none p-0 small" 
                                    onClick={() => navigate(`/property/${id}`)}
                                >
                                    <FontAwesomeIcon icon={faArrowLeft} className="me-1" />
                                    Volver detalles
                                </button>
                            </div>
                        </div>
                    </div>

                    {/* Calendario */}
                    <div className="mb-4">
                        <h5 className="mb-3">Selecciona tus fechas</h5>
                        <AvailabilityCalendar 
                            availableRanges={property.availableRanges || []}
                            reservedRanges={property.reservedRanges || []}
                            onRangeSelected={setSelectedRange} 
                        />
                    </div>
                </div>

                {/* Columna derecha: Resumen de reserva */}
                <div className="col-lg-4">
                    <ReservationSummary 
                        property={property}
                        selectedRange={selectedRange}
                        onReserve={handleReserve}
                        loading={loading}
                        error={error}
                    />
                </div>
            </div>
        </div>
    );
}
