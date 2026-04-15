import { Routes, Route } from 'react-router-dom'
import AppLayout from './components/AppLayout'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import AdminPage from './pages/AdminPage'
import StaffPage from './pages/StaffPage'
import ProtectedRoute from './components/ProtectedRoute'
import UnauthorizedPage from './pages/UnauthorizedPage'

function App() {
    return (
        <Routes>
            {/* LOGIN com navbar simples */}
            <Route element={<AppLayout simple />}>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/unauthorized" element={<UnauthorizedPage />} />
            </Route>

            {/* RESTO normal */}
            <Route element={<AppLayout />}>
                <Route path="/" element={<HomePage />} />

                <Route
                    path="/admin"
                    element={
                        <ProtectedRoute allowedRoles={['admin']}>
                            <AdminPage />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/staff"
                    element={
                        <ProtectedRoute allowedRoles={['staff', 'admin']}>
                            <StaffPage />
                        </ProtectedRoute>
                    }
                />
            </Route>
        </Routes>
    )
}

export default App