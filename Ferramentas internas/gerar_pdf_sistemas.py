"""
Gerador de PDF - Documento de Análise de Sistemas Internos
HealthGoClient & HealthGoStudio
"""

import json
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import cm
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, PageBreak, KeepTogether
from reportlab.lib.colors import HexColor, white, black, Color
from reportlab.lib.enums import TA_LEFT, TA_CENTER, TA_JUSTIFY
from reportlab.pdfgen import canvas as pdfcanvas
from reportlab.platypus import Flowable
import os

# Cores - tema escuro
FUNDO_PRETO = HexColor('#1a1a1a')
TEXTO_CLARO = HexColor('#e0e0e0')
TITULO_COR = HexColor('#4CAF50')
SUBTITULO_COR = HexColor('#2196F3')
SECAO_COR = HexColor('#FF9800')
AVISO_COR = HexColor('#f44336')
BRANCO = HexColor('#ffffff')
FUNDO_CARD = HexColor('#2d2d2d')

class FundoEscuro(Flowable):
    """Flowable que desenha um fundo escuro na página"""
    def __init__(self, width, height):
        Flowable.__init__(self)
        self.width = width
        self.height = height

    def draw(self):
        pass  # Não desenha nada, o fundo é adicionado na página

class DarkPageCanvas(pdfcanvas.Canvas):
    """Canvas que adiciona fundo escuro em cada página"""

    def __init__(self, *args, **kwargs):
        pdfcanvas.Canvas.__init__(self, *args, **kwargs)
        self._page_number = 0

    def drawBackground(self):
        self.setFillColor(FUNDO_PRETO)
        self.rect(0, 0, A4[0], A4[1], fill=1, stroke=0)

    def showPage(self):
        self.drawBackground()
        pdfcanvas.Canvas.showPage(self)

    def save(self):
        self.drawBackground()
        pdfcanvas.Canvas.showPage(self)
        pdfcanvas.Canvas.save(self)

def create_styles():
    """Cria estilos customizados"""
    styles = getSampleStyleSheet()

    styles.add(ParagraphStyle(
        name='TituloPrincipal',
        parent=styles['Heading1'],
        fontSize=28,
        textColor=TITULO_COR,
        spaceAfter=20,
        alignment=TA_CENTER,
        fontName='Helvetica-Bold'
    ))

    styles.add(ParagraphStyle(
        name='Subtitulo',
        parent=styles['Normal'],
        fontSize=14,
        textColor=TEXTO_CLARO,
        spaceAfter=10,
        alignment=TA_CENTER,
        fontName='Helvetica'
    ))

    styles.add(ParagraphStyle(
        name='Secao',
        parent=styles['Heading1'],
        fontSize=18,
        textColor=SECAO_COR,
        spaceBefore=20,
        spaceAfter=15,
        fontName='Helvetica-Bold'
    ))

    styles.add(ParagraphStyle(
        name='SubSecao',
        parent=styles['Heading2'],
        fontSize=13,
        textColor=SUBTITULO_COR,
        spaceBefore=15,
        spaceAfter=8,
        fontName='Helvetica-Bold'
    ))

    styles.add(ParagraphStyle(
        name='TextoNormal',
        parent=styles['Normal'],
        fontSize=10,
        textColor=TEXTO_CLARO,
        spaceAfter=8,
        alignment=TA_JUSTIFY,
        fontName='Helvetica',
        leading=14
    ))

    styles.add(ParagraphStyle(
        name='TextoPequeno',
        parent=styles['Normal'],
        fontSize=9,
        textColor=TEXTO_CLARO,
        spaceAfter=4,
        fontName='Helvetica',
        leading=12
    ))

    styles.add(ParagraphStyle(
        name='BulletItem',
        parent=styles['Normal'],
        fontSize=10,
        textColor=TEXTO_CLARO,
        leftIndent=20,
        spaceAfter=4,
        fontName='Helvetica',
        leading=14
    ))

    styles.add(ParagraphStyle(
        name='Creditos',
        parent=styles['Normal'],
        fontSize=10,
        textColor=TEXTO_CLARO,
        alignment=TA_CENTER,
        spaceAfter=6,
        fontName='Helvetica-Oblique'
    ))

    styles.add(ParagraphStyle(
        name='GargaloAlto',
        parent=styles['Normal'],
        fontSize=10,
        textColor=AVISO_COR,
        leftIndent=15,
        spaceAfter=6,
        fontName='Helvetica-Bold',
        leading=14
    ))

    styles.add(ParagraphStyle(
        name='GargaloMedio',
        parent=styles['Normal'],
        fontSize=10,
        textColor=SECAO_COR,
        leftIndent=15,
        spaceAfter=6,
        fontName='Helvetica-Bold',
        leading=14
    ))

    styles.add(ParagraphStyle(
        name='GargaloBaixo',
        parent=styles['Normal'],
        fontSize=10,
        textColor=TEXTO_CLARO,
        leftIndent=15,
        spaceAfter=6,
        fontName='Helvetica-Bold',
        leading=14
    ))

    return styles

def formatar_documento(documento, styles):
    """Formata todo o documento"""
    story = []

    # ===== CAPA =====
    story.append(Spacer(1, 4*cm))
    story.append(Paragraph("DOCUMENTO DE ANALISE", styles['TituloPrincipal']))
    story.append(Spacer(1, 0.5*cm))
    story.append(Paragraph("Sistemas Internos", styles['Subtitulo']))
    story.append(Spacer(1, 1*cm))
    story.append(Paragraph("HealthGoClient & HealthGoStudio", styles['Subtitulo']))
    story.append(Spacer(1, 3*cm))
    story.append(Paragraph("Ferramentas de Suporte Tecnico", styles['Subtitulo']))
    story.append(Spacer(1, 4*cm))
    story.append(Paragraph("Versao 1.0", styles['Creditos']))
    story.append(Paragraph("11 de Maio de 2026", styles['Creditos']))
    story.append(Spacer(1, 2*cm))
    story.append(Paragraph("_" * 50, styles['Creditos']))
    story.append(Spacer(1, 1*cm))
    story.append(Paragraph("Desenvolvido por:", styles['Creditos']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("<b>Andre Luiz de Souza</b>", styles['Creditos']))
    story.append(Paragraph("Especialista de Suporte Pleno N3", styles['Creditos']))
    story.append(PageBreak())

    # ===== RESUMO EXECUTIVO =====
    story.append(Paragraph("RESUMO EXECUTIVO", styles['Secao']))
    story.append(Paragraph(f"<b>Objetivo:</b> {documento['resumo_executivo']['objetivo']}", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("<b>Sistemas Cobertos:</b>", styles['TextoNormal']))
    for sistema in documento['resumo_executivo']['sistemas_cobertos']:
        story.append(Paragraph(f"  - {sistema}", styles['BulletItem']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph(f"<b>Tempo Medio de Resolucao:</b> {documento['resumo_executivo']['tempo_medio_resolucao']}", styles['TextoNormal']))
    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("<b>Principais Desafios:</b>", styles['TextoNormal']))
    for desafio in documento['resumo_executivo']['principais_desafios']:
        story.append(Paragraph(f"  - {desafio}", styles['BulletItem']))
    story.append(PageBreak())

    # ===== HEALTHGOCLIENT =====
    story.append(Paragraph("HEALTHGOCLIENT", styles['Secao']))
    story.append(Paragraph(f"<b>Descricao:</b> {documento['sistemas']['HealthGoClient']['descricao']}", styles['TextoNormal']))
    story.append(Paragraph(f"<b>Tipo:</b> {documento['sistemas']['HealthGoClient']['tipo']}", styles['TextoNormal']))
    story.append(Paragraph(f"<b>Usuarios:</b> {', '.join(documento['sistemas']['HealthGoClient']['usuarios'])}", styles['TextoNormal']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("FUNCIONALIDADES", styles['SubSecao']))
    for modulo in documento['sistemas']['HealthGoClient']['funcionalidades']['modulos']:
        story.append(Paragraph(f"<b>>> {modulo['nome']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    {modulo['descricao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("REQUISITOS TECNICOS", styles['SubSecao']))
    req = documento['sistemas']['HealthGoClient']['requisitos_tecnicos']
    story.append(Paragraph(f"<b>Sistema Operacional:</b> {', '.join(req['sistema_operacional']['suportado'])}", styles['BulletItem']))
    story.append(Paragraph(f"<b>Rede:</b> {req['rede']['ferramenta']} - {req['rede']['descricao']}", styles['BulletItem']))
    story.append(Paragraph(f"<b>Conexao:</b> Obrigatoria com URL do servidor HealthGo", styles['BulletItem']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("INTEGRACOES", styles['SubSecao']))
    for integracao in documento['sistemas']['HealthGoClient']['integracoes']:
        story.append(Paragraph(f"  - <b>{integracao['sistema']}</b>: {integracao['funcao']}", styles['BulletItem']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("GARGALOS", styles['SubSecao']))
    for gargalo in documento['sistemas']['HealthGoClient']['gargalos']:
        severidade = gargalo['severidade']
        style_name = f'Gargalo{severidade.replace("a", "A").replace("A", "")}'
        if style_name not in ['GargaloAlta', 'GargaloMedia', 'GargaloBaixa']:
            style_name = 'GargaloAlto' if severidade == 'Alta' else 'GargaloMedio'
        story.append(Paragraph(f"[{severidade.upper()}] {gargalo['titulo']}", styles[style_name]))
        story.append(Paragraph(f"  {gargalo['descricao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("IMPACTO NA DEVOLUTIVA", styles['SubSecao']))
    for indicador in documento['sistemas']['HealthGoClient']['impacto_devolutiva_cliente']['indicadores']:
        valor = indicador.get('valor', indicador.get('proporcao', 'N/A'))
        story.append(Paragraph(f"  - <b>{indicador['metrica']}:</b> {valor}", styles['BulletItem']))

    story.append(PageBreak())

    # ===== HEALTHGOSTUDIO =====
    story.append(Paragraph("HEALTHGOSTUDIO", styles['Secao']))
    story.append(Paragraph(f"<b>Descricao:</b> {documento['sistemas']['HealthGoStudio']['descricao']}", styles['TextoNormal']))
    story.append(Paragraph(f"<b>Tipo:</b> {documento['sistemas']['HealthGoStudio']['tipo']}", styles['TextoNormal']))
    story.append(Paragraph(f"<b>Usuarios:</b> {', '.join(documento['sistemas']['HealthGoStudio']['usuarios'])}", styles['TextoNormal']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("FUNCIONALIDADES", styles['SubSecao']))
    for modulo in documento['sistemas']['HealthGoStudio']['funcionalidades']['modulos']:
        story.append(Paragraph(f"<b>>> {modulo['nome']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    {modulo['descricao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("REQUISITOS TECNICOS", styles['SubSecao']))
    req = documento['sistemas']['HealthGoStudio']['requisitos_tecnicos']
    story.append(Paragraph(f"<b>Sistema Operacional:</b> {', '.join(req['sistema_operacional']['suportado'])}", styles['BulletItem']))
    story.append(Paragraph(f"<b>Rede:</b> {req['rede']['ferramenta']} - {req['rede']['descricao']}", styles['BulletItem']))
    story.append(Paragraph(f"<b>Hardware:</b> Processador Intel i7+, 16GB RAM, 512GB SSD", styles['BulletItem']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("GARGALOS", styles['SubSecao']))
    for gargalo in documento['sistemas']['HealthGoStudio']['gargalos']:
        severidade = gargalo['severidade']
        style_name = 'GargaloAlto' if severidade == 'Alta' else 'GargaloMedio'
        story.append(Paragraph(f"[{severidade.upper()}] {gargalo['titulo']}", styles[style_name]))
        story.append(Paragraph(f"  {gargalo['descricao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("IMPACTO NA DEVOLUTIVA", styles['SubSecao']))
    for indicador in documento['sistemas']['HealthGoStudio']['impacto_devolutiva_cliente']['indicadores']:
        valor = indicador.get('valor', indicador.get('proporcao', 'N/A'))
        story.append(Paragraph(f"  - <b>{indicador['metrica']}:</b> {valor}", styles['BulletItem']))

    story.append(PageBreak())

    # ===== FLUXO DE ATENDIMENTO =====
    story.append(Paragraph("FLUXO DE ATENDIMENTO", styles['Secao']))
    story.append(Paragraph(f"<b>Descricao:</b> {documento['fluxo_de_atendimento']['descricao']}", styles['TextoNormal']))
    story.append(Spacer(1, 0.4*cm))

    for etapa in documento['fluxo_de_atendimento']['etapas']:
        story.append(Paragraph(f"<b>ETAPA {etapa['ordem']}:</b> {etapa['nome']}", styles['BulletItem']))
        story.append(Paragraph(f"    Sistema: {etapa['sistema']} | Responsavel: {etapa['responsavel']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    {etapa['descricao']}", styles['TextoPequeno']))
        if 'desvio' in etapa:
            story.append(Paragraph(f"    <font color='#FF9800'>Desvio:</font> {etapa['desvio']}", styles['TextoPequeno']))
        story.append(Spacer(1, 0.2*cm))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("PONTOS DE BLOQUEIO", styles['SubSecao']))
    for bloqueio in documento['fluxo_de_atendimento']['pontos_de_bloqueio']:
        story.append(Paragraph(f"<b>>> {bloqueio['local']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    Bloqueio: {bloqueio['bloqueio']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    Acao: {bloqueio['acao_corrigiva']}", styles['TextoPequeno']))

    story.append(PageBreak())

    # ===== GARGALOS GLOBAIS =====
    story.append(Paragraph("ANALISE DE GARGALOS GLOBAIS", styles['Secao']))
    story.append(Paragraph(f"<b>Resumo:</b> {documento['analise_de_gargalos_globais']['resumo']}", styles['TextoNormal']))

    story.append(Spacer(1, 0.4*cm))
    story.append(Paragraph("INFRAESTRUTURA", styles['SubSecao']))
    for g in documento['analise_de_gargalos_globais']['por_categoria']['infraestrutura']:
        story.append(Paragraph(f"<b>>> {g['gargalo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    Impacto: {g['impacto']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    Prioridade: {g['prioridade']} | Recomendacao: {g['recomendacao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("PROCESSOS MANUAIS", styles['SubSecao']))
    for g in documento['analise_de_gargalos_globais']['por_categoria']['processos_manuais']:
        story.append(Paragraph(f"<b>>> {g['gargalo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    Impacto: {g['impacto']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    Recomendacao: {g['recomendacao']}", styles['TextoPequeno']))

    story.append(Spacer(1, 0.3*cm))
    story.append(Paragraph("VISIBILIDADE DE DADOS", styles['SubSecao']))
    for g in documento['analise_de_gargalos_globais']['por_categoria']['visibilidade_de_dados']:
        story.append(Paragraph(f"<b>>> {g['gargalo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    Impacto: {g['impacto']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    Recomendacao: {g['recomendacao']}", styles['TextoPequeno']))

    story.append(PageBreak())

    # ===== MELHORIAS =====
    story.append(Paragraph("MELHORIAS SOLICITADAS", styles['Secao']))

    story.append(Paragraph("PRIORIDADE ALTA", styles['SubSecao']))
    for m in documento['melhorias_solicitadas']['prioridade_alta']:
        story.append(Paragraph(f"<b>>> {m['titulo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    {m['descricao']}", styles['TextoPequeno']))
        story.append(Paragraph(f"    Beneficio: {m['beneficio']}", styles['TextoPequeno']))
        story.append(Spacer(1, 0.2*cm))

    story.append(Paragraph("PRIORIDADE MEDIA", styles['SubSecao']))
    for m in documento['melhorias_solicitadas']['prioridade_media']:
        story.append(Paragraph(f"<b>>> {m['titulo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    {m['descricao']}", styles['TextoPequeno']))
        story.append(Spacer(1, 0.1*cm))

    story.append(Paragraph("PRIORIDADE BAIXA", styles['SubSecao']))
    for m in documento['melhorias_solicitadas']['prioridade_baixa']:
        story.append(Paragraph(f"<b>>> {m['titulo']}</b>", styles['BulletItem']))
        story.append(Paragraph(f"    {m['descricao']}", styles['TextoPequeno']))

    story.append(PageBreak())

    # ===== GLOSSARIO =====
    story.append(Paragraph("GLOSSARIO", styles['Secao']))
    for termo in documento[' glossario']['termos']:
        story.append(Paragraph(f"<b>>> {termo['termo']}:</b> {termo['definicao']}", styles['BulletItem']))

    # ===== CREDITOS FINAIS =====
    story.append(Spacer(1, 2*cm))
    story.append(Paragraph("_" * 50, styles['Creditos']))
    story.append(Spacer(1, 0.5*cm))
    story.append(Paragraph("Documento desenvolvido por Andre Luiz de Souza", styles['Creditos']))
    story.append(Paragraph("Especialista de Suporte Pleno N3", styles['Creditos']))
    story.append(Paragraph("Gerado em: 11/05/2026", styles['Creditos']))

    return story

def page_template(canvas, doc):
    """Template para adicionar fundo escuro em cada pagina"""
    canvas.saveState()
    canvas.setFillColor(FUNDO_PRETO)
    canvas.rect(0, 0, A4[0], A4[1], fill=1, stroke=0)
    canvas.restoreState()

def generate_pdf(json_path, output_path):
    """Gera o PDF"""

    with open(json_path, 'r', encoding='utf-8') as f:
        documento = json.load(f)

    styles = create_styles()

    doc = SimpleDocTemplate(
        output_path,
        pagesize=A4,
        rightMargin=2*cm,
        leftMargin=2*cm,
        topMargin=2*cm,
        bottomMargin=2*cm
    )

    story = formatar_documento(documento, styles)

    doc.build(story, onFirstPage=page_template, onLaterPages=page_template)

    print(f"[OK] PDF gerado com sucesso!")
    print(f"[FILE] Local: {output_path}")

if __name__ == "__main__":
    base_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas"
    json_path = os.path.join(base_path, "documento_sistemas_healthgo.json")
    output_path = os.path.join(base_path, "documento_sistemas_healthgo.pdf")

    generate_pdf(json_path, output_path)
