// Tipos de cuenta que un usuario puede elegir al registrarse. Reflejan el enum
// Role del backend (Domain/Enums/Role.cs):
//   - 'value'       es el código numérico que se envía a la API al registrar.
//   - 'name'        es el nombre del enum tal como lo devuelve la API (en inglés).
//   - 'label'       es el texto en español que se muestra al usuario.
//   - 'description' es la aclaración corta que acompaña a cada opción.
//   - 'icon'        es el ícono asociado.
// Admin se excluye a propósito: no es un rol auto-registrable (el backend lo rechaza).
import { faHouse, faKey } from '@fortawesome/free-solid-svg-icons';

export const ACCOUNT_TYPES = [
    { value: 0, name: "User",  label: "Huésped",     description: "Reservá alojamientos",    icon: faHouse },
    { value: 1, name: "Owner", label: "Propietario", description: "Publicá tus propiedades", icon: faKey },
];
