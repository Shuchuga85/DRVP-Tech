const testimonials = [
    {
        name: 'Encarregado de educação',
        text: 'Um espaço acolhedor, com acompanhamento atento e muito profissionalismo.',
    },
    {
        name: 'Aluna',
        text: 'Gosto do ambiente da escola e da forma como evoluí em cada aula.',
    },
    {
        name: 'Família Ent’Artes',
        text: 'Uma escola onde a dança é vivida com dedicação, exigência e carinho.',
    },
]

function Testimonials() {
    return (
        <section className="testimonials" id="testemunhos">
            <div className="container">
                <h2 className="section-title">O que dizem sobre a escola</h2>

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