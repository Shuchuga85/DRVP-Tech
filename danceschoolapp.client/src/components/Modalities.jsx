import balletImg from '../assets/ballet.jpg'
import contemporaneoImg from '../assets/contemporaneo.jpg'
import hiphopImg from '../assets/hiphop.jpg'
import jazzImg from '../assets/jazz.jpg'

const modalities = [
    {
        title: 'Ballet Clássico',
        text: 'Base técnica, postura, musicalidade e trabalho corporal.',
        image: balletImg,
    },
    {
        title: 'Contemporâneo',
        text: 'Movimento, expressão e exploração artística do corpo.',
        image: contemporaneoImg,
    },
    {
        title: 'Jazz',
        text: 'Energia, coordenação e ritmo numa abordagem dinâmica.',
        image: jazzImg,
    },
    {
        title: 'Hip Hop',
        text: 'Estilo urbano, presença e liberdade de movimento.',
        image: hiphopImg,
    },
]

function Modalities() {
    return (
        <section className="modalities" id="modalidades">
            <div className="container">
                <h2 className="section-title">Modalidades</h2>
                <p className="section-subtitle">
                    Diferentes estilos de dança para crescer, aprender e ganhar confiança.
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