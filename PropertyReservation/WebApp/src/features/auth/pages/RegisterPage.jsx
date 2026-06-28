// Página de registro: valida el formulario y crea la cuenta en el backend.
// Tras registrarse, redirige al login para que el usuario inicie sesión.
import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { registerUser } from "@/features/auth/services/authService";
import { useToast } from "@/shared/ui/Toast";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { ACCOUNT_TYPES } from "@/features/auth/constants/accountTypes";

export default function RegisterPage() {
  const navigate = useNavigate();
  const toast = useToast();

  const [formData, setFormData] = useState({
    nombre: "",
    apellido: "",
    email: "",
    password: "",
    confirmPassword: "",
    role: 0, // 0 = Huésped (por defecto), 1 = Propietario
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // Verificamos que ambas contraseñas coincidan antes de llamar al backend.
    if (formData.password !== formData.confirmPassword) {
      setError("Las contraseñas no coinciden.");
      return;
    }

    setLoading(true);
    try {
      // Mapeamos los campos del formulario (en español) a las claves que espera
      // el backend (Name, LastName, Email, Password).
      const userData = {
        Name: formData.nombre,
        LastName: formData.apellido,
        Email: formData.email,
        Password: formData.password,
        role: formData.role, // número: 0 = Huésped, 1 = Propietario
      };
      await registerUser(userData);
      toast("¡Registro exitoso! Ahora podés iniciar sesión.", "success");
      navigate("/login");
    } catch (err) {
      setError(err.message || "Error al registrar el usuario.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card animate-scale-in" style={{ maxWidth: '520px' }}>
        <div className="auth-logo">
          <span className="auth-logo-dot">◆</span>
          Seminario I
        </div>

        <h1 className="auth-heading">Crear cuenta</h1>
        <p className="auth-subheading">Completá tus datos para comenzar</p>

        {error && (
          <div className="alert alert-danger mb-3" role="alert">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {/* Tipo de cuenta: el usuario elige registrarse como Huésped u Propietario */}
          <div className="mb-4">
            <label className="form-label">Tipo de cuenta</label>
            <div className="row g-2">
              {ACCOUNT_TYPES.map((tipo) => (
                <div className="col" key={tipo.value}>
                  <button
                    type="button"
                    className={`role-card${formData.role === tipo.value ? " selected" : ""}`}
                    onClick={() => setFormData((prev) => ({ ...prev, role: tipo.value }))}
                    aria-pressed={formData.role === tipo.value}
                  >
                    <FontAwesomeIcon icon={tipo.icon} className="role-card-icon" />
                    <span className="role-card-title">{tipo.label}</span>
                    <span className="role-card-desc">{tipo.description}</span>
                  </button>
                </div>
              ))}
            </div>
          </div>

          {/* Nombre y Apellido */}
          <div className="row mb-3">
            <div className="col">
              <label className="form-label" htmlFor="reg-nombre">Nombre</label>
              <input
                id="reg-nombre"
                type="text"
                className="form-control"
                name="nombre"
                placeholder="Martín"
                value={formData.nombre}
                onChange={handleChange}
                required
              />
            </div>
            <div className="col">
              <label className="form-label" htmlFor="reg-apellido">Apellido</label>
              <input
                id="reg-apellido"
                type="text"
                className="form-control"
                name="apellido"
                placeholder="García"
                value={formData.apellido}
                onChange={handleChange}
                required
              />
            </div>
          </div>

          {/* Email */}
          <div className="mb-3">
            <label className="form-label" htmlFor="reg-email">Correo electrónico</label>
            <input
              id="reg-email"
              type="email"
              className="form-control"
              name="email"
              placeholder="tu@email.com"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          {/* Password */}
          <div className="mb-3">
            <label className="form-label" htmlFor="reg-password">Contraseña</label>
            <input
              id="reg-password"
              type="password"
              className="form-control"
              name="password"
              placeholder="Mínimo 6 caracteres"
              value={formData.password}
              onChange={handleChange}
              required
              minLength={6}
            />
          </div>

          {/* Confirm Password */}
          <div className="mb-4">
            <label className="form-label" htmlFor="reg-confirm">Confirmar contraseña</label>
            <input
              id="reg-confirm"
              type="password"
              className="form-control"
              name="confirmPassword"
              placeholder="••••••••"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
            />
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100"
            disabled={loading}
            style={{ padding: '0.7rem' }}
          >
            {loading ? (
              <span className="spinner-border spinner-border-sm me-2" role="status" />
            ) : (
              <FontAwesomeIcon icon={faUserPlus} className="me-2" />
            )}
            {loading ? "Registrando..." : "Crear cuenta"}
          </button>
        </form>

        <hr className="auth-divider" />

        <p className="auth-footer-text">
          ¿Ya tenés cuenta?{" "}
          <Link to="/login">Iniciá sesión</Link>
        </p>
      </div>
    </div>
  );
}
