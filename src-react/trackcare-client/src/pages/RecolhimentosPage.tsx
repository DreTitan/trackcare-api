import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import Header from '../components/layout/Header';
import { apiClient } from '../api/apiClient';
import { STATUS_OPTIONS } from '../types';
import StatusBadge from '../components/recolhimentos/StatusBadge';
import { Button } from '../components/ui/Input';
import { Plus, Trash2, Eye, Edit } from 'lucide-react';

export default function RecolhimentosPage() {
  const [termo, setTermo] = useState('');
  const [status, setStatus] = useState('');
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['recolhimentos', termo, status],
    queryFn: () => apiClient.search(termo || undefined, status || undefined),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => apiClient.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['recolhimentos'] }),
  });

  return (
    <div className="flex flex-col min-h-screen">
      <Header title="Recolhimentos" />
      <main className="flex-1 p-8">
        {/* Filters */}
        <div className="bg-white rounded-xl shadow-sm p-4 flex flex-wrap gap-4 items-end mb-6">
          <div className="flex-1 min-w-[200px]">
            <label className="text-xs text-slate-500 mb-1 block">Buscar</label>
            <input
              type="text"
              placeholder="HGID, série, cliente, ticket..."
              value={termo}
              onChange={e => setTermo(e.target.value)}
              className="w-full border border-slate-300 rounded-lg px-4 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="min-w-[180px]">
            <label className="text-xs text-slate-500 mb-1 block">Status</label>
            <select
              value={status}
              onChange={e => setStatus(e.target.value)}
              className="w-full border border-slate-300 rounded-lg px-4 py-2.5 text-sm bg-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              {STATUS_OPTIONS.map(o => (
                <option key={o.value} value={o.value}>{o.label}</option>
              ))}
            </select>
          </div>
          <Link to="/recolhimentos/novo">
            <Button><Plus size={16} className="inline mr-1" /> Novo</Button>
          </Link>
        </div>

        {/* Table */}
        <div className="bg-white rounded-xl shadow-sm overflow-hidden">
          <table className="w-full text-sm">
            <thead className="bg-slate-50 border-b border-slate-200">
              <tr>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">HGID</th>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">Nº Série</th>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">Cliente</th>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">Status</th>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">Ticket Hub</th>
                <th className="text-left py-3 px-4 font-semibold text-slate-600">Data</th>
                <th className="text-right py-3 px-4 font-semibold text-slate-600">Ações</th>
              </tr>
            </thead>
            <tbody>
              {(data || []).map((r: any) => (
                <tr key={r.id} className="border-b border-slate-100 hover:bg-slate-50">
                  <td className="py-3 px-4 font-semibold">{r.hgid}</td>
                  <td className="py-3 px-4">{r.numeroSerie}</td>
                  <td className="py-3 px-4">{r.clienteNome}</td>
                  <td className="py-3 px-4"><StatusBadge status={r.status} /></td>
                  <td className="py-3 px-4 text-slate-500">{r.ticketHub || '—'}</td>
                  <td className="py-3 px-4 text-slate-500">
                    {new Date(r.dataSolicitacao).toLocaleDateString('pt-BR')}
                  </td>
                  <td className="py-3 px-4 text-right">
                    <div className="flex gap-2 justify-end">
                      <Link to={`/recolhimentos/${r.id}`} className="p-1.5 rounded hover:bg-slate-100 text-slate-500" title="Ver">
                        <Eye size={16} />
                      </Link>
                      <Link to={`/recolhimentos/${r.id}/editar`} className="p-1.5 rounded hover:bg-slate-100 text-slate-500" title="Editar">
                        <Edit size={16} />
                      </Link>
                      <button
                        onClick={() => {
                          if (confirm(`Excluir ${r.hgid}?`)) deleteMutation.mutate(r.id);
                        }}
                        className="p-1.5 rounded hover:bg-red-50 text-red-500"
                        title="Excluir"
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {isLoading && (
                <tr><td colSpan={7} className="py-8 text-center text-slate-400">Carregando...</td></tr>
              )}
              {!isLoading && (!data || data.length === 0) && (
                <tr><td colSpan={7} className="py-8 text-center text-slate-400">Nenhum recolhimento encontrado</td></tr>
              )}
            </tbody>
          </table>
        </div>
      </main>
    </div>
  );
}
