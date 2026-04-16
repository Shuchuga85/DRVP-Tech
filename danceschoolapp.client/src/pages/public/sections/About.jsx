function About() {
    return (
        <section className="about" id="sobre">
            <div className="container about-content">
                <div className="about-text">
                    <h2>Sobre a Ent&apos;Artes</h2>

                    <p className="about-lead">
                        A Ent&apos;Artes - Escola de Dança conta já com mais de 14 anos,
                        durante os quais criou um percurso de excelência e mérito,
                        quer a nível nacional, quer a nível internacional.
                    </p>

                    <p>
                        Focada na formação em dança, a Ent&apos;Artes recebe alunos
                        a partir dos 2 anos e meio e disponibiliza aulas das mais
                        diversas modalidades.
                    </p>

                    <p>
                        Para além de um regime lúdico, a Ent&apos;Artes oferece também
                        a possibilidade de um trabalho dirigido à formação intensiva em dança.
                    </p>

                    <div className="about-extra">
                        <h3>Os nossos valores</h3>
                        <ul>
                            <li>Profissionalismo</li>
                            <li>Excelência</li>
                            <li>Rigor</li>
                            <li>Respeito</li>
                            <li>Qualidade</li>
                        </ul>

                        <h3>A nossa missão</h3>
                        <p>
                            Promover a formação em dança com elevado nível de qualidade e excelência,
                            sensibilizando o público para a dança e contribuindo para a valorização
                            e enriquecimento cultural.
                        </p>

                        <h3>A nossa visão</h3>
                        <p>
                            Ser uma instituição de referência, a nível internacional, para a formação em dança.
                        </p>
                    </div>
                </div>

                <div className="about-map-card">
                    <div className="about-map-info">
                        <h3>Onde estamos</h3>
                        <p>
                            Rua Dr. Manuel de Oliveira Machado, n.º 21 e 23,
                            R/ Chão, 4700-054 Braga
                        </p>
                    </div>

                    <div className="about-map">
                        <iframe
                            title="Mapa Ent'Artes"
                            src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d644.1943119671502!2d-8.427090462311034!3d41.57218485028663!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e1!3m2!1spt-PT!2spt!4v1776339197661!5m2!1spt-PT!2spt"
                            loading="lazy"
                            referrerPolicy="no-referrer-when-downgrade"
                        />
                    </div>
                </div>
            </div>
        </section>
    )
}

export default About