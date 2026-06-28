import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { createPayment } from "@/features/payment/services/paymentService";
import { getReservationById } from "@/features/reservation/services/reservationService";
import { PAYMENT_METHODS } from "@/features/payment/constants/paymentMethods";
import { useToast } from "@/shared/ui/Toast";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faCloudUploadAlt,
    faSpinner
} from '@fortawesome/free-solid-svg-icons';

// Checkout de la reserva: el huésped elige el método de pago y sube el
// comprobante. Una vez enviado, la reserva queda "en revisión" para el dueño.

export default function ReservationCheckoutPage() {
    const navigate = useNavigate();
    const { reservationId } = useParams();
    const toast = useToast();
    const [reservation, setReservation] = useState(null);
    const [file, setFile] = useState(null);
    const [paymentMethod, setPaymentMethod] = useState("");
    const [uploading, setUploading] = useState(false);
    const [loadingInfo, setLoadingInfo] = useState(true);

    useEffect(() => {
        const loadInfo = async () => {
            if (!reservationId) return;
            try {
                const data = await getReservationById(reservationId);
                setReservation(data);
            } catch (error) {
                console.error("Error cargando reserva", error);
                toast("No pudimos cargar los detalles de la reserva.", "error");
            } finally {
                setLoadingInfo(false);
            }
        };
        loadInfo();
    }, [reservationId]);

    const handleFileChange = (e) => {
        setFile(e.target.files[0]);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Tanto el comprobante como el método de pago son obligatorios.
        if (!file) { toast("Por favor seleccioná el comprobante.", "warning"); return; }
        if (!paymentMethod) { toast("Por favor seleccioná un método de pago.", "warning"); return; }

        setUploading(true);

        try {
            // Usamos FormData porque enviamos un archivo (multipart), no JSON.
            const formData = new FormData();

            formData.append("ReservationId", reservationId);
            formData.append("File", file);
            formData.append("PaymentMethod", paymentMethod);

            await createPayment(formData);

            toast("Comprobante subido exitosamente. Tu reserva está en revisión.", "success");
            navigate("/my-reservations");
        } catch (error) {
            console.error(error);
            toast(getApiErrorMessage(error, "No se pudo subir el pago. Intentá nuevamente."), "error");
        } finally {
            setUploading(false);
        }
    };

    if (loadingInfo) return <div className="p-5 text-center">Cargando información...</div>;

    return (
        <div className="container mt-5 mb-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className="card shadow">
                        <div className="card-header bg-success text-white text-center">
                            <h4 className="mb-0">Finalizar Pago</h4>
                        </div>
                        <div className="card-body p-4">
                            {reservation && (
                                <div className="alert alert-light border text-center mb-4">
                                    <h5 className="text-muted">Total a pagar:</h5>
                                    <h2 className="text-success">${reservation.totalPrice?.toLocaleString()}</h2>
                                    <p className="small text-muted mb-0">Reserva #{reservationId}</p>
                                </div>
                            )}

                            <form onSubmit={handleSubmit}>
                                {/* 1. Selector de Método de Pago */}
                                <div className="mb-4">
                                    <label className="form-label fw-bold">Método de Pago</label>
                                    <select
                                        className="form-select"
                                        value={paymentMethod}
                                        onChange={(e) => setPaymentMethod(e.target.value)}
                                        required
                                    >
                                        <option value="">Selecciona una opción...</option>
                                        {PAYMENT_METHODS.map((method) => (
                                            <option key={method.value} value={method.value}>
                                                {method.label}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                {/* 2. Input de Archivo */}
                                <div className="mb-4">
                                    <label htmlFor="file-upload" className="form-label fw-bold">
                                        Subir Comprobante
                                    </label>
                                    <input
                                        id="file-upload"
                                        type="file"
                                        className="form-control"
                                        accept="image/jpeg,image/png,application/pdf"
                                        onChange={handleFileChange}
                                        required
                                    />
                                    <div className="form-text">
                                        Aceptamos JPG, PNG o PDF.
                                    </div>
                                </div>

                                {/* Botón de Envío */}
                                <button
                                    type="submit"
                                    className="btn btn-primary w-100 py-2 fw-bold"
                                    disabled={uploading || !file || !paymentMethod}
                                >
                                    {uploading ? (
                                        <span><FontAwesomeIcon icon={faSpinner} spin /> Procesando...</span>
                                    ) : (
                                        <span>
                                            <FontAwesomeIcon icon={faCloudUploadAlt} className="me-2" />
                                            Confirmar Pago
                                        </span>
                                    )}
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}