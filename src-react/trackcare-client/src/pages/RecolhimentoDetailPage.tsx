import { useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/apiClient';
import { STATUS_OPTIONS, SETOR_OPTIONS } from '../types';
import Header from '../components/layout/Header';
import StatusBadge from '../components/recolhimentos/StatusBadge';
import { Button, Select, Textarea } from '../components/ui/Input';
import { ArrowLeft, FileText, MessageSquare, RefreshCw, Upload, Trash2, ExternalLink } from 'lucide-react';

export default function RecolhimentoDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const recolhimentoId = Number(id);

  const [novoStatus, setNovoStatus] = useState('');
  const [observacaoStatus, setObservacaoStatus] = useState('');
  const [novoComentario, setNovoComentario] = useState('');
  const [setorComentario, setSetorComentario] = useState('N3');
  const [usuario] = useState('system');

  const { data: recolhimento, isLoading } = useQuery({
    queryKey: ['recolhimento', id],
    queryFn: () => apiClient.getById(recolhimentoId),
  });

  const { data: comentarios } = useQuery({
    queryKey: ['comentarios', id],
    queryFn: () => apiClient.getComentarios(recolhimentoId),
    enabled: !!recolhimentoId,
  });

  const { data: anexos } = useQuery({
    queryKey: ['anexos', id],
    queryFn: () => apiClient.getAnexos(recolhimentoId),
    enabled: !!recolhimentoId,
  });

  const { data: historico } = useQuery({
    queryKey: ['historico', id],
    queryFn: () => apiClient.getHistorico(recolhimentoId),
    enabled: !!recolhimentoId,
  });

  const statusMutation = useMutation({
    mutationFn: (data: any) => apiClient.updateStatus(recolhimentoId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['recolhimento', id] });
      queryClient.invalidateQueries({ queryKey: ['historico', id] });
      queryClient.invalidateQueries({ queryKey: ['dashboard-stats'] });
      setNovoStatus('');
      setObservacaoStatus('');
    },
  });

  const comentarioMutation = useMutation({
    mutationFn: (data: any) => apiClient.addComentario(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['comentarios', id] });
      setNovoComentario('');
    },
  });

  const deleteComentarioMutation = useMutation({
    mutationFn: (commentId: number) => apiClient.deleteComentario(commentId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['comentarios', id] }),
  });

  const uploadAnexoMutation = useMutation({
    mutationFn: (file: File) => apiClient.uploadAnexo(recolhimentoId, file),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['anexos', id] }),
  });

  const deleteAnexoMutation = useMutation({
    mutationFn: (anexoId: number) => apiClient.deleteAnexo(anexoId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['anexos', id] }),
  });

  if (isLoading) return <div className="p-8 text-center text-slate-400">Carregando...</div>;
  if (!recolhimento) return <div className="p-8 text-center text-red-400">Recolhimento não encontrado</div>;

  return (
    <div className="flex flex-col min-h-screen">
      <Header title={`Detalhes - ${recolhimento.hgid}`} />
      <main className="flex-1 p-8">
        <button onClick={() => navigate('/recolhimentos')} className="flex items-center gap-2 text-slate-500 hover:text-slate-800 mb-6">
          <ArrowLeft size={18} /> Voltar para Lista
        </button>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Main content */}
          <div className="lg:col-span-2 space-y-6">

            {/* Header */}
            <div className="bg-white rounded-xl shadow-sm p-6">
              <div className="flex items-center gap-4 mb-4">
                <span className="text-xl font-bold text-slate-800">{recolhimento.hgid}</span>
                <StatusBadge status={recolhimento.status} statusTexto={recolhimento.statusTexto} />
                <span className="text-sm text-slate-400 ml-auto">
                  Solicitado em {new Date(recolhimento.dataSolicitacao).toLocaleDateString('pt-BR')}
                </span>
              </div>

              <div className="grid grid-cols-2 gap-4 text-sm">
                <div>
                  <p className="font-semibold text-slate-800">{recolhimento.clienteNome}</p>
                  <p className="text-slate-500">{recolhimento.clienteEmail || '—'}</p>
                  <p className="text-slate-500">{recolhimento.clienteTelefone || '—'}</p>
                  <span className="inline-block mt-1 text-xs bg-blue-100 text-blue-700 px-2 py-0.5 rounded">{recolhimento.clientePlano}</span>
                </div>
                <div>
                  <p className="text-slate-600"><span className="font-medium">Série:</span> {recolhimento.numeroSerie}</p>
                  <p className="text-slate-600"><span className="font-medium">Modelo:</span> {recolhimento.modelo || '—'}</p>
                  <p className="text-slate-600"><span className="font-medium">Ticket Hub:</span> {recolhimento.ticketHub || '—'}</p>
                  <p className="text-slate-600"><span className="font-medium">Ticket BLiP:</span> {recolhimento.ticketBlip || '—'}</p>
                </div>
              </div>

              {recolhimento.descricaoProblema && (
                <div className="mt-4 pt-4 border-t">
                  <p className="text-xs font-bold text-blue-600 uppercase mb-1">Problema</p>
                  <p className="text-sm text-slate-700 whitespace-pre-wrap">{recolhimento.descricaoProblema}</p>
                </div>
              )}

              {recolhimento.relatorioN3 && (
                <div className="mt-4 pt-4 border-t">
                  <p className="text-xs font-bold text-blue-600 uppercase mb-1">Relatório N3</p>
                  <p className="text-sm text-slate-700 whitespace-pre-wrap">{recolhimento.relatorioN3}</p>
                </div>
              )}
            </div>

            {/* Update Status */}
            <div className="bg-white rounded-xl shadow-sm p-6">
              <h3 className="font-semibold text-slate-800 mb-4 flex items-center gap-2"><RefreshCw size={16} /> Alterar Status</h3>
              <div className="flex gap-3">
                <select
                  value={novoStatus}
                  onChange={e => setNovoStatus(e.target.value)}
                  className="flex-1 border border-slate-300 rounded-lg px-4 py-2.5 text-sm bg-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">Selecione novo status...</option>
                  {STATUS_OPTIONS.filter(o => o.value).map(o => (
                    <option key={o.value} value={o.value}>{o.label}</option>
                  ))}
                </select>
                <Button onClick={() => novoStatus && statusMutation.mutate({ novoStatus, observacao: observacaoStatus, usuario })} disabled={!novoStatus || statusMutation.isPending}>
                  Atualizar
                </Button>
              </div>
            </div>

            {/* Comentários */}
            <div className="bg-white rounded-xl shadow-sm p-6">
              <h3 className="font-semibold text-slate-800 mb-4 flex items-center gap-2"><MessageSquare size={16} /> Comentários</h3>

              {/* Add comment */}
              <div className="flex gap-3 mb-4">
                <Textarea
                  placeholder="Adicionar comentário..."
                  rows={3}
                  value={novoComentario}
                  onChange={e => setNovoComentario(e.target.value)}
                  className="flex-1"
                />
                <div className="flex flex-col gap-2">
                  <select value={setorComentario} onChange={e => setSetorComentario(e.target.value)}
                    className="border border-slate-300 rounded-lg px-3 py-2.5 text-sm bg-white">
                    {SETOR_OPTIONS.map(s => <option key={s} value={s}>{s}</option>)}
                  </select>
                  <Button onClick={() => novoComentario && comentarioMutation.mutate({ recolhimentoId, texto: novoComentario, usuario, setor: setorComentario })} disabled={!novoComentario}>
                    Comentar
                  </Button>
                </div>
              </div>

              {/* Comments list */}
              <div className="space-y-3">
                {(comentarios || []).map((c: any) => (
                  <div key={c.id} className="border border-slate-200 rounded-lg p-4">
                    <div className="flex items-center justify-between mb-2">
                      <div className="flex items-center gap-2 text-xs">
                        <span className="font-semibold text-blue-600">{c.setor}</span>
                        <span className="text-slate-400">•</span>
                        <span className="text-slate-500">{c.usuario}</span>
                        <span className="text-slate-400">•</span>
                        <span className="text-slate-400">{new Date(c.dataCriacao).toLocaleString('pt-BR')}</span>
                      </div>
                      <button onClick={() => deleteComentarioMutation.mutate(c.id)} className="text-red-400 hover:text-red-600 text-xs">
                        <Trash2 size={14} />
                      </button>
                    </div>
                    <p className="text-sm text-slate-700 whitespace-pre-wrap">{c.texto}</p>
                  </div>
                ))}
                {(!comentarios || comentarios.length === 0) && (
                  <p className="text-sm text-slate-400 text-center py-4">Nenhum comentário ainda.</p>
                )}
              </div>
            </div>
          </div>

          {/* Sidebar */}
          <div className="space-y-6">
            {/* Anexos */}
            <div className="bg-white rounded-xl shadow-sm p-6">
              <h3 className="font-semibold text-slate-800 mb-4 flex items-center gap-2"><FileText size={16} /> Anexos</h3>

              <input
                type="file"
                accept=".pdf"
                id="upload-anexo"
                className="hidden"
                onChange={e => {
                  const file = e.target.files?.[0];
                  if (file) uploadAnexoMutation.mutate(file);
                  e.target.value = '';
                }}
              />
              <label htmlFor="upload-anexo" className="flex items-center gap-2 px-4 py-2 border border-slate-300 rounded-lg text-sm cursor-pointer hover:bg-slate-50 w-fit mb-4">
                <Upload size={14} /> Anexar PDF
              </label>

              <div className="space-y-2">
                {(anexos || []).map((a: any) => (
                  <div key={a.id} className="flex items-center justify-between p-3 bg-slate-50 rounded-lg">
                    <div className="flex items-center gap-2 min-w-0">
                      <span className="text-sm truncate">{a.nomeOriginal}</span>
                    </div>
                    <div className="flex gap-1">
                      <a href={a.caminhoCompleto} target="_blank" rel="noreferrer" className="p-1 text-slate-500 hover:text-blue-600">
                        <ExternalLink size={14} />
                      </a>
                      <button onClick={() => deleteAnexoMutation.mutate(a.id)} className="p-1 text-slate-500 hover:text-red-600">
                        <Trash2 size={14} />
                      </button>
                    </div>
                  </div>
                ))}
                {(!anexos || anexos.length === 0) && (
                  <p className="text-sm text-slate-400 text-center py-2">Nenhum anexo.</p>
                )}
              </div>
            </div>

            {/* Histórico */}
            <div className="bg-white rounded-xl shadow-sm p-6">
              <h3 className="font-semibold text-slate-800 mb-4">Histórico</h3>
              <div className="space-y-3">
                {(historico || []).map((h: any) => (
                  <div key={h.id} className="text-xs border-l-2 border-slate-200 pl-3">
                    <p className="font-medium text-slate-700">
                      {h.statusAnterior.replace(/_/g, ' ')} → {h.statusNovo.replace(/_/g, ' ')}
                    </p>
                    <p className="text-slate-400">{new Date(h.dataAlteracao).toLocaleString('pt-BR')}</p>
                    {h.observacao && <p className="text-slate-500 mt-1">{h.observacao}</p>}
                    {h.usuario && <p className="text-slate-400">por {h.usuario}</p>}
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
