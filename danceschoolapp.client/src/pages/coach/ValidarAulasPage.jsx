import ClassValidationCard from '../../components/ClassValidationCard'
import { useState, useEffect } from 'react'

function ValidarAulasPage() {
    const [aulas, setAulas] = useState([])

    useEffect(() => {
        fetch('/api/classes/pending')
            .then(res => res.json())
            .then(data => setAulas(data))
    }, [])

    const confirmar = async (id) => {
        await fetch(`/api/classes/${id}/confirm-professor`, {
            method: 'POST'
        })
    }

    const rejeitar = async (id) => {
        await fetch(`/api/classes/${id}/reject-professor`, {
            method: 'POST'
        })
    }

    return (
        <>
            <h1>Validar Aulas</h1>

            {aulas.map(aula => (
                <ClassValidationCard
                    key={aula.id}
                    aula={aula}
                    tipo="professor"
                    onConfirm={confirmar}
                    onReject={rejeitar}
                />
            ))}
        </>
    )
}

export default ValidarAulasPage