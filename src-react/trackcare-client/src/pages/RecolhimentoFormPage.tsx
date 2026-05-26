import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { apiClient } from '../api/apiClient';
import { PLANO_OPTIONS } from '../types';
import Header from '../components/layout/Header';
import { Input, Select, Textarea, Button } from '../components/ui/Input';
import { ArrowLeft } from 'lucide-react';

export default function RecolhimentoFormPage() {
  const navigate = useNavigate();
  const [usuario] = useState('system');

  const [form, setForm] = useState({
    hgid: '', numeroSerie: '', modelo: '',
    clienteNome: '', clienteContato: '', clienteEmail: '', clienteTelefone: '',
    clientePlano: 'Simples',
    ticketHub: '', ticketBlip: '',
    descricaoProblema: '', relatorioN3: '',
    jaRecolhido: false,
    observacoes: '',
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => apiClient.create(data),
    onSuccess: () => navigate('/recolhimentos'),
  });

  const set = (field: string, value: any) => setForm(prev => ({ ...prev, [field]: value }));

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.hgid || !form.numeroSerie || !form.clienteNome) {
      alert('Preencha os campos obrigatórios');
      return;
    }
    createMutation.mutate({ ...form, usuario });
  };

  return (
    <div className="flex flex-col min-h-screen">
      <Header title="Novo Recolhimento" />
      <main className="flex-1 p-8">
        <div className="max-w-4xl mx-auto">
          <button onClick={() => navigate('/recolhimentos')} className="flex items-center gap-2 text-slate-500 hover:text-slate-800 mb-6">
            <ArrowLeft size={18} /> Voltar
          </button>

          <form onSubmit={handleSubmit} className="bg-white rounded-xl shadow-sm p-8 space-y-8">

            {/* Cliente */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Informações do Cliente</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input label="Nome do Cliente *" required value={form.clienteNome} onChange={e => set('clienteNome', e.target.value)} />
                <Input label="Contato" value={form.clienteContato} onChange={e => set('clienteContato', e.target.value)} />
                <Input label="E-mail" type="email" value={form.clienteEmail} onChange={e => set('clienteEmail', e.target.value)} />
                <Input label="Telefone" value={form.clienteTelefone} onChange={e => set('clienteTelefone', e.target.value)} />
                <Select label="Plano" options={PLANO_OPTIONS.map(p => ({ value: p, label: p }))} value={form.clientePlano} onChange={e => set('clientePlano', e.target.value)} className="md:col-span-2" />
              </div>
            </section>

            {/* Equipamento */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Informações do Equipamento</h3>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <Input label="HGID *" required value={form.hgid} onChange={e => set('hgid', e.target.value)} />
                <Input label="Número de Série *" required value={form.numeroSerie} onChange={e => set('numeroSerie', e.target.value)} />
                <Input label="Modelo" value={form.modelo} onChange={e => set('modelo', e.target.value)} />
              </div>
            </section>

            {/* Tickets */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Tickets</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input label="Ticket HubSpot" value={form.ticketHub} onChange={e => set('ticketHub', e.target.value)} />
                <Input label="Ticket BLiP" value={form.ticketBlip} onChange={e => set('ticketBlip', e.target.value)} />
              </div>
            </section>

            {/* Problema */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Descrição do Problema</h3>
              <Textarea label="Problema" rows={3} value={form.descricaoProblema} onChange={e => set('descricaoProblema', e.target.value)} />
            </section>

            {/* Relatório N3 */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Relatório N3 (Análise Técnica)</h3>
              <Textarea label="Relatório N3" rows={5} value={form.relatorioN3} onChange={e => set('relatorioN3', e.target.value)} />
            </section>

            {/* Observações */}
            <section>
              <h3 className="text-sm font-bold text-blue-600 uppercase tracking-wider mb-4">Observações</h3>
              <Textarea label="Observações" rows={3} value={form.observacoes} onChange={e => set('observacoes', e.target.value)} />
            </section>

            {/* Checkbox */}
            <section>
              <label className="flex items-center gap-3 cursor-pointer">
                <input type="checkbox" checked={form.jaRecolhido} onChange={e => set('jaRecolhido', e.target.checked)}
                  className="w-4 h-4 accent-blue-600" />
                <span className="text-sm text-slate-700">Equipamento já foi recolhido?</span>
              </label>
            </section>

            {/* Actions */}
            <div className="flex gap-3 justify-end pt-4 border-t">
              <Button variant="secondary" type="button" onClick={() => navigate('/recolhimentos')}>Cancelar</Button>
              <Button type="submit" disabled={createMutation.isPending}>
                {createMutation.isPending ? 'Salvando...' : 'Salvar Recolhimento'}
              </Button>
            </div>
          </form>
        </div>
      </main>
    </div>
  );
}
