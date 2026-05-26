# -*- coding: utf-8 -*-
from reportlab.lib.pagesizes import A4
from reportlab.lib import colors
from reportlab.lib.units import cm, mm
from reportlab.platypus import (
    SimpleDocTemplate, Table, TableStyle, Paragraph, Spacer,
    HRFlowable, KeepTogether
)
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.enums import TA_CENTER, TA_LEFT, TA_RIGHT
from reportlab.pdfgen import canvas
from reportlab.lib.colors import HexColor, Color
import math

# Paleta
AZUL       = HexColor('#005A8D')
AZUL_ESC   = HexColor('#003D5C')
CINZA_BG   = HexColor('#F5F7FA')
BRANCO     = colors.white
TEXTO      = HexColor('#2C3E50')
CINZA_MED  = HexColor('#95A5A6')
VERMELHO   = HexColor('#E74C3C')
LARANJA    = HexColor('#E67E22')
ROXO       = HexColor('#9B59B6')
AZUL_LEVE  = HexColor('#3498DB')
CINZA_LINHA= HexColor('#ECF0F1')
VERMELHO_CL= HexColor('#FDECEA')
LARANJA_CL = HexColor('#FEF5E7')
ROXO_CL    = HexColor('#F5EEF8')
AZUL_CL    = HexColor('#EAF4FB')
CINZA_CARD = HexColor('#F0F0F0')

# Dados
dados = [
    ("Dr. LORENZON SERVICOS MEDICOS LTDA",           "18/05", 2, 1, 0, 2),
    ("Dr. Elias Benjamin Moran Gonzalez",             "19/05", 1, 1, 1, 0),
    ("Dra. Carla Moura Fe Elias",                     "21/05", 2, 1, 0, 1),
    ("Dra. Juliana Ferrari",                          "21/05", 1, 2, 1, 1),
    ("Dra. Cyrla Zaltman",                            "21/05", 1, 1, 1, 1),
    ("Dra. Stela Scaglioni Marini",                   "22/05", 1, 1, 1, 1),
    ("Dr. FLAVIO HIROSHI ANANIAS MORITA",            "22/05", 1, 1, 2, 0),
    ("Dra. Rafaela Muniz",                           "25/05", 2, 1, 0, 1),
    ("Dra. Cristina Melo",                            "25/05", 0, 1, 0, 0),
    ("Dra. Juliane Santos da Silveira Ramacciotti",   "26/05", 0, 0, 0, 0),
    ("Dra. Jacklinne",                               "26/05", 1, 1, 0, 0),
    ("Dra. Luiza Lage",                              "26/05", 1, 1, 0, 0),
]
tot_com, tot_equ, tot_ent, tot_nao = 12, 12, 6, 7
tot_geral = 37

# Estilos de paragrafo
styles = getSampleStyleSheet()

def S(name, **kw):
    return ParagraphStyle(name, **kw)

s_titulo    = S('T', fontSize=20, textColor=BRANCO, alignment=TA_CENTER,
                fontName='Helvetica-Bold', leading=26, spaceAfter=2)
s_sub       = S('Sub', fontSize=10, textColor=BRANCO, alignment=TA_CENTER,
                fontName='Helvetica', leading=14)
s_per       = S('Per', fontSize=8.5, textColor=BRANCO, alignment=TA_CENTER,
                fontName='Helvetica-Oblique', leading=12)
s_card_v    = S('CV', fontSize=24, textColor=AZUL, alignment=TA_CENTER,
                fontName='Helvetica-Bold', leading=28)
s_card_l    = S('CL', fontSize=7, textColor=CINZA_MED, alignment=TA_CENTER,
                fontName='Helvetica', leading=9)
s_card_v2   = S('CV2', fontSize=24, textColor=BRANCO, alignment=TA_CENTER,
                fontName='Helvetica-Bold', leading=28)
s_card_l2   = S('CL2', fontSize=7, textColor=HexColor('#D0E8F5'),
                alignment=TA_CENTER, fontName='Helvetica', leading=9)
s_sec       = S('Sec', fontSize=10.5, textColor=AZUL, fontName='Helvetica-Bold',
                leading=13, spaceBefore=4)
s_n         = S('N', fontSize=8, textColor=TEXTO, fontName='Helvetica', leading=11)
s_n_b       = S('NB', fontSize=8, textColor=TEXTO, fontName='Helvetica-Bold', leading=11)
s_b         = S('B', fontSize=8.5, alignment=TA_CENTER, textColor=AZUL,
                fontName='Helvetica-Bold', leading=11)
s_it        = S('IT', fontSize=6.5, textColor=CINZA_MED, alignment=TA_CENTER,
                fontName='Helvetica-Oblique', leading=9)
s_foot      = S('FT', fontSize=7, textColor=CINZA_MED, alignment=TA_CENTER,
                fontName='Helvetica-Oblique')
s_dt        = S('DT', fontSize=8, alignment=TA_CENTER, textColor=TEXTO,
                fontName='Helvetica')
s_tag       = S('TG', fontSize=7, textColor=TEXTO, fontName='Helvetica', leading=9)
s_tag_b     = S('TGB', fontSize=7, textColor=TEXTO, fontName='Helvetica-Bold', leading=9)
s_prov      = S('PV', fontSize=8.5, textColor=TEXTO, fontName='Helvetica-Bold', leading=11)
s_prov_sub  = S('PVS', fontSize=7.5, textColor=CINZA_MED, fontName='Helvetica', leading=10)
s_tt        = S('TT', fontSize=8, alignment=TA_RIGHT, textColor=AZUL,
                fontName='Helvetica-Bold')

# Canvas com header/footer
class HFCanvas(canvas.Canvas):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self._pages = []

    def showPage(self):
        self._pages.append(dict(self.__dict__))
        self._startPage()

    def save(self):
        total = len(self._pages)
        for i, state in enumerate(self._pages):
            self.__dict__.update(state)
            self._draw_hf(i + 1, total)
            canvas.Canvas.showPage(self)
        canvas.Canvas.save(self)

    def _draw_hf(self, num, total):
        w, h = A4
        # Header
        self.setFillColor(AZUL)
        self.rect(0, h - 2.8*cm, w, 2.8*cm, fill=1, stroke=0)
        self.setFillColor(AZUL_ESC)
        self.rect(0, h - 3.1*cm, w, 0.3*cm, fill=1, stroke=0)
        # Header text
        self.setFillColor(BRANCO)
        self.setFont('Helvetica-Bold', 18)
        self.drawCentredString(w/2, h - 1.5*cm, "HealthGo Air")
        self.setFont('Helvetica', 9)
        self.drawCentredString(w/2, h - 2.0*cm,
            "Relatorio Consolidado - Motivos de Recolhimento")
        # Footer
        self.setFillColor(CINZA_LINHA)
        self.rect(0, 0.5*cm, w, 0.4*cm, fill=1, stroke=0)
        self.setFillColor(CINZA_MED)
        self.setFont('Helvetica-Oblique', 6.5)
        self.drawCentredString(w/2, 0.6*cm,
            f"HealthGo Air | Periodo: 18/05 a 26/05/2026 | Pagina {num}/{total}")


OUTPUT = "Relatorio_Consolidado_Recolhimentos_18a26Maio2026.pdf"
doc = SimpleDocTemplate(
    OUTPUT, pagesize=A4,
    leftMargin=1.5*cm, rightMargin=1.5*cm,
    topMargin=3.4*cm, bottomMargin=1.0*cm,
    title="Relatorio Consolidado - Motivos de Recolhimento HealthGo Air"
)

PW = A4[0] - 3.0*cm
story = []

# HEADER BLOCK
story.append(Spacer(1, 0.1*cm))
story.append(Paragraph("HealthGo Air", s_titulo))
story.append(Paragraph("Relatorio Consolidado - Motivos de Recolhimento", s_sub))
story.append(Paragraph("Periodo: 18/05/2026 a 26/05/2026", s_per))
story.append(Spacer(1, 0.4*cm))

# CARD VISÃO GERAL
card_vals  = [str(tot_com), str(tot_equ), str(tot_ent), str(tot_nao), str(tot_geral)]
card_lbls  = ["Erro / Falha\nde Comunicacao", "Equipamento\nDanificado",
              "Entrega\nIncorreta", "Nao Atendimento\nda Remessa",
              "Total\nRecolhimentos"]
card_subs  = ["COM", "EQUIP", "ENT", "NAO ATEND", f"{len(dados)} PRESTADORES"]
card_bg    = [VERMELHO_CL, LARANJA_CL, ROXO_CL, AZUL_CL, AZUL]
card_vcor  = [VERMELHO, LARANJA, ROXO, AZUL_LEVE, BRANCO]
card_lcor  = [VERMELHO, LARANJA, ROXO, AZUL_LEVE, HexColor('#D0E8F5')]

cw = PW / 5
card_row = []
for val, lbl, sub, bg, vc, lc in zip(card_vals, card_lbls, card_subs,
                                       card_bg, card_vcor, card_lcor):
    vs = S(f'CV_{val}', fontSize=22, textColor=vc, alignment=TA_CENTER,
           fontName='Helvetica-Bold', leading=26)
    ls = S(f'CL_{lbl}', fontSize=6.5, textColor=TEXTO, alignment=TA_CENTER,
           fontName='Helvetica', leading=9)
    ss = S(f'CS_{sub}', fontSize=6, textColor=CINZA_MED, alignment=TA_CENTER,
           fontName='Helvetica-Oblique', leading=8)
    cell = [
        Paragraph(val, vs),
        Paragraph(lbl.replace('\n', ' '), ls),
        Paragraph(sub, ss),
    ]
    card_row.append(cell)

card_tbl = Table([card_row], colWidths=[cw]*5, rowHeights=[2.5*cm])
cts = TableStyle([
    ('ALIGN',      (0,0), (-1,-1), 'CENTER'),
    ('VALIGN',     (0,0), (-1,-1), 'MIDDLE'),
    ('TOPPADDING', (0,0), (-1,-1), 4),
    ('BOTTOMPADDING',(0,0), (-1,-1), 4),
    ('LEFTPADDING',(0,0), (-1,-1), 2),
    ('RIGHTPADDING',(0,0), (-1,-1), 2),
    ('BOX',        (0,0), (3,0), 0.5, CINZA_LINHA),
])
for i, bg in enumerate(card_bg):
    cts.add('BACKGROUND', (i,0), (i,0), bg)
card_tbl.setStyle(cts)
story.append(card_tbl)
story.append(Spacer(1, 0.4*cm))

# GRAFICO DE BARRAS
story.append(Paragraph("Distribuicao por Motivo de Recolhimento", s_sec))
story.append(HRFlowable(width=PW, thickness=1.5, color=AZUL, spaceAfter=6))

maxv = max(tot_com, tot_equ, tot_ent, tot_nao)
bar_data = [
    ("Erro / Falha de Comunicacao",  tot_com, tot_geral, VERMELHO),
    ("Equipamento Danificado",         tot_equ, tot_geral, LARANJA),
    ("Nao Atendimento da Remessa",     tot_nao, tot_geral, AZUL_LEVE),
    ("Entrega Incorreta",              tot_ent, tot_geral, ROXO),
]
bar_tbl_data = []
for label, val, total, cor in bar_data:
    pct_s = S(f'pct_{val}', fontSize=8, textColor=TEXTO, fontName='Helvetica-Bold',
              alignment=TA_RIGHT, leading=11)
    pct = f"{val} ({val/total*100:.1f}%)"
    bar_tbl_data.append([
        Paragraph(label, s_n),
        Paragraph(pct, pct_s),
        Paragraph("", s_n),  # placeholder da barra
    ])

bar_tbl = Table(bar_tbl_data, colWidths=[PW*0.34, PW*0.24, PW*0.42],
                rowHeights=[0.65*cm]*4)
bts = TableStyle([
    ('ALIGN',      (0,0), (-1,-1), 'LEFT'),
    ('VALIGN',     (0,0), (-1,-1), 'MIDDLE'),
    ('TOPPADDING', (0,0), (-1,-1), 6),
    ('BOTTOMPADDING',(0,0), (-1,-1), 6),
    ('LEFTPADDING',(0,0), (-1,-1), 4),
    ('RIGHTPADDING',(0,0), (-1,-1), 4),
    ('LINEBELOW',  (0,0), (-1,-1), 0.3, CINZA_LINHA),
])
for i, (_, val, _, cor) in enumerate(bar_data):
    pct_w = val / maxv
    bg_cor = Color(cor.red*0.25+pct_w*cor.red*0.75,
                   cor.green*0.25+pct_w*cor.green*0.75,
                   cor.blue*0.25+pct_w*cor.blue*0.75)
    bts.add('BACKGROUND', (2, i), (2, i), bg_cor)
bar_tbl.setStyle(bts)
story.append(bar_tbl)
story.append(Spacer(1, 0.4*cm))

# TABELA PRINCIPAL
story.append(Paragraph("Resumo por Prestador - Periodo 18/05 a 26/05/2026", s_sec))
story.append(HRFlowable(width=PW, thickness=1.5, color=AZUL, spaceAfter=6))

def cel(txt, cor_txt, bold=True, sz=8, align=TA_CENTER):
    fn = 'Helvetica-Bold' if bold else 'Helvetica'
    return Paragraph(txt, S(f'c_{txt}_{sz}', fontSize=sz, textColor=cor_txt,
                              fontName=fn, alignment=align, leading=sz+3))

hdr_style = S('hdr', fontSize=7.5, textColor=BRANCO, fontName='Helvetica-Bold',
               alignment=TA_CENTER, leading=10)

hdr_row = [
    cel("Prestador / Estabelecimento", BRANCO, sz=7.5, align=TA_LEFT),
    cel("Data", BRANCO, sz=7.5),
    cel("Erro/Falha Com.", BRANCO, sz=7),
    cel("Equip. Danif.", BRANCO, sz=7),
    cel("Entrega Incor.", BRANCO, sz=7),
    cel("Nao Atend.", BRANCO, sz=7),
    cel("Total", BRANCO, sz=8),
]

tdata = [hdr_row]
for nome, dt, c, e, ent, nao in dados:
    tl = c + e + ent + nao
    def badge_cell(v, cor_txt, bg_cor, sz=8):
        return cel(str(v) if v > 0 else "0", cor_txt if v > 0 else CINZA_MED,
                   bold=(v > 0), sz=sz)
    tdata.append([
        Paragraph(nome, s_n),
        Paragraph(dt, s_dt),
        badge_cell(c, VERMELHO, VERMELHO_CL),
        badge_cell(e, LARANJA, LARANJA_CL),
        badge_cell(ent, ROXO, ROXO_CL),
        badge_cell(nao, AZUL_LEVE, AZUL_CL),
        cel(str(tl), AZUL, sz=9),
    ])

# Total row
tdata.append([
    Paragraph("TOTAL GERAL", S('tg', fontSize=8.5, textColor=BRANCO,
               fontName='Helvetica-Bold', leading=11)),
    "",
    cel(str(tot_com), BRANCO, sz=8.5),
    cel(str(tot_equ), BRANCO, sz=8.5),
    cel(str(tot_ent), BRANCO, sz=8.5),
    cel(str(tot_nao), BRANCO, sz=8.5),
    cel(str(tot_geral), BRANCO, sz=9),
])

col_w = [PW*0.33, 1.3*cm, 1.7*cm, 1.7*cm, 1.7*cm, 1.7*cm, 1.2*cm]
main_tbl = Table(tdata, colWidths=col_w, repeatRows=1)
mts = TableStyle([
    # Header
    ('BACKGROUND',   (0,0), (-1,0), AZUL),
    ('VALIGN',       (0,0), (-1,-1), 'MIDDLE'),
    ('TOPPADDING',   (0,0), (-1,0), 6),
    ('BOTTOMPADDING',(0,0), (-1,0), 6),
    # Dados
    ('ROWBACKGROUNDS',(0,1), (-1,-2), [BRANCO, CINZA_BG]),
    ('TOPPADDING',   (0,1), (-1,-2), 5),
    ('BOTTOMPADDING',(0,1), (-1,-2), 5),
    ('LEFTPADDING',  (0,1), (0,-2), 4),
    ('LEFTPADDING',  (1,1), (-1,-2), 2),
    ('RIGHTPADDING', (0,1), (-1,-2), 2),
    ('GRID',         (0,1), (-1,-2), 0.3, CINZA_LINHA),
    # Zebra para badges
    ('BACKGROUND',   (2,1), (2,-2), VERMELHO_CL),
    ('BACKGROUND',   (3,1), (3,-2), LARANJA_CL),
    ('BACKGROUND',   (4,1), (4,-2), ROXO_CL),
    ('BACKGROUND',   (5,1), (5,-2), AZUL_CL),
    # Total row
    ('BACKGROUND',   (0,-1), (-1,-1), AZUL_ESC),
    ('SPAN',         (0,-1), (1,-1)),
    ('TOPPADDING',   (0,-1), (-1,-1), 6),
    ('BOTTOMPADDING',(0,-1), (-1,-1), 6),
    ('LEFTPADDING',  (0,-1), (-1,-1), 4),
])
main_tbl.setStyle(mts)
story.append(main_tbl)
story.append(Spacer(1, 0.4*cm))

# DETALHES
story.append(Paragraph("Detalhes por Estabelecimento", s_sec))
story.append(HRFlowable(width=PW, thickness=1.5, color=AZUL, spaceAfter=8))

detalhes = [
    ("Dr. LORENZON SERVICOS MEDICOS LTDA", "18/05", [
        ("Erro de Comunicacao", 2, VERMELHO),
        ("Equipamento Danificado", 1, LARANJA),
        ("Nao Atendimento da Remessa", 2, AZUL_LEVE),
    ]),
    ("Dr. Elias Benjamin Moran Gonzalez", "19/05", [
        ("Falha de comunicacao - NFe nao transmitiu", 1, VERMELHO),
        ("Equipamento Danificado - oximetro com defeito", 1, LARANJA),
        ("Entrega Incorreta", 1, ROXO),
    ]),
    ("Dra. Carla Moura Fe Elias", "21/05", [
        ("Erro de comunicacao", 2, VERMELHO),
        ("Equipamento Danificado - glicose com defeito", 1, LARANJA),
        ("Nao Atendimento da Remessa", 1, AZUL_LEVE),
    ]),
    ("Dra. Juliana Ferrari", "21/05", [
        ("Erro de comunicacao - NFe nao gerou", 1, VERMELHO),
        ("Equipamento Danificado", 2, LARANJA),
        ("Entrega Incorreta", 1, ROXO),
        ("Nao Atendimento da Remessa", 1, AZUL_LEVE),
    ]),
    ("Dra. Cyrla Zaltman", "21/05", [
        ("Erro de comunicacao", 1, VERMELHO),
        ("Equipamento Danificado - detector fetal com defeito", 1, LARANJA),
        ("Entrega Incorreta", 1, ROXO),
        ("Nao Atendimento da Remessa", 1, AZUL_LEVE),
    ]),
    ("Dra. Stela Scaglioni Marini", "22/05", [
        ("Erro de comunicacao - NFe nao gerou", 1, VERMELHO),
        ("Equipamento Danificado - desfibrilador com defeito", 1, LARANJA),
        ("Entrega Incorreta", 1, ROXO),
        ("Nao Atendimento da Remessa", 1, AZUL_LEVE),
    ]),
    ("Dr. FLAVIO HIROSHI ANANIAS MORITA", "22/05", [
        ("Erro de comunicacao - NFe nao gerou", 1, VERMELHO),
        ("Equipamento Danificado - bisturi eletrico com defeito", 1, LARANJA),
        ("Entrega Incorreta - endereco incorreto", 2, ROXO),
    ]),
    ("Dra. Rafaela Muniz", "25/05", [
        ("Erro de comunicacao - NFe nao gerou", 2, VERMELHO),
        ("Equipamento Danificado - cabo de ureteroscopia", 1, LARANJA),
        ("Nao Atendimento da Remessa", 1, AZUL_LEVE),
    ]),
    ("Dra. Cristina Melo", "25/05", [
        ("Equipamento Danificado", 1, LARANJA),
    ]),
    ("Dra. Juliane Santos da Silveira Ramacciotti", "26/05", []),
    ("Dra. Jacklinne", "26/05", [
        ("Erro de comunicacao - NFe nao gerou", 1, VERMELHO),
        ("Equipamento Danificado - equipo com defeito", 1, LARANJA),
    ]),
    ("Dra. Luiza Lage", "26/05", [
        ("Erro de comunicacao - NFe nao gerou", 1, VERMELHO),
        ("Equipamento Danificado - sensor com defeito", 1, LARANJA),
    ]),
]

# Agrupar motivos em blocos de 2 por linha
def chunk(lst, n=2):
    return [lst[i:i+n] for i in range(0, len(lst), n)]

tag_colors = [
    (VERMELHO_CL, VERMELHO),
    (LARANJA_CL, LARANJA),
    (AZUL_CL, AZUL_LEVE),
    (ROXO_CL, ROXO),
]

blocks = []
for nome, dt, motivos in detalhes:
    total = sum(q for _, q, _ in motivos)
    titulo_p = Paragraph(nome, s_prov)
    data_p   = Paragraph(f"  |  {dt}/05/2026", s_prov_sub)
    tot_p    = Paragraph(f"{total} recol." if total > 0 else "Sem recolhimentos",
                          s_tt)

    # Header do bloco
    hdr_tbl = Table(
        [[titulo_p, data_p, tot_p]],
        colWidths=[PW*0.55, PW*0.25, PW*0.20],
        style=TableStyle([
            ('ALIGN',     (1,0), (2,0), 'RIGHT'),
            ('VALIGN',    (0,0), (-1,-1), 'MIDDLE'),
            ('LEFTPADDING',(0,0), (-1,-1), 0),
            ('RIGHTPADDING',(0,0), (-1,-1), 0),
            ('TOPPADDING',(0,0), (-1,-1), 0),
            ('BOTTOMPADDING',(0,0), (-1,-1), 0),
        ])
    )

    rows = [hdr_tbl]
    if motivos:
        for row_mot in chunk(motivos):
            tag_cells = []
            for desc, qtd, cor in row_mot:
                label = f"[{desc}]{'  x'+str(qtd) if qtd>1 else ''}"
                tc = tag_colors[0]
                for i, (_, _, c) in enumerate(motivos):
                    if c == cor:
                        tc = tag_colors[i % len(tag_colors)]
                        break
                # find index
                idx = 0
                for i2, (_, _, c2) in enumerate(motivos):
                    if c2 == cor:
                        idx = i2
                        break
                bg, fg = tag_colors[idx % len(tag_colors)]
                p = Paragraph(label, S(f'tg_{desc}', fontSize=7, textColor=fg,
                             fontName='Helvetica-Bold', leading=10))
                tag_cells.append(p)
            while len(tag_cells) < 2:
                tag_cells.append(Paragraph("", s_n))
            rows.append(tag_cells)

    rows_flat = []
    for r in rows:
        if isinstance(r, list):
            rows_flat.extend(r)
        else:
            rows_flat.append(r)

    bg_color = CINZA_BG if total > 0 else HexColor('#FAFAFA')
    bloco = KeepTogether([
        Table(
            [rows_flat],
            colWidths=[PW*0.50, PW*0.50],
            style=TableStyle([
                ('BACKGROUND',   (0,0), (-1,-1), bg_color),
                ('LINEBEFORE',   (0,0), (-1,-1), 3, AZUL),
                ('LINEBELOW',    (0,0), (-1,-1), 0.4, CINZA_LINHA),
                ('TOPPADDING',   (0,0), (-1,-1), 5),
                ('BOTTOMPADDING',(0,0), (-1,-1), 5),
                ('LEFTPADDING',  (0,0), (-1,-1), 8),
                ('RIGHTPADDING', (0,0), (-1,-1), 8),
            ])
        )
    ])
    blocks.append(bloco)

story.extend(blocks)
story.append(Spacer(1, 0.3*cm))
story.append(HRFlowable(width=PW, thickness=0.5, color=CINZA_LINHA))
story.append(Spacer(1, 0.15*cm))
story.append(Paragraph(
    "HealthGo Air | Relatorio Consolidado Motivos de Recolhimento | "
    "Periodo: 18/05/2026 a 26/05/2026 | Gerado em 26/05/2026",
    s_foot))

doc.build(story, canvasmaker=HFCanvas)
print(f"PDF gerado: {OUTPUT}")
