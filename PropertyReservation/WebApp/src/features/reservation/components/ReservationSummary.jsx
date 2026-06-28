import { useState } from "react";
import { format, differenceInDays } from "date-fns";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarCheck } from '@fortawesome/free-solid-svg-icons';

// Panel lateral de la reserva: deja elegir huéspedes y notas, calcula el total
// según las noches seleccionadas y dispara la confirmación (onReserve).
export default function ReservationSummary({
    property,
    selectedRange,
    onReserve,
    loading,
    error
}) {
    const [guests, setGuests] = useState(1);
    const [notes, setNotes] = useState("");
    const [showNotes, setShowNotes] = useState(false);

    // Total = cantidad de noches × precio por noche (0 si no hay rango completo).
    const calculateTotalPrice = () => {
        if (!selectedRange?.from || !selectedRange?.to) return 0;
        const nights = differenceInDays(selectedRange.to, selectedRange.from);
        return nights * property.nightlyPrice;
    };

    const handleConfirm = () => {
        onReserve({ guests, notes });
    };

    const totalPrice = calculateTotalPrice();
    const nights = selectedRange?.from && selectedRange?.to 
        ? differenceInDays(selectedRange.to, selectedRange.from) 
        : 0;

    return (
        <div className="card shadow sticky-top" style={{ top: '20px' }}>
            <div className="card-body">
                <h5 className="card-title mb-4">Resumen de reserva</h5>

                {error && (
                    <div className="alert alert-danger small">{error}</div>
                )}

                <div className="mb-3 pb-3 border-bottom">
                    <div className="d-flex justify-content-between mb-2">
                        <div>
                            <small className="text-muted d-block">Check-in</small>
                            <span className="fw-semibold">
                                {selectedRange?.from ? format(selectedRange.from, "dd/MM/yyyy") : "-"}
                            </span>
                        </div>
                        <div className="text-end">
                            <small className="text-muted d-block">Check-out</small>
                            <span className="fw-semibold">
                                {selectedRange?.to ? format(selectedRange.to, "dd/MM/yyyy") : "-"}
                            </span>
                        </div>
                    </div>
                </div>

                <div className="mb-3">
                    <label className="form-label small fw-semibold">Número de huéspedes</label>
                    <input
                        type="number"
                        min={1}
                        max={property.maxGuests}
                        value={guests}
                        className="form-control"
                        onChange={(e) => setGuests(Number(e.target.value))}
                    />
                    <small className="text-muted">Máximo: {property.maxGuests} huéspedes</small>
                </div>

                <div className="mb-3">
                    <button
                        type="button"
                        className="btn btn-link text-decoration-none p-0 small"
                        onClick={() => setShowNotes(!showNotes)}
                    >
                        {showNotes ? '▼' : '▶'} Agregar notas adicionales
                    </button>
                    {showNotes && (
                        <div className="mt-2 small">
                            <textarea
                                className="form-control"
                                rows={3}
                                value={notes}
                                onChange={(e) => setNotes(e.target.value)}
                                placeholder="Ej: Hora estimada de llegada, solicitudes especiales..."
                                maxLength={500}
                            />
                            <small className="text-muted">{notes.length}/500 caracteres</small>
                        </div>
                    )}
                </div>

                {nights > 0 && (
                    <>
                        <hr />
                        <div className="mb-2 d-flex justify-content-between small">
                            <span>${property.nightlyPrice.toLocaleString()} x {nights} {nights === 1 ? 'noche' : 'noches'}</span>
                            <span>${totalPrice.toLocaleString()}</span>
                        </div>
                        <hr />
                        <div className="mb-4 d-flex justify-content-between">
                            <span className="fw-bold">Total</span>
                            <span className="fw-bold fs-5 text-primary">${totalPrice.toLocaleString()}</span>
                        </div>
                    </>
                )}

                <button 
                    className="btn btn-primary w-100" 
                    onClick={handleConfirm}
                    disabled={loading || !selectedRange?.from || !selectedRange?.to}
                >
                    {loading ? 'Procesando...' : (
                        <>
                            <FontAwesomeIcon icon={faCalendarCheck} className="me-2" />
                            Confirmar reserva
                        </>
                    )}
                </button>

                {!selectedRange?.from && (
                    <p className="text-muted text-center small mt-3 mb-0">
                        Selecciona las fechas para continuar
                    </p>
                )}
            </div>
        </div>
    );
}
