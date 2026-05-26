import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export const api = axios.create({
  baseURL: API_URL,
  headers: { 'Content-Type': 'application/json' },
});

export const apiClient = {
  // Dashboard
  getStats: () => api.get('/recolhimentos/dashboard/stats').then(r => r.data),
  getRecent: (count: number) => api.get(`/recolhimentos/recent/${count}`).then(r => r.data),

  // Recolhimentos
  getAll: () => api.get('/recolhimentos').then(r => r.data),
  getById: (id: number) => api.get(`/recolhimentos/${id}`).then(r => r.data),
  search: (termo?: string, status?: string) =>
    api.get('/recolhimentos/search', { params: { termo, status } }).then(r => r.data),
  create: (data: any) => api.post('/recolhimentos', data).then(r => r.data),
  update: (id: number, data: any) => api.put(`/recolhimentos/${id}`, data).then(r => r.data),
  updateStatus: (id: number, data: any) => api.put(`/recolhimentos/${id}/status`, data).then(r => r.data),
  delete: (id: number) => api.delete(`/recolhimentos/${id}`).then(r => r),

  // Histórico
  getHistorico: (id: number) => api.get(`/recolhimentos/${id}/historico`).then(r => r.data),

  // Anexos
  getAnexos: (recolhimentoId: number) =>
    api.get(`/anexos/${recolhimentoId}`).then(r => r.data),
  uploadAnexo: (recolhimentoId: number, file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return api.post(`/anexos/${recolhimentoId}`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    }).then(r => r.data);
  },
  deleteAnexo: (id: number) => api.delete(`/anexos/${id}`).then(r => r),

  // Comentários
  getComentarios: (recolhimentoId: number) =>
    api.get(`/comentarios/${recolhimentoId}`).then(r => r.data),
  addComentario: (data: any) => api.post('/comentarios', data).then(r => r.data),
  deleteComentario: (id: number) => api.delete(`/comentarios/${id}`).then(r => r),
};
