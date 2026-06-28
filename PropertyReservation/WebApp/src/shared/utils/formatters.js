// Utilidades de formato para mostrar precios y fechas con la convención local
// argentina (es-AR). Centralizarlas evita repetir lógica de formato en la UI.

// Formatea un número como precio en pesos (ej. 12000 -> "$12.000").
// Si el monto es null/undefined se usa 0 como valor por defecto.
export const formatPrice = (amount) =>
    `$${(amount ?? 0).toLocaleString("es-AR")}`;

// Parsea una fecha "solo fecha" (DateOnly del backend, ej. "2026-07-01") como
// fecha LOCAL. Si se pasara a new Date() directamente, el string se interpreta
// en UTC y, al mostrarlo en hora local (Argentina, UTC-3), se corre un día hacia
// atrás. Por eso construimos la fecha por componentes, sin conversión de zona.
// No usar para timestamps con hora (DateTime): para esos va new Date() directo.
export const parseLocalDate = (value) => {
    if (!value) return null;
    if (value instanceof Date) return value;
    const [year, month, day] = String(value).split("T")[0].split("-").map(Number);
    if (!year || !month || !day) return null;
    return new Date(year, month - 1, day);
};

// Convierte un objeto Date a string "yyyy-MM-dd" usando sus componentes LOCALES.
// Es el inverso de parseLocalDate y se usa para mandar fechas (DateOnly) al backend.
// No usar date.toISOString(): eso pasa a UTC y en husos UTC+ adelanta un día.
export const toDateOnlyString = (date) => {
    if (!(date instanceof Date) || Number.isNaN(date.getTime())) return "";
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
};

// Helper interno: parsea la fecha como local y la formatea según 'options'.
// Devuelve "-" cuando no hay fecha válida, para no mostrar "Invalid Date".
const formatDateOnly = (date, options) => {
    const parsed = parseLocalDate(date);
    return parsed ? parsed.toLocaleDateString("es-AR", options) : "-";
};

// Formato libre: el llamador pasa sus propias opciones de toLocaleDateString.
export const formatDateAR = (date, options) => formatDateOnly(date, options);

// Formato corto numérico, ej. "01/07/2026".
export const formatDateShort = (date) =>
    formatDateOnly(date, {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
    });

// Formato largo con el mes en palabras, ej. "1 de julio de 2026".
export const formatDateLong = (date) =>
    formatDateOnly(date, {
        day: "numeric",
        month: "long",
        year: "numeric",
    });
