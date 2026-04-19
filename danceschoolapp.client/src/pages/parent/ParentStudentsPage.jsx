// GET /api/ee/students
// POST /api/students  body: { personInfo: { firstName, lastName, birthDate, phone, address, nif } }
// PUT /api/students/{id}  |  PATCH /api/students/{id}/deactivate
function ParentStudentsPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Meus Estudantes</h2>
            <p>Gerir dados dos estudantes a seu cargo.</p>
        </section>
    )
}

export default ParentStudentsPage
