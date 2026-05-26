"""
Conversor de HTML para PDF
"""
import subprocess
import os

# caminho do arquivo HTML
html_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas\documento_sistemas_healthgo.html"
pdf_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas\documento_sistemas_healthgo.pdf"

# tentar usar o weasyprint ou criar um script simples
try:
    from weasyprint import HTML
    HTML(html_path).write_pdf(pdf_path)
    print(f"[OK] PDF gerado com WeasyPrint!")
except ImportError:
    # instalar weasyprint
    print("Instalando weasyprint...")
    subprocess.run(["pip", "install", "weasyprint", "-q"], check=True)
    from weasyprint import HTML
    HTML(html_path).write_pdf(pdf_path)
    print(f"[OK] PDF gerado com WeasyPrint!")

print(f"[FILE] Local: {pdf_path}")
