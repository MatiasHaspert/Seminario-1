// Punto de entrada de la aplicación: monta el árbol de React en el DOM.
// Aquí se importan los estilos y scripts globales (Bootstrap) una sola vez.
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './index.css'
import App from '@/app/App.jsx'

// Buscamos el <div id="root"> del index.html y renderizamos la app dentro.
// StrictMode activa chequeos y advertencias extra de React en desarrollo.
createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
