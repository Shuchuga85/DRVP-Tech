import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../../../context/useAuth'

function getPortalPath(roles) {
    const r = (roles || []).map((role) => role.toLowerCase())
    if (r.includes('admin')) return '/admin'
    if (r.includes('staff')) return '/staff'
    if (r.includes('coach')) return '/coach'
    if (r.includes('parent')) return '/parent'
    return '/'
}

function getPortalLabel(roles) {
    const r = (roles || []).map((role) => role.toLowerCase())
    if (r.includes('admin')) return 'Administração'
    if (r.includes('staff')) return 'Direção'
    if (r.includes('coach')) return 'Área do Professor'
    if (r.includes('parent')) return 'Área do Encarregado'
    return 'Portal'
}

function Navbar({ simple }) {
    const { user, isAuthenticated, logout } = useAuth()
    const navigate = useNavigate()

    const handleLogout = async () => {
        await logout()
        navigate('/')
    }

    return (
        <header className={`navbar ${simple ? 'navbar-simple' : ''}`}>
            <div className="container navbar-content">
                <Link to="/" className="logo">
                    Ent&apos;Artes
                </Link>

                {!simple && (
                    <nav className="nav-links">
                        <a href="#sobre">Sobre</a>
                        <a href="#modalidades">Modalidades</a>
                        <a href="#footer">Contacto</a>
                    </nav>
                )}

                {!simple && (
                    <div className="navbar-actions">
                        {isAuthenticated ? (
                            <>
                                <Link
                                    to={getPortalPath(user?.roles)}
                                    className="btn btn-primary"
                                >
                                    {getPortalLabel(user?.roles)}
                                </Link>
                                <button
                                    type="button"
                                    className="btn btn-secondary"
                                    onClick={handleLogout}
                                >
                                    Sair
                                </button>
                            </>
                        ) : (
                            <Link to="/login" className="btn btn-primary">
                                Login
                            </Link>
                        )}
                    </div>
                )}
            </div>
        </header>
    )
}

export default Navbar
