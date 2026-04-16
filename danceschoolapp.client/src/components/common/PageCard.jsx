function PageCard({ title, description, action, children }) {
    return (
        <section className="page-card">
            <div className="page-card__header">
                <div>
                    <h1 className="page-card__title">{title}</h1>
                    {description && (
                        <p className="page-card__description">{description}</p>
                    )}
                </div>

                {action && <div className="page-card__action">{action}</div>}
            </div>

            <div className="page-card__body">{children}</div>
        </section>
    )
}

export default PageCard