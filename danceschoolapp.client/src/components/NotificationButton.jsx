import React, { useEffect, useRef, useState } from 'react'
import { useAuth } from '../context/useAuth'

function NotificationButton() {
    const { user } = useAuth()
    const userId = user?.UserId ?? user?.userId ?? null
    const [unreadCount, setUnreadCount] = useState(0)
    const [open, setOpen] = useState(false)
    const [items, setItems] = useState([])
    const mountedRef = useRef(true)

    const fetchUnread = async () => {
        if (!userId) return
        try {
            const res = await fetch(`/api/notifications/user/${userId}?page=1&pageSize=1`, {
                method: 'GET',
                credentials: 'include'
            })
            if (res.ok) {
                const data = await res.json()
                // Assumes API returns something like { totalUnread: X } or a list with totalCount
                if (Array.isArray(data)) {
                    // fallback: count unread in returned array
                    const count = data.filter(n => !n.isRead).length
                    setUnreadCount(count)
                } else if (data.totalUnread !== undefined) {
                    setUnreadCount(data.totalUnread)
                } else if (data.totalCount !== undefined && data.items) {
                    setUnreadCount(data.items.filter(n => !n.isRead).length)
                } else {
                    // safe fallback: 0
                    setUnreadCount(0)
                }
            }
        } catch (err) {
            console.error('Erro ao obter contagem de notificações:', err)
        }
    }

    const fetchList = async () => {
        if (!userId) return
        try {
            const res = await fetch(`/api/notifications/user/${userId}?page=1&pageSize=10`, {
                method: 'GET',
                credentials: 'include'
            })
            if (res.ok) {
                const data = await res.json()
                // Expecting array or { items: [...] }
                const list = Array.isArray(data) ? data : data.items ?? []
                if (mountedRef.current) setItems(list)
            }
        } catch (err) {
            console.error('Erro ao carregar notificações:', err)
        }
    }

    useEffect(() => {
        mountedRef.current = true
        fetchUnread()

        // Poll every 60s
        const id = setInterval(fetchUnread, 60000)

        return () => {
            mountedRef.current = false
            clearInterval(id)
        }
    }, [userId])

    const toggle = async () => {
        setOpen(v => !v)
        if (!open) {
            // opening: load list
            await fetchList()
        }
    }

    const markAsRead = async (notificationId) => {
        try {
            const res = await fetch(`/api/notifications/${notificationId}/read`, {
                method: 'PATCH',
                credentials: 'include'
            })
            if (res.ok) {
                // update local state
                setItems(prev => prev.map(n => n.id === notificationId ? { ...n, isRead: true } : n))
                setUnreadCount(prev => Math.max(0, prev - 1))
            }
        } catch (err) {
            console.error('Erro ao marcar notificação como lida:', err)
        }
    }

    return (
        <div className="notification-root" style={{ position: 'relative', marginLeft: 12 }}>
            <button
                aria-label="Notificações"
                className="notification-btn"
                onClick={toggle}
                style={{
                    width: 44,
                    height: 44,
                    borderRadius: 10,
                    background: '#fff',
                    boxShadow: '0 4px 12px rgba(0,0,0,0.06)',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    border: 'none',
                    cursor: 'pointer'
                }}
            >
                {/* simple bell SVG */}
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M15 17H9" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                    <path d="M12 22a2.5 2.5 0 0 0 2.5-2.5h-5A2.5 2.5 0 0 0 12 22z" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                    <path d="M18 8a6 6 0 1 0-12 0c0 7-3 8-3 8h18s-3-1-3-8" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
                {unreadCount > 0 && (
                    <span
                        className="notification-badge"
                        style={{
                            position: 'absolute',
                            top: -4,
                            right: -4,
                            background: '#E11D48',
                            color: '#fff',
                            borderRadius: '999px',
                            minWidth: 18,
                            height: 18,
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            fontSize: 11,
                            padding: '0 5px',
                            boxShadow: '0 2px 6px rgba(0,0,0,0.15)'
                        }}
                    >
                        {unreadCount > 99 ? '99+' : unreadCount}
                    </span>
                )}
            </button>

            {open && (
                <div
                    className="notification-dropdown"
                    style={{
                        position: 'absolute',
                        right: 0,
                        marginTop: 10,
                        width: 320,
                        maxHeight: 420,
                        overflowY: 'auto',
                        background: '#fff',
                        borderRadius: 12,
                        boxShadow: '0 10px 30px rgba(0,0,0,0.12)',
                        padding: 12,
                        zIndex: 60
                    }}
                >
                    <div style={{ marginBottom: 8, fontWeight: 600 }}>Notificações</div>
                    {items.length === 0 && (
                        <div style={{ color: '#666', padding: '18px 8px' }}>Sem notificações recentes.</div>
                    )}
                    {items.map(n => (
                        <div
                            key={n.id}
                            style={{
                                display: 'flex',
                                gap: 10,
                                padding: 10,
                                borderRadius: 8,
                                background: n.isRead ? 'transparent' : 'rgba(99,102,241,0.06)',
                                alignItems: 'flex-start',
                                marginBottom: 6
                            }}
                        >
                            <div style={{ flex: 1 }}>
                                <div style={{ fontSize: 14, color: '#111', marginBottom: 4 }}>{n.title ?? n.message ?? 'Notificação'}</div>
                                {n.message && <div style={{ fontSize: 13, color: '#555' }}>{n.message}</div>}
                                <div style={{ fontSize: 12, color: '#888', marginTop: 6 }}>{new Date(n.createdAt ?? n.createdAtUtc ?? Date.now()).toLocaleString()}</div>
                            </div>
                            {!n.isRead && (
                                <button
                                    onClick={() => markAsRead(n.id)}
                                    style={{
                                        border: 'none',
                                        background: '#06b6d4',
                                        color: '#fff',
                                        padding: '6px 8px',
                                        borderRadius: 8,
                                        cursor: 'pointer',
                                        alignSelf: 'center'
                                    }}
                                >
                                    Marcar lida
                                </button>
                            )}
                        </div>
                    ))}
                    <div style={{ textAlign: 'center', marginTop: 8 }}>
                        <button
                            onClick={fetchList}
                            style={{
                                border: 'none',
                                background: 'transparent',
                                color: '#6b7280',
                                cursor: 'pointer'
                            }}
                        >
                            Atualizar
                        </button>
                    </div>
                </div>
            )}
        </div>
    )
}

export default NotificationButton