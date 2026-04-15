import { Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function ProtectedRoute({ children, allowedRoles = [] }) {
    const { user, loading, isAuthenticated } = useAuth()

    if (loading) {
        return <p style={{ padding: '2rem' }}>A carregar...</p>
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }

    if (allowedRoles.length > 0) {
        const userRoles = user?.roles || []
        const hasPermission = allowedRoles.some(role => userRoles.includes(role))

        if (!hasPermission) {
            return <Navigate to="/unauthorized" replace />
        }
    }

    return children
}

export default ProtectedRoute