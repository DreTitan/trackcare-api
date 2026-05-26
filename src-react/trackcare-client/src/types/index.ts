export interface DashboardStats {
  totalAtivos: number;
  aguardaColeta: number;
  emReparo: number;
  encerradosMes: number;
}

export interface Recolhimento {
  id: number;
  hgid: string;
  numeroSerie: string;
  modelo: string | null;
  clienteNome: string;
  clienteContato: string | null;
  clienteEmail: string | null;
  clienteTelefone: string | null;
  clientePlano: string;
  ticketHub: string | null;
  ticketBlip: string | null;
  descricaoProblema: string | null;
  relatorioN3: string | null;
  jaRecolhido: boolean;
  status: string;
  statusTexto: string;
  dataSolicitacao: string;
  dataPrevistaColeta: string | null;
  dataColetaReal: string | null;
  dataPrevistaDevolucao: string | null;
  dataDevolucaoReal: string | null;
  observacoes: string | null;
  criadoPor: string | null;
  criadoEm: string;
  atualizadoEm: string;
}

export interface CreateRecolhimento {
  hgid: string;
  numeroSerie: string;
  modelo?: string;
  clienteNome: string;
  clienteContato?: string;
  clienteEmail?: string;
  clienteTelefone?: string;
  clientePlano: string;
  ticketHub?: string;
  ticketBlip?: string;
  descricaoProblema?: string;
  relatorioN3?: string;
  jaRecolhido: boolean;
  dataPrevistaColeta?: string;
  observacoes?: string;
  usuario: string;
}

export interface UpdateRecolhimento extends CreateRecolhimento {
  id: number;
}

export interface Anexo {
  id: number;
  recolhimentoId: number;
  tipo: string;
  nomeOriginal: string;
  nomeArquivo: string;
  tamanhoBytes: number;
  usuarioUpload: string | null;
  dataUpload: string;
  caminhoCompleto: string;
}

export interface Comentario {
  id: number;
  recolhimentoId: number;
  texto: string;
  usuario: string | null;
  setor: string | null;
  dataCriacao: string;
}

export interface HistoricoStatus {
  id: number;
  recolhimentoId: number;
  statusAnterior: string;
  statusNovo: string;
  observacao: string | null;
  usuario: string | null;
  dataAlteracao: string;
}

export const STATUS_COLORS: Record<string, string> = {
  'N3_Enviou': 'bg-blue-100 text-blue-800',
  'Aguarda_Admin': 'bg-orange-100 text-orange-800',
  'Aguarda_Coleta': 'bg-yellow-100 text-yellow-800',
  'Em_Transito': 'bg-purple-100 text-purple-800',
  'Recebido_Fabrica': 'bg-indigo-100 text-indigo-800',
  'Em_Reparo': 'bg-purple-100 text-purple-800',
  'Calibracao': 'bg-cyan-100 text-cyan-800',
  'Pronto_Devolver': 'bg-teal-100 text-teal-800',
  'Enviado': 'bg-blue-100 text-blue-800',
  'Follow_Up': 'bg-gray-100 text-gray-800',
  'Encerrado': 'bg-green-100 text-green-800',
};

export const STATUS_OPTIONS = [
  { value: '', label: 'Todos os status' },
  { value: 'N3_Enviou', label: 'N3 Enviou' },
  { value: 'Aguarda_Admin', label: 'Aguarda Admin' },
  { value: 'Aguarda_Coleta', label: 'Aguarda Coleta' },
  { value: 'Em_Transito', label: 'Em Trânsito' },
  { value: 'Recebido_Fabrica', label: 'Recebido Fábrica' },
  { value: 'Em_Reparo', label: 'Em Reparo' },
  { value: 'Calibracao', label: 'Calibração' },
  { value: 'Pronto_Devolver', label: 'Pronto Devolver' },
  { value: 'Enviado', label: 'Enviado' },
  { value: 'Follow_Up', label: 'Follow Up' },
  { value: 'Encerrado', label: 'Encerrado' },
];

export const SETOR_OPTIONS = ['N1', 'N2', 'N3', 'Admin', 'Logística', 'Técnico'];
export const PLANO_OPTIONS = ['GoPremium', 'Simples'];
