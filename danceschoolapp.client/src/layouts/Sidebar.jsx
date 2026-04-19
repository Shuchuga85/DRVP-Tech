import { NavLink } from 'react-router-dom'

function Sidebar({ items = [] }) {
    return (
        <aside className="dashboard-sidebar">
            <nav className="dashboard-sidebar-nav">
                {items.map((item, index) => (
                    <NavLink
                        key={item.to}
                        to={item.to}
                        end={index === 0}
                        className={({ isActive }) =>
                            isActive
                                ? 'dashboard-sidebar-link active'
                                : 'dashboard-sidebar-link'
                        }
                    >
                        {item.label}
                    </NavLink>
                ))}
            </nav>
        </aside>
    )
}

export default Sidebar