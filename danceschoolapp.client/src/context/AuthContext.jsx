import { createContext, useContext, useEffect, useMemo, useState } from 'react'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null)
    const [loading, setLoading] = useState(true)

    const fetchMe = async () => {
        try {
            const response = await fetch('https://localhost:7003/api/auth/me', {
                method: 'GET',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
            })

            if (!response.ok) {
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
        fetchMe()
    }, [])

    const login = (userData) => {
        setUser(userData)
    }

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
        }
    }

    const hasRole = (role) => {
        if (!user?.roles) return false
        return user.roles.includes(role)
    }

    const value = useMemo(() => ({
        user,
        loading,
        login,
        logout,
        hasRole,
        refreshSession: fetchMe,
        isAuthenticated: !!user,
    }), [user, loading])

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const context = useContext(AuthContext)

    if (!context) {
        throw new Error('useAuth tem de ser usado dentro de AuthProvider')
    }

    return context
}