import balletImg from '../assets/ballet.jpg'
import contemporaneoImg from '../assets/contemporaneo.jpg'
import jazzImg from '../assets/jazz.jpg'
import hiphopImg from '../assets/hiphop.jpg'
import sevilhanasImg from '../assets/sevilhanas.jpg'
import teatroMusicalImg from '../assets/teatro-musical.jpg'
import ginasticaImg from '../assets/ginastica-acrobatica.jpg'
import flexibilidadeImg from '../assets/flexibilidade.jpg'
import bodyBalanceImg from '../assets/body-balance.jpg'
import acrodanceImg from '../assets/acrodance.jpg'

function Modalities() {
    const modalities = [
        {
            title: 'Ballet Clássico',
            text: 'Base técnica, postura e musicalidade.',
            image: balletImg,
        },
        {
            title: 'Dança Contemporânea',
            text: 'Expressão, movimento e criatividade corporal.',
            image: contemporaneoImg,
        },
        {
            title: 'Jazz',
            text: 'Energia, ritmo e coordenação dinâmica.',
            image: jazzImg,
        },
        {
            title: 'Hip Hop',
            text: 'Estilo urbano, presença e liberdade de movimento.',
            image: hiphopImg,
        },
        {
            title: 'Sevilhanas',
            text: 'Ritmo, elegância e tradição espanhola.',
            image: sevilhanasImg,
        },
        {
            title: 'Teatro Musical',
            text: 'Interpretação, expressão e performance em palco.',
            image: teatroMusicalImg,
        },
        {
            title: 'Ginástica Acrobática',
            text: 'Força, equilíbrio e trabalho em equipa.',
            image: ginasticaImg,
        },
        {
            title: 'Flexibilidade',
            text: 'Mobilidade, alongamento e controlo corporal.',
            image: flexibilidadeImg,
        },
        {
            title: 'Body Balance',
            text: 'Bem-estar, postura e consciência do corpo.',
            image: bodyBalanceImg,
        },
        {
            title: 'Acrodance',
            text: 'Técnica de dança com elementos acrobáticos.',
            image: acrodanceImg,
        },
    ]

    return (
        <section className="modalities" id="modalidades">
            <div className="container">
                <h2 className="section-title">Modalidades</h2>
                <p className="section-subtitle">
                    Diferentes estilos de dança para crescer, aprender e evoluir.
                </p>

                <div className="modalities-grid">
                    {modalities.map((item, index) => (
                        <article className="modality-card" key={index}>
                            <img src={item.image} alt={item.title} />
                            <div className="modality-card-body">
                                <h3>{item.title}</h3>
                                <p>{item.text}</p>
                            </div>
                        </article>
                    ))}
                </div>
            </div>
        </section>
    )
}

export default Modalities