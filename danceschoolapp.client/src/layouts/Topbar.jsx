function Topbar() {
    return (
        <header className="dashboard-topbar">
            <div className="dashboard-brand">
                <div className="dashboard-brand-glow"></div>

                <div>
                    <h1>Ent&apos;Artes</h1>
                    <span>Escola de Dança</span>
                </div>
            </div>

            <div className="dashboard-topbar-right">
                <div className="dashboard-user-pill">
                    <div className="dashboard-user-avatar">A</div>
                    <span>Ana Costa</span>
                </div>

                <button className="dashboard-logout-btn">Sair</button>
            </div>
        </header>
    )
}

export default Topbar