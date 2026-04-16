const API = 'https://localhost:7003'

export async function getAdminDashboardStats() {
    const res = await fetch(`${API}/api/admin/dashboard`, {
        credentials: 'include',
    })

    if (!res.ok) {
        throw new Error('Erro ao carregar dashboard')
    }

    return res.json()
}