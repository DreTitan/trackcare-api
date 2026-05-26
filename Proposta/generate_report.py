import json
import os
from fpdf import FPDF

class HealthGoPDF(FPDF):
    def header(self):
        self.set_fill_color(10, 14, 23)
        self.rect(0, 0, 210, 40, 'F')
        self.set_font('Helvetica', 'B', 22)
        self.set_text_color(0, 212, 255)
        self.set_y(12)
        self.cell(0, 10, 'HealthGo', 0, 1, 'C')
        self.set_font('Helvetica', '', 10)
        self.set_text_color(148, 163, 184)
        self.cell(0, 5, 'Ecossistemas Internos - Revisao Tecnica de Sistemas v2.0', 0, 1, 'C')
        self.ln(15)

    def footer(self):
        self.set_y(-15)
        self.set_font('Helvetica', 'I', 8)
        self.set_text_color(148, 163, 184)
        self.cell(0, 10, f'HealthGo - Documento Confidencial | Pagina {self.page_no()}', 0, 0, 'C')

    def system_card(self, system):
        # Title
        self.set_font('Helvetica', 'B', 16)
        self.set_text_color(241, 245, 249)
        self.set_fill_color(17, 24, 39)
        self.cell(0, 10, f" {system['name']}", 0, 1, 'L', True)

        # Type badge
        self.set_font('Helvetica', 'B', 8)
        self.set_text_color(0, 212, 255)
        self.cell(0, 5, f" {system['type'].upper()}", 0, 1, 'L')

        # Severity
        severity_color = (239, 68, 68) if system['severity'] == 'CRITICA' else (245, 158, 11)
        self.set_text_color(*severity_color)
        self.set_font('Helvetica', 'B', 9)
        self.cell(0, 5, f" CRITICIDADE: {system['severity']}", 0, 1, 'L')
        self.ln(5)

        # Explanation
        self.section_title("EXPLICACAO")
        self.set_text_color(100, 116, 139)
        self.set_font('Helvetica', '', 10)
        self.multi_cell(190, 5, system['explanation'])
        self.ln(3)

        # Function
        self.section_title("FUNCAO PRINCIPAL")
        self.set_text_color(100, 116, 139)
        self.set_font('Helvetica', '', 10)
        self.multi_cell(190, 5, system['function'])
        self.ln(3)

        # Technical Requirements Table
        self.section_title("REQUISITOS TECNICOS")
        self.ln(2)
        self.set_font('Helvetica', 'B', 9)
        self.set_fill_color(31, 41, 55)
        self.set_text_color(148, 163, 184)
        self.cell(50, 7, ' Categoria', 1, 0, 'L', True)
        self.cell(140, 7, ' Requisito', 1, 1, 'L', True)

        self.set_font('Helvetica', '', 9)
        self.set_text_color(226, 232, 240)
        for req in system['technical_requirements']:
            self.cell(50, 6, f" {req['category']}", 1, 0, 'L')
            self.cell(140, 6, f" {req['requirement']}", 1, 1, 'L')
        self.ln(5)

        # Bottlenecks
        self.section_title("MAIORES GARGALOS")
        self.ln(2)
        for bottleneck in system['bottlenecks']:
            # Severity indicator
            sev_color = (239, 68, 68) if bottleneck['severity'] == 'Alta' else (59, 130, 246)
            self.set_fill_color(*sev_color)
            self.set_text_color(255, 255, 255)
            self.set_font('Helvetica', 'B', 7)
            severity_text = "ALTA" if bottleneck['severity'] == 'Alta' else "MEDIA"
            self.cell(12, 5, severity_text, 0, 0, 'C', True)

            # Name and description
            self.set_text_color(226, 232, 240)
            self.set_font('Helvetica', 'B', 9)
            self.cell(50, 5, f" {bottleneck['name']}", 0, 0, 'L')
            self.set_text_color(100, 116, 139)
            self.set_font('Helvetica', '', 9)
            self.cell(128, 5, f" {bottleneck['description']}", 0, 1, 'L')
        self.ln(5)

        # Impact Box
        self.ln(3)
        self.set_fill_color(254, 242, 242)
        self.set_draw_color(239, 68, 68)
        self.set_line_width(0.5)
        self.rect(10, self.get_y(), 190, 40, 'DF')

        self.set_xy(15, self.get_y() + 3)
        self.set_font('Helvetica', 'B', 10)
        self.set_text_color(225, 29, 72)
        self.cell(0, 5, 'IMPACTO NA DEVOLUTIVA AO CLIENTE', 0, 1)

        self.set_x(15)
        self.set_font('Helvetica', '', 9)
        self.set_text_color(100, 116, 139)
        for point in system['impact_points']:
            self.set_x(18)
            self.cell(0, 5, f"* {point}", 0, 1)

        self.ln(15)

    def section_title(self, title):
        self.set_font('Helvetica', 'B', 10)
        self.set_text_color(0, 212, 255)
        self.cell(0, 6, title, 0, 1, 'L')
        self.ln(1)

def generate_report():
    data_file = 'systems_data.json'
    if not os.path.exists(data_file):
        print(f"Erro: {data_file} nao encontrado.")
        return

    with open(data_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    # Generate PDF
    pdf = HealthGoPDF()
    pdf.set_auto_page_break(auto=True, margin=20)
    pdf.add_page()

    # Summary section
    pdf.set_font('Helvetica', 'B', 14)
    pdf.set_text_color(241, 245, 249)
    pdf.cell(0, 8, ' RESUMO EXECUTIVO', 0, 1, 'L')
    pdf.ln(3)

    # Summary cards
    pdf.set_fill_color(17, 24, 39)
    card_width = 60
    card_x_positions = [10, 75, 140]

    summaries = [
        ("2", "Sistemas\nDocumentados"),
        ("8", "Gargalos\nIdentificados"),
        ("4", "Prioridades de\nMelhoria")
    ]

    for i, (num, label) in enumerate(summaries):
        pdf.set_xy(card_x_positions[i], pdf.get_y())
        pdf.set_fill_color(17, 24, 39)
        pdf.rect(card_x_positions[i], pdf.get_y(), card_width, 25, 'F')
        pdf.set_font('Helvetica', 'B', 20)
        pdf.set_text_color(0, 212, 255)
        pdf.set_xy(card_x_positions[i], pdf.get_y() + 2)
        pdf.cell(card_width, 10, num, 0, 1, 'C')
        pdf.set_font('Helvetica', '', 8)
        pdf.set_text_color(148, 163, 184)
        pdf.set_x(card_x_positions[i])
        pdf.cell(card_width, 5, label.replace('\n', ' '), 0, 1, 'C')

    pdf.ln(20)

    # System Cards
    for system in data['systems']:
        pdf.system_card(system)

    # Priorities Section
    pdf.set_font('Helvetica', 'B', 14)
    pdf.set_text_color(241, 245, 249)
    pdf.set_fill_color(17, 24, 39)
    pdf.cell(0, 8, ' PRIORIDADES DE MELHORIA', 0, 1, 'L', True)
    pdf.ln(5)

    priorities = data['summary']['priorities']
    for p in priorities:
        # Priority number
        pdf.set_fill_color(0, 212, 255)
        pdf.set_text_color(10, 14, 23)
        pdf.set_font('Helvetica', 'B', 10)
        pdf.cell(10, 8, str(p['priority']), 0, 0, 'C', True)

        # Title
        pdf.set_text_color(226, 232, 240)
        pdf.set_font('Helvetica', 'B', 10)
        pdf.cell(130, 8, f" {p['title']}", 0, 0, 'L')

        # Timeline tag
        tag_color = (239, 68, 68) if p['timeline'] == 'Urgente' else (59, 130, 246)
        pdf.set_fill_color(*tag_color)
        pdf.set_text_color(255, 255, 255)
        pdf.set_font('Helvetica', 'B', 8)
        pdf.cell(40, 8, f" {p['timeline']}", 0, 1, 'C', True)
        pdf.ln(2)

    pdf.output('HealthGo_Ecossistemas_Internos.pdf')
    print("Sucesso! PDF gerado: HealthGo_Ecossistemas_Internos.pdf")

if __name__ == "__main__":
    generate_report()
