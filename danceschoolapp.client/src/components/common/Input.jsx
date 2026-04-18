function Input({
    id,
    type = 'text',
    value,
    onChange,
    placeholder = '',
    onKeyDown,
    disabled = false,
    className = '',
}) {
    return (
        <input
            id={id}
            type={type}
            value={value}
            placeholder={placeholder}
            onChange={(e) => onChange(e.target.value)}
            onKeyDown={onKeyDown}
            disabled={disabled}
            className={`input ${className}`.trim()}
        />
    )
}

export default Input
