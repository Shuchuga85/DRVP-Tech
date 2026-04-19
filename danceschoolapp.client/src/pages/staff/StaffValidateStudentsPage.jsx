// GET /api/staff/validate-students?status=pending&page=1&pageSize=10
// PATCH /api/students/{id}/accept  |  PATCH /api/students/{id}/reject  body: { reason }
function StaffValidateStudentsPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Validar Estudantes</h2>
            <p>Aprovar ou recusar estudantes introduzidos pelos encarregados.</p>
        </section>
    )
}

export default StaffValidateStudentsPage
