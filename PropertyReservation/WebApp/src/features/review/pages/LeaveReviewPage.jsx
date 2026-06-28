import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getReservationById } from "@/features/reservation/services/reservationService";
import { createReview } from "@/features/review/services/reviewService";
import { formatDateLong } from "@/shared/utils/formatters";
import { getApiErrorMessage } from "@/shared/utils/apiError";
import { useToast } from "@/shared/ui/Toast";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faStar,
    faSpinner,
    faMapMarkerAlt,
    faCalendarAlt,
    faArrowLeft
} from "@fortawesome/free-solid-svg-icons";
import { faStar as faStarRegular } from "@fortawesome/free-regular-svg-icons";

// Página para dejar una reseña tras una estadía finalizada. Valida que la reserva
// esté "Completed" y que no tenga una reseña previa antes de permitir calificar.

// Límites de longitud del comentario (deben coincidir con la validación del backend).
const MIN_COMMENT = 10;
const MAX_COMMENT = 1000;

// Texto orientativo que se muestra según la cantidad de estrellas elegidas.
const RATING_HINTS = {
    1: "Muy mala",
    2: "Mala",
    3: "Aceptable",
    4: "Muy buena",
    5: "Excelente"
};

export default function LeaveReviewPage() {
    const navigate = useNavigate();
    const { reservationId } = useParams();
    const toast = useToast();

    const [reservation, setReservation] = useState(null);
    const [loadingInfo, setLoadingInfo] = useState(true);
    const [submitting, setSubmitting] = useState(false);

    const [rating, setRating] = useState(0);
    const [hoverRating, setHoverRating] = useState(0);
    const [comment, setComment] = useState("");

    useEffect(() => {
        const loadInfo = async () => {
            if (!reservationId) return;
            try {
                const data = await getReservationById(reservationId);
                setReservation(data);
                // Resguardos: solo se reseña una estadía finalizada y una sola vez.
                if (data.status !== "Completed") {
                    toast("Solo podés calificar reservas finalizadas.", "warning");
                    navigate("/my-reservations");
                    return;
                }
                if (data.hasReview) {
                    toast("Ya enviaste una reseña para esta estadía.", "info");
                    navigate("/my-reservations");
                }
            } catch (error) {
                console.error("Error cargando reserva", error);
                toast("No pudimos cargar los detalles de la reserva.", "error");
                navigate("/my-reservations");
            } finally {
                setLoadingInfo(false);
            }
        };
        loadInfo();
    }, [reservationId]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (rating < 1 || rating > 5) {
            toast("Seleccioná una calificación de 1 a 5 estrellas.", "warning");
            return;
        }
        if (comment.trim().length < MIN_COMMENT) {
            toast(`El comentario debe tener al menos ${MIN_COMMENT} caracteres.`, "warning");
            return;
        }
        if (comment.length > MAX_COMMENT) {
            toast(`El comentario no puede superar los ${MAX_COMMENT} caracteres.`, "warning");
            return;
        }

        setSubmitting(true);
        try {
            await createReview({
                rating,
                comment: comment.trim(),
                propertyId: reservation.propertyId
            });
            toast("¡Gracias por compartir tu experiencia!", "success");
            navigate("/my-reservations");
        } catch (error) {
            console.error(error);
            toast(getApiErrorMessage(error, "No pudimos publicar tu reseña. Intentá nuevamente."), "error");
        } finally {
            setSubmitting(false);
        }
    };

    if (loadingInfo) {
        return (
            <div className="d-flex justify-content-center mt-5">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Cargando...</span>
                </div>
            </div>
        );
    }

    if (!reservation) return null;

    // Mostramos las estrellas según el hover (vista previa) o, si no, la elección fija.
    const displayedRating = hoverRating || rating;

    return (
        <div className="container mt-4 mb-5">
            <button
                type="button"
                className="btn btn-outline-secondary btn-sm mb-3"
                onClick={() => navigate("/my-reservations")}
            >
                <FontAwesomeIcon icon={faArrowLeft} className="me-2" />
                Volver a mis reservas
            </button>

            <div className="row justify-content-center">
                <div className="col-lg-8">
                    <div className="card shadow-sm border-0 overflow-hidden mb-4">
                        <div className="row g-0">
                            <div className="col-md-4 position-relative" style={{ minHeight: "180px" }}>
                                <img
                                    src={reservation.propertyImageUrl || "https://via.placeholder.com/400x300?text=Propiedad"}
                                    alt={reservation.propertyTitle}
                                    style={{ width: "100%", height: "100%", objectFit: "cover", position: "absolute" }}
                                />
                            </div>
                            <div className="col-md-8">
                                <div className="card-body">
                                    <small className="label-overline">Calificá tu estadía</small>
                                    <h3 className="mb-2">{reservation.propertyTitle}</h3>
                                    <p className="text-muted small mb-2">
                                        <FontAwesomeIcon icon={faCalendarAlt} className="me-2 text-secondary" />
                                        {formatDateLong(reservation.startDate)} al {formatDateLong(reservation.endDate)}
                                    </p>
                                    <p className="text-muted small mb-0">
                                        <FontAwesomeIcon icon={faMapMarkerAlt} className="me-2 text-danger" />
                                        Reserva #{reservation.id}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="card shadow-sm border-0">
                        <div className="card-body p-4">
                            <form onSubmit={handleSubmit}>
                                <div className="mb-4 text-center">
                                    <label className="form-label fw-bold d-block mb-3">
                                        ¿Cómo fue tu experiencia?
                                    </label>
                                    <div
                                        className="d-inline-flex gap-2"
                                        onMouseLeave={() => setHoverRating(0)}
                                    >
                                        {[1, 2, 3, 4, 5].map((value) => {
                                            const filled = value <= displayedRating;
                                            return (
                                                <button
                                                    key={value}
                                                    type="button"
                                                    className="btn p-1 border-0 bg-transparent"
                                                    style={{ fontSize: "2rem", lineHeight: 1, color: filled ? "#f59e0b" : "#a8a29e" }}
                                                    onMouseEnter={() => setHoverRating(value)}
                                                    onClick={() => setRating(value)}
                                                    aria-label={`${value} estrellas`}
                                                >
                                                    <FontAwesomeIcon icon={filled ? faStar : faStarRegular} />
                                                </button>
                                            );
                                        })}
                                    </div>
                                    <div className="text-muted small mt-2" style={{ minHeight: "1.25rem" }}>
                                        {displayedRating > 0 ? RATING_HINTS[displayedRating] : "Tocá las estrellas para calificar"}
                                    </div>
                                </div>

                                <div className="mb-3">
                                    <label htmlFor="review-comment" className="form-label fw-bold">
                                        Contanos cómo te fue
                                    </label>
                                    <textarea
                                        id="review-comment"
                                        className="form-control"
                                        rows={6}
                                        value={comment}
                                        onChange={(e) => setComment(e.target.value)}
                                        placeholder="Compartí los detalles que te parezcan más útiles para futuros huéspedes..."
                                        maxLength={MAX_COMMENT}
                                        required
                                    />
                                    <div className="d-flex justify-content-between form-text">
                                        <span>Mínimo {MIN_COMMENT} caracteres.</span>
                                        <span>{comment.length} / {MAX_COMMENT}</span>
                                    </div>
                                </div>

                                <button
                                    type="submit"
                                    className="btn btn-primary w-100 py-2 fw-bold"
                                    disabled={submitting || rating < 1 || comment.trim().length < MIN_COMMENT}
                                >
                                    {submitting ? (
                                        <span><FontAwesomeIcon icon={faSpinner} spin className="me-2" /> Publicando...</span>
                                    ) : (
                                        <span><FontAwesomeIcon icon={faStar} className="me-2" /> Publicar reseña</span>
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
