// Tabs: minhas-marcacoes | marcar | grupo | validar
// GET /api/ee/classes/my?from=&to=
// GET /api/ee/classes/available-slots?from=&to=&modalityId=&coachId=
// GET /api/ee/classes/open?page=1&pageSize=10
// GET /api/ee/classes/validate?page=1&pageSize=10
// POST /api/coachclasses  |  POST /api/participants
// PATCH /api/participants/{id}/parent-validate  body: { attended: true|false }
import { useCallback, useEffect, useMemo, useState } from 'react'
import PageCard from '../../components/common/PageCard'
import Button from '../../components/common/Button'
import { getMyClasses, getValidateClasses, parentValidateParticipant } from '../../services/classesService'
import '../../styles/AdminPage.css'

const TABS = [
    { id: 'minhas-marcacoes', label: 'Minhas Marcações' },
    { id: 'marcar', label: 'Criar Aula' },
    { id: 'grupo', label: 'Aulas Existentes' },
    { id: 'validar', label: 'Validar Aulas' },
]

function ParentClassesPage() {
    const [activeTab, setActiveTab] = useState('minhas-marcacoes')
    const [myClasses, setMyClasses] = useState([])
    const [validateItems, setValidateItems] = useState([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')

    // Fetch once when parent /parent route is entered. We rely on this page mounting
    // only when user navigates to the parent area. Fetch both sets needed by tabs.
    const fetchParentData = useCallback(async () => {
        setLoading(true)
        setError('')

        try {
            const [mine, toValidate] = await Promise.all([
                getMyClasses(),
                getValidateClasses({ page: 1, pageSize: 20 }),
            ])

            // getMyClasses may return an array or an object with .items
            if (Array.isArray(mine)) {
                setMyClasses(mine)
            } else if (mine && Array.isArray(mine.items)) {
                setMyClasses(mine.items)
            } else {
                setMyClasses([])
            }

            // getValidateClasses usually returns { items: [...], totalCount }
            if (Array.isArray(toValidate)) {
                setValidateItems(toValidate)
            } else if (toValidate && Array.isArray(toValidate.items)) {
                setValidateItems(toValidate.items)
            } else {
                setValidateItems([])
            }
        } catch (err) {
            setError(err.message)
        } finally {
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchParentData()
    }, [fetchParentData])

    const handleValidate = async (participantId, attended) => {
        if (!window.confirm('Confirmar validação?')) return

        try {
            await parentValidateParticipant(participantId, attended)
            // update local state
            setValidateItems((prev) => prev.filter((p) => p.participantId !== participantId))
        } catch (err) {
            alert(err.message)
        }
    }

    const tabContent = useMemo(() => {
        const fmtDate = (input) => {
            if (!input) return ''
            try {
                const d = new Date(input)
                return d.toLocaleDateString()
            } catch {
                return input
            }
        }

        const fmtTime = (input) => {
            if (!input) return ''
            try {
                const d = new Date(input)
                return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
            } catch {
                return input
            }
        }

        if (activeTab === 'minhas-marcacoes') {
            if (loading) return <p>Carregando...</p>
            if (error) return <p className="admin-error">{error}</p>
            if (!myClasses.length) return <p>Sem marcações.</p>
            return myClasses.map((c, idx) => {
                const id = c.classId ?? c.id ?? idx
                const modality = c.modalityName ?? c.modality ?? c.name ?? 'Aula'
                const student = c.studentName ?? c.student?.name ?? c.student ?? ''
                const start = c.startDatetime ?? c.date ?? c.datetime
                const time = fmtTime(start)
                const date = fmtDate(start)

                return (
                    <div key={id} className="ee-class-card">
                        <div>
                            <h4>{modality}</h4>
                            <p>{student}</p>
                            <div className="ee-class-meta">
                                <span>{date}</span>
                                <span>{time}</span>
                            </div>
                            {c.validationDeadline || c.deadline ? (
                                <div className="ee-deadline">Prazo: {fmtDate(c.validationDeadline ?? c.deadline ?? start)}{time ? `, ${time}` : ''}</div>
                            ) : null}
                        </div>
                        <div />
                    </div>
                )
            })
        }

        if (activeTab === 'validar') {
            if (loading) return <p>Carregando...</p>
            if (error) return <p className="admin-error">{error}</p>
            if (!validateItems.length) return <p>Sem itens para validar.</p>

            return validateItems.map((p, idx) => {
                const id = p.participantId ?? p.id ?? idx
                const modality = p.modalityName ?? p.modality ?? p.name ?? 'Aula'
                const student = p.studentName ?? p.student?.name ?? p.student ?? ''
                const coach = p.coachName ?? p.teacherName ?? p.coach ?? ''
                const start = p.startDatetime ?? p.date ?? p.datetime
                const time = fmtTime(start)
                const date = fmtDate(start)

                return (
                    <div key={id} className="ee-validate-card">
                        <div>
                            <h4>{modality}</h4>
                            <p>{student}</p>
                            <div className="ee-class-meta">
                                <span>{date}</span>
                                <span>{time}</span>
                                {coach ? <span>Prof. {coach}</span> : null}
                            </div>
                            {p.validationDeadline || p.deadline ? (
                                <div className="ee-deadline">Prazo: {fmtDate(p.validationDeadline ?? p.deadline ?? start)}{time ? `, ${time}` : ''}</div>
                            ) : null}
                        </div>

                        <div className="ee-validate-actions">
                            <button className="btn-confirm" onClick={() => handleValidate(id, true)}>
                                Confirmar Realização
                            </button>
                            <button className="btn-notrealized" onClick={() => handleValidate(id, false)}>
                                Não Realizada
                            </button>
                        </div>
                    </div>
                )
            })
        }

        return <p>Área em construção.</p>
    }, [activeTab, myClasses, validateItems, loading, error])

    return (
        <PageCard>
            <div className="admin-page-header">
                <div>
                    <h2>Aulas</h2>
                    <p>Gerir marcações, validações e aulas de grupo.</p>
                </div>
            </div>

            <div className="tabs">
                {TABS.map((t) => (
                    <button
                        key={t.id}
                        className={t.id === activeTab ? 'tab tab--active' : 'tab'}
                        onClick={() => setActiveTab(t.id)}
                    >
                        {t.label}
                    </button>
                ))}
            </div>

            <div className="tab-content">{tabContent}</div>
        </PageCard>
    )
}

export default ParentClassesPage
