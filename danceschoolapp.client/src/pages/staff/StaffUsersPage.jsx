// GET /api/users?page=1&pageSize=20
// POST /api/users  |  PATCH /api/users/{id}/activate  |  PATCH /api/users/{id}/deactivate
function StaffUsersPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Utilizadores</h2>
            <p>Criar e gerir contas de encarregados e professores.</p>
        </section>
    )
}

export default StaffUsersPage
