// Página "Mis Reservas" (vista del huésped): lista sus reservas con el estado de
// cada una y las acciones disponibles según ese estado (pagar, reseñar, etc.).
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getMyReservations } from "@/features/reservation/services/reservationService";
import { formatDateLong } from "@/shared/utils/formatters";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faMapMarkerAlt,
    faMoneyBillWave,
    faClock,
    faCheckCircle,
    faFileUpload,
    faBan,
    faExclamationTriangle,
    faFlagCheckered,
    faStar,
    faSearch,
    faEye
} from '@fortawesome/free-solid-svg-icons';

// Mapa de cada estado de la reserva a su presentación (etiqueta, color, ícono) y
// a la acción que el huésped puede realizar desde la tarjeta (action).
const STATUS_MAP = {
    // Estado inicial: El usuario eligió fechas, falta subir el comprobante.
    "PendingPayment": { label: "Pendiente de Pago", color: "bg-warning text-dark", icon: faMoneyBillWave, action: "upload_payment" },
    // El usuario subió el comprobante: Espera revisión del Owner.
    "PaymentUploaded": { label: "Pago Subido - En Revisión", color: "bg-info text-white", icon: faClock, action: null },
    // El Owner aprobó el pago: Todo listo.
    "Confirmed": { label: "Confirmada", color: "bg-success text-white", icon: faCheckCircle, action: "view_receipt" },
    // Cancelada por el usuario o sistema.
    "Cancelled": { label: "Cancelada", color: "bg-secondary text-white", icon: faBan, action: null },
    // El owner una vez finalizada la estadía marca como completada la reserva. El usuario deja una review
    "Completed" : { label: "Estadía Finalizada", color: "bg-dark text-white", icon: faFlagCheckered, action: "leave_review"}
};

export default function MyReservationsPage() {
    const navigate = useNavigate();
    const [reservations, setReservations] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadReservations();
    }, []);

    const loadReservations = async () => {
        try {
            const data = await getMyReservations();
            // Mantenemos el sort del lado cliente por seguridad visual
            const sortedData = data.sort((a, b) => new Date(b.startDate) - new Date(a.startDate));
            setReservations(sortedData);
        } catch (error) {
            console.error("Error al cargar reservas:", error);
            setError("Hubo un problema al cargar tus reservas.");
        } finally {
            setLoading(false);
        }
    };

    // Esta función lleva al Checkout para subir el comprobante
    const handleUploadPayment = (reservationId) => {
        navigate(`/reservation/${reservationId}/checkout`);
    };

    const handleLeaveReview = (reservationId) => {
        navigate(`/reservation/${reservationId}/review`);
    };

    if (loading) return (
        <div className="d-flex justify-content-center mt-5">
            <div className="spinner-border text-primary" role="status"><span className="visually-hidden">Cargando...</span></div>
        </div>
    );

    if (error) return (
        <div className="container mt-5 alert alert-danger">
            <FontAwesomeIcon icon={faExclamationTriangle} className="me-2" /> {error}
        </div>
    );

    if (reservations.length === 0) {
        return (
            <div className="container mt-5 text-center py-5 bg-light rounded shadow-sm">
                <h3>Aún no tienes reservas</h3>
                <button className="btn btn-primary mt-3" onClick={() => navigate('/')}><FontAwesomeIcon icon={faSearch} className="me-2" />Explorar Propiedades</button>
            </div>
        );
    }

    return (
        <div className="container mt-4 mb-5">
            <h2 className="mb-4 border-bottom pb-2">Mis Reservas</h2>
            <div className="row">
                {reservations.map((res) => {
                    const statusConfig = STATUS_MAP[res.status] || { 
                        label: res.status, 
                        color: "bg-secondary text-white", 
                        icon: faClock 
                    };

                    return (
                        <div key={res.id} className="col-12 mb-4">
                            <div className="card shadow-sm border-0 overflow-hidden">
                                <div className="row g-0">
                                    {/* Imagen */}
                                    <div className="col-md-4 col-lg-3 position-relative">
                                        <div style={{ height: "100%", minHeight: "220px", width: "100%" }}>
                                            <img 
                                                src={res.propertyImageUrl || 'https://via.placeholder.com/400x300?text=Propiedad'} 
                                                alt={res.propertyTitle}
                                                style={{ width: "100%", height: "100%", objectFit: "cover", position: "absolute" }}
                                            />
                                        </div>
                                    </div>

                                    {/* Contenido */}
                                    <div className="col-md-8 col-lg-9">
                                        <div className="card-body d-flex flex-column h-100">
                                            
                                            {/* Título y Badge de Estado */}
                                            <div className="d-flex justify-content-between align-items-center mb-2">
                                                <h5 className="card-title mb-0 text-primary fw-bold text-truncate" style={{maxWidth: "60%"}}>
                                                    {res.propertyTitle}
                                                </h5>
                                                <span className={`badge ${statusConfig.color} px-3 py-2 rounded-pill fw-normal`}>
                                                    <FontAwesomeIcon icon={statusConfig.icon} className="me-2" />
                                                    {statusConfig.label}
                                                </span>
                                            </div>

                                            {/* Dirección */}
                                            <p className="card-text text-muted mb-3 small">
                                                <FontAwesomeIcon icon={faMapMarkerAlt} className="me-2 text-danger" />
                                                {res.propertyAddress}
                                            </p>

                                            {/* Datos Grid: Fechas y Precio */}
                                            <div className="row g-3 mb-3 bg-light p-2 rounded mx-0">
                                                <div className="col-sm-6">
                                                    <small className="text-uppercase text-muted fw-bold d-block mb-1" style={{fontSize: "0.75rem"}}>Fechas</small>
                                                    <span className="text-dark">{formatDateLong(res.startDate)} al {formatDateLong(res.endDate)}</span>
                                                </div>
                                                <div className="col-sm-6">
                                                    <small className="text-uppercase text-muted fw-bold d-block mb-1" style={{fontSize: "0.75rem"}}>Total</small>
                                                    <span className="text-success fs-5 fw-bold">${res.totalPrice?.toLocaleString()}</span>
                                                </div>
                                            </div>

                                            {/* BOTONES DE ACCIÓN */}
                                            <div className="mt-auto d-flex justify-content-end gap-2 border-top pt-3">
                                                <button 
                                                    className="btn btn-outline-secondary btn-sm"
                                                    onClick={() => navigate(`/property/${res.propertyId}`)}
                                                >
                                                    <FontAwesomeIcon icon={faEye} className="me-1" />
                                                    Ver Propiedad
                                                </button>

                                                {/* Acción: Pagar / Subir Comprobante */}
                                                {statusConfig.action === 'upload_payment' && (
                                                    <button 
                                                        className="btn btn-warning btn-sm fw-bold shadow-sm"
                                                        onClick={() => handleUploadPayment(res.id)}
                                                    >
                                                        <FontAwesomeIcon icon={faFileUpload} className="me-2" />
                                                        Subir Comprobante
                                                    </button>
                                                )}

                                                {/* Acción: Ver Recibo (Solo visual por ahora) */}
                                                {statusConfig.action === 'view_receipt' && (
                                                    <button className="btn btn-success btn-sm fw-bold shadow-sm disabled">
                                                        <FontAwesomeIcon icon={faCheckCircle} className="me-2" />
                                                        Reserva Confirmada
                                                    </button>
                                                )}

                                                {/* Acción: Dejar Reseña (Estado Completed sin reseña previa) */}
                                                {statusConfig.action === 'leave_review' && !res.hasReview && (
                                                    <button
                                                        className="btn btn-primary btn-sm fw-bold shadow-sm"
                                                        onClick={() => handleLeaveReview(res.id)}
                                                    >
                                                        <FontAwesomeIcon icon={faStar} className="me-2" />
                                                        Calificar Estadía
                                                    </button>
                                                )}

                                                {/* Estado: Reseña ya enviada */}
                                                {statusConfig.action === 'leave_review' && res.hasReview && (
                                                    <span className="badge bg-light text-success border border-success-subtle px-3 py-2 d-flex align-items-center">
                                                        <FontAwesomeIcon icon={faStar} className="me-2" />
                                                        Reseña enviada
                                                    </span>
                                                )}
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
