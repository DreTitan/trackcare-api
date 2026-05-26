using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Diagnostics;
using HealthGoWPF.Models;
using HealthGoWPF.Services;

namespace HealthGoWPF.Views;

public partial class MainWindow : Window
{
    private readonly RecolhimentoService _service;
    private int _recolhimentoEditandoId = 0;
    private int _recolhimentoDetalheId = 0;

    public MainWindow()
    {
        InitializeComponent();
        _service = new RecolhimentoService();

        TxtData.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");

        // Popular ComboBox de Status
        CmbStatus.ItemsSource = Enum.GetValues<StatusRecolhimento>().ToList();
        CmbStatus.SelectedIndex = 0;

        // Popular ComboBox de Novo Status
        CmbNovoStatus.ItemsSource = Enum.GetValues<StatusRecolhimento>().ToList();

        CarregarDashboard();
        EsconderTodosPaineis();
        MostrarPainel("Dashboard");
    }

    private void EsconderTodosPaineis()
    {
        PainelDashboard.Visibility = Visibility.Collapsed;
        PainelLista.Visibility = Visibility.Collapsed;
        PainelFormulario.Visibility = Visibility.Collapsed;
        PainelDetalhe.Visibility = Visibility.Collapsed;
    }

    private void MostrarPainel(string painel)
    {
        EsconderTodosPaineis();
        ResetarBotoesMenu();

        switch (painel)
        {
            case "Dashboard":
                PainelDashboard.Visibility = Visibility.Visible;
                TxtTitulo.Text = "Dashboard";
                BtnDashboard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
                CarregarDashboard();
                break;
            case "Lista":
                PainelLista.Visibility = Visibility.Visible;
                TxtTitulo.Text = "Recolhimentos";
                BtnRecolhimentos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
                CarregarRecolhimentos();
                break;
            case "Formulario":
                PainelFormulario.Visibility = Visibility.Visible;
                TxtTitulo.Text = "Novo Recolhimento";
                BtnNovoRecolhimento.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
                LimparFormulario();
                break;
            case "Detalhe":
                PainelDetalhe.Visibility = Visibility.Visible;
                TxtTitulo.Text = "Detalhes";
                break;
        }
    }

    private void ResetarBotoesMenu()
    {
        var cinza = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#475569"));
        BtnDashboard.Background = new SolidColorBrush(Colors.Transparent);
        BtnRecolhimentos.Background = new SolidColorBrush(Colors.Transparent);
        BtnNovoRecolhimento.Background = new SolidColorBrush(Colors.Transparent);
    }

    private void CarregarDashboard()
    {
        var stats = _service.GetStats();
        TxtTotalAtivos.Text = stats.totalAtivos.ToString();
        TxtAguardaColeta.Text = stats.aguardaColeta.ToString();
        TxtEmReparo.Text = stats.emReparo.ToString();
        TxtEncerradosMes.Text = stats.encerradosMes.ToString();

        var recentes = _service.GetAll().Take(5).ToList();
        DgRecentes.ItemsSource = recentes;
    }

    private void CarregarRecolhimentos()
    {
        var termo = TxtBusca.Text;
        StatusRecolhimento? status = CmbStatus.SelectedItem as StatusRecolhimento?;

        var lista = _service.Search(termo, status);
        DgRecolhimentos.ItemsSource = lista;
    }

    private void LimparFormulario()
    {
        TxtClienteNome.Text = "";
        TxtClienteContato.Text = "";
        TxtClienteEmail.Text = "";
        TxtClienteTelefone.Text = "";
        RbSimples.IsChecked = true;
        TxtHgid.Text = "";
        TxtNumeroSerie.Text = "";
        TxtModelo.Text = "";
        TxtTicketHub.Text = "";
        TxtTicketBlip.Text = "";
        TxtDescricaoProblema.Text = "";
        TxtRelatorioN3.Text = "";
        ChkJaRecolhido.IsChecked = false;
        TxtObservacoes.Text = "";
        _recolhimentoEditandoId = 0;
        LstAnexos.ItemsSource = null;
    }

    private void PreencherFormulario(Recolhimento r)
    {
        TxtClienteNome.Text = r.ClienteNome;
        TxtClienteContato.Text = r.ClienteContato;
        TxtClienteEmail.Text = r.ClienteEmail;
        TxtClienteTelefone.Text = r.ClienteTelefone;
        if (r.ClientePlano == TipoPlano.GoPremium)
            RbGoPremium.IsChecked = true;
        else
            RbSimples.IsChecked = true;
        TxtHgid.Text = r.Hgid;
        TxtNumeroSerie.Text = r.NumeroSerie;
        TxtModelo.Text = r.Modelo;
        TxtTicketHub.Text = r.TicketHub;
        TxtTicketBlip.Text = r.TicketBlip;
        TxtDescricaoProblema.Text = r.DescricaoProblema;
        TxtRelatorioN3.Text = r.RelatorioN3;
        ChkJaRecolhido.IsChecked = r.JaRecolhido;
        TxtObservacoes.Text = r.Observacoes;
        _recolhimentoEditandoId = r.Id;
        CarregarAnexosFormulario();
    }

    private void CarregarAnexosFormulario()
    {
        if (_recolhimentoEditandoId > 0)
        {
            LstAnexos.ItemsSource = _service.GetAnexos(_recolhimentoEditandoId);
        }
    }

    private Recolhimento ObterDoFormulario()
    {
        return new Recolhimento
        {
            Id = _recolhimentoEditandoId,
            ClienteNome = TxtClienteNome.Text,
            ClienteContato = TxtClienteContato.Text,
            ClienteEmail = TxtClienteEmail.Text,
            ClienteTelefone = TxtClienteTelefone.Text,
            ClientePlano = RbGoPremium.IsChecked == true ? TipoPlano.GoPremium : TipoPlano.Simples,
            Hgid = TxtHgid.Text,
            NumeroSerie = TxtNumeroSerie.Text,
            Modelo = TxtModelo.Text,
            TicketHub = TxtTicketHub.Text,
            TicketBlip = TxtTicketBlip.Text,
            DescricaoProblema = TxtDescricaoProblema.Text,
            RelatorioN3 = TxtRelatorioN3.Text,
            JaRecolhido = ChkJaRecolhido.IsChecked == true,
            Status = StatusRecolhimento.N3_Enviou,
            Observacoes = TxtObservacoes.Text
        };
    }

    private void MostrarDetalhe(Recolhimento r)
    {
        EsconderTodosPaineis();
        PainelDetalhe.Visibility = Visibility.Visible;

        _recolhimentoDetalheId = r.Id;

        TxtTitulo.Text = $"Recolhimento {r.Hgid}";
        TxtTituloDetalhe.Text = $"Detalhes - {r.Hgid}";

        TxtHgidDetalhe.Text = r.Hgid;
        TxtStatusDetalhe.Text = r.StatusTexto;
        TxtDataSolicitacaoDetalhe.Text = $"Solicitado em: {r.DataSolicitacao:dd/MM/yyyy HH:mm}";

        TxtClienteDetalhe.Text = r.ClienteNome;
        TxtContatoDetalhe.Text = $"{r.ClienteContato} | {r.ClienteEmail} | {r.ClienteTelefone}";
        TxtPlanoDetalhe.Text = $"Plano: {(r.ClientePlano == TipoPlano.GoPremium ? "GoPremium" : "Simples")}";

        TxtSerieDetalhe.Text = r.NumeroSerie;
        TxtModeloDetalhe.Text = r.Modelo ?? "-";

        TxtTicketHubDetalhe.Text = r.TicketHub ?? "-";
        TxtTicketBlipDetalhe.Text = r.TicketBlip ?? "-";

        TxtProblemaDetalhe.Text = r.DescricaoProblema ?? "-";
        TxtRelatorioDetalhe.Text = r.RelatorioN3 ?? "-";

        // Cor do status
        var corStatus = r.Status switch
        {
            StatusRecolhimento.N3_Enviou => "#6B7280",
            StatusRecolhimento.Aguarda_Admin => "#F59E0B",
            StatusRecolhimento.Aguarda_Coleta => "#F59E0B",
            StatusRecolhimento.Em_Transito => "#3B82F6",
            StatusRecolhimento.Recebido_Fabrica => "#3B82F6",
            StatusRecolhimento.Em_Reparo => "#8B5CF6",
            StatusRecolhimento.Calibracao => "#8B5CF6",
            StatusRecolhimento.Pronto_Devolver => "#3B82F6",
            StatusRecolhimento.Enviado => "#3B82F6",
            StatusRecolhimento.Follow_Up => "#06B6D4",
            StatusRecolhimento.Encerrado => "#10B981",
            _ => "#6B7280"
        };
        BorderStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(corStatus));

        CmbNovoStatus.SelectedItem = r.Status;

        // Carregar anexos
        CarregarAnexosDetalhe();

        // Carregar comentários
        CarregarComentarios();

        // Selecionar primeiro setor por padrão
        CmbSetor.SelectedIndex = 0;
    }

    private void CarregarComentarios()
    {
        if (_recolhimentoDetalheId > 0)
        {
            LstComentarios.ItemsSource = _service.GetComentarios(_recolhimentoDetalheId);
        }
    }

    private void CarregarAnexosDetalhe()
    {
        if (_recolhimentoDetalheId > 0)
        {
            LstAnexosDetalhe.ItemsSource = _service.GetAnexos(_recolhimentoDetalheId);
        }
    }

    // Eventos dos botões do menu
    private void BtnDashboard_Click(object sender, RoutedEventArgs e)
    {
        MostrarPainel("Dashboard");
    }

    private void BtnRecolhimentos_Click(object sender, RoutedEventArgs e)
    {
        TxtBusca.Text = "";
        CmbStatus.SelectedIndex = 0;
        MostrarPainel("Lista");
    }

    private void BtnNovoRecolhimento_Click(object sender, RoutedEventArgs e)
    {
        LimparFormulario();
        TxtTitulo.Text = "Novo Recolhimento";
        MostrarPainel("Formulario");
    }

    private void BtnSair_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void BtnVoltar_Click(object sender, RoutedEventArgs e)
    {
        MostrarPainel("Lista");
    }

    // Eventos de busca
    private void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
    {
        CarregarRecolhimentos();
    }

    private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgRecolhimentos != null)
            CarregarRecolhimentos();
    }

    private void BtnBuscar_Click(object sender, RoutedEventArgs e)
    {
        CarregarRecolhimentos();
    }

    // Eventos do DataGrid
    private void DgRecentes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DgRecentes.SelectedItem is Recolhimento r)
            MostrarDetalhe(r);
    }

    private void DgRecolhimentos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DgRecolhimentos.SelectedItem is Recolhimento r)
            MostrarDetalhe(r);
    }

    private void BtnVerRecolhimento_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.DataContext is Recolhimento r)
            MostrarDetalhe(r);
    }

    private void BtnEditarRecolhimento_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.DataContext is Recolhimento r)
        {
            var recolhimento = _service.GetById(r.Id);
            if (recolhimento != null)
            {
                PreencherFormulario(recolhimento);
                TxtTitulo.Text = "Editar Recolhimento";
                MostrarPainel("Formulario");
            }
        }
    }

    private void BtnEditarRecolhimentoDetalhe_Click(object sender, RoutedEventArgs e)
    {
        if (TxtHgidDetalhe.Text != "")
        {
            var r = _service.GetAll().FirstOrDefault(x => x.Hgid == TxtHgidDetalhe.Text);
            if (r != null)
            {
                PreencherFormulario(r);
                TxtTitulo.Text = "Editar Recolhimento";
                MostrarPainel("Formulario");
            }
        }
    }

    private void BtnExcluirRecolhimento_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.DataContext is Recolhimento r)
        {
            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o recolhimento {r.Hgid}?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                _service.Delete(r.Id);
                MessageBox.Show("Recolhimento excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarRecolhimentos();
            }
        }
    }

    // Salvar recolhimento
    private void BtnSalvarRecolhimento_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtClienteNome.Text) ||
            string.IsNullOrWhiteSpace(TxtHgid.Text) ||
            string.IsNullOrWhiteSpace(TxtNumeroSerie.Text) ||
            string.IsNullOrWhiteSpace(TxtDescricaoProblema.Text))
        {
            MessageBox.Show("Por favor, preencha os campos obrigatórios.", "Campos obrigatórios", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var recolhimento = ObterDoFormulario();

            if (_recolhimentoEditandoId > 0)
            {
                _service.Update(recolhimento);
                MessageBox.Show("Recolhimento atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _service.Create(recolhimento, "Admin");
                MessageBox.Show("Recolhimento criado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            CarregarRecolhimentos();
            MostrarPainel("Lista");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Atualizar status
    private void BtnAtualizarStatus_Click(object sender, RoutedEventArgs e)
    {
        if (TxtHgidDetalhe.Text == "") return;

        var r = _service.GetAll().FirstOrDefault(x => x.Hgid == TxtHgidDetalhe.Text);
        if (r == null) return;

        if (CmbNovoStatus.SelectedItem is StatusRecolhimento novoStatus)
        {
            _service.UpdateStatus(r.Id, novoStatus, "Atualizado via sistema", "Admin");
            MessageBox.Show("Status atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            var atualizado = _service.GetById(r.Id);
            if (atualizado != null)
                MostrarDetalhe(atualizado);
        }
    }

    // ========== MÉTODOS DE ANEXOS ==========

    private void BtnAnexarPdf_Click(object sender, RoutedEventArgs e)
    {
        // Primeiro salvar o recolhimento se ainda não foi salvo
        if (_recolhimentoEditandoId == 0)
        {
            MessageBox.Show("Por favor, salve o recolhimento primeiro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dialog = new OpenFileDialog
        {
            Filter = "Arquivos PDF (*.pdf)|*.pdf",
            Title = "Selecionar arquivo PDF"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var arquivo = dialog.FileName;
                var nomeOriginal = System.IO.Path.GetFileName(arquivo);

                _service.AdicionarAnexo(_recolhimentoEditandoId, arquivo, nomeOriginal, "Admin");
                CarregarAnexosFormulario();
                MessageBox.Show($"PDF '{nomeOriginal}' anexado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao anexar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnAnexarPdfDetalhe_Click(object sender, RoutedEventArgs e)
    {
        if (_recolhimentoDetalheId == 0)
        {
            MessageBox.Show("Nenhum recolhimento selecionado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dialog = new OpenFileDialog
        {
            Filter = "Arquivos PDF (*.pdf)|*.pdf",
            Title = "Selecionar arquivo PDF"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var arquivo = dialog.FileName;
                var nomeOriginal = System.IO.Path.GetFileName(arquivo);

                _service.AdicionarAnexo(_recolhimentoDetalheId, arquivo, nomeOriginal, "Admin");
                CarregarAnexosDetalhe();
                MessageBox.Show($"PDF '{nomeOriginal}' anexado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao anexar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnAbrirAnexo_Click(object sender, RoutedEventArgs e)
    {
        if (LstAnexosDetalhe.SelectedItem is Anexo anexo)
        {
            var caminho = _service.ObterCaminhoAnexo(anexo.Id);
            if (caminho != null && System.IO.File.Exists(caminho))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = caminho,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao abrir arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Arquivo não encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Selecione um anexo primeiro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void BtnExcluirAnexo_Click(object sender, RoutedEventArgs e)
    {
        if (LstAnexosDetalhe.SelectedItem is Anexo anexo)
        {
            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o anexo '{anexo.NomeOriginal}'?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                if (_service.ExcluirAnexo(anexo.Id))
                {
                    MessageBox.Show("Anexo excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarAnexosDetalhe();
                }
                else
                {
                    MessageBox.Show("Erro ao excluir anexo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Selecione um anexo primeiro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void LstAnexos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Não é necessário implementar para ListBox simples
    }

    // ========== MÉTODOS DE COMENTÁRIOS ==========

    private void BtnAdicionarComentario_Click(object sender, RoutedEventArgs e)
    {
        if (_recolhimentoDetalheId == 0)
        {
            MessageBox.Show("Nenhum recolhimento selecionado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtNovoComentario.Text))
        {
            MessageBox.Show("Por favor, digite o comentário.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var setor = (CmbSetor.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Admin";
            _service.AdicionarComentario(_recolhimentoDetalheId, TxtNovoComentario.Text, "Admin", setor);
            TxtNovoComentario.Text = "";
            CarregarComentarios();
            MessageBox.Show("Comentário adicionado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao adicionar comentário: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnExcluirComentario_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Tag is int comentarioId)
        {
            var resultado = MessageBox.Show(
                "Deseja realmente excluir este comentário?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                if (_service.ExcluirComentario(comentarioId))
                {
                    MessageBox.Show("Comentário excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarComentarios();
                }
                else
                {
                    MessageBox.Show("Erro ao excluir comentário.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
