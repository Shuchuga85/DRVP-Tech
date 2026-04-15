import aboutImg from '../assets/logo-entartes.png'

function About() {
    return (
        <section className="about" id="sobre">
            <div className="container about-content">
                <div className="about-text">
                    <h2>Sobre a Ent&apos;Artes</h2>
                    <p>
                        A Ent&apos;Artes é uma escola de dança em Braga dedicada à formação
                        artística, técnica e pessoal dos seus alunos.
                    </p>
                    <p>
                        Aqui valorizamos a disciplina, a criatividade e o prazer de dançar,
                        com aulas pensadas para diferentes idades e níveis.
                    </p>

                    <ul>
                        <li>Ensino artístico com acompanhamento próximo</li>
                        <li>Turmas ajustadas ao nível de cada aluno</li>
                        <li>Projetos, apresentações e momentos em palco</li>
                        <li>Ambiente acolhedor e formativo</li>
                    </ul>
                </div>

                <div className="">
                    <img src={aboutImg} alt="Aulas na Ent'Artes" />
                </div>
            </div>
        </section>
    )
}

export default About