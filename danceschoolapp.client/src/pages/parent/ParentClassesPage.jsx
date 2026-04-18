// Tabs: minhas-marcacoes | marcar | grupo | validar
// GET /api/ee/classes/my?from=&to=
// GET /api/ee/classes/available-slots?from=&to=&modalityId=&coachId=
// GET /api/ee/classes/open?page=1&pageSize=10
// GET /api/ee/classes/validate?page=1&pageSize=10
// POST /api/coachclasses  |  POST /api/participants
// PATCH /api/participants/{id}/parent-validate  body: { attended: true|false }
function ParentClassesPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Aulas</h2>
            <p>Gerir marcações, validações e aulas de grupo.</p>
        </section>
    )
}

export default ParentClassesPage
