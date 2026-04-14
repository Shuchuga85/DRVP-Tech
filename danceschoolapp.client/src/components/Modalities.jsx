const modalities = [
    {
        title: 'Ballet Clássico',
        text: 'Base técnica e postura para todas as idades.',
        image: 'https://images.unsplash.com/photo-1508804185872-d7badad00f7d?q=80&w=800&auto=format&fit=crop',
    },
    {
        title: 'Contemporâneo',
        text: 'Expressão, movimento livre e criatividade.',
        image: 'https://images.unsplash.com/photo-1504609773096-104ff2c73ba4?q=80&w=800&auto=format&fit=crop',
    },
    {
        title: 'Jazz',
        text: 'Energia, ritmo e técnica numa modalidade dinâmica.',
        image: 'https://images.unsplash.com/photo-1544717302-de2939b7ef71?q=80&w=800&auto=format&fit=crop',
    },
    {
        title: 'Hip Hop',
        text: 'Movimento urbano, atitude e coordenação.',
        image: 'https://images.unsplash.com/photo-1508700929628-666bc8bd84ea?q=80&w=800&auto=format&fit=crop',
    },
]

function Modalities() {
    return (
        <section className="modalities" id="modalidades">
            <div className="container">
                <h2 className="section-title">Modalidades</h2>
                <p className="section-subtitle">
                    Oferecemos uma variedade de estilos de dança para todas as idades e níveis.
                </p>

                <div className="modalities-grid">
                    {modalities.map((item, index) => (
                        <div className="modality-card" key={index}>
                            <img src={item.image} alt={item.title} />
                            <div className="modality-card-body">
                                <h3>{item.title}</h3>
                                <p>{item.text}</p>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    )
}

export default Modalities