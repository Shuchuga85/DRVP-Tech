function Hero() {
    return (
        <section className="hero">
            <div className="container hero-content">
                <div className="hero-text">
                    <h1>Bem-vindo à Ent&apos;Artes</h1>
                    <p>
                        A sua escola de dança de referência. Sistema integrado de gestão
                        de aulas, inscrições e eventos para alunos, professores e staff.
                    </p>

                    <div className="hero-buttons">
                        <button className="btn btn-primary">Agendar aula</button>
                        <button className="btn btn-secondary">Saber mais</button>
                    </div>
                </div>

                <div className="hero-image">
                    <img
                        src="https://images.unsplash.com/photo-1515169067868-5387ec356754?q=80&w=1200&auto=format&fit=crop"
                        alt="Escola de dança"
                    />
                </div>
            </div>
        </section>
    )
}

export default Hero