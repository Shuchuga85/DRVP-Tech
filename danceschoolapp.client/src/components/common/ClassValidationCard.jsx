function ClassValidationCard({ aula, onConfirm, onReject, tipo }) {
    const id = aula?.ClassId ?? aula?.classId ?? aula?.id ?? null

    const formatDate = (iso) => {
        if (!iso) return 'Data não informada'
        try {
            const d = new Date(iso)
            return d.toLocaleString('pt-PT', {
                day: '2-digit', month: 'long', year: 'numeric',
                hour: '2-digit', minute: '2-digit'
            })
        } catch (e) {
            return String(iso)
        }
    }

    // Fields for requests
    const modality = aula?.ModalityName ?? aula?.modalityName ?? aula?.Modality ?? aula?.modality ?? ''
    const studio = aula?.StudioName ?? aula?.studioName ?? aula?.Studio ?? aula?.studio ?? ''
    const preferred = aula?.PreferredDatetime ?? aula?.preferredDatetime ?? aula?.PreferredDate ?? aula?.Preferred ?? aula?.StartDatetime ?? aula?.startDatetime ?? null
    const requestedAt = aula?.RequestedAt ?? aula?.requestedAt ?? aula?.RequestedOn ?? null
    const duration = aula?.DurationMinutes ?? aula?.durationMinutes ?? aula?.Duration ?? null
    const parentName = aula?.ParentName ?? aula?.parentName ?? aula?.Parent ?? aula?.ParentRequester ?? ''
    const studentNames = aula?.StudentNames ?? aula?.studentNames ?? aula?.students ?? []

    // Fields for validations
    const start = aula?.StartDatetime ?? aula?.startDatetime ?? aula?.PreferredDatetime ?? preferred
    const expiresAt = aula?.ExpiresAt ?? aula?.expiresAt ?? null
    const coachValidationStatus = aula?.CoachValidationStatus ?? aula?.coachValidationStatus ?? null

    const title = modality ? `${modality} - Individual` : (studentNames[0] ?? 'Aula')

    return (
        <div className="validation-card card">
            <div className="validation-card-body">
                <div className="validation-card-main">
                    <div style={{display: 'flex', alignItems: 'center', gap: '0.75rem'}}>
                        <h3 className="validation-title">{title}</h3>
                        {tipo === 'professor' && (<span className="status-badge">Aguarda validação</span>)}
                    </div>

                    {/* Info grid */}
                    <div className="validation-info">
                        {requestedAt && (
                            <div className="info-item">
                                <strong>Requisitado em:</strong>
                                <span>{formatDate(requestedAt)}</span>
                            </div>
                        )}

                        {studentNames && studentNames.length > 0 && (
                            <div className="info-item">
                                <strong>Aluno:</strong>
                                <span>{studentNames.join(', ')}</span>
                            </div>
                        )}

                        {preferred && (
                            <div className="info-item">
                                <strong>Data preferida:</strong>
                                <span>{formatDate(preferred)}</span>
                            </div>
                        )}

                        {studio && (
                            <div className="info-item">
                                <strong>Estúdio:</strong>
                                <span>{studio}</span>
                            </div>
                        )}

                        {duration && (
                            <div className="info-item">
                                <strong>Duração:</strong>
                                <span>{duration} minutos</span>
                            </div>
                        )}

                        {parentName && (
                            <div className="info-item">
                                <strong>Requisitado por:</strong>
                                <span>{parentName}</span>
                            </div>
                        )}

                        {expiresAt && (
                            <div className="info-item">
                                <strong>Expira em:</strong>
                                <span>{formatDate(expiresAt)}</span>
                            </div>
                        )}
                    </div>
                </div>

                <div className="validation-actions">
                    {tipo === 'coach-request' && (
                        <>
                            <button className="btn btn-accept" onClick={() => id && onConfirm(id)}>
                                Aceitar Aula
                            </button>
                            <button className="btn btn-reject" onClick={() => id && onReject(id)}>
                                Recusar Aula
                            </button>
                        </>
                    )}

                    {tipo === 'professor' && (
                        <>
                            <button className="btn btn-reject" onClick={() => id && onReject(id)}>
                                Não realizada
                            </button>
                            <button className="btn btn-accept" onClick={() => id && onConfirm(id)}>
                                Realizada
                            </button>
                        </>
                    )}
                </div>
            </div>
        </div>
    )
}

export default ClassValidationCard
