// Lista de reseñas de una propiedad (solo lectura). Pinta las estrellas llenas
// según el puntaje de cada reseña y muestra autor, comentario y fecha.
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar as solidStar } from "@fortawesome/free-solid-svg-icons";
import { faStar as regularStar } from "@fortawesome/free-regular-svg-icons";


export default function Reviews({ reviews }) {
  // Sin reseñas mostramos un texto y no renderizamos la sección.
  if (!reviews || reviews.length === 0)
    return <p className="text-muted fst-italic">No hay reseñas aún.</p>;

  return (
    <div className="mt-5">
      <h5 className="mb-4">Reseñas</h5>

      <div className="d-flex flex-column gap-3">
        {reviews.map((r) => (
          <div
            key={r.id}
            className="p-3 border rounded shadow-sm bg-light-subtle"
            style={{ backgroundColor: "#f9f9f9" }}
          >
          <div className="mb-2">
            {[1, 2, 3, 4, 5].map((star) => (
              <FontAwesomeIcon
                key={star}
                icon={star <= r.rating ? solidStar : regularStar}
                className={star <= r.rating ? "text-warning" : "text-muted"}
              />
            ))}
            {r.userName}
            <span className="ms-2 text-muted">{r.rating}/5</span>
          </div>

          <p className="mb-1" style={{ color: "#333" }}>
            {r.comment}
          </p>

          <small className="text-muted">
            Publicado el {new Date(r.date).toLocaleDateString("es-AR")}
          </small>
          {r.userName && (<small className="text-muted ms-2">
            por {r.userName}
          </small>)}
        </div>
        ))}
      </div>
    </div>
  );
}
