import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import '../../styles/LoginPage.css'
import logo from '../../assets/logo-entartes.png'
import { useAuth } from '../../context/AuthContext'

function LoginPage() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

    const { refreshSession } = useAuth()
    const navigate = useNavigate()

    const handleSubmit = async (e) => {
        e.preventDefault()
        setError('')
        setLoading(true)

        try {
            const response = await fetch('https://localhost:7003/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify({
                    email: email,
                    password: password,
                }),
            })

            if (!response.ok) {
                throw new Error('Credenciais inválidas')
            }

            const data = await response.json()

            console.log('Login OK:', data)
            console.log('Roles:', data.roles)
            await refreshSession()

            const roleNames = (data.roles || []).map((role) => {
                if (typeof role === 'string') {
                    return role.toLowerCase()
                }

                if (role.roleName) {
                    return role.roleName.toLowerCase()
                }

                if (role.RoleName) {
                    return role.RoleName.toLowerCase()
                }

                return ''
            })

            // guardar user (opcional)
            localStorage.setItem(
                'user',
                JSON.stringify({
                    id: data.userId,
                    username: data.username,
                    email: data.email,
                    roles: data.roles,
                })
            )

            if (roleNames.includes('admin')) {
                navigate('/admin')
            } else if (roleNames.includes('staff')) {
                navigate('/staff')
            } else if (roleNames.includes('coach')) {
                navigate('/coach')
            } else if (roleNames.includes('parent')) {
                navigate('/parent')
            } else {
                navigate('/')
            }
        } catch (err) {
            console.error(err)
            setError(err.message || 'Erro no login')
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="login-page">
            <div className="login-card">
                <img src={logo} alt="Ent'Artes" className="login-logo" />

                <h1>Iniciar sessão</h1>

                <form onSubmit={handleSubmit} className="login-form">
                    <div className="form-group">
                        <label htmlFor="email">Email</label>
                        <input
                            id="email"
                            type="email"
                            placeholder="email@exemplo.com"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="password">Palavra-passe</label>
                        <input
                            id="password"
                            type="password"
                            placeholder="••••••••"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button
                        type="submit"
                        className="login-btn"
                        disabled={loading}
                    >
                        {loading ? 'A entrar...' : 'Entrar'}
                    </button>

                    {error && (
                        <p style={{ color: 'red', marginTop: '12px' }}>
                            {error}
                        </p>
                    )}
                </form>
            </div>
        </div>
    )
}

export default LoginPage