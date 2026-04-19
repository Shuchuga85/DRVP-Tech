// Tabs: school | community
// GET /api/ee/inventory/school?page=1&pageSize=12
// GET /api/ee/inventory/community?page=1&pageSize=12
// GET /api/item-categories  |  GET /api/items/{id}
// POST /api/requisitions  |  POST /api/items (community)
function ParentInventoryPage() {
    return (
        <section className="dashboard-page-card">
            <h2>Inventário</h2>
            <p>Consultar artigos da escola e comunidade.</p>
        </section>
    )
}

export default ParentInventoryPage
