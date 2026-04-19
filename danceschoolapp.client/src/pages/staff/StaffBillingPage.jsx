// Tabs: students | coaches
// GET /api/staff/billing/students?month={YYYY-MM}&page=1&pageSize=25
// GET /api/staff/billing/coaches?month={YYYY-MM}&page=1&pageSize=25
function StaffBillingPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Faturação</h2>
            <p>Consultar faturação de alunos e professores.</p>
        </section>
    )
}

export default StaffBillingPage
