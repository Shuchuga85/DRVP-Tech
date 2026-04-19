function Modal({ open, title, children, onClose }) {
    if (!open) return null

    return (
        <div className="overlay">
            <div className="modal">
                <div className="modal-header">
                    <h3>{title}</h3>
                    <button
                        type="button"
                        className="modal-close-btn"
                        onClick={onClose}
                    >
                        ✕
                    </button>
                </div>

                <div className="modal-body">
                    {children}
                </div>
            </div>
        </div>
    )
}

export default Modal