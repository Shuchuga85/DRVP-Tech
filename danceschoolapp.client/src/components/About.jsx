function About() {
    return (
        <section className="about" id="sobre">
            <div className="container about-content">
                <div className="about-text">
                    <h2>Sobre a Ent&apos;Artes</h2>
                    <p>
                        A Ent&apos;Artes é uma escola de dança dedicada ao desenvolvimento
                        artístico e técnico de crianças, jovens e adultos.
                    </p>
                    <p>
                        Oferecemos um ambiente acolhedor, com professores qualificados e
                        modalidades para vários níveis.
                    </p>

                    <ul>
                        <li>Professores qualificados</li>
                        <li>Ensino adaptado aos níveis</li>
                        <li>Sistema de gestão fácil e intuitivo</li>
                        <li>Eventos e espetáculos regulares</li>
                    </ul>
                </div>

                <div className="about-image">
                    <img
                        src="https://images.unsplash.com/photo-1508804185872-d7badad00f7d?q=80&w=900&auto=format&fit=crop"
                        alt="Bailarina"
                    />
                </div>
            </div>
        </section>
    )
}

export default About