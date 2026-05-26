interface HeaderProps {
  title: string;
}

export default function Header({ title }: HeaderProps) {
  const today = new Date().toLocaleDateString('pt-BR', {
    weekday: 'long',
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });

  return (
    <header className="bg-white border-b border-slate-200 px-8 py-5 flex items-center justify-between">
      <h2 className="text-xl font-bold text-slate-800">{title}</h2>
      <span className="text-sm text-slate-500 capitalize">{today}</span>
    </header>
  );
}
