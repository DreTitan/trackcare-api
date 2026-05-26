import pdfplumber
import sys

pdf_path = r"C:\Users\HealthGo\Desktop\HealthGo\Treinamento HealthGo\POP.001_HealthGoAir_v04.pdf"

try:
    with pdfplumber.open(pdf_path) as pdf:
        print(f"Total de páginas: {len(pdf.pages)}")
        for i, page in enumerate(pdf.pages):
            text = page.extract_text()
            if text and len(text.strip()) > 0:
                sys.stdout.buffer.write(f"\n=== PÁGINA {i+1} ===\n".encode('utf-8'))
                sys.stdout.buffer.write(f"{text}\n".encode('utf-8'))
            else:
                sys.stdout.buffer.write(f"\n=== PÁGINA {i+1} === (sem texto)\n".encode('utf-8'))
except Exception as e:
    print(f"Erro: {e}")
