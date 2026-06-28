// src/routes/ProtectedRoute.jsx
// Componente "guardián" de rutas: controla el acceso a las páginas privadas.
// Se usa como ruta envolvente; si el usuario está autenticado (y tiene el rol
// permitido) renderiza las rutas hijas mediante <Outlet>, si no, redirige.
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth }  from "@/shared/auth/AuthContext";

const ProtectedRoute = ({ allowedRoles }) => {
    const { user, loading } = useAuth();
    const location = useLocation();

    // Si todavia estamos verificando el token (ej: al refrescar pagina), mostramos nada o un spinner
    if (loading) {
        return (
            <div className="d-flex vh-100 justify-content-center align-items-center">
                <span className="spinner-border text-primary" />
            </div>
        );
    }
    // Si no hay usuario autenticado, redirigimos al Login
    if (!user) {
        // 'state={{ from: location }}' nos sirve para que, tras loguearse,
        // podamos redirigir al usuario a la pagina que intentaba ver originalmente.
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    // Si la ruta restringe por roles y el usuario no esta en la lista, lo mandamos al Home
    if (allowedRoles && allowedRoles.length > 0 && !allowedRoles.includes(user.role)) {
        return <Navigate to="/" replace />;
    }

    // Si esta autenticado (y autorizado), renderizamos las rutas hijas (Outlet)
    return <Outlet />;
};

export default ProtectedRoute;
