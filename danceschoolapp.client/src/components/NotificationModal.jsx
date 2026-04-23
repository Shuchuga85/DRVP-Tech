import React, { useEffect, useState, useRef } from 'react'
import { createPortal } from 'react-dom'
import { useAuth } from '../context/useAuth'
import './notification.css'

export default function NotificationModal({ userId, onClose }) {
  const { user } = useAuth()
  const roles = (user?.Roles || user?.roles || []).map(r => r.toString().toLowerCase())
  const isCoach = roles.includes('coach')
  const isParent = roles.includes('parent')
  const isStaff = roles.includes('staff') || roles.includes('admin')

  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(false)
  const [tab, setTab] = useState('all')
  const [loadedFromApi, setLoadedFromApi] = useState(false)
  const dialogRef = useRef(null)

  const inferSenderRole = (n) => {
    const title = (n.Title ?? n.title ?? '').toString().toLowerCase()
    const msg = (n.Message ?? n.message ?? n.body ?? '').toString().toLowerCase()
    const entity = (n.EntityType ?? n.entityType ?? '').toString().toLowerCase()

    if (/scheduled|requested|request|inscrever|pedido|marcador/i.test(title + ' ' + msg)) return 'parent'
    if (/validated|rejected|rejeitado|validad/i.test(title + ' ' + msg)) return 'staff'
    if (entity.includes('coachclass') || entity.includes('coach')) {
      if (/request|scheduled|awaiting|inscrever|pedido/i.test(title + ' ' + msg)) return 'parent'
      if (/validated|validation|confirm/i.test(title + ' ' + msg)) return 'staff'
      return 'coach'
    }
    return null
  }

  useEffect(() => {
    let mounted = true
    let intervalId = null

    const fetchList = async () => {
      if (!userId) return
      setLoading(true)
      try {
        const res = await fetch(`/api/notifications/user/${userId}?page=1&pageSize=20`, {
          credentials: 'include'
        })
        if (!res.ok) {
          if (mounted) {
            setItems([])
            setLoadedFromApi(false)
          }
          return
        }
        const data = await res.json()
        const list = Array.isArray(data) ? data : data.items ?? data.Items ?? []
        const mapped = list.map((n) => {
          const createdAt = n.CreatedAt ?? n.createdAt ?? n.createdAtUtc ?? null
          return {
            id: n.NotificationId ?? n.notificationId ?? n.id ?? null,
            title: n.Title ?? n.title ?? n.subject ?? '',
            message: n.Message ?? n.message ?? n.body ?? '',
            createdAt,
            displayDate: createdAt ? new Date(createdAt).toLocaleString() : '',
            isRead: (n.IsRead ?? n.isRead ?? (n.ReadAt ? true : false)) ?? false,
            senderRole: (n.SenderRole ?? n.senderRole ?? n.CreatedByRole ?? n.createdByRole ?? null) || inferSenderRole(n),
            raw: n
          }
        })
        if (mounted) {
          setItems(mapped)
          setLoadedFromApi(true)
        }
      } catch (err) {
        console.error('Erro fetchList', err)
        if (mounted) {
          setItems([])
          setLoadedFromApi(false)
        }
      } finally {
        if (mounted) setLoading(false)
      }
    }

    // chamada inicial + polling
    void fetchList()
    intervalId = setInterval(fetchList, 60000)

    const onKey = (e) => { if (e.key === 'Escape') onClose() }
    window.addEventListener('keydown', onKey)

    return () => {
      mounted = false
      clearInterval(intervalId)
      window.removeEventListener('keydown', onKey)
    }
  }, [userId, onClose])

  const markAsRead = async (id) => {
    try {
      const res = await fetch(`/api/notifications/${id}/read`, {
        method: 'PATCH',
        credentials: 'include'
      })
      if (res.ok) {
        setItems(prev => prev.map(n => (n.id === id ? { ...n, isRead: true } : n)))
      }
    } catch (err) {
      console.error('Erro markAsRead', err)
    }
  }

  const deleteNotification = async (id) => {
    if (!id) return
    // optional: simple confirm
    if (!window.confirm('Deseja eliminar esta notificação?')) return
    try {
      const res = await fetch(`/api/notifications/${id}`, {
        method: 'DELETE',
        credentials: 'include'
      })
      if (res.ok) {
        setItems(prev => prev.filter(n => n.id !== id))
      } else {
        console.error('Erro ao eliminar notificação', res.status)
      }
    } catch (err) {
      console.error('Erro deleteNotification', err)
    }
  }

  const allowedSendersForCurrentUser = (() => {
    if (isCoach) return ['parent', 'staff']
    if (isParent) return ['coach', 'staff']
    if (isStaff) return ['coach', 'parent']
    return []
  })()

  // Build the sender tabs dynamically based on current user's allowed senders
  const senderTabMap = {
    coach: { key: 'coaches', label: 'Professores' },
    parent: { key: 'parents', label: 'Encarregados' },
    staff: { key: 'staff', label: 'Direção' }
  }

  const availableTabs = allowedSendersForCurrentUser
    .map(s => senderTabMap[s])
    .filter(Boolean)

  // If selected tab becomes unavailable for the user, revert to 'all'
  useEffect(() => {
    if (tab === 'all') return
    const allowedKeys = availableTabs.map(t => t.key)
    if (!allowedKeys.includes(tab)) setTab('all')
  }, [availableTabs, tab])

  const relevanceFiltered = items.filter(n => {
    if (!allowedSendersForCurrentUser.length) return true
    if (!n.senderRole) return true
    return allowedSendersForCurrentUser.includes(n.senderRole)
  })

  const tabFiltered = relevanceFiltered.filter(n => {
    if (tab === 'all') return true
    if (!n.senderRole) return false
    if (tab === 'coaches') return n.senderRole.includes('coach') || n.senderRole.includes('professor')
    if (tab === 'parents') return n.senderRole.includes('parent') || n.senderRole.includes('encarregado')
    if (tab === 'staff') return n.senderRole.includes('staff') || n.senderRole.includes('admin') || n.senderRole.includes('direção')
    return true
  })

  const content = (
    <div className="notif-backdrop" onMouseDown={(e) => { if (e.target === e.currentTarget) onClose() }}>
      <div className="notif-modal" ref={dialogRef} role="dialog" aria-modal="true">
        <div className="notif-topline" />
        <div className="notif-header">
          <div>
            <h2>Notificações</h2>
          </div>
          <button className="notif-close" onClick={onClose} aria-label="Fechar">✕</button>
        </div>

        <div className="notif-tabs">
          <button className={`pill ${tab === 'all' ? 'active' : ''}`} onClick={() => setTab('all')}>Todas</button>
          {availableTabs.map(t => (
            <button key={t.key} className={`pill ${tab === t.key ? 'active' : ''}`} onClick={() => setTab(t.key)}>{t.label}</button>
          ))}
        </div>

        <div className="notif-body">
          {loading && <div className="empty">A carregar...</div>}

          {!loading && tabFiltered.length === 0 && (
            <div className="empty">
              <svg width="48" height="48" viewBox="0 0 24 24" fill="none">
                <path d="M18 8a6 6 0 1 0-12 0c0 7-3 8-3 8h18s-3-1-3-8" stroke="#9CA3AF" strokeWidth="1.4" strokeLinecap="round" strokeLinejoin="round"/>
                <path d="M15 17H9" stroke="#9CA3AF" strokeWidth="1.4" strokeLinecap="round" strokeLinejoin="round"/>
                <path d="M12 22a2.5 2.5 0 0 0 2.5-2.5h-5A2.5 2.5 0 0 0 12 22z" stroke="#9CA3AF" strokeWidth="1.4" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
              <div className="empty-title">Sem notificações</div>
            </div>
          )}

          {!loading && tabFiltered.length > 0 && (
            <div className="list" role="list">
              {tabFiltered.map((n, idx) => (
                <div key={n.id ?? idx} className={`notif-item ${n.isRead ? 'read' : 'unread'}`} role="listitem">
                  <div className="notif-main">
                    <div className="notif-title">{n.title}</div>
                    <div className="notif-message">{n.message}</div>
                    {n.senderRole && <div style={{ marginTop: 6, fontSize: 12, color: '#9CA3AF' }}>Origem: {n.senderRole}</div>}
                  </div>
                  <div className="notif-meta">
                    <div className="notif-date">{n.displayDate}</div>
                    <div style={{ display: 'flex', gap: 8 }}>
                      {!n.isRead && (
                        <button className="mark-read" onClick={() => markAsRead(n.id)}>Marcar lida</button>
                      )}
                      <button
                        className="delete-btn"
                        onClick={() => deleteNotification(n.id)}
                        aria-label="Eliminar notificação"
                        title="Eliminar"
                      >
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" focusable="false">
                          <path d="M3 6h18" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
                          <path d="M8 6v14a2 2 0 0 0 2 2h4a2 2 0 0 0 2-2V6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
                          <path d="M10 11v6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
                          <path d="M14 11v6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
                          <path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
                        </svg>
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        <div className="notif-footer">
          <button className="btn-close" onClick={onClose}>Fechar</button>
        </div>
      </div>
    </div>
  )

  return createPortal(content, document.body)
}