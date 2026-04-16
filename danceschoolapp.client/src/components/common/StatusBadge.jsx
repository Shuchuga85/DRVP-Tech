function StatusBadge({ isActive }) {
    return (
        <span
            className={`status-badge ${isActive ? 'status-badge--active' : 'status-badge--inactive'
                }`}
        >
            {isActive ? 'Ativo' : 'Inativo'}
        </span>
    )
}

export default StatusBadge