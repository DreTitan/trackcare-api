import pdfplumber
import os
import re

pdf_path = r"C:\Users\HealthGo\Desktop\HealthGo\Ferramentas internas\POP_HealthGo_Suporte_Tecnico.pdf"
output_html = r"C:\Users\HealthGo\Desktop\HealthGo\POP_HealthGo_Suporte_Tecnico.html"

def clean_text(text):
    if not text:
        return ""
    text = re.sub(r'\n{3,}', '\n\n', text)
    return text.strip()

def pdf_to_html(pdf_path, output_html):
    html_parts = []
    html_parts.append("""<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>POP - HealthGo Suporte Técnico</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background: #f5f5f5;
            padding: 20px;
        }
        .container {
            max-width: 900px;
            margin: 0 auto;
            background: white;
            padding: 40px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        h1 {
            color: #2c3e50;
            border-bottom: 3px solid #3498db;
            padding-bottom: 15px;
            margin-bottom: 30px;
            font-size: 2em;
        }
        h2 {
            color: #2980b9;
            margin-top: 30px;
            margin-bottom: 15px;
            font-size: 1.5em;
            border-left: 4px solid #3498db;
            padding-left: 15px;
        }
        h3 {
            color: #34495e;
            margin-top: 20px;
            margin-bottom: 10px;
            font-size: 1.2em;
        }
        p {
            margin-bottom: 15px;
            text-align: justify;
        }
        .page-break {
            page-break-before: always;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 2px dashed #ddd;
        }
        ul, ol {
            margin-left: 30px;
            margin-bottom: 15px;
        }
        li {
            margin-bottom: 8px;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 15px 0;
        }
        th, td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }
        th {
            background: #3498db;
            color: white;
        }
        tr:nth-child(even) {
            background: #f9f9f9;
        }
        .cover {
            text-align: center;
            padding: 100px 20px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            margin: -40px -40px 40px -40px;
        }
        .cover h1 {
            color: white;
            border: none;
            font-size: 2.5em;
        }
        .cover .subtitle {
            font-size: 1.3em;
            margin-top: 20px;
            opacity: 0.9;
        }
        @media print {
            body { background: white; padding: 0; }
            .container { box-shadow: none; }
            .page-break { border-top: 1px solid #ccc; }
        }
    </style>
</head>
<body>
    <div class="container">
""")

    with pdfplumber.open(pdf_path) as pdf:
        for i, page in enumerate(pdf.pages):
            text = page.extract_text()

            if text:
                text = clean_text(text)
                lines = text.split('\n')

                for line in lines:
                    line = line.strip()
                    if not line:
                        continue

                    if i > 0 and (line.startswith('POP ') or 'HealthGo' in line and 'versão' in line.lower()):
                        continue

                    if line.startswith('# '):
                        html_parts.append(f'<h1>{line[2:]}</h1>')
                    elif line.startswith('## '):
                        html_parts.append(f'<h2>{line[3:]}</h2>')
                    elif line.startswith('### '):
                        html_parts.append(f'<h3>{line[4:]}</h3>')
                    else:
                        if line.startswith(('• ', '- ')):
                            html_parts.append(f'<li>{line[2:]}</li>')
                        else:
                            html_parts.append(f'<p>{line}</p>')

            if i > 0:
                html_parts.append('<div class="page-break"></div>')

    html_parts.append("""
    </div>
</body>
</html>
""")

    with open(output_html, 'w', encoding='utf-8') as f:
        f.write('\n'.join(html_parts))

    print(f"HTML gerado com sucesso: {output_html}")

pdf_to_html(pdf_path, output_html)
