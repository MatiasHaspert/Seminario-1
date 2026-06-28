// Barra de búsqueda principal (destino, fechas y huéspedes). Al enviar, arma una
// query string con los filtros y navega a /search para mostrar los resultados.
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { faSearch, faMapMarkerAlt, faUsers } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import DateRangePicker from "@/shared/ui/DateRangePicker";
import { toDateOnlyString } from "@/shared/utils/formatters";
import "./SearchBar.css";

export default function SearchBar() {
  const navigate = useNavigate();
  const [city, setCity] = useState("");
  const [guests, setGuests] = useState(1);
  const [checkIn, setCheckIn] = useState("");
  const [checkOut, setCheckOut] = useState("");
  const [range, setRange] = useState();

  // Arma la URL de búsqueda agregando solo los filtros que tienen valor,
  // para no ensuciar la query string con parámetros vacíos.
  const handleSubmit = (e) => {
    e.preventDefault();
    const params = new URLSearchParams();
    if (city)    params.append("city", city);
    if (guests)  params.append("guests", String(guests));
    if (checkIn) params.append("checkIn", checkIn);
    if (checkOut) params.append("checkOut", checkOut);
    const qs = params.toString();
    navigate(`/search${qs ? `?${qs}` : ""}`);
  };

  // Al elegir un rango en el calendario, guardamos las fechas en formato
  // "YYYY-MM-DD" (en hora local, sin pasar por UTC) para mandarlas al backend.
  const handleRangeSelect = (r) => {
    setRange(r);
    setCheckIn(r?.from ? toDateOnlyString(r.from) : "");
    setCheckOut(r?.to   ? toDateOnlyString(r.to)   : "");
  };

  const handleClearDates = () => {
    setRange(undefined);
    setCheckIn("");
    setCheckOut("");
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="search-bar-pill">
        {/* Destination */}
        <div className="search-field">
          <FontAwesomeIcon icon={faMapMarkerAlt} className="icon" />
          <input
            type="text"
            placeholder="¿A dónde vas?"
            value={city}
            onChange={(e) => setCity(e.target.value)}
          />
        </div>

        <div className="search-bar-divider" />

        {/* Dates */}
        <DateRangePicker
          range={range}
          onRangeChange={handleRangeSelect}
          placeholderStart="Check-in"
          placeholderEnd="Check-out"
          showClearButton={true}
          onClear={handleClearDates}
        />

        <div className="search-bar-divider" />

        {/* Guests */}
        <div className="search-field">
          <FontAwesomeIcon icon={faUsers} className="icon" />
          <input
            type="number"
            min="1"
            placeholder="Huéspedes"
            value={guests}
            onChange={(e) => setGuests(e.target.value ? Number(e.target.value) : "")}
          />
        </div>

        {/* Submit */}
        <button type="submit" className="search-submit-btn">
          <FontAwesomeIcon icon={faSearch} />
          Buscar
        </button>
      </div>
    </form>
  );
}
