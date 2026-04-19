function StatusBadge({ active }) {
    return (
        <span className={`badge ${active ? 'badge--active' : 'badge--inactive'}`}>
            {active ? 'Ativo' : 'Inativo'}
        </span>
    )
}

export default StatusBadge