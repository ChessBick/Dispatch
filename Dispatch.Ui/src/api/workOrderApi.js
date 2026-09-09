import http from './http'

export const listWorkOrders = ({ status, page = 1, pageSize = 20 } = {}) =>
  http.get('/workorders', { params: { status: status || undefined, page, pageSize } }).then(r => r.data.data)

export const getWorkOrder = (id) =>
  http.get(`/workorders/${id}`).then(r => r.data.data)

export const createWorkOrder = (payload) =>
  http.post('/workorders', payload).then(r => r.data.data)

export const updateWorkOrder = (id, payload) =>
  http.put(`/workorders/${id}`, payload).then(r => r.data.data)

export const changeWorkOrderStatus = (id, payload) =>
  http.patch(`/workorders/${id}/status`, payload).then(r => r.data.data)

export const getWorkOrderActivities = (id, take = 20) =>
  http.get(`/workorders/${id}/activities`, { params: { take } }).then(r => r.data.data)