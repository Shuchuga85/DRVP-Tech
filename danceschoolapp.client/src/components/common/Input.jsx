function Input({
    id,
    type = 'text',
    value,
    onChange,
    placeholder = '',
    onKeyDown,
}) {
    return (
        <input
            id={id}
            type={type}
            value={value}
            placeholder={placeholder}
            onChange={(e) => onChange(e.target.value)}
            onKeyDown={onKeyDown}
            className="input"
        />
    )
}

export default Input