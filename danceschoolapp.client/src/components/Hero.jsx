import heroImg from '../assets/hero-entartes.jpg'

function Hero() {
    return (
        <section className="hero">
            <div className="container hero-content">
                <div className="hero-text">
                    <h1>Bem-vindo à Ent&apos;Artes</h1>
                    <p>
                        Escola de dança em Braga, com formação artística para crianças,
                        jovens e adultos, num ambiente de aprendizagem, expressão e
                        crescimento.
                    </p>

                    <div className="hero-buttons">
                        <button className="btn btn-primary">Marcar aula</button>
                        <button className="btn btn-secondary">Conhecer a escola</button>
                    </div>
                </div>

                <div className="hero-image">
                    <img src={heroImg} alt="Ent'Artes Escola de Dança" />
                </div>
            </div>
        </section>
    )
}

export default Hero