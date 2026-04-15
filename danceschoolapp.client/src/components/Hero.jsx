import heroImg from '../assets/logo-entartes.png'

function Hero() {
    return (
        <section className="hero">
            <div className="container hero-content">
                <div className="hero-text">
                    <h1>
                        Aprender, criar e evoluir <br />
                        através da dança
                    </h1>

                    <p>
                        Aulas para diferentes idades e níveis, num ambiente criativo e acolhedor.
                    </p>
                </div>

                <div className="">
                    <img src={heroImg} alt="Ent'Artes Escola de Dança" />
                </div>
            </div>
        </section>
    )
}

export default Hero