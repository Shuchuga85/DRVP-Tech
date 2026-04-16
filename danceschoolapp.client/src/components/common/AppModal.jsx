function AppModal({ title, subtitle, children, onClose }) {
    return (
        <div className="app-modal-overlay" onClick={onClose}>
            <div
                className="app-modal"
                onClick={(e) => e.stopPropagation()}
            >
                <div className="app-modal__header">
                    <div>
                        <h3 className="app-modal__title">{title}</h3>
                        {subtitle && (
                            <p className="app-modal__subtitle">{subtitle}</p>
                        )}
                    </div>
                </div>

                <div className="app-modal__body">{children}</div>
            </div>
        </div>
    )
}

export default AppModal