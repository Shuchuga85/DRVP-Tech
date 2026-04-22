import { useEffect, useState } from 'react'
import ClassValidationCard from '../../components/common/ClassValidationCard'
import Tabs from '../../components/common/Tabs'

// Tabs: requests | validations
// GET /api/coach/validate?tab={tab}&page=1&pageSize=10
// PATCH /api/coachclasses/{id}/coach-accept  |  /coach-reject
// PATCH /api/coachclasses/{id}/coach-validate  body: { didTeach: true|false }

function CoachValidateClassesPage() {
    const tabs = [
        { label: 'Pedidos', value: 'requests' },
        { label: 'Validações', value: 'validations' }
    ]

    const [activeTab, setActiveTab] = useState('requests')
    const [aulas, setAulas] = useState([])
    const [loading, setLoading] = useState(false)

    const fetchAulas = async () => {
        setLoading(true)
        try {
            const res = await fetch(`/api/coach/validate?tab=${activeTab}&page=1&pageSize=20`)
            if (!res.ok) throw new Error('Erro ao carregar')
            const data = await res.json()
            // API devolve um PagedResult com Items (PascalCase) ou Items lowercase, ou um array
            setAulas(data.Items ?? data.items ?? data)
        } catch (e) {
            console.error(e)
            setAulas([])
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        fetchAulas()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [activeTab])

    const handleAccept = async (id) => {
        try {
            const route = activeTab === 'requests'
                ? `/api/coachclasses/${id}/coach-accept`
                : `/api/coachclasses/${id}/coach-validate`

            const body = activeTab === 'requests' ? null : { didTeach: true }

            await fetch(route, {
                method: 'PATCH',
                headers: body ? { 'Content-Type': 'application/json' } : undefined,
                body: body ? JSON.stringify(body) : undefined
            })
        } catch (e) {
            console.error(e)
        } finally {
            fetchAulas()
        }
    }

    const handleReject = async (id) => {
        try {
            const route = activeTab === 'requests'
                ? `/api/coachclasses/${id}/coach-reject`
                : `/api/coachclasses/${id}/coach-validate`

            const body = activeTab === 'requests' ? null : { didTeach: false }

            await fetch(route, {
                method: 'PATCH',
                headers: body ? { 'Content-Type': 'application/json' } : undefined,
                body: body ? JSON.stringify(body) : undefined
            })
        } catch (e) {
            console.error(e)
        } finally {
            fetchAulas()
        }
    }

    return (
        <section className="dashboard-page-card">
            <h2>Validar Aulas</h2>
            <p>Aceitar pedidos e confirmar realização de aulas.</p>

            <Tabs tabs={tabs} activeTab={activeTab} onTabChange={setActiveTab} />

            {loading && <p>Carregando...</p>}

            {!loading && aulas.length === 0 && (
                <div className="empty-card">
                    <div className="empty-card-body">
                        {activeTab === 'requests' ? (
                            <>
                                <h3>Sem pedidos de aulas por enquanto</h3>
                            </>
                        ) : (
                            <>
                                <h3>Sem validações por enquanto</h3>
                            </>
                        )}
                    </div>
                </div>
            )}

            {!loading && aulas.map(aula => (
                <ClassValidationCard
                    key={aula.id}
                    aula={aula}
                    tipo={activeTab === 'requests' ? 'coach-request' : 'professor'}
                    onConfirm={handleAccept}
                    onReject={handleReject}
                />
            ))}
        </section>
    )
}

export default CoachValidateClassesPage
