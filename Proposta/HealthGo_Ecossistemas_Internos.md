# HealthGo - Ecossistemas Internos
## Revisão Técnica de Sistemas | Versão 2.0

**Data:** 11/05/2026  
**Empresa:** HealthGo  
**Objetivo:** Documentar os ecossistemas internos utilizados diariamente, com análise de requisitos técnicos, gargalos e impactos na devolutiva ao cliente.

---

## Índice
1. [HealthGoClient](#1-healthgoclient)
2. [HealthGoStudio](#2-healthgostudio)
3. [Resumo Executivo](#3-resumo-executivo)

---

## 1. HealthGoClient

### 1.1 Descrição e Função

**Tipo:** Software de Ponta (Cliente)

**Explicação:**  
Interface principal utilizada por médicos e técnicos em clínicas para a execução de exames de sopro em tempo real. O software atua como a ponte entre o hardware HealthGo Air e o profissional de saúde.

**Função Principal:**
- Realizar a coleta de dados dos sensores
- Visualização gráfica de curvas basais (H2/CH4)
- Geração de relatórios de exames
- Sincronização de dados com a nuvem
- Interface para calibração guiada

---

### 1.2 Requisitos Técnicos para Operação

| Categoria | Requisito |
|-----------|-----------|
| **Sistema Operacional** | Windows 10 ou 11 (x64) |
| **Porta USB** | USB 2.0/3.0 funcional |
| **Drivers** | FTDI/Silicon Labs (atualizados) |
| **Conectividade** | Internet estável para login e sincronização |
| **Hardware** | Dispositivo HealthGo Air com firmware compatível |

---

### 1.3 Maiores Gargalos na Utilização

| Gargalo | Descrição | Severidade |
|---------|-----------|------------|
| **Falhas de Reconhecimento** | Problemas frequentes de detecção na porta COM/USB | Alta |
| **Autenticação HTU** | Erros de segurança que impedem o reconhecimento do dispositivo | Alta |
| **Limites de Calibração** | Travamento após 250 exames ou 1 ano sem manutenção | Média |
| **Conflitos de Drivers** | Incompatibilidade com outros periféricos na mesma máquina | Média |

---

### 1.4 Impacto na Devolutiva ao Cliente

> **CRÍTICO:** A impossibilidade de conexão ou falha no software interrompe a agenda da clínica, causando:
> 
> - **Atrasos no atendimento** ao paciente
> - **Percepção de instabilidade técnica** do produto
> - **Perda de receita** da clínica por exames não realizados
> - **Desgaste no relacionamento** com o cliente

---

## 2. HealthGoStudio

### 2.1 Descrição e Função

**Tipo:** Plataforma Interna de Gestão e Suporte

**Explicação:**  
Ferramenta avançada de uso interno (Níveis N2 e N3) para monitoramento, diagnóstico e manutenção remota do parque de dispositivos ativos.

**Função Principal:**
- Análise profunda de logs técnicos
- Execução de SVM (Self Verification Mode)
- Atualização remota de firmware
- Verificação de offsets de sensores
- Auditoria de hardware em tempo real

---

### 2.2 Requisitos Técnicos para Operação

| Categoria | Requisito |
|-----------|-----------|
| **Navegador** | Google Chrome ou Microsoft Edge (versões recentes) |
| **Acesso** | Autenticação via SSO ou VPN da empresa |
| **Permissões** | Nível N2/N3 configurado no HubSpot/Database |
| **Conectividade** | Alta velocidade para visualização de logs volumosos |

---

### 2.3 Maiores Gargalos na Utilização

| Gargalo | Descrição | Severidade |
|---------|-----------|------------|
| **Latência de Dados** | Atraso na sincronização entre HealthGoClient e Studio | Alta |
| **Complexidade da Interface** | Curva de aprendizado elevada para novos analistas | Média |
| **Dependência de Status** | Comandos remotos exigem software cliente aberto | Alta |
| **Carga do Banco de Dados** | Lentidão ao buscar históricos de dispositivos antigos | Média |

---

### 2.4 Impacto na Devolutiva ao Cliente

> **ALTO IMPACTO:** Esta é a "caixa preta" que permite resolver problemas sem recolhimento físico do dispositivo. Quando o Studio falha ou apresenta lentidão:
> 
> - **Aumento do tempo de análise técnica**
> - **Impacto direto no SLA de devolução**
> - **Aumento de custos logísticos** (recolhimento/reparo físico)
> - **Maior tempo de espera** para resolução do problema pelo cliente

---

## 3. Resumo Executivo

### 3.1 Comparativo de Impactos

| Sistema | Criticidade | Impacto no Cliente | Tempo Médio de Impacto |
|---------|-------------|-------------------|----------------------|
| HealthGoClient | **CRÍTICA** | Interrupção imediata do atendimento | Imediato |
| HealthGoStudio | **ALTA** | Aumento do tempo de resolução | 2-24 horas |

### 3.2 Prioridades de Melhoria

1. **Urgente:** Resolver problemas de detecção USB/COM no HealthGoClient
2. **Urgente:** Reduzir latência de sincronização no HealthGoStudio
3. **Médio prazo:** Implementar alertas proativos de calibração
4. **Médio prazo:** Simplificar interface do HealthGoStudio para novos analistas

### 3.3 Recomendações

- **Monitoramento proativo:** Implementar alertas automáticos para limite de calibração (250 exames ou 12 meses)
- **Documentação de drivers:** Criar guia de instalação de drivers FTDI/Silicon Labs
- **Treinamento:** Desenvolver material de capacitação para HealthGoStudio
- **Escalabilidade:** Avaliar arquitetura do banco de dados para reduzir lentidão em históricos extensos

---

**Documento gerado em:** 11/05/2026  
**Versão:** 2.0  
**Status:** Ativo
