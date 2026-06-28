// Métodos de pago aceptados. Reflejan el enum PaymentMethod del backend:
//   - 'value' es el código numérico que se envía al crear el pago.
//   - 'name'  es el nombre del enum tal como lo devuelve la API (en inglés).
//   - 'label' es el texto en español que se muestra al usuario.
//   - 'icon'  es el ícono asociado al método.
// Centralizamos esto acá para que tanto el checkout (huésped) como la revisión
// de pagos (dueño) usen las mismas etiquetas y no se dupliquen valores.
import {
    faMoneyCheckAlt,
    faCreditCard,
    faUniversity
} from '@fortawesome/free-solid-svg-icons';
import { faPaypal } from '@fortawesome/free-brands-svg-icons';

export const PAYMENT_METHODS = [
    { value: 0, name: "CreditCard", label: "Tarjeta de Credito", icon: faCreditCard },
    { value: 1, name: "DebitCard", label: "Tarjeta de Debito", icon: faMoneyCheckAlt },
    { value: 2, name: "PayPal", label: "PayPal", icon: faPaypal },
    { value: 3, name: "BankTransfer", label: "Transferencia Bancaria", icon: faUniversity },
];

// Traduce el método de pago a su etiqueta en español. Acepta tanto el nombre
// del enum que devuelve la API (ej. "CreditCard") como su código numérico.
// Si no se reconoce, devuelve el valor original para no perder información.
export function getPaymentMethodLabel(method) {
    const found = PAYMENT_METHODS.find(
        (m) => m.name === method || m.value === method
    );
    return found?.label ?? method ?? "-";
}
