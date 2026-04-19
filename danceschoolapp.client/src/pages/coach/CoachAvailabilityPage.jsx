// GET /api/coachavailability/coach/{coachId}
// POST /api/coachavailability  body: { coachId, weekday, startTime, endTime, validFrom?, validUntil? }
// PUT /api/coachavailability/{id}  |  DELETE /api/coachavailability/{id}
function CoachAvailabilityPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Disponibilidade</h2>
            <p>Gerir horários de disponibilidade semanal.</p>
        </section>
    )
}

export default CoachAvailabilityPage
