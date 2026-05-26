"""
Gerador de PDF - Relatório de Sistemas Internos
"""
import json
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import cm
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, PageBreak, Table, TableStyle
from reportlab.lib.colors import HexColor, white, black
from reportlab.lib.enums import TA_LEFT, TA_CENTER, TA_JUSTIFY
from reportlab.pdfgen import canvas
import os

# Cores tema escuro
FUNDO_PRETO = HexColor('#1a1a1a')
TEXTO_CLARO = HexColor('#e0e0e0')
TITULO_COR = HexColor('#4CAF50')
SECAO_COR = HexColor('#FF9800')
SUBTITULO_COR = HexColor('#00d4ff')
AVISO_COR = HexColor('#ef4444')
MEDIA_COR = HexColor('#3b82f6')

def create_styles():
    styles = getSampleStyleSheet()

    styles.add(ParagraphStyle(name='TituloPrincipal', parent=styles['Heading1'], fontSize=24, textColor=TITULO_COR, spaceAfter=15, alignment=TA_CENTER, fontName='Helvetica-Bold'))
    styles.add(ParagraphStyle(name='Subtitulo', parent=styles['Normal'], fontSize=12, textColor=TEXTO_CLARO, spaceAfter=8, alignment=TA_CENTER, fontName='Helvetica'))
    styles.add(ParagraphStyle(name='Secao', parent=styles['Heading1'], fontSize=16, textColor=SECAO_COR, spaceBefore=20, spaceAfter=10, fontName='Helvetica-Bold'))
    styles.add(ParagraphStyle(name='SubSecao', parent=styles['Heading2'], fontSize=12, textColor=SUBTITULO_COR, spaceBefore=12, spaceAfter=8, fontName='Helvetica-Bold'))
    styles.add(ParagraphStyle(name='TextoNormal', parent=styles['Normal'], fontSize=10, textColor=TEXTO_CLARO, spaceAfter=6, fontName='Helvetica', leading=14))
    styles.add(ParagraphStyle(name='BulletItem', parent=styles['Normal'], fontSize=10, textColor=TEXTO_CLARO, leftIndent=15, spaceAfter=4, fontName='Helvetica', leading=14))
    styles.add(ParagraphStyle(name='GargaloAlto', parent=styles['Normal'], fontSize=10, textColor=AVISO_COR, leftIndent=10, spaceAfter=6, fontName='Helvetica-Bold', leading=14))
    styles.add(ParagraphStyle(name='GargaloMedio', parent=styles['Normal'], fontSize=10, textColor=MEDIA_COR, leftIndent=10, spaceAfter=6, fontName='Helvetica-Bold', leading=14))
    styles.add(ParagraphStyle(name='Creditos', parent=styles['Normal'], fontSize=10, textColor=TEXTO_CLARO, alignment=TA_CENTER, spaceAfter=6, fontName='Helvetica-Oblique'))
    return styles

def page_template(canvas, doc):
    canvas.saveState()
    canvas.setFillColor(HexColor('#0a0e17'))
    canvas.rect(0, 0, A4[0], A4[1], fill=1, stroke=0)
    canvas.restoreState()

def gerar_pdf():
    base_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas"
    output_path = os.path.join(base_path, "documento_sistemas_healthgo.pdf")

    styles = create_styles()

    doc = SimpleDocTemplate(output_path, pagesize=A4, rightMargin=2*cm, leftMargin=2*cm, topMargin=2*cm, bottomMargin=2*cm)

    story = []

    # CAPA
    story.append(Spacer(1, 4*cm))
    story.append(Paragraph("ANALISE DE SISTEMAS INTERNOS", styles['TituloPrincipal']))
    story.append(Paragraph("HealthGoClient & HealthGoStudio", styles['Subtitulo']))
    story.append(Spacer(1, 2*cm))
    story.append(Paragraph("Ferramentas de Suporte Tecnico", styles['Subtitulo']))
    story.append(Spacer(1, 4*cm))
    story.append(Paragraph("Versao 1.0 | 11/05/2026", styles['Creditos']))
    story.append(Spacer(1, 2*cm))
    story.append(Paragraph("_" * 40, styles['Creditos']))
    story.append(Spacer(1, 1*cm))
    story.append(Paragraph("Desenvolvido por:", styles['Creditos']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("<b>Andre Luiz de Souza</b>", styles['Creditos']))
    story.append(Paragraph("Especialista de Suporte Pleno N3", styles['Creditos']))
    story.append(PageBreak())

    # RESUMO
    story.append(Paragraph("RESUMO EXECUTIVO", styles['Secao']))
    story.append(Paragraph("Este documento apresenta a analise dos sistemas internos HealthGoClient e HealthGoStudio, ferramentas essenciais para o suporte tecnico da HealthGo.", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("<b>Sistemas documentados:</b> 2", styles['BulletItem']))
    story.append(Paragraph("<b>Gargalos identificados:</b> 6", styles['BulletItem']))
    story.append(Paragraph("<b>Tempo medio de resolucao:</b> 2 horas", styles['BulletItem']))
    story.append(PageBreak())

    # HEALTHGOCLIENT
    story.append(Paragraph("HEALTHGOCLIENT", styles['Secao']))
    story.append(Paragraph("<b>Tipo:</b> Desktop Application - Suporte", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("EXPLICACAO", styles['SubSecao']))
    story.append(Paragraph("Aplicativo desktop utilizado pelos especialistas de Suporte. Vinculado ao servidor interno da HealthGo com conexao ao banco de dados Nocobase/AWS e Datalake para envio de SVM ao HealthGo Studio.", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("REQUISITOS TECNICOS", styles['SubSecao']))
    story.append(Paragraph("- Sistema Operacional: Windows 10 ou superior", styles['BulletItem']))
    story.append(Paragraph("- Rede: Netbird VPN conectado ao SV HealthGo", styles['BulletItem']))
    story.append(Paragraph("- Processador: Intel Core i5 ou superior", styles['BulletItem']))
    story.append(Paragraph("- Memoria RAM: 8GB", styles['BulletItem']))
    story.append(Paragraph("- Disco: 256GB SSD", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("MAIORES GARGALOS", styles['SubSecao']))
    story.append(Paragraph("[ALTA] Instabilidade por Queda do Servidor - Quando o servidor principal cai, o HealthGoClient tambem fica indisponivel", styles['GargaloAlto']))
    story.append(Paragraph("[ALTA] Dependencia do Datalake para Upload de HGID - Se a Producao nao subir o HGID para o Datalake, nao e possivel emitir SVM", styles['GargaloAlto']))
    story.append(Paragraph("[MEDIA] Sincronizacao com HealthGoStudio - HGIDs sincronizados podem nao aparecer imediatamente ate liberacao manual", styles['GargaloMedio']))
    story.append(PageBreak())

    # HEALTHGOSTUDIO
    story.append(Paragraph("HEALTHGOSTUDIO", styles['Secao']))
    story.append(Paragraph("<b>Tipo:</b> Ferramenta Interna - Suporte N2/N3", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("EXPLICACAO", styles['SubSecao']))
    story.append(Paragraph("Ferramenta de Suporte para uso operacional do N2 e N3. Vinculado ao servidor interno da HealthGo com conexao ao banco de dados Nocobase/AWS para armazenamento de todos os dados dos equipamentos.", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("FUNCOES PRINCIPAIS", styles['SubSecao']))
    story.append(Paragraph("- Gestao de SVM: Subir SVM do Datalake para Nocobase e emitir para instalacao", styles['BulletItem']))
    story.append(Paragraph("- Importar Dados de Calibracao: Processar dados dos sensores", styles['BulletItem']))
    story.append(Paragraph("- Processamento de Dados: Remocao de outliers, analise estatistica", styles['BulletItem']))
    story.append(Paragraph("- Modelo SVM: Treinar novo modelo, revisar predicoes e graficos", styles['BulletItem']))
    story.append(Paragraph("- Compilacao de Firmware: Compilar e enviar para Nocobase/AWS", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("REQUISITOS TECNICOS", styles['SubSecao']))
    story.append(Paragraph("- Sistema Operacional: Windows 10 ou superior", styles['BulletItem']))
    story.append(Paragraph("- Rede: Netbird VPN conectado ao SV HealthGo", styles['BulletItem']))
    story.append(Paragraph("- Processador: Intel Core i5 ou superior", styles['BulletItem']))
    story.append(Paragraph("- Memoria RAM: 8GB", styles['BulletItem']))
    story.append(Paragraph("- Disco: 256GB SSD", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("MAIORES GARGALOS", styles['SubSecao']))
    story.append(Paragraph("[ALTA] Dashboard em Tempo Real com Dados Imprecisos - Frequentemente fica offline", styles['GargaloAlto']))
    story.append(Paragraph("[MEDIA] Falta de Visibilidade de Manutencao Preventiva - Nao e possivel visualizar exames por HGID", styles['GargaloMedio']))
    story.append(Paragraph("[MEDIA] Falta de Controle Remoto de Componentes - Nao ha funcionalidade de bypass remoto", styles['GargaloMedio']))
    story.append(PageBreak())

    # REQUISITOS TECNICOS
    story.append(Paragraph("REQUISITOS TECNICOS PARA OPERAR", styles['Secao']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("HARDWARE E SISTEMA OPERACIONAL", styles['SubSecao']))
    story.append(Paragraph("- Sistema Operacional: Windows 10/11", styles['BulletItem']))
    story.append(Paragraph("- Processador: Intel Core i5 (min) / i7 (rec)", styles['BulletItem']))
    story.append(Paragraph("- Memoria RAM: 8GB (min) / 16GB (rec)", styles['BulletItem']))
    story.append(Paragraph("- Armazenamento: 256GB SSD (min) / 512GB SSD (rec)", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("FERRAMENTAS OBRIGATORIAS", styles['SubSecao']))
    story.append(Paragraph("- Netbird VPN: Conexao obligatoria com servidor HealthGo", styles['BulletItem']))
    story.append(Paragraph("- Navegador: Google Chrome ou Microsoft Edge", styles['BulletItem']))
    story.append(Paragraph("- Acesso a Internet: Para BLiP Desk e HubSpot", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("PERMISSOES E ACESSOS", styles['SubSecao']))
    story.append(Paragraph("- Nocobase: Acesso completo para gestao de dados e SVMs", styles['BulletItem']))
    story.append(Paragraph("- Datalake: Leitura e escrita para upload de HGIDs", styles['BulletItem']))
    story.append(Paragraph("- AWS: Upload de firmwares compilados", styles['BulletItem']))
    story.append(Paragraph("- HubSpot: Nivel N2/N3 para gestao de tickets", styles['BulletItem']))
    story.append(Spacer(1, 1*cm))
    story.append(Paragraph("_" * 40, styles['Creditos']))
    story.append(Spacer(1, 0.5*cm))
    story.append(Paragraph("HealthGo - Analise de Sistemas Internos | v1.0", styles['Creditos']))
    story.append(Paragraph("Andre Luiz de Souza - Especialista de Suporte Pleno N3", styles['Creditos']))

    doc.build(story, onFirstPage=page_template, onLaterPages=page_template)
    print(f"[OK] PDF gerado com sucesso!")
    print(f"[FILE] Local: {output_path}")

if __name__ == "__main__":
    gerar_pdf()
