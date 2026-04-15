import { useState } from 'react'
import '../styles/AdminUsersPage.css'

function AdminUsersPage() {
    const [showModal, setShowModal] = useState(true)

    const users = [
        {
            id: 1,
            name: 'Carlos Rodrigues',
            email: 'carlos@example.com',
            status: 'Ativo',
        },
    ]

    return (
        <div className="admin-layout">
            <aside className="admin-sidebar">
                <div className="admin-brand">
                    <div className="admin-brand-logo">Ent&apos;Artes</div>
                    <div className="admin-brand-subtitle">Escola de Dança</div>
                </div>

                <nav className="admin-nav">
                    <button className="admin-nav-item">
                        <span>🏠</span>
                        <span>Dashboard</span>
                    </button>

                    <button className="admin-nav-item active">
                        <span>👤</span>
                        <span>Utilizadores</span>
                    </button>
                </nav>
            </aside>

            <main className="admin-main">
                <header className="admin-topbar">
                    <div className="admin-user-chip">
                        <div className="admin-user-avatar">A</div>
                        <span>Ana Costa</span>
                    </div>

                    <button className="admin-logout">Sair</button>
                </header>

                <section className="admin-content-card">
                    <div className="admin-header-row">
                        <div>
                            <h1>Contas de Direção</h1>
                            <p>Criar e gerir contas de utilizadores da Direção.</p>
                        </div>

                        <button
                            className="primary-btn"
                            onClick={() => setShowModal(true)}
                        >
                            Nova Conta
                        </button>
                    </div>

                    <div className="admin-search-box">
                        <input
                            type="text"
                            placeholder="Pesquisar por nome ou email..."
                        />
                    </div>

                    <div className="admin-table-wrapper">
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
                                {users.map((user) => (
                                    <tr key={user.id}>
                                        <td>{user.name}</td>
                                        <td>{user.email}</td>
                                        <td>
                                            <span className="status-badge active">
                                                {user.status}
                                            </span>
                                        </td>
                                        <td>
                                            <div className="table-actions">
                                                <button className="icon-btn edit">✏️</button>
                                                <button className="icon-btn delete">✖</button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </section>
            </main>

            {showModal && (
                <div className="modal-overlay">
                    <div className="modal-card">
                        <div className="modal-header">
                            <h2>Nova Conta de Direção</h2>
                            <button
                                className="modal-close"
                                onClick={() => setShowModal(false)}
                            >
                                ×
                            </button>
                        </div>

                        <div className="modal-body">
                            <label htmlFor="email">Email *</label>
                            <input
                                id="email"
                                type="email"
                                placeholder="carlos@entartes.pt"
                            />
                        </div>

                        <div className="modal-footer">
                            <button
                                className="secondary-btn"
                                onClick={() => setShowModal(false)}
                            >
                                Cancelar
                            </button>

                            <button className="primary-btn">Criar Conta</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}

export default AdminUsersPage