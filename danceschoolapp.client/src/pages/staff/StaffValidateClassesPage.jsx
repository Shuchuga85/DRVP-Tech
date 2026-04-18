// Tabs: requested | finished | pending
// GET /api/staff/validate-classes?tab={tab}&page=1&pageSize=15
// PATCH /api/coachclasses/{id}/staff-approve  |  /reject  |  /staff-validate
function StaffValidateClassesPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Validar Aulas</h2>
            <p>Gerir pedidos de aulas e validações pós-48h.</p>
        </section>
    )
}

export default StaffValidateClassesPage
