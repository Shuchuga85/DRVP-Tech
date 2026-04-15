import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import '../styles/LoginPage.css'
import logo from '../assets/logo-entartes.png'
function LoginPage() {
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

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
                body: JSON.stringify({
                    email: login.includes('@') ? login : null,
                    username: login.includes('@') ? null : login,
                    password,
                }),
            })

            if (!response.ok) {
                throw new Error('Credenciais inválidas')
            }

            const data = await response.json()

            console.log('Login OK:', data)

            // guardar token
            localStorage.setItem('token', data.Token)

            // guardar user
            localStorage.setItem(
                'user',
                JSON.stringify({
                    id: data.UserId,
                    username: data.Username,
                    roles: data.Roles,
                })
            )

            // redirecionar para homepage
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
                    {/* LOGIN (email ou username) */}
                    <div className="form-group">
                        <label htmlFor="login">Email ou username</label>
                        <input
                            id="login"
                            type="text"
                            placeholder="Email ou username"
                            value={login}
                            onChange={(e) => setLogin(e.target.value)}
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

                <a href="#" className="forgot-password">
                    Recuperar palavra-passe
                </a>
            </div>
        </div>
    )
}

export default LoginPage