// Página de inicio: muestra el hero con la barra de búsqueda y un listado de
// propiedades destacadas que se cargan del backend al montar el componente.
import { useEffect, useState } from "react";
import { getProperties } from "@/features/property/services/propertyService";
import PropertyCard from "@/features/property/components/PropertyCard";
import SearchBar from "@/shared/ui/SearchBar";

export default function HomePage() {
  const [properties, setProperties] = useState([]);

  // Al montar la página pedimos las propiedades una sola vez (deps vacías).
  useEffect(() => {
    getProperties()
      .then(setProperties)
      .catch((err) => console.error("Error cargando propiedades:", err));
  }, []);

  return (
    <>
      {/* Hero */}
      <section className="hero-section">
        <div className="hero-glow" />
        <div className="container hero-content">
          <span className="hero-overline animate-fade-up">Descubrí tu próximo destino</span>
          <h1 className="hero-title animate-fade-up animate-fade-up-delay-1">
            Alojamientos<br />
            <em>únicos</em>, experiencias<br />
            inolvidables
          </h1>
          <p className="hero-lead animate-fade-up animate-fade-up-delay-2">
            Explorá propiedades cuidadosamente seleccionadas para cada tipo de viaje,
            desde escapadas íntimas hasta aventuras en familia.
          </p>
          <div className="animate-fade-up animate-fade-up-delay-3">
            <SearchBar />
          </div>
        </div>
      </section>

      {/* Properties */}
      <section className="properties-section">
        <div className="container">
          <div className="section-header animate-fade-in">
            <div>
              <p className="section-overline">Explorar</p>
              <h2 className="section-title">Alojamientos destacados</h2>
            </div>
          </div>

          <div className="properties-grid">
            {properties.length === 0 ? (
              <div className="empty-state">
                <p>No hay propiedades disponibles en este momento.</p>
              </div>
            ) : (
              properties.map((p, i) => (
                <div
                  key={p.id}
                  className="animate-fade-up"
                  style={{ animationDelay: `${0.05 + i * 0.07}s` }}
                >
                  <PropertyCard property={p} />
                </div>
              ))
            )}
          </div>
        </div>
      </section>
    </>
  );
}
