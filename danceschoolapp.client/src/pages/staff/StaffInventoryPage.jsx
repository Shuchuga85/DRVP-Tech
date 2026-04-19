// GET /api/items?fromSchool=true&page=1  |  POST /api/items  |  PUT /api/items/{id}
// GET /api/item-categories  |  GET /api/requisitions
// PATCH /api/requisitions/{id}/review  |  PATCH /api/requisitions/{id}/return
function StaffInventoryPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Inventário</h2>
            <p>Gerir artigos da escola e pedidos de empréstimo.</p>
        </section>
    )
}

export default StaffInventoryPage
