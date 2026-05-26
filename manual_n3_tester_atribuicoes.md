# Manual de Atribuições e Operações – Suporte N3 e Tester (HealthGo)

## 1. Propósito e Visão Geral
Este documento tem como finalidade primordial mapear as atribuições, os fluxos e as responsabilidades inerentes à cadeira conjunta de **Suporte Avançado N3** e **Tester**. A documentação consiste na unificação do Procedimento Operacional Padrão (POP) de Atendimento (Mapeado no POP-SUP-001) e o POP Operacional Técnico, garantindo que todo o capital intelectual fique consolidado em um roteiro linear, permitindo a total capacitação de qualquer novo coordenador ou colaborador que venha a assumir estas posições.

## 2. Acordo de Nível de Serviço (SLA)
- **Tempo Limite (Atuação e Resolução):** Após a recepção do chamado, o N3 dispõe de **até 3 horas** para aplicar análises, tentar corrigir via software, alterar bases geradas e emitir o Parecer Técnico.

---

## 3. Fluxo Prático de Atendimento (Suporte N3)
A partir do momento em que um chamado impeditivo é transferido pela camada técnica N2, a operação padrão passa pelo seguinte checklist rigoroso:

### 3.1. Recepção e Conexão de Dispositivos
1. **Recepção de Ticket e Triagem Inicial:** Receber o chamado vindo do N2 para atuar no escopo do SLA.
2. **Telemetria Básica:** Consultar o aparelho referenciado usando o seu **HGID** no *HealthGo Studio* (se não possuir, pode-se usar o e-mail ou número de série).

### 3.2. Diagnóstico a Fundo e Análise de Componentes Analógicos
3. **Sensores e Calibração:** Levantar os logs de calibração para servir de "Base Line" do aparelho do cliente.
4. **Leitura dos Três Estágios Físicos:**
   - Analisar exaustivamente os Sensores (Antes, durante e pós sopro do paciente).
   - Analisar voltagem e tensão aplicada nos sensores ao longo dos três cenários de medição, comparando sempre com a calibração inicial estabelecida.
   - Analisar como está se portando a umidade e a flutuação nos três estados (especialmente capturado no canal `ExamChamberCirculate`).
   - Analisar o histórico de temperatura aferida antes dos exames passados iniciarem.
5. **Bombas Pneumáticas e Gás:** Analisar os pistões/bombas operacionais durante os estresses de entrada. Analisar também os valores de partes por milhão (PPMS) capturados na Dashboard, unindo o resultado frente à métrica de tensão nominal do aparelho.

### 3.3. Monitoramento de Software, Firmware e Resolução Preditiva
6. **Dados de Exames no HealthGo Studio:** Avaliar os exames que ocorreram e se houveram exames que ficaram estagnados em processamento (em andamento) durante intermitências.
7. **Manipulação de Firmware Remoto:**
   - Checar qual é o firmware da máquina.
   - Enviar links de novos Instaladores de Firmware (Atualização e Downgrades/SVM).
   - Deliberar comandos remotos na arquitetura do HealthGo Studio para forçar atitudes em máquina.

### 3.4. Atuação via Banco de Dados (Mitigando erros em Base DBeaver)
8. **Edição do Banco Local:**
   - Extrair e carregar a base de dados do cliente na plataforma **DBeaver** (Interface Low-Code de Gestão Relacional baseada em SQLite e sintaxe JSON).
   - Realizar mineração dos relatórios guardados da base procurando códigos de erro escondidos que causam as falhas de ambiente.
   - Alterar instâncias imperfeitas e resolver gargalos operacionais registrados nos arquivos de configuração do banco nativo.
9. **Fechamento de Pacote Lógico:**
   - Re-compactar a base da dados.
   - Anexar este arquivo resolvido no chamado da requisição para retorno imediato.

### 3.5. Parecer e Repasse de Chamado
10. **Anotações Oficiais e Formalização:** Documentar tudo o que foi analisado de software, firmware e calibrações.
11. **Classificar Saídas (SOP e Qualidade ISO):**
    - Identificar a Resolução – **Saída A:** Solução viabilizada via configurações sem devolução *vs* **Saída B:** Reprovação por hardware determinando recolhimento em rito da Política *"MonaLisa"*.
12. **Parecer e Encerramento:** Gerar o PDF/Parecer final anexado com a solução detalhada. Em seguida, encaminhar para os Técnicos N2 assumirem para realizar o Deploy e devolução do andamento para o cliente de ponta.

---

## 4. Atribuições da Posição de "Tester"

Esta responsabilidade trabalha proativamente com os desenvolvimentos de Firmware e Softwares da engenharia da empresa.

- **Check-lista de Validação (Ferramentas e Links):** O papel tem intuito exclusivo de tentar *quebrar e engarrafar* sistemas a fim de encontrar erros de desenvolvimento não previstos. O intuito é caçar bugs, instabilidades não reportadas e precessos limitadores na usabilidade do dia-a-dia da ferramenta.

- **Banco de Dados no DBeaver:** O Tester obrigatoriamente se beneficia do DBeaver ([Download Oficial Aqui](https://dbeaver.io/download/)) para adentrar tabelas em formato SQlite e códigos estáticos JSON logados durante seus processos de simulação. A ferramenta guarda nos "porões do arquivo" as falhas cruciais e as intermitências.  

- **Regra de Ouro (Terminal Debug):**
  - Todo e qualquer pacote disponibilizado pelas áreas de Desenvolvimento/Engenharia da Produção deverá **obrigatoriamente** possuir o **Terminal de Debug** da aplicação ligado.
  - A evidência de funcionamento dos testes exige a exibição destes prompts para acompanhamento mais crítico do software. Não é possível fazer a cobertura visual focando apenas no Front-End do App.

---

## 5. Práticas Paliativas para Mitigação de Bugs
Quando o problema encontrado não apresenta solução simples a curtíssimo prazo, o profissional N3 atua em vias paliativas a fim de manter o cliente com a máquina operando:
1. Orientação a respeito de contornos lógicos para clientes;
2. Pedido de Acesso Remoto à máquina final e auxílio "mãos curtas";
3. Solicitar autorizações a instâncias superiores (Engenharia) visando efetuar a *instalação de retroação/rollback* de software e retornar para versões onde a máquina operava normal.
4. Injeção de "Software com Debug" instalado localmente na máquina do cliente para mapear de onde o "bug novo" está emergindo.
5. Retorno de apontamento para os níveis de engenharia de software da HealthGo.
