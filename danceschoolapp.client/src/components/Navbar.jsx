import { Link } from 'react-router-dom'

function Navbar({ simple }) {
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
                        {/*<a href="#testemunhos">Testemunhos</a>*/}
                        <a href="#footer">Contacto</a>
                    </nav>
                )}

                {!simple && (
                    <Link to="/login" className="btn btn-primary">
                        Login
                    </Link>
                )}
            </div>
        </header>
    )
}

export default Navbar