import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import Header from '../components/layout/Header';
import { apiClient } from '../api/apiClient';
import StatusBadge from '../components/recolhimentos/StatusBadge';
import { ClipboardList, Clock, Wrench, CheckCircle } from 'lucide-react';

function MetricCard({ title, value, icon: Icon, color }: { title: string; value: number; icon: any; color: string }) {
  return (
    <div className={`${color} rounded-xl p-6 text-white shadow-lg`}>
      <div className="flex justify-end">
        <Icon size={28} />
      </div>
      <p className="text-4xl font-bold mt-4">{value}</p>
      <p className="text-sm opacity-90 mt-1">{title}</p>
    </div>
  );
}

export default function DashboardPage() {
  const { data: stats } = useQuery({
    queryKey: ['dashboard-stats'],
    queryFn: () => apiClient.getStats(),
  });

  const { data: recent } = useQuery({
    queryKey: ['recent-recolhimentos'],
    queryFn: () => apiClient.getRecent(5),
  });

  return (
    <div className="flex flex-col min-h-screen">
      <Header title="Dashboard" />
      <main className="flex-1 p-8">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <MetricCard title="Recolhimentos Ativos" value={stats?.totalAtivos ?? 0} icon={ClipboardList} color="bg-blue-600" />
          <MetricCard title="Aguardam Coleta" value={stats?.aguardaColeta ?? 0} icon={Clock} color="bg-amber-500" />
          <MetricCard title="Em Reparo" value={stats?.emReparo ?? 0} icon={Wrench} color="bg-purple-600" />
          <MetricCard title="Encerrados (Mês)" value={stats?.encerradosMes ?? 0} icon={CheckCircle} color="bg-emerald-500" />
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-lg font-semibold text-slate-800">Recolhimentos Recentes</h3>
            <Link to="/recolhimentos" className="text-sm text-blue-600 hover:underline">Ver todos</Link>
          </div>
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-slate-100">
                  <th className="text-left py-3 px-2 text-slate-500 font-medium">HGID</th>
                  <th className="text-left py-3 px-2 text-slate-500 font-medium">Cliente</th>
                  <th className="text-left py-3 px-2 text-slate-500 font-medium">Status</th>
                  <th className="text-left py-3 px-2 text-slate-500 font-medium">Data</th>
                  <th className="text-right py-3 px-2 text-slate-500 font-medium">Ação</th>
                </tr>
              </thead>
              <tbody>
                {(recent || []).map((r: any) => (
                  <tr key={r.id} className="border-b border-slate-50 hover:bg-slate-50">
                    <td className="py-3 px-2 font-medium">{r.hgid}</td>
                    <td className="py-3 px-2">{r.clienteNome}</td>
                    <td className="py-3 px-2"><StatusBadge status={r.status} /></td>
                    <td className="py-3 px-2 text-slate-500">
                      {new Date(r.dataSolicitacao).toLocaleDateString('pt-BR')}
                    </td>
                    <td className="py-3 px-2 text-right">
                      <Link to={`/recolhimentos/${r.id}`} className="text-blue-600 hover:underline text-sm">
                        Ver →
                      </Link>
                    </td>
                  </tr>
                ))}
                {(!recent || recent.length === 0) && (
                  <tr>
                    <td colSpan={5} className="py-8 text-center text-slate-400">Nenhum recolhimento encontrado</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  );
}
