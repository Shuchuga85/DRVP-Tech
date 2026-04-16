import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import '../../styles/AdminPage.css'

function AdminDashboardPage() {
    const [stats, setStats] = useState(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')
    const navigate = useNavigate()

    useEffect(() => {
        const fetchDashboard = async () => {
            try {
                const res = await fetch('https://localhost:7003/api/admin/dashboard', {
                    credentials: 'include',
                })
                if (!res.ok) throw new Error('Erro ao carregar dashboard')
                const data = await res.json()
                setStats(data)
            } catch (err) {
                setError(err.message)
            } finally {
                setLoading(false)
            }
        }

        fetchDashboard()
    }, [])

    return (
        <section className="dashboard-page-card">
            <h2>Painel de Administração</h2>
            <p>Gestão de contas de Direção.</p>

            {error && <p className="admin-error">{error}</p>}

            <div className="admin-kpi-grid">
                <div className="admin-kpi-card">
                    <span className="admin-kpi-label">Contas de Direção</span>
                    <span className="admin-kpi-value">
                        {loading ? '—' : stats?.totalStaffAccounts ?? '—'}
                    </span>
                </div>
                <div className="admin-kpi-card">
                    <span className="admin-kpi-label">Contas Ativas</span>
                    <span className="admin-kpi-value">
                        {loading ? '—' : stats?.activeStaffAccounts ?? '—'}
                    </span>
                </div>
            </div>

            {/*<div className="admin-quick-links">*/}
            {/*    <p className="admin-section-label">Acesso rápido</p>*/}
            {/*    <div*/}
            {/*        className="admin-quick-card"*/}
            {/*        onClick={() => navigate('/admin/users')}*/}
            {/*    >*/}
            {/*        <div>*/}
            {/*            <p className="admin-quick-title">Contas de Direção</p>*/}
            {/*            <p className="admin-quick-sub">*/}
            {/*                Criar e gerir utilizadores com acesso de direção*/}
            {/*            </p>*/}
            {/*        </div>*/}
            {/*        <span className="admin-quick-arrow">→</span>*/}
            {/*    </div>*/}
            {/*</div>*/}
        </section>
    )
}

export default AdminDashboardPage