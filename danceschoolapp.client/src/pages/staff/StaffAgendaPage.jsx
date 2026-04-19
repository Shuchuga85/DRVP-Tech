// GET /api/staff/agenda?from={weekStart}&to={weekEnd}
// Filters: studioId, status
function StaffAgendaPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Agenda Global</h2>
            <p>Visualizar calendário semanal de todas as aulas.</p>
        </section>
    )
}

export default StaffAgendaPage
