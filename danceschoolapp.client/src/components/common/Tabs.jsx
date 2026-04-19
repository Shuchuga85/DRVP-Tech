function Tabs({ tabs = [], activeTab, onTabChange }) {
    return (
        <div className="tabs">
            {tabs.map((tab) => (
                <button
                    key={tab.value}
                    type="button"
                    className={`tab ${activeTab === tab.value ? 'tab--active' : ''}`}
                    onClick={() => onTabChange(tab.value)}
                >
                    {tab.label}
                </button>
            ))}
        </div>
    )
}

export default Tabs
