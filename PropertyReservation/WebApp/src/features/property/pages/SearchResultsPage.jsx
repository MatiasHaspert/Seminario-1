// Página de resultados de búsqueda: lee los filtros desde la query string de la
// URL (?city=...&checkIn=...) y muestra las propiedades que coinciden.
import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { searchProperties } from "@/features/property/services/propertyService";
import PropertyCard from "@/features/property/components/PropertyCard";

export default function SearchResultsPage() {
    const { search } = useLocation(); // la query string actual, ej. "?city=Rosario"
    const [results, setResults] = useState([]);

    // Re-ejecutamos la búsqueda cada vez que cambian los filtros de la URL.
    useEffect(() => {
        loadResults();
    }, [search]);

    const loadResults = async () => {
        // Convertimos la query string en un objeto {clave: valor} para el backend.
        const params = Object.fromEntries(new URLSearchParams(search));
        const data = await searchProperties(params);
        setResults(data);
    };


    return (
        <div className="container mt-4">
            <h5>Resultados</h5>
            <hr />
            <div className="d-flex flex-wrap gap-3">
                {results.length === 0 ? (<p>No existen propiedades para mostrar en este momento</p>)
                :
                 (results.map((p) => <PropertyCard key={p.id} property={p} />))}
            </div>
        </div>
    );
}
