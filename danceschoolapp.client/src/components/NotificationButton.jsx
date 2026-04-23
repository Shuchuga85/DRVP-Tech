import React, { useEffect, useState } from 'react'
import NotificationModal from './NotificationModal'
import { useAuth } from '../context/useAuth'

export default function NotificationButton() {
  const { user } = useAuth()
  const userId = user?.UserId ?? user?.userId ?? user?.id ?? null

  const [unreadCount, setUnreadCount] = useState(0)
  const [open, setOpen] = useState(false)

  useEffect(() => {
    let mounted = true
    let intervalId = null

    const doFetch = async () => {
      if (!userId) {
        if (mounted) setUnreadCount(0)
        return
      }
      try {
        const res = await fetch(`/api/notifications/user/${userId}?page=1&pageSize=1`, {
          credentials: 'include'
        })
        if (!res.ok) return
        const data = await res.json()
        let count = 0
        if (Array.isArray(data)) count = data.filter(n => !n.isRead && !n.is_read).length
        else if (data.totalUnread !== undefined) count = data.totalUnread
        else if (data.items) count = (data.items || []).filter(n => !n.isRead && !n.is_read).length
        if (mounted) setUnreadCount(count)
      } catch (err) {
        console.error('Erro fetchUnread', err)
      }
    }

    void doFetch()
    intervalId = setInterval(doFetch, 60000)

    return () => {
      mounted = false
      clearInterval(intervalId)
    }
  }, [userId])

  return (
    <>
      <div style={{ position: 'relative', marginLeft: 12 }}>
        <button
          aria-label="Notificações"
          onClick={() => setOpen(true)}
          className="notification-btn"
          style={{
            width: 44,
            height: 44,
            borderRadius: 10,
            background: '#fff',
            boxShadow: '0 6px 18px rgba(2,6,23,0.06)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            border: 'none',
            cursor: 'pointer'
          }}
        >
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M15 17H9" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            <path d="M12 22a2.5 2.5 0 0 0 2.5-2.5h-5A2.5 2.5 0 0 0 12 22z" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            <path d="M18 8a6 6 0 1 0-12 0c0 7-3 8-3 8h18s-3-1-3-8" stroke="#6B6B6B" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>

          {unreadCount > 0 && (
            <span
              style={{
                position: 'absolute',
                top: -4,
                right: -4,
                background: '#E11D48',
                color: '#fff',
                borderRadius: 999,
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
      </div>

      {open && (
        <NotificationModal
          userId={userId}
          onClose={() => {
            setOpen(false)
            setTimeout(() => {
              fetch(`/api/notifications/user/${userId}?page=1&pageSize=1`, { credentials: 'include' })
                .then(r => r.ok ? r.json() : null)
                .then(data => {
                  if (!data) return
                  let count = 0
                  if (Array.isArray(data)) count = data.filter(n => !n.isRead && !n.is_read).length
                  else if (data.totalUnread !== undefined) count = data.totalUnread
                  else if (data.items) count = (data.items || []).filter(n => !n.isRead && !n.is_read).length
                  setUnreadCount(count)
                })
                .catch(() => {})
            }, 200)
          }}
        />
      )}
    </>
  )
}