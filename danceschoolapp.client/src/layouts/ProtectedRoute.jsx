import { Navigate } from 'react-router-dom'
import { useAuth } from '../context/useAuth'
function ProtectedRoute({ children, allowedRoles = [] }) {
    const { user, loading, isAuthenticated } = useAuth()

    if (loading) {
        return <p style={{ padding: '2rem' }}>A carregar...</p>
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }

    const userRoles = (user?.roles || []).map((role) => role.toLowerCase())
    const normalizedAllowedRoles = allowedRoles.map((role) => role.toLowerCase())

    const hasAccess = normalizedAllowedRoles.some((role) =>
        userRoles.includes(role)
    )

    if (!hasAccess) {
        return <Navigate to="/unauthorized" replace />
    }

    return children
}

export default ProtectedRoute