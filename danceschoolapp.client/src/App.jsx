import { Routes, Route } from 'react-router-dom'
import AppLayout from './components/AppLayout'
import HomePage from './pages/public/HomePage'
import LoginPage from './pages/public/LoginPage'
import ProtectedRoute from './components/ProtectedRoute'
import UnauthorizedPage from './pages/public/UnauthorizedPage'

import AdminPage from './pages/admin/AdminPage'
import DirectionPage from './pages/Direction/DirectionPage'

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
                    path="/Direction"
                    element={
                        <ProtectedRoute allowedRoles={['Direction', 'admin']}>
                            <DirectionPage />
                        </ProtectedRoute>
                    }
                />
            </Route>
        </Routes>
    )
}

export default App