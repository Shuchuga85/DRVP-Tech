function Select({
    id,
    value,
    onChange,
    options = [],
    placeholder = '',
    disabled = false,
    className = '',
}) {
    return (
        <select
            id={id}
            value={value}
            onChange={(e) => onChange(e.target.value)}
            disabled={disabled}
            className={`input ${className}`.trim()}
        >
            {placeholder && (
                <option value="" disabled>
                    {placeholder}
                </option>
            )}
            {options.map((opt) => (
                <option key={opt.value} value={opt.value}>
                    {opt.label}
                </option>
            ))}
        </select>
    )
}

export default Select
