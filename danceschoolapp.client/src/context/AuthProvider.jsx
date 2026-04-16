import { useEffect, useMemo, useState } from 'react'
import { AuthContext } from './AuthContext'

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null)
    const [loading, setLoading] = useState(true)

    const refreshSession = async () => {
        try {
            const response = await fetch('https://localhost:7003/api/auth/me', {
                method: 'GET',
                credentials: 'include',
            })

            if (response.status === 401) {
                setUser(null)
                return
            }

            const data = await response.json()
            setUser(data)
        } catch (error) {
            console.error('Erro ao restaurar sessão:', error)
            setUser(null)
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        refreshSession()
    }, [])

    const logout = async () => {
        try {
            await fetch('https://localhost:7003/api/auth/logout', {
                method: 'POST',
                credentials: 'include',
            })
        } catch (error) {
            console.error('Erro no logout:', error)
        } finally {
            setUser(null)
            localStorage.removeItem('user')
        }
    }

    const value = useMemo(() => ({
        user,
        loading,
        logout,
        refreshSession,
        isAuthenticated: !!user,
    }), [user, loading])

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}