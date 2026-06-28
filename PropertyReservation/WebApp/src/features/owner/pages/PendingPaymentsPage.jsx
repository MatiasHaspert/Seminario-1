import { useEffect, useState } from "react";
import { useOutletContext } from "react-router-dom";
import {
    getPaymentsUnderReview,
    changePaymentStatus,
    getPaymentProof
} from "@/features/payment/services/paymentService";
import { getPaymentMethodLabel } from "@/features/payment/constants/paymentMethods";
import { useToast } from "@/shared/ui/Toast";
import { useConfirm } from "@/shared/ui/ConfirmDialog";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faFileInvoiceDollar,
    faCheck,
    faTimes,
    faSpinner,
    faEye,
    faExclamationTriangle,
    faUser,
    faCalendarAlt
} from '@fortawesome/free-solid-svg-icons';

// Página del dueño para revisar los comprobantes de pago subidos por los
// huéspedes: permite ver el comprobante en un modal y aprobar o rechazar el pago.
export default function PendingPaymentsPage() {
    const { refreshPendingPayments } = useOutletContext() ?? {};
    const toast = useToast();
    const confirm = useConfirm();
    const [payments, setPayments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [processingId, setProcessingId] = useState(null);

    // Estados para el Modal del Comprobante
    const [selectedProofUrl, setSelectedProofUrl] = useState(null);
    const [loadingProof, setLoadingProof] = useState(false);
    const [showModal, setShowModal] = useState(false);

    useEffect(() => {
        loadPendingPayments();
    }, []);

    const loadPendingPayments = async () => {
        try {
            setLoading(true);
            const data = await getPaymentsUnderReview();
            setPayments(data || []);
        } catch (error) {
            console.error("Error al cargar pagos:", error);
            setError("No pudimos cargar los pagos pendientes.");
        } finally {
            setLoading(false);
        }
    };

    // --- ACCIONES DE ESTADO ---
    const handleStatusChange = async (paymentId, newStatus) => {
        const actionText = newStatus === 1 ? 'aprobar' : 'rechazar'; // Asumo 1=Aprobado, 2=Rechazado
        
        const ok = await confirm(`¿Confirmás que querés ${actionText} este pago?`, {
            title: newStatus === 1 ? "Aprobar pago" : "Rechazar pago",
            confirmText: newStatus === 1 ? "Sí, aprobar" : "Sí, rechazar",
            variant: newStatus === 1 ? "default" : "danger",
        });
        if (!ok) return;

        setProcessingId(paymentId);
        try {
            // El backend espera [FromQuery] PaymentStatusDTO. Pasamos un objeto con la propiedad status
            await changePaymentStatus(paymentId, { paymentStatus: newStatus });
            
            // Removemos el pago procesado de la vista
            setPayments(prev => prev.filter(p => p.paymentId !== paymentId));
            refreshPendingPayments?.();
            
            if (newStatus === 1) {
                toast(`Pago aprobado exitosamente.`, "success");
            } else {
                toast(`Pago rechazado exitosamente.`, "info");
            }

        } catch (error) {
            console.error("Error al actualizar pago:", error);
            toast(getApiErrorMessage(error, "Ocurrió un error al procesar el pago."), "error");
        } finally {
            setProcessingId(null);
        }
    };

    // --- VISUALIZAR COMPROBANTE ---
    const handleViewProof = async (paymentId) => {
        try {
            setLoadingProof(true);
            setShowModal(true); // Abrimos el modal mostrando spinner
            const proofUrl = await getPaymentProof(paymentId);
            setSelectedProofUrl(proofUrl);
        } catch (error) {
            console.error("Error al obtener comprobante:", error);
            toast("No se pudo cargar el comprobante del servidor.", "error");
            closeModal();
        } finally {
            setLoadingProof(false);
        }
    };

    const closeModal = () => {
        setShowModal(false);
        // Limpiamos la URL de memoria para no dejar basura en el navegador
        if (selectedProofUrl) {
            URL.revokeObjectURL(selectedProofUrl);
            setSelectedProofUrl(null);
        }
    };

    // Helpers
    const formatDate = (dateString) => {
        if (!dateString) return "-";
        return new Date(dateString).toLocaleDateString('es-AR');
    };

    if (loading) return (
        <div className="d-flex justify-content-center mt-5">
            <div className="spinner-border text-warning" role="status"><span className="visually-hidden">Cargando...</span></div>
        </div>
    );

    return (
        <div className="container mt-4 mb-5">
            <div className="d-flex align-items-center mb-4 border-bottom pb-3">
                <h2 className="mb-0 fw-bold text-dark">
                    <FontAwesomeIcon icon={faFileInvoiceDollar} className="me-2 text-warning" />
                    Pagos por Revisar
                </h2>
            </div>

            {error && (
                <div className="alert alert-danger">
                    <FontAwesomeIcon icon={faExclamationTriangle} className="me-2" /> {error}
                </div>
            )}

            {payments.length === 0 && !error ? (
                <div className="text-center py-5 bg-light rounded shadow-sm">
                    <h4 className="text-muted mb-3">No hay pagos pendientes de revisión.</h4>
                    <p>Todo está al día.</p>
                </div>
            ) : (
                <div className="row">
                    {payments.map((payment) => (
                        <div key={payment.paymentId} className="col-12 col-lg-6 mb-4">
                            <div className="card shadow-sm h-100 border-0 border-start border-warning border-4">
                                <div className="card-body">
                                    <div className="d-flex justify-content-between align-items-start mb-3">
                                        <div>
                                            <h5 className="card-title text-primary fw-bold mb-1">
                                                Reserva #{payment.reservationId}
                                            </h5>
                                            <small className="text-muted d-block">
                                                Método: <span className="fw-bold">{getPaymentMethodLabel(payment.paymentMethod)}</span>
                                            </small>
                                        </div>
                                        <div className="text-end">
                                            <span className="fs-4 fw-bold text-success">
                                                ${payment.amount?.toLocaleString() || "0"}
                                            </span>
                                            <small className="d-block text-muted">Monto transferido</small>
                                        </div>
                                    </div>

                                    <div className="bg-light p-2 rounded mb-3 small">
                                        <div className="row">
                                            <div className="col-6 mb-1">
                                                <FontAwesomeIcon icon={faUser} className="text-muted me-2" />
                                                <span className="fw-bold">Huésped:</span> {payment.guestName || "Usuario"}
                                            </div>
                                            <div className="col-6 mb-1">
                                                <FontAwesomeIcon icon={faCalendarAlt} className="text-muted me-2" />
                                                <span className="fw-bold">Fecha Pago:</span> {formatDate(payment.uploadedAt)}
                                            </div>
                                        </div>
                                    </div>

                                    {/* Botones */}
                                    <div className="d-flex gap-2">
                                        <button 
                                            className="btn btn-outline-info fw-bold d-flex align-items-center justify-content-center"
                                            style={{ flex: 1 }}
                                            onClick={() => handleViewProof(payment.paymentId)}
                                        >
                                            <FontAwesomeIcon icon={faEye} className="me-2" /> Ver Recibo
                                        </button>
                                        
                                        <button 
                                            className="btn btn-outline-danger fw-bold"
                                            onClick={() => handleStatusChange(payment.paymentId, 2)} // 2 = Rejected Enum (?)
                                            disabled={processingId === payment.paymentId}
                                            title="Rechazar Pago"
                                        >
                                            {processingId === payment.paymentId ? <FontAwesomeIcon icon={faSpinner} spin /> : <FontAwesomeIcon icon={faTimes} />}
                                        </button>

                                        <button 
                                            className="btn btn-success fw-bold"
                                            onClick={() => handleStatusChange(payment.paymentId, 1)} // 1 = Approved Enum (?)
                                            disabled={processingId === payment.paymentId}
                                            title="Aprobar Pago"
                                        >
                                            {processingId === payment.paymentId ? <FontAwesomeIcon icon={faSpinner} spin /> : <FontAwesomeIcon icon={faCheck} className="me-1" /> } Aprobar
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* MODAL PARA VER COMPROBANTE */}
            {showModal && (
                <div className="modal d-block" style={{ backgroundColor: 'rgba(0,0,0,0.8)', zIndex: 1050 }}>
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header bg-dark text-white">
                                <h5 className="modal-title">Comprobante de Pago</h5>
                                <button type="button" className="btn-close btn-close-white" onClick={closeModal}></button>
                            </div>
                            <div className="modal-body text-center p-0" style={{ height: '60vh', backgroundColor: '#f8f9fa' }}>
                                {loadingProof ? (
                                    <div className="d-flex justify-content-center align-items-center h-100">
                                        <div className="spinner-border text-primary" role="status"></div>
                                    </div>
                                ) : selectedProofUrl ? (
                                    // Utilizamos un iframe para que el navegador decida automáticamente 
                                    // si muestra una imagen o renderiza un PDF nativamente.
                                    <iframe 
                                        src={selectedProofUrl} 
                                        title="Comprobante" 
                                        width="100%" 
                                        height="100%" 
                                        style={{ border: 'none' }}
                                    />
                                ) : (
                                    <p className="mt-5 text-muted">No se pudo cargar el archivo.</p>
                                )}
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={closeModal}><FontAwesomeIcon icon={faTimes} className="me-2" />Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}