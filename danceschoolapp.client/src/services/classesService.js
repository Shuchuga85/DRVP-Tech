import { get, patch } from '@/api/client'
// See EndpointsMapping.md for full API reference

export function getMyClasses({ from, to } = {}) {
    const params = new URLSearchParams()
    if (from) params.set('from', from)
    if (to) params.set('to', to)
    const qs = params.toString()
    return get(`/api/ee/classes/my${qs ? `?${qs}` : ''}`)
}

export function getAvailableSlots({ from, to, modalityId, coachId } = {}) {
    const params = new URLSearchParams()
    if (from) params.set('from', from)
    if (to) params.set('to', to)
    if (modalityId) params.set('modalityId', modalityId)
    if (coachId) params.set('coachId', coachId)
    const qs = params.toString()
    return get(`/api/ee/classes/available-slots${qs ? `?${qs}` : ''}`)
}

export function getOpenClasses({ page = 1, pageSize = 10 } = {}) {
    const params = new URLSearchParams({ page, pageSize })
    return get(`/api/ee/classes/open?${params}`)
}

export function getValidateClasses({ page = 1, pageSize = 10 } = {}) {
    const params = new URLSearchParams({ page, pageSize })
    return get(`/api/ee/classes/validate?${params}`)
}

export function parentValidateParticipant(participantId, attended) {
    return patch(`/api/participants/${participantId}/parent-validate`, { attended })
}
