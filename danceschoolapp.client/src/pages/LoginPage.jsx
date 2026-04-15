import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import '../styles/LoginPage.css'
import logo from '../assets/logo-entartes.png'
import { useAuth } from '../context/AuthContext'

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

            await refreshSession()

            // guardar user (opcional)
            localStorage.setItem(
                'user',
                JSON.stringify({
                    id: data.UserId,
                    username: data.Username,
                    roles: data.Roles,
                })
            )

            navigate('/')
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

                    {/* EMAIL */}
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

                    {/* PASSWORD */}
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