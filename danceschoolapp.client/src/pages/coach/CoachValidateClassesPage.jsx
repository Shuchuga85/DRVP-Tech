// Tabs: requests | validations
// GET /api/coach/validate?tab={tab}&page=1&pageSize=10
// PATCH /api/coachclasses/{id}/coach-accept  |  /coach-reject
// PATCH /api/coachclasses/{id}/coach-validate  body: { didTeach: true|false }
function CoachValidateClassesPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Validar Aulas</h2>
            <p>Aceitar pedidos e confirmar realização de aulas.</p>
        </section>
    )
}

export default CoachValidateClassesPage
