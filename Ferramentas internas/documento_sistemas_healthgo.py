"""
Documento de Análise de Sistemas Internos - HealthGoClient & HealthGoStudio
Versão: 1.0
Data: 2026-05-11
"""

import json
from datetime import datetime

documento = {
    "metadados": {
        "titulo": "Análise de Sistemas Internos - HealthGoClient & HealthGoStudio",
        "versao": "1.0",
        "data_criacao": "2026-05-11",
        "autores": ["Suporte Técnico N3"],
        "ultima_atualizacao": datetime.now().isoformat()
    },

    "resumo_executivo": {
        "objetivo": "Documentar os sistemas internos utilizados no suporte técnico, identificando fluxos, requisitos, gargalos e impactos na devolutiva ao cliente.",
        "sistemas_cobertos": ["HealthGoClient", "HealthGoStudio", "BLiP Desk", "HubSpot", "Netbird"],
        "tempo_medio_resolucao": "2 horas",
        "principais_desafios": [
            "Instabilidade de conexão e servidores",
            "Dependência de múltiplos serviços (Datalake, Nocobase, AWS)",
            "Processos manuais de liberação",
            "Falta de visibilidade em tempo real"
        ]
    },

    "sistemas": {
        "HealthGoClient": {
            "descricao": "Aplicativo desktop para visualização e gerenciamento de dados dos equipamentos HealthGo.",
            "tipo": "Desktop Application",
            "usuarios": ["Suporte Técnico N2", "Suporte Técnico N3"],

            "funcionalidades": {
                "visao_geral": "Plataforma principal para gerenciamento de equipamentos e dados dos clientes",
                "modulos": [
                    {
                        "nome": "Banco de Dados",
                        "descricao": "Conexão com AWS/Nocobase para armazenamento de todos os dados",
                        "dados_disponiveis": [
                            "Parâmetros ideais dos sensores vs valores reais",
                            "Versões de software",
                            "Versões de firmware",
                            "Tempo de funcionamento total",
                            "Tempo ligado",
                            "Dados em tempo real",
                            "Histórico de calibragem"
                        ]
                    },
                    {
                        "nome": "Gestão de Usuários",
                        "descricao": "Criação e controle de permissões de acesso ao software",
                        "recursos": [
                            "Cadastro de usuários",
                            "Níveis de permissão",
                            "Controle de acesso por HGID"
                        ]
                    },
                    {
                        "nome": "Sincronização Datalake ↔ Nocobase",
                        "descricao": "Upload de HGIDs e dados entre os ambientes",
                        "fluxo": "Datalake → Nocobase → Cliente"
                    }
                ]
            },

            "requisitos_tecnicos": {
                "sistema_operacional": {
                    "suportado": ["Windows 10", "Windows 11"],
                    "observacao": "Apenas SO Windows, a partir do Windows 10"
                },
                "rede": {
                    "ferramenta": "Netbird",
                    "descricao": "VPN interna para conexão segura com o servidor",
                    "obrigatorio": True,
                    "configuracao": "Conexão ativa com URL do servidor HealthGo"
                },
                "hardware_recomendado": {
                    "processador": "Intel Core i5 ou superior",
                    "memoria_ram": "8GB ou superior",
                    "disco": "256GB SSD ou superior",
                    "rede": "Conexão estável com a VPN Netbird"
                }
            },

            "integracoes": [
                {
                    "sistema": "Netbird",
                    "tipo": "VPN/Conexão de Rede",
                    "funcao": "Estabelecer túnel seguro com o servidor interno"
                },
                {
                    "sistema": "Nocobase",
                    "tipo": "Banco de Dados",
                    "funcao": "Armazenamento principal de dados e SVMs"
                },
                {
                    "sistema": "AWS",
                    "tipo": "Infraestrutura em Nuvem",
                    "funcao": "Host do Nocobase e dados complementares"
                },
                {
                    "sistema": "Datalake",
                    "tipo": "Repositório de Dados",
                    "funcao": "Armazenamento intermediário de dados de equipamentos"
                }
            ],

            "gargalos": [
                {
                    "id": "GC-001",
                    "titulo": "Dependência do Datalake para Upload de HGID",
                    "descricao": "Se a Produção não subir o HGID para o Datalake, não é possível emitir SVM ou subir para o Nocobase",
                    "severidade": "Alta",
                    "impacto": "Bloqueio total do fluxo de atendimento quando HGID não está disponível",
                    "causa_raiz": "Processo dependente de sistema externo (Produção)",
                    "workaround_atual": "Verificar manualmente se HGID foi carregado pela Produção"
                },
                {
                    "id": "GC-002",
                    "titulo": "Instabilidade por Queda do Servidor",
                    "descricao": "Quando o servidor principal cai, o HealthGoClient também fica indisponível",
                    "severidade": "Alta",
                    "impacto": "Impossibilidade de acessar dados e emitir SVMs durante outages",
                    "causa_raiz": "Alta dependência de conectividade com servidor central",
                    "workaround_atual": "Aguardar restabelecimento do servidor"
                },
                {
                    "id": "GC-003",
                    "titulo": "Sincronização entre GoClient e GoStudio",
                    "descricao": "HGID pode subir pelo GoClient para o Nocobase, mas não fica acessível no GoStudio até liberação manual",
                    "severidade": "Média",
                    "impacto": "Atraso no atendimento por dependência de analista para liberar acesso",
                    "causa_raiz": "Processo de sincronização com etapa de aprovação manual",
                    "workaround_atual": "Solicitar liberação manual ao analista responsável"
                },
                {
                    "id": "GC-004",
                    "titulo": "Falta de Controle Remoto de Componentes",
                    "descricao": "Não há funcionalidade de bypass remoto para componentes do equipamento",
                    "severidade": "Média",
                    "impacto": "Limitação no atendimento remoto, forçando visitas técnicas",
                    "causa_raiz": "Recurso não implementado na plataforma",
                    "workaround_atual": "Encaminhar para suporte presencial"
                }
            ],

            "impacto_devolutiva_cliente": {
                "indicadores": [
                    {
                        "metrica": "Tempo de resolução médio",
                        "valor": "2 horas",
                        "dentro_sla": True,
                        "observacao": "Meta atingida quando todos os sistemas estão operacionais"
                    },
                    {
                        "metrica": "Tickets escalados para N3",
                        "proporcao": "Menor que 30%",
                        "descricao": "Maioria dos problemas resolvidos no N2"
                    },
                    {
                        "metrica": "Problemas por hardware",
                        "proporcao": "Estimado 15-20%",
                        "descricao": "Principal fonte de retrabalho"
                    }
                ],
                "fatores_impacto": [
                    "Instabilidades causam atrasos variáveis e imprevisíveis",
                    "Processos manuais de liberação adicionam tempo ao atendimento",
                    "Dependência externa (Produção/Datalake) pode bloquear fluxo",
                    "Problemas de hardware não são resolvidos remotamente"
                ]
            }
        },

        "HealthGoStudio": {
            "descricao": "Ferramenta desktop para análise técnica avançada, compilação de firmware e emissão de SVMs.",
            "tipo": "Desktop Application",
            "usuarios": ["Suporte Técnico N3", "Engenharia"],

            "funcionalidades": {
                "visao_geral": "Ambiente de análise profunda e manipulação técnica de equipamentos",
                "modulos": [
                    {
                        "nome": "Gestão de SVM",
                        "descricao": "Upload de SVM do Datalake para Nocobase e emissão para instalação no cliente",
                        "fluxo": [
                            "1. Selecionar SVM no Datalake",
                            "2. Fazer upload para Nocobase",
                            "3. Emitir SVM para instalação no equipamento"
                        ],
                        "obs_importante": "Depende do HGID estar disponível no Datalake (bloqueio do GoClient)"
                    },
                    {
                        "nome": "Análise de Calibração",
                        "descricao": "Processamento de dados de calibração dos sensores",
                        "recursos": [
                            "Importação de dados de calibração",
                            "Processamento de dados",
                            "Remoção de outliers",
                            "Comparação de valores ideais vs reais"
                        ]
                    },
                    {
                        "nome": "Modelo SVM",
                        "descricao": "Treinamento e revisão de modelos de Support Vector Machine",
                        "recursos": [
                            "Treinar novo modelo SVM",
                            "Revisar predições",
                            "Visualização de gráficos"
                        ]
                    },
                    {
                        "nome": "Análise de Sensores",
                        "descricao": "Análise técnica detalhada de dados dos sensores",
                        "dados_analisados": [
                            "Logs em tempo real",
                            "Tensão nominal dos sensores",
                            "Média variável de cada sensor",
                            "Razão Rs/R0",
                            "Comparativo entre sensores"
                        ]
                    },
                    {
                        "nome": "Compilação de Firmware",
                        "descricao": "Compilação e envio de firmware para dispositivos",
                        "fluxo": [
                            "1. Selecionar dispositivo",
                            "2. Compilar firmware",
                            "3. Enviar para Nocobase/AWS"
                        ],
                        "restricao": "Versões de firmware para teste requerem solicitação a engenheiro responsável"
                    }
                ]
            },

            "requisitos_tecnicos": {
                "sistema_operacional": {
                    "suportado": ["Windows 10", "Windows 11"],
                    "observacao": "Apenas SO Windows, a partir do Windows 10"
                },
                "rede": {
                    "ferramenta": "Netbird",
                    "descricao": "VPN interna para conexão segura com o servidor",
                    "obrigatorio": True,
                    "configuracao": "Conexão ativa com URL do servidor HealthGo"
                },
                "hardware_recomendado": {
                    "processador": "Intel Core i7 ou superior (para compilação)",
                    "memoria_ram": "16GB ou superior (para processamento de dados)",
                    "disco": "512GB SSD ou superior",
                    "rede": "Conexão estável com a VPN Netbird"
                },
                "dependencias": {
                    "datalake": "Obrigatório para acessar SVMs",
                    "nocobase": "Obrigatório para sincronização de dados",
                    "aws": "Para upload de firmwares compilados"
                }
            },

            "integracoes": [
                {
                    "sistema": "Netbird",
                    "tipo": "VPN/Conexão de Rede",
                    "funcao": "Estabelecer túnel seguro com o servidor interno"
                },
                {
                    "sistema": "Datalake",
                    "tipo": "Repositório de Dados",
                    "funcao": "Acesso a SVMs e dados intermediários"
                },
                {
                    "sistema": "Nocobase",
                    "tipo": "Banco de Dados",
                    "funcao": "Sincronização de SVMs e dados de equipamentos"
                },
                {
                    "sistema": "AWS",
                    "tipo": "Infraestrutura em Nuvem",
                    "funcao": "Host para firmwares compilados"
                }
            ],

            "gargalos": [
                {
                    "id": "GS-001",
                    "titulo": "Dashboard em Tempo Real com Dados Imprecisos",
                    "descricao": "A dashboard em tempo real envia dados com baixa precisão e velocidade, frequentemente fica offline",
                    "severidade": "Alta",
                    "impacto": "Dificulta análise em tempo real e tomada de decisão durante atendimento",
                    "causa_raiz": "Instabilidade no serviço de streaming de dados",
                    "workaround_atual": "Verificar manualmente via outros serviços quando está offline"
                },
                {
                    "id": "GS-002",
                    "titulo": "Bloqueio de Firmware para Teste",
                    "descricao": "Versões de firmware para teste não podem ser alteradas pelo N3, requer solicitação a engenheiro responsável",
                    "severidade": "Média",
                    "impacto": "Atraso no atendimento por dependência de outro setor",
                    "causa_raiz": "Restrição de segurança/controle de versões",
                    "workaround_atual": "Abrir solicitação formal para Engenharia"
                },
                {
                    "id": "GS-003",
                    "titulo": "Falta de Visibilidade de Manutenção Preventiva",
                    "descricao": "Não é possível visualizar o número de exames realizados por HGID para acompanhar prazo de manutenção preventiva",
                    "severidade": "Média",
                    "impacto": "Risco de não cumprimento do prazo de 250 exames ou 1 ano para manutenção",
                    "causa_raiz": "Funcionalidade não implementada",
                    "solicitacao_pendente": "Exibir contador de exames por HGID com alerta de manutenção"
                },
                {
                    "id": "GS-004",
                    "titulo": "Sincronização com HealthGoClient",
                    "descricao": "HGIDs sincronizados no GoClient podem não aparecer imediatamente no GoStudio",
                    "severidade": "Baixa",
                    "impacto": "Pequeno delay na disponibilização de dados",
                    "causa_raiz": "Processo de replicação entre sistemas",
                    "workaround_atual": "Aguardar sincronização ou solicitar liberação manual"
                }
            ],

            "impacto_devolutiva_cliente": {
                "indicadores": [
                    {
                        "metrica": "Tickets resolvidos pelo N3",
                        "proporcao": "Menos de 30% do total",
                        "descricao": "Maioria dos casos resolvidos no N2"
                    },
                    {
                        "metrica": "Casos que requerem engenharia",
                        "proporcao": "Estimado 10%",
                        "descricao": "Principalmente para mudanças de firmware de teste"
                    }
                ],
                "fatores_impacto": [
                    "Dados imprecisos em tempo real dificultam diagnóstico",
                    "Restrições de firmware atrasam testes de solução",
                    "Falta de contador de exames impede previsão de manutenções",
                    "Análise precisa depende de estabilidade da dashboard"
                ]
            }
        },

        "BLiP_Desk": {
            "descricao": "Plataforma de chat e comunicação com clientes",
            "funcao_no_fluxo": "Ponto de entrada do chamado do cliente",

            "requisitos_tecnicos": {
                "acesso": "Web (navegador)",
                "integracao": "API com HubSpot"
            },

            "integracoes": [
                {
                    "sistema": "HubSpot",
                    "tipo": "CRM/Ticketing",
                    "funcao": "Geração automática de ticket na pipeline de Suporte"
                }
            ],

            "gargalos": [
                {
                    "id": "BL-001",
                    "titulo": "Integração com HubSpot",
                    "descricao": "A integração já funciona automaticamente, mas tickets podem ficar pendentes se não categorizados",
                    "severidade": "Baixa",
                    "impacto": "Possível delay na triagem inicial",
                    "workaround_atual": "N1 responsável por triagem imediata"
                }
            ]
        },

        "HubSpot": {
            "descricao": "CRM para registro e gestão de tickets de suporte",
            "funcao_no_fluxo": "Centralização de tickets e acompanhamento do ciclo de vida do atendimento",

            "pipeline": {
                "nome": "Pipeline de Suporte",
                "estagios": [
                    "Recebido (via BLiP)",
                    "Triagem N1",
                    "Atendimento N2",
                    "Análise N3",
                    "Resolvido",
                    "Fechado"
                ]
            },

            "requisitos_tecnicos": {
                "acesso": "Web (navegador)",
                "integracao": "Recebe tickets do BLiP Desk"
            },

            "gargalos": [
                {
                    "id": "HS-001",
                    "titulo": "Categorização de Tickets",
                    "descricao": "N1 precisa categorizar corretamente para direcionamento adequado",
                    "severidade": "Média",
                    "impacto": "Escalonamentos incorretos podem atrasar resolução",
                    "workaround_atual": "Treinamento contínuo da equipe N1"
                }
            ]
        },

        "Netbird": {
            "descricao": "Solução de VPN para conexão de rede interna",
            "funcao_no_fluxo": "Estabelecer conexão segura entre as máquinas do suporte e os servidores internos",

            "requisitos_tecnicos": {
                "instalacao": "Obrigatória em todas as máquinas de suporte",
                "configuracao": "Conexão ativa com URL do servidor HealthGo",
                "sistema_operacional": "Windows 10+"
            },

            "integracoes": [
                {
                    "sistema": "HealthGoClient",
                    "funcao": "Conexão com servidor e Nocobase"
                },
                {
                    "sistema": "HealthGoStudio",
                    "funcao": "Conexão com servidor, Datalake e Nocobase"
                }
            ],

            "gargalos": [
                {
                    "id": "NB-001",
                    "titulo": "Dependência de Conexão VPN",
                    "descricao": "Todas as ferramentas dependem do Netbird estar conectado",
                    "severidade": "Alta",
                    "impacto": "Sem VPN, impossível acessar qualquer sistema interno",
                    "causa_raiz": "Arquitetura dependente de VPN",
                    "workaround_atual": "Verificar conexão Netbird como primeiro passo de troubleshooting"
                }
            ]
        }
    },

    "fluxo_de_atendimento": {
        "descricao": "Fluxo completo desde o reporte do cliente até a resolução",

        "etapas": [
            {
                "ordem": 1,
                "nome": "Reporte do Cliente",
                "sistema": "BLiP Desk",
                "responsavel": "Cliente",
                "descricao": "Cliente reporta problema via chat no BLiP",
                "saida": "Ticket criado automaticamente no HubSpot"
            },
            {
                "ordem": 2,
                "nome": "Triagem N1",
                "sistema": "HubSpot",
                "responsavel": "Suporte N1",
                "descricao": "N1 categoriza o ticket e faz triagem inicial",
                "acao": "Classificar tipo de problema e direcionar para N2"
            },
            {
                "ordem": 3,
                "nome": "Análise N2",
                "sistema": "HealthGoStudio",
                "responsavel": "Suporte N2",
                "descricao": "N2 analisa calibração dos sensores",
                "acao": "Emitir SVM para reset dos sensores se necessário",
                "desvio": "Se não resolver, abre sub-chamado para N3"
            },
            {
                "ordem": 4,
                "nome": "Análise N3",
                "sistema": "HealthGoStudio",
                "responsavel": "Suporte N3",
                "descricao": "Análise técnica avançada com gráficos e predições",
                "acao": "Verificar logs em tempo real, tensão nominal, Rs/R0, comparativos",
                "desvio": "Se problema de hardware, escalar para equipe de campo"
            },
            {
                "ordem": 5,
                "nome": "Resolução",
                "sistema": "Múltiplos",
                "responsavel": "N2/N3",
                "descricao": "Aplicar solução e validar com cliente",
                "acao": "Fechar ticket no HubSpot"
            }
        ],

        "pontos_de_bloqueio": [
            {
                "local": "Etapa 3 - Análise N2",
                "bloqueio": "HGID não disponível no Datalake",
                "acao_corrigiva": "Verificar com equipe de Produção"
            },
            {
                "local": "Etapa 4 - Análise N3",
                "bloqueio": "HGID não sincronizado entre GoClient e GoStudio",
                "acao_corrigiva": "Solicitar liberação manual"
            },
            {
                "local": "Etapa 4 - Análise N3",
                "bloqueio": "Firmware de teste necessário",
                "acao_corrigiva": "Solicitar a Engenharia"
            }
        ]
    },

    "analise_de_gargalos_globais": {
        "resumo": "Os principais gargalos identificados afetam o fluxo de atendimento em três fronts: infraestrutura, processos manuais e visibilidade de dados.",

        "por_categoria": {
            "infraestrutura": [
                {
                    "gargalo": "Instabilidade do servidor",
                    "impacto": "Todos os sistemas ficam indisponíveis",
                    "prioridade": "Crítica",
                    "recomendacao": "Implementar redundância de servidores e backup de conectividade"
                },
                {
                    "gargalo": "Dashboard em tempo real imprecisa",
                    "impacto": "Dificulta diagnóstico durante atendimento",
                    "prioridade": "Alta",
                    "recomendacao": "Revisar arquitetura de streaming e implementar heartbeat/monitoring"
                },
                {
                    "gargalo": "Dependência Netbird",
                    "impacto": "Sem VPN, todo o fluxo para",
                    "prioridade": "Alta",
                    "recomendacao": "Documentar troubleshooting de VPN e criar alternativas de contingência"
                }
            ],
            "processos_manuais": [
                {
                    "gargalo": "Liberação manual de HGIDs",
                    "impacto": "Adiciona tempo ao atendimento",
                    "prioridade": "Média",
                    "recomendacao": "Automatizar sincronização ou reduzir etapas de aprovação"
                },
                {
                    "gargalo": "Solicitação de firmware para Engenharia",
                    "impacto": "Delay quando N3 precisa testar diferentes versões",
                    "prioridade": "Média",
                    "recomendacao": "Avaliar se N3 pode ter acesso controlado a firmwares de teste"
                }
            ],
            "visibilidade_de_dados": [
                {
                    "gargalo": "Falta de contador de exames por HGID",
                    "impacto": "Impossível prever necessidade de manutenção preventiva",
                    "prioridade": "Média",
                    "recomendacao": "Implementar dashboard de manutenção com alertas de 250 exames ou 1 ano"
                },
                {
                    "gargalo": "Sincronização GoClient ↔ GoStudio",
                    "impacto": "Inconsistência de dados entre ferramentas",
                    "prioridade": "Baixa",
                    "recomendacao": "Revisar processo de replicação"
                }
            ]
        },

        "impacto_no_sla": {
            "tempo_medio_atual": "2 horas",
            "fatores_que_ameacam_sla": [
                "Quedas de servidor",
                "HGID não disponível no Datalake",
                "Firmware de teste não liberado",
                "Problemas de hardware (não resolvidos remotamente)"
            ],
            "disponibilidade_ideal": "99.5%",
            "disponibilidade_atual_estimada": "95%"
        }
    },

    "melhorias_solicitadas": {
        "prioridade_alta": [
            {
                "titulo": "Contador de Exames por HGID",
                "descricao": "Exibir número de exames realizados em cada HGID selecionado",
                "regra_negocio": "Manutenção preventiva a cada 250 exames ou 1 ano",
                "beneficio": "Proatividade na identificação de equipamentos que precisam de manutenção"
            },
            {
                "titulo": "Bypass Remoto de Componentes",
                "descricao": "Funcionalidade para enviar comandos de bypass aos componentes remotamente",
                "beneficio": "Redução de visitas técnicas e aumento de resolução remota"
            }
        ],
        "prioridade_media": [
            {
                "titulo": "Melhoria na Dashboard em Tempo Real",
                "descricao": "Aumentar precisão e velocidade dos dados em tempo real",
                "beneficio": "Diagnósticos mais assertivos durante atendimentos"
            },
            {
                "titulo": "Automação de Liberação de HGIDs",
                "descricao": "Sincronização automática entre GoClient e GoStudio",
                "beneficio": "Eliminação de etapa manual"
            },
            {
                "titulo": "Acesso a Firmwares de Teste",
                "descricao": "Permitir que N3 altere versões de firmware para testes",
                "beneficio": "Autonomia para testes sem depender de Engenharia"
            }
        ],
        "prioridade_baixa": [
            {
                "titulo": "Monitoramento de Serviços",
                "descricao": "Dashboard unificada mostrando status de todos os serviços (Datalake, Nocobase, AWS, Netbird)",
                "beneficio": "Identificação rápida de qual serviço está com problema"
            }
        ]
    },

    "requisitos_tecnicos_consolidados": {
        "sistemas_operacionais": {
            "HealthGoClient": ["Windows 10", "Windows 11"],
            "HealthGoStudio": ["Windows 10", "Windows 11"],
            "BLiP Desk": ["Windows 10", "Windows 11", "macOS", "Linux"],
            "HubSpot": ["Windows 10", "Windows 11", "macOS", "Linux"],
            "Netbird": ["Windows 10", "Windows 11"]
        },
        "ferramentas_obrigatorias": ["Netbird (VPN)", "Navegador Web atualizado"],
        "conectividade": [
            "VPN Netbird ativa com conexão ao servidor HealthGo",
            "Acesso à internet para BLiP e HubSpot",
            "Latência recomendada: < 100ms para servidores internos"
        ],
        "permissões": [
            "Acesso ao Nocobase",
            "Acesso ao Datalake",
            "Acesso à AWS (para firmwares)",
            "Permissão de escrita para emissão de SVMs"
        ]
    },

    " glossario": {
        "termos": [
            {"termo": "HGID", "definicao": "Número de série interno do aparelho HealthGo"},
            {"termo": "SVM", "definicao": "Software Version Manager - Arquivo de atualização de software/firmware do equipamento"},
            {"termo": "Nocobase", "definicao": "Sistema de gerenciamento de banco de dados interno da HealthGo"},
            {"termo": "Datalake", "definicao": "Repositório de dados intermediário para equipamentos"},
            {"termo": "Rs/R0", "definicao": "Razão entre resistência do sensor e resistência de referência (métrica de calibração)"},
            {"termo": "Outliers", "definicao": "Dados atípicos ou fora do padrão esperado nos sensores"},
            {"termo": "SVM Model", "definicao": "Support Vector Machine - Modelo de machine learning para análise de padrões"},
            {"termo": "Netbird", "definicao": "Solução de VPN para conexão de rede privada"},
            {"termo": "Calibração", "definicao": "Processo de ajuste dos sensores para valores de referência"},
            {"termo": "Firmware", "definicao": "Software embutido no hardware do equipamento"}
        ]
    }
}

# Salvar como JSON
output_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas\documento_sistemas_healthgo.json"
with open(output_path, 'w', encoding='utf-8') as f:
    json.dump(documento, f, ensure_ascii=False, indent=2)

print(f"[OK] Documento JSON gerado com sucesso!")
print(f"[FILE] Local: {output_path}")
print(f"[SIZE] Tamanho: {len(json.dumps(documento, ensure_ascii=False))} caracteres")
