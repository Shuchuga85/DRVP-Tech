import Button from '../../../components/common/Button'
import Input from '../../../components/common/Input'
import Modal from '../../../components/common/Modal'

function CreateUserModal({
    open,
    title,
    description,
    email,
    onEmailChange,
    onClose,
    onConfirm,
    error,
    loading,
}) {
    return (
        <Modal open={open} title={title} onClose={onClose}>
            <p>{description}</p>

            <label htmlFor="newEmail">Email *</label>
            <Input
                id="newEmail"
                type="email"
                value={email}
                placeholder="carlos@entartes.pt"
                onChange={onEmailChange}
                onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                        onConfirm()
                    }
                }}
            />

            {error && <p className="admin-modal-error">{error}</p>}

            <div className="modal-actions">
                <Button variant="secondary" onClick={onClose}>
                    Cancelar
                </Button>

                <Button
                    variant="primary"
                    onClick={onConfirm}
                    disabled={loading}
                >
                    {loading ? 'A criar...' : 'Criar Conta'}
                </Button>
            </div>
        </Modal>
    )
}

export default CreateUserModal