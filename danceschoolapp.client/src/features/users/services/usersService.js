const API = 'https://localhost:7003'

export async function getAdminUsers({ page = 1, pageSize = 20, search = '' }) {
    const params = new URLSearchParams({
        page,
        pageSize,
    })

    if (search) {
        params.set('search', search)
    }

    const res = await fetch(`${API}/api/admin/users?${params}`, {
        credentials: 'include',
    })

    if (!res.ok) {
        throw new Error('Erro ao carregar utilizadores')
    }

    return res.json()
}

export async function createUser({ email, firstRole }) {
    const res = await fetch(`${API}/api/users`, {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: email.split('@')[0],
            email,
            firstRole,
        }),
    })

    if (!res.ok) {
        const e = await res.json().catch(() => ({}))
        const firstError =
            e?.errors ? Object.values(e.errors).flat()[0] : null

        throw new Error(
            firstError ||
            e.message ||
            e.title ||
            'Erro ao criar conta'
        )
    }

    return res.json().catch(() => null)
}

export async function activateUser(userId) {
    const res = await fetch(`${API}/api/users/${userId}/activate`, {
        method: 'PATCH',
        credentials: 'include',
    })

    if (!res.ok) {
        const text = await res.text()
        throw new Error(text || `Erro ${res.status} ao reativar conta`)
    }
}

export async function deactivateUser(userId) {
    const res = await fetch(`${API}/api/users/${userId}/deactivate`, {
        method: 'PATCH',
        credentials: 'include',
    })

    if (!res.ok) {
        const text = await res.text()
        throw new Error(text || `Erro ${res.status} ao desativar conta`)
    }
}