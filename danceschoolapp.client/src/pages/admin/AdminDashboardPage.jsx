import { useEffect, useState } from 'react'
import PageCard from '../../components/common/PageCard'
import KpiCard from '../../components/common/KpiCard'
import { getAdminDashboardStats } from '../../services/dashboardService'
import '../../styles/AdminPage.css'

function AdminDashboardPage() {
    const [stats, setStats] = useState(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')

    useEffect(() => {
        const fetchDashboard = async () => {
            try {
                const data = await getAdminDashboardStats()
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
        <PageCard>
            <h2>Painel de Administração</h2>
            <p>Gestão de contas de Direção.</p>

            {error && <p className="admin-error">{error}</p>}

            <div className="kpi-grid">
                <KpiCard
                    label="Contas de Direção"
                    value={stats?.totalStaffAccounts}
                    loading={loading}
                />
                <KpiCard
                    label="Contas Ativas"
                    value={stats?.activeStaffAccounts}
                    loading={loading}
                />
            </div>
        </PageCard>
    )
}

export default AdminDashboardPage