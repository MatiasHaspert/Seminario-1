// Panel compacto de reserva (fechas, huéspedes y total) que crea la reserva
// directamente. Variante simplificada del flujo de ReservationPage/Summary.
import { useEffect, useState } from "react";
import { differenceInDays, format } from "date-fns";
import { createReservation } from "@/features/reservation/services/reservationService";
import { useToast } from "@/shared/ui/Toast";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendarCheck } from '@fortawesome/free-solid-svg-icons';

export default function ReservationPanel({ selectedRange, nightlyPrice, maxGuests, propertyId }) {
    const toast = useToast();
    const [guests, setGuests] = useState(1);
    const [totalPrice, setTotalPrice] = useState(0);

    // Recalcula el total cada vez que cambian las fechas o el precio por noche.
    useEffect(() => {
        if (selectedRange?.from && selectedRange?.to) {
            const nights = differenceInDays(selectedRange.to, selectedRange.from);
            setTotalPrice(nights * nightlyPrice);
        } else {
            setTotalPrice(0);
        }
    }, [selectedRange, nightlyPrice]);

    const handleReserve = async () => {
        if (!selectedRange?.from || !selectedRange?.to) return;

        try {
            await createReservation({
                propertyId,
                startDate: selectedRange.from,
                endDate: selectedRange.to,
                totalGuests: guests
            });
            toast("Reserva creada con éxito.", "success");
        } catch (error) {
            console.error(error);
            toast(getApiErrorMessage(error, "No se pudo crear la reserva."), "error");
        }
    };

    return (
        <div className="border rounded p-3 shadow-sm bg-white">
            <h5 className="fw-bold mb-3">Tu reserva</h5>

            <div className="mb-2">
                <strong>Check-in:</strong>{" "}
                {selectedRange?.from ? format(selectedRange.from, "dd/MM/yyyy") : "-"}
            </div>
            <div className="mb-2">
                <strong>Check-out:</strong>{" "}
                {selectedRange?.to ? format(selectedRange.to, "dd/MM/yyyy") : "-"}
            </div>

            <div className="mb-3">
                <label className="form-label">Huespedes</label>
                <input
                    type="number"
                    min={1}
                    max={maxGuests}
                    value={guests}
                    className="form-control"
                    onChange={(e) => setGuests(Number(e.target.value))}
                />
            </div>

            {totalPrice > 0 && (
                <div className="mb-3 fw-bold">
                    Total: ${totalPrice.toLocaleString()}
                </div>
            )}

            <button className="btn btn-primary w-100" onClick={handleReserve}>
                <FontAwesomeIcon icon={faCalendarCheck} className="me-2" />
                Reservar
            </button>
        </div>
    );
}
