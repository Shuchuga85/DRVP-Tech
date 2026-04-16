import { Routes, Route } from 'react-router-dom'
import AppLayout from './layouts/AppLayout'
import ProtectedRoute from './layouts/ProtectedRoute'
import DashboardLayout from './layouts/DashboardLayout'

import HomePage from './pages/public/HomePage'
import LoginPage from './pages/public/LoginPage'
import UnauthorizedPage from './pages/public/UnauthorizedPage'

import AdminDashboardPage from './pages/admin/AdminDashboardPage'
import AdminUsersPage from './pages/admin/AdminUsersPage'
import StaffDashboardPage from './pages/direction/DirecaoDashboardPage'
import CoachDashboardPage from './pages/prof/ProfessorDashboardPage'
import ParentDashboardPage from './pages/parent/EncarregadoDashboardPage'

function App() {
    return (
        <Routes>
            {/* PÚBLICO */}
            <Route element={<AppLayout />}>
                <Route path="/" element={<HomePage />} />
            </Route>

            {/* LOGIN / UNAUTHORIZED */}
            <Route element={<AppLayout simple />}>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/unauthorized" element={<UnauthorizedPage />} />
            </Route>

            {/* ADMIN */}
            <Route
                element={
                    <ProtectedRoute allowedRoles={['admin']}>
                        <DashboardLayout role="admin" />
                    </ProtectedRoute>
                }
            >
                <Route path="/admin" element={<AdminDashboardPage />} />
                <Route path="/admin/utilizadores" element={<AdminUsersPage />} />
            </Route>

            {/* STAFF */}
            <Route
                element={
                    <ProtectedRoute allowedRoles={['staff', 'admin']}>
                        <DashboardLayout role="staff" />
                    </ProtectedRoute>
                }
            >
                <Route path="/staff" element={<StaffDashboardPage />} />
            </Route>

            {/* COACH */}
            <Route
                element={
                    <ProtectedRoute allowedRoles={['coach', 'admin']}>
                        <DashboardLayout role="coach" />
                    </ProtectedRoute>
                }
            >
                <Route path="/coach" element={<CoachDashboardPage />} />
            </Route>

            {/* PARENT */}
            <Route
                element={
                    <ProtectedRoute allowedRoles={['parent', 'admin']}>
                        <DashboardLayout role="parent" />
                    </ProtectedRoute>
                }
            >
                <Route path="/parent" element={<ParentDashboardPage />} />
            </Route>
        </Routes>
    )
}

export default App