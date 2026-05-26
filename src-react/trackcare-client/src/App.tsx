import { Routes, Route } from 'react-router-dom';
import Sidebar from './components/layout/Sidebar';
import DashboardPage from './pages/DashboardPage';
import RecolhimentosPage from './pages/RecolhimentosPage';
import RecolhimentoFormPage from './pages/RecolhimentoFormPage';
import RecolhimentoDetailPage from './pages/RecolhimentoDetailPage';

export default function App() {
  return (
    <div className="flex min-h-screen bg-slate-100">
      <Sidebar />
      <div className="flex-1 flex flex-col min-h-screen">
        <Routes>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/recolhimentos" element={<RecolhimentosPage />} />
          <Route path="/recolhimentos/novo" element={<RecolhimentoFormPage />} />
          <Route path="/recolhimentos/:id" element={<RecolhimentoDetailPage />} />
          <Route path="/recolhimentos/:id/editar" element={<RecolhimentoFormPage />} />
        </Routes>
      </div>
    </div>
  );
}
