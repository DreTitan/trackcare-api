# Revisão Técnica: Ecossistema de Sistemas Internos HealthGo

Este documento apresenta uma análise detalhada das ferramentas fundamentais para a operação técnica da HealthGo, consolidando informações do **HealthGo Client** e **HealthGo Studio**, com foco em requisitos, limitações e impacto no cliente final.

---

## 1. HealthGo Client (Ferramenta de Atualização SVM v2.0)

O **HealthGo Client** atua como o motor de inteligência e calibração da empresa. É uma ferramenta de pipeline que transforma dados brutos do Datalake em inteligência embarcada nos dispositivos.

### 🧠 Função e Explicação
Sua função primária é o processamento e a subida de modelos **SVM (Support Vector Machine)** para o Nocobase. Ele executa um fluxo de 7 etapas críticas:
1.  **Importação de Dados:** Coleta dados de calibração e de campo (Datalake).
2.  **Processamento:** Limpeza de dados e remoção de *outliers*.
3.  **Treinamento:** Geração do modelo matemático SVM.
4.  **Revisão (N3/Engenharia):** Validação técnica via gráficos interativos.
5.  **Compilação:** Geração do binário de Firmware com o modelo integrado.
6.  **Sincronização:** Envio do firmware e metadados para a nuvem (Nocobase/AWS).

### ⚙️ Requisitos Técnicos para Operação
Para operar o Client, o analista (N3/Tester) deve possuir:
*   **Domínio Analítico:** Capacidade de interpretação de gráficos de dispersão e curvas de sensores.
*   **Conhecimento Químico/Físico:** Entendimento da relação entre gases (H2, CH4) e a resistência dos sensores (Rs/R0).
*   **Análise Comparativa:** Habilidade para validar erros absolutos e predições versus valores reais de calibração.
*   **Infraestrutura:** Conexão estável com o servidor interno e permissões de escrita no Datalake/Nocobase.

### 🚧 Gargalos Identificados
*   **Processo Manual:** Necessidade de replicar dados do Datalake para o Nocobase de forma manual.
*   **Limitação de Comando:** Não permite o envio de comandos diretos ao hardware durante o processo.
*   **Dependência de Servidor:** Inoperante caso o servidor local/interno esteja indisponível.

---

## 2. HealthGo Studio (Plataforma de Monitoramento e Gestão)

O **HealthGo Studio** é a central de comando para o suporte técnico e sucesso do cliente, permitindo a gestão proativa do parque de dispositivos ativos.

### 📊 Função e Explicação
Funciona como o "painel de controle" de cada dispositivo em campo, oferecendo visibilidade total sobre:
*   **Telemetria:** Temperatura, umidade e status de funcionamento dos sensores em tempo real.
*   **Status Operacional:** Tempo de atividade (*uptime*), última conexão e versão de FW/SW.
*   **Gestão de Identidade:** Vínculo de HGID (Série Interno) com o Número de Série comercial e contatos responsáveis.
*   **Controle de Acesso:** Criação de usuários e gestão de permissões de acesso ao software.
*   **Documentação:** Acesso direto aos POPs (Procedimentos Operacionais Padrão) dos produtos.

### ⚙️ Requisitos Técnicos para Operação
*   **Navegador Moderno:** Chrome ou Edge atualizados para renderização dos dashboards.
*   **Nível de Acesso:** Credenciais configuradas para Níveis N2 ou N3.
*   **Conectividade:** Acesso à rede corporativa/VPN, dado que a ferramenta é dependente do servidor central.

### 🚧 Gargalos Identificados
*   **Ausência de Controle Remoto:** Não é possível alterar o firmware do aparelho ou enviar comandos (reset, calibração remota) pela interface.
*   **Rigidez de Dados:** Não há integração automática para atualização de dados de calibração vindo do Datalake.
*   **Vulnerabilidade de Infra:** Total dependência da estabilidade do servidor para operação fora da sede.

---

## 3. Impacto na Devolutiva para o Cliente

A eficiência destas ferramentas dita diretamente a qualidade da experiência do cliente HealthGo:

| Fator de Impacto | Descrição | Consequência da Falha |
| :--- | :--- | :--- |
| **SLA de Análise** | O Studio é a "caixa preta" que evita o recolhimento físico. | Se falha, o tempo de diagnóstico sobe de minutos para dias. |
| **Custos Logísticos** | Resolução remota via Studio/Client. | Falhas forçam o envio de equipamentos para a fábrica, elevando custos. |
| **Confiança Técnica** | Precisão dos dados de calibração via Client. | Modelos mal treinados geram resultados imprecisos nos exames clínicos. |
| **Agilidade de Suporte** | Monitoramento em tempo real. | Sem o Studio, o suporte torna-se reativo, aguardando a reclamação do cliente. |

---

> [!IMPORTANT]
> **Conclusão Estratégica:** A modernização para permitir **comandos remotos** e **atualização de firmware via Studio**, aliada à **automação do fluxo Datalake → Nocobase**, reduziria drasticamente o esforço manual do N3 e o tempo de resposta ao cliente final.
