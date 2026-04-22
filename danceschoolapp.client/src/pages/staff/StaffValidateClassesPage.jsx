// Tabs: requested | finished | pending
// GET /api/staff/validate-classes?tab={tab}&page=1&pageSize=15
// PATCH /api/coachclasses/{id}/staff-approve  |  /reject  |  /staff-validate
import { useEffect, useState } from 'react'
import Tabs from '../../components/common/Tabs'
import ClassValidationCard from '../../components/common/ClassValidationCard'
import KpiCard from '../../components/common/KpiCard'
import { getValidateClasses, staffApprove, staffReject, staffValidate } from '../../services/staffService'

function StaffValidateClassesPage() {
    const tabs = [
        { label: 'Requisitadas', value: 'requested' },
        { label: 'Pendentes', value: 'pending' },
    ]

    const [activeTab, setActiveTab] = useState('requested')
    const [items, setItems] = useState([])
    const [loading, setLoading] = useState(false)
    const [stats, setStats] = useState({ requested: 0, pending: 0 })

    const fetchData = async () => {
        setLoading(true)
        try {
            const res = await getValidateClasses({ tab: activeTab, page: 1, pageSize: 20 })
            // API may return paged result
            const data = res?.Items ?? res?.items ?? res ?? []
            setItems(data)

            // If API exposes stats at top-level, use them; otherwise compute from data
            setStats(prev => ({
                requested: res?.RequestedCount ?? (activeTab === 'requested' ? data.length : prev.requested),
                pending: res?.PendingCount ?? (activeTab === 'pending' ? data.length : prev.pending),
            }))
        } catch (e) {
            console.error(e)
            setItems([])
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        fetchData()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [activeTab])

    const refresh = () => fetchData()

    const handleApprove = async (id) => {
        try {
            await staffApprove(id)
            refresh()
        } catch (e) { console.error(e) }
    }

    const handleReject = async (id) => {
        try {
            await staffReject(id)
            refresh()
        } catch (e) { console.error(e) }
    }

    const handleValidate = async (id, didTeach) => {
        try {
            await staffValidate(id, didTeach)
            refresh()
        } catch (e) { console.error(e) }
    }

    return (
        <section className="dashboard-page-card">
            <h2>Validações de Aulas</h2>
            <p>Aprovar aulas requisitadas e validar aulas pendentes após o prazo de 48h.</p>

            <div style={{display: 'flex', gap: '1rem', margin: '1rem 0'}}>
                <KpiCard label="Requisitadas" value={stats.requested} loading={loading} />
                <KpiCard label="Pendentes" value={stats.pending} loading={loading} />
            </div>

            <Tabs tabs={tabs} activeTab={activeTab} onTabChange={setActiveTab} />

            {loading && <p>Carregando...</p>}

            {!loading && items.length === 0 && (
                <div className="empty-card">
                    <div className="empty-card-body">
                        <h3>Nenhuma aula encontrada</h3>
                    </div>
                </div>
            )}

            {!loading && items.map(aula => (
                <ClassValidationCard
                    key={aula.id ?? aula.classId}
                    aula={aula}
                    tipo={activeTab === 'requested' ? 'coach-request' : 'professor'}
                    onConfirm={(id) => activeTab === 'requested' ? handleApprove(id) : handleValidate(id, true)}
                    onReject={(id) => activeTab === 'requested' ? handleReject(id) : handleValidate(id, false)}
                />
            ))}
        </section>
    )
}

export default StaffValidateClassesPage
