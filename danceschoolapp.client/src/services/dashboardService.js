import { get } from '@/api/client'

export function getAdminDashboardStats() {
    return get('/api/admin/dashboard')
}

export function getStaffDashboardStats() {
    return get('/api/staff/dashboard')
}

export function getCoachDashboardStats() {
    return get('/api/coach/dashboard')
}

export function getParentDashboardStats() {
    return get('/api/ee/dashboard')
}
