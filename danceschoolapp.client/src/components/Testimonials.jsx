const testimonials = [
    {
        name: 'Maria Fernandes',
        text: 'A plataforma tornou muito mais simples marcar aulas e acompanhar o meu progresso.',
    },
    {
        name: 'João Silva',
        text: 'Excelente escola, bons professores e uma comunidade muito acolhedora.',
    },
    {
        name: 'Ana Ruiz',
        text: 'Sistema intuitivo e uma experiência muito positiva desde o início.',
    },
]

function Testimonials() {
    return (
        <section className="testimonials" id="testemunhos">
            <div className="container">
                <h2 className="section-title">O que dizem os nossos alunos</h2>

                <div className="testimonials-grid">
                    {testimonials.map((item, index) => (
                        <div className="testimonial-card" key={index}>
                            <div className="stars">★★★★★</div>
                            <p>{item.text}</p>
                            <strong>{item.name}</strong>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    )
}

export default Testimonials