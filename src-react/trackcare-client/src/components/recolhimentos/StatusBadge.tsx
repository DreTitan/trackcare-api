import { STATUS_COLORS } from '../../types';

interface StatusBadgeProps {
  status: string;
  statusTexto?: string;
}

export default function StatusBadge({ status, statusTexto }: StatusBadgeProps) {
  const colorClass = STATUS_COLORS[status] || 'bg-gray-100 text-gray-800';
  const text = statusTexto || status.replace(/_/g, ' ');

  return (
    <span className={`inline-block px-3 py-1 rounded-full text-xs font-semibold ${colorClass}`}>
      {text}
    </span>
  );
}
