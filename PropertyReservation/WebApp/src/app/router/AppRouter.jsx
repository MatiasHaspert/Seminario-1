// Define el mapa de rutas de la aplicación (URL -> componente de página).
// Las rutas privadas se protegen con <ProtectedRoute> según el rol del usuario,
// y todas comparten el <Layout> común (navbar + footer).
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "@/shared/layout/Layout";
import ProtectedRoute from "./ProtectedRoute";

import HomePage from "@/features/property/pages/HomePage";
import SearchResultsPage from "@/features/property/pages/SearchResultsPage";
import PropertyDetailPage from "@/features/property/pages/PropertyDetailPage";
import MyPropertiesPage from "@/features/property/pages/MyPropertiesPage";
import CreatePropertyPage from "@/features/property/pages/CreatePropertyPage";
import EditPropertyPage from "@/features/property/pages/EditPropertyPage";

import LoginPage from "@/features/auth/pages/LoginPage";
import RegisterPage from "@/features/auth/pages/RegisterPage";

import ReservationPage from "@/features/reservation/pages/ReservationPage";
import ReservationCheckoutPage from "@/features/reservation/pages/ReservationCheckoutPage";
import MyReservationsPage from "@/features/reservation/pages/MyReservationsPage";
import ReservationHistoryPage from "@/features/reservation/pages/ReservationHistoryPage";

import LeaveReviewPage from "@/features/review/pages/LeaveReviewPage";

import OwnerLayout from "@/features/owner/components/OwnerLayout";
import OwnerSummaryPage from "@/features/owner/pages/OwnerSummaryPage";
import PendingPaymentsPage from "@/features/owner/pages/PendingPaymentsPage";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Layout>
                <Routes>
                    {/* Rutas Publicas */}
                    <Route path="/" element={<HomePage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route path="/search" element={<SearchResultsPage />} />
                    <Route path="/property/:id" element={<PropertyDetailPage />} />

                    {/* Rutas Privadas exclusivas del Owner — anidadas bajo OwnerLayout */}
                    <Route element={<ProtectedRoute allowedRoles={['Owner']} />}>
                        <Route path="/owner" element={<OwnerLayout />}>
                            <Route index element={<OwnerSummaryPage />} />
                            <Route path="properties" element={<MyPropertiesPage />} />
                            <Route path="properties/create" element={<CreatePropertyPage />} />
                            <Route path="properties/edit/:id" element={<EditPropertyPage />} />
                            <Route path="reservations" element={<ReservationHistoryPage />} />
                            <Route path="payments" element={<PendingPaymentsPage />} />
                        </Route>
                    </Route>

                    {/* Rutas Privadas exclusivas del User */}
                    <Route element={<ProtectedRoute allowedRoles={['User']} />}>
                        <Route path="/property/:id/reservation" element={<ReservationPage />} />
                        <Route path="/reservation/:reservationId/checkout" element={<ReservationCheckoutPage />} />
                        <Route path="/my-reservations" element={<MyReservationsPage />} />
                        <Route path="/reservation/:reservationId/review" element={<LeaveReviewPage />} />
                    </Route>
                </Routes>
            </Layout>
        </BrowserRouter>
    );
}
