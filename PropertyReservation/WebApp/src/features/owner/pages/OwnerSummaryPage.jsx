import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faFileInvoiceDollar,
    faCalendarCheck,
    faHouse,
    faArrowRight
} from "@fortawesome/free-solid-svg-icons";
import { useAuth } from "@/shared/auth/AuthContext";
import { getPaymentsUnderReview } from "@/features/payment/services/paymentService";
import { getReservationsByOwner } from "@/features/reservation/services/reservationService";
import { getPropertiesByOwner } from "@/features/property/services/propertyService";

// Estados que cuentan como reservas "activas" en el resumen.
const ACTIVE_RESERVATION_STATUSES = new Set([
    "PendingPayment",
    "PaymentUploaded",
    "Confirmed"
]);

// Página de inicio del panel del dueño: muestra tarjetas-resumen con los pagos
// por revisar, las reservas activas y la cantidad de propiedades publicadas.
export default function OwnerSummaryPage() {
    const { user } = useAuth();
    const [counts, setCounts] = useState({
        pendingPayments: null,
        activeReservations: null,
        properties: null
    });

    useEffect(() => {
        // 'cancelled' evita actualizar el estado si el componente se desmonta
        // antes de que terminen las peticiones (evita warnings de React).
        let cancelled = false;
        // allSettled: pedimos las tres métricas en paralelo y, aunque alguna
        // falle, mostramos las demás (la fallida cae a 0).
        Promise.allSettled([
            getPaymentsUnderReview(),
            getReservationsByOwner(),
            getPropertiesByOwner()
        ]).then(([payments, reservations, properties]) => {
            if (cancelled) return;
            setCounts({
                pendingPayments: payments.status === "fulfilled"
                    ? (payments.value?.length || 0) : 0,
                activeReservations: reservations.status === "fulfilled"
                    ? (reservations.value || []).filter(
                        r => ACTIVE_RESERVATION_STATUSES.has(r.status)
                    ).length
                    : 0,
                properties: properties.status === "fulfilled"
                    ? (properties.value?.length || 0) : 0
            });
        });
        return () => { cancelled = true; };
    }, []);

    const greeting = user?.name ? `Hola, ${user.name}` : "Tu Panel";

    return (
        <>
            <header className="owner-page-header">
                <div className="owner-page-overline">Resumen</div>
                <h1 className="owner-page-title">{greeting}</h1>
                <p className="owner-page-subtitle">
                    Acá está lo que pasa con tus propiedades hoy.
                </p>
            </header>

            <div className="owner-shortcut-grid">
                <ShortcutCard
                    to="/owner/payments"
                    icon={faFileInvoiceDollar}
                    label="Pagos"
                    text={renderPaymentsText(counts.pendingPayments)}
                    highlight={counts.pendingPayments > 0}
                />
                <ShortcutCard
                    to="/owner/reservations"
                    icon={faCalendarCheck}
                    label="Reservas"
                    text={renderReservationsText(counts.activeReservations)}
                />
                <ShortcutCard
                    to="/owner/properties"
                    icon={faHouse}
                    label="Propiedades"
                    text={renderPropertiesText(counts.properties)}
                />
            </div>
        </>
    );
}

function ShortcutCard({ to, icon, label, text, highlight }) {
    return (
        <Link to={to} className="owner-shortcut-card">
            <div className={`owner-shortcut-icon${highlight ? "" : " is-neutral"}`}>
                <FontAwesomeIcon icon={icon} />
            </div>
            <div className="owner-shortcut-body">
                <div className="owner-shortcut-label">{label}</div>
                <p className="owner-shortcut-text">{text}</p>
                <span className="owner-shortcut-arrow">
                    Ir <FontAwesomeIcon icon={faArrowRight} />
                </span>
            </div>
        </Link>
    );
}

function renderPaymentsText(count) {
    if (count === null) return "Cargando...";
    if (count === 0) return "Todos los pagos están al día.";
    return `Tenés ${count} pago${count === 1 ? "" : "s"} por revisar.`;
}

function renderReservationsText(count) {
    if (count === null) return "Cargando...";
    if (count === 0) return "Sin reservas activas en este momento.";
    return `Tenés ${count} reserva${count === 1 ? "" : "s"} activa${count === 1 ? "" : "s"}.`;
}

function renderPropertiesText(count) {
    if (count === null) return "Cargando...";
    if (count === 0) return "Todavía no publicaste ninguna propiedad.";
    return `Gestionás ${count} propiedad${count === 1 ? "" : "es"}.`;
}
