import { Link } from 'react-router-dom'

function Navbar() {
    return (
        <header className="navbar">
            <div className="container navbar-content">
                <h2 className="logo">Ent&apos;Artes</h2>

                <nav className="nav-links">
                    <a href="#sobre">Sobre</a>
                    <a href="#modalidades">Modalidades</a>
                    <a href="#testemunhos">Testemunhos</a>
                    <a href="#footer">Contacto</a>
                </nav>

                <Link to="/login" className="btn btn-primary">
                    Login
                </Link>            </div>
        </header>
    )
}

export default Navbar