// Estructura común a todas las páginas: navbar arriba, contenido en el medio y
// footer abajo. La navbar cambia según el estado de sesión y el rol del usuario.
import React from "react";
import { Link } from "react-router-dom";
import { useAuth } from "@/shared/auth/AuthContext";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faRightFromBracket } from '@fortawesome/free-solid-svg-icons';

export default function Layout({ children }) {
  const { token, logout, user, loading } = useAuth();

  // Mientras se valida la sesión inicial mostramos un spinner para evitar el
  // "parpadeo" entre el estado deslogueado y el logueado.
  if (loading) {
    return (
      <div className="d-flex vh-100 justify-content-center align-items-center" style={{ background: 'var(--ivory)' }}>
        <span className="spinner-border" style={{ width: '2rem', height: '2rem' }} />
      </div>
    );
  }

  return (
    <div className="d-flex flex-column min-vh-100">
      {/* Navbar */}
      <nav className="navbar navbar-expand-lg fixed-top navbar-elegant">
        <div className="container">
          <Link className="navbar-brand" to="/">
            <span className="navbar-brand-dot">◆</span>
            Seminario I
          </Link>

          <button
            className="navbar-toggler border-0 shadow-none"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNav"
            aria-controls="navbarNav"
            aria-expanded="false"
            aria-label="Toggle navigation"
            style={{ color: 'var(--charcoal)', fontSize: '0.85rem' }}
          >
            <span className="navbar-toggler-icon" />
          </button>

          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav ms-auto align-items-center gap-1">
              <li className="nav-item">
                <Link className="nav-link" to="/">Inicio</Link>
              </li>

              {/* Si hay sesión mostramos los accesos según el rol; si no, login/registro */}
              {token ? (
                <>
                  {user?.role === 'Owner' && (
                    <li className="nav-item">
                      <Link className="nav-link" to="/owner">Panel</Link>
                    </li>
                  )}
                  {user?.role === 'User' && (
                    <li className="nav-item">
                      <Link className="nav-link" to="/my-reservations">Mis reservas</Link>
                    </li>
                  )}
                  <li className="nav-item">
                    <button type="button" onClick={logout} className="navbar-logout-btn">
                      <FontAwesomeIcon icon={faRightFromBracket} className="me-2" />
                      Cerrar sesión
                    </button>
                  </li>
                </>
              ) : (
                <>
                  <li className="nav-item">
                    <Link className="nav-link" to="/register">Registrarse</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link nav-cta-btn" to="/login">Iniciar sesión</Link>
                  </li>
                </>
              )}
            </ul>
          </div>
        </div>
      </nav>

      {/* Main content */}
      <main className="flex-grow-1" style={{ paddingTop: '66px', paddingBottom: '56px' }}>
        {children}
      </main>

      {/* Footer */}
      <footer className="footer-elegant">
        <div className="container d-flex justify-content-between align-items-center flex-wrap gap-2">
          <span className="footer-brand">
            <span style={{ color: 'var(--brand)' }}>◆ </span>Seminario I
          </span>
          <span style={{ color: 'var(--stone)' }}>
            © {new Date().getFullYear()} — Todos los derechos reservados
          </span>
        </div>
      </footer>
    </div>
  );
}
