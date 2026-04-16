import { useState, useEffect, useCallback } from 'react'
import '../../styles/AdminPage.css'


const PAGE_SIZE = 20
const API = 'https://localhost:7003'

function AdminUsersPage() {
    const [users, setUsers] = useState([])
    const [total, setTotal] = useState(0)
    const [page, setPage] = useState(1)
    const [search, setSearch] = useState('')
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')

    const [showModal, setShowModal] = useState(false)
    const [newEmail, setNewEmail] = useState('')
    const [modalError, setModalError] = useState('')
    const [creating, setCreating] = useState(false)

    const fetchUsers = useCallback(async (currentPage, currentSearch) => {
        setLoading(true)
        setError('')
        try {
            const params = new URLSearchParams({
                page: currentPage,
                pageSize: PAGE_SIZE,
            })
            if (currentSearch) params.set('search', currentSearch)

            const res = await fetch(`${API}/api/admin/users?${params}`, {
                credentials: 'include',
            })
            if (!res.ok) throw new Error('Erro ao carregar utilizadores')
            const data = await res.json()
            setUsers(data.items ?? [])
            setTotal(data.totalCount ?? 0)
        } catch (err) {
            setError(err.message)
        } finally {
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        const timer = setTimeout(() => {
            setPage(1)
            fetchUsers(1, search)
        }, 350)
        return () => clearTimeout(timer)
    }, [search, fetchUsers])

    useEffect(() => {
        fetchUsers(page, search)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page])

    const handleCreate = async () => {
        if (!newEmail.trim()) {
            setModalError('O email é obrigatório.')
            return
        }

        setCreating(true)
        setModalError('')

        try {
            const email = newEmail.trim()

            const res = await fetch(`${API}/api/users`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username: email.split('@')[0],
                    email,
                    firstRole: 1,
                })
            })

            if (!res.ok) {
                const e = await res.json().catch(() => ({}))
                const firstError =
                    e?.errors
                        ? Object.values(e.errors).flat()[0]
                        : null

                throw new Error(
                    firstError ||
                    e.message ||
                    e.title ||
                    'Erro ao criar conta'
                )
            }

            setShowModal(false)
            setNewEmail('')
            setPage(1)
            fetchUsers(1, search)
        } catch (err) {
            setModalError(err.message)
        } finally {
            setCreating(false)
        }
    }

    const handleActivate = async (userId) => {
        if (!window.confirm('Reativar esta conta?')) return

        try {
            const res = await fetch(`${API}/api/users/${userId}/activate`, {
                method: 'PATCH',
                credentials: 'include',
            })

            if (!res.ok) {
                const text = await res.text()
                throw new Error(text || `Erro ${res.status} ao reativar conta`)
            }

            fetchUsers(page, search)
        } catch (err) {
            alert(err.message)
        }
    }

    const handleDeactivate = async (userId) => {
        if (!window.confirm('Desativar esta conta?')) return

        try {
            const res = await fetch(`${API}/api/users/${userId}/deactivate`, {
                method: 'PATCH',
                credentials: 'include',
            })
            

            if (!res.ok) {
                const text = await res.text()
                throw new Error(text || `Erro ${res.status} ao desativar conta`)
            }

            fetchUsers(page, search)
        } catch (err) {
            alert(err.message)
        }
    }

    const openModal = () => {
        setNewEmail('')
        setModalError('')
        setShowModal(true)
    }

    const from = total === 0 ? 0 : (page - 1) * PAGE_SIZE + 1
    const to = Math.min(page * PAGE_SIZE, total)

    return (
        <section className="dashboard-page-card">
            <div className="admin-page-header">
                <div>
                    <h2>Contas de Direção</h2>
                    <p>Criar e gerir contas de utilizadores da Direção.</p>
                </div>
                <button className="admin-btn-primary" onClick={openModal}>
                    + Nova Conta
                </button>
            </div>

            <div className="admin-search-bar">
                <input
                    type="text"
                    placeholder="Pesquisar por nome ou email..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            {error && <p className="admin-error">{error}</p>}

            <div className="admin-table-wrap">
                <table className="admin-table">
                    <thead>
                        <tr>
                            <th>Nome</th>
                            <th>Email</th>
                            <th>Estado</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {loading ? (
                            <tr>
                                <td colSpan={4} className="admin-table-empty">
                                    A carregar...
                                </td>
                            </tr>
                        ) : users.length === 0 ? (
                            <tr>
                                <td colSpan={4} className="admin-table-empty">
                                    Nenhum utilizador encontrado.
                                </td>
                            </tr>
                        ) : (
                            users.map((u) => (
                                <tr key={u.userId}>
                                    <td>{u.name || '—'}</td>
                                    <td>{u.email || '—'}</td>
                                    <td>
                                        <span
                                            className={`admin-badge ${u.isActive ? 'admin-badge--active' : 'admin-badge--inactive'}`}
                                        >
                                            {u.isActive ? 'Ativo' : 'Inativo'}
                                        </span>
                                    </td>
                                    <td className="admin-actions">
                                        {u.isActive ? (
                                            <>
                                                <button
                                                    className="admin-icon-btn"
                                                    title="Editar"
                                                    onClick={() => console.log('editar', u.userId)}
                                                >
                                                    ✏
                                                </button>

                                                <button
                                                    className="admin-icon-btn admin-icon-btn--danger"
                                                    title="Desativar"
                                                    onClick={() => handleDeactivate(u.userId)}
                                                >
                                                    ✕
                                                </button>
                                            </>
                                        ) : (
                                            <button
                                                className="admin-icon-btn"
                                                title="Reativar"
                                                onClick={() => handleActivate(u.userId)}
                                            >
                                                ↺
                                            </button>
                                        )}
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>
            </div>

            <div className="admin-pagination">
                <span className="admin-pag-info">
                    {total > 0 ? `${from}–${to} de ${total}` : ''}
                </span>
                <div className="admin-pag-btns">
                    <button
                        className="admin-pag-btn"
                        onClick={() => setPage((p) => p - 1)}
                        disabled={page <= 1}
                    >
                        ‹ Anterior
                    </button>
                    <button
                        className="admin-pag-btn"
                        onClick={() => setPage((p) => p + 1)}
                        disabled={page * PAGE_SIZE >= total}
                    >
                        Próxima ›
                    </button>
                </div>
            </div>

            {showModal && (
                <div className="admin-overlay">
                    <div className="admin-modal">
                        <h3>Nova Conta de Direção</h3>
                        <p>Criar uma nova conta com acesso de Direção.</p>

                        <label htmlFor="newEmail">Email *</label>
                        <input
                            id="newEmail"
                            type="email"
                            placeholder="carlos@entartes.pt"
                            value={newEmail}
                            onChange={(e) => setNewEmail(e.target.value)}
                            onKeyDown={(e) =>
                                e.key === 'Enter' && handleCreate()
                            }
                        />

                        {modalError && (
                            <p className="admin-modal-error">{modalError}</p>
                        )}

                        <div className="admin-modal-actions">
                            <button
                                className="admin-btn-cancel"
                                onClick={() => setShowModal(false)}
                            >
                                Cancelar
                            </button>
                            <button
                                className="admin-btn-primary"
                                onClick={handleCreate}
                                disabled={creating}
                            >
                                {creating ? 'A criar...' : 'Criar Conta'}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </section>
    )
}

export default AdminUsersPage