import pdfplumber
import re

pdf_path = r"C:\Users\HealthGo\Desktop\HealthGo\Ferramentas internas\Manual_Atribuicoes_N3_Tester_Otimizado.pdf"
output_path = r"C:\Users\HealthGo\Desktop\HealthGo\Manual_Atribuicoes_N3_Tester_Otimizado.html"

def clean_text(text):
    if not text:
        return ""
    text = re.sub(r'\n{3,}', '\n', text)
    return text.strip()

all_text = []
with pdfplumber.open(pdf_path) as pdf:
    for page in pdf.pages:
        text = page.extract_text()
        if text:
            lines = text.split('\n')
            for line in lines:
                line = line.strip()
                if line and not line.startswith('file://') and not line.startswith('file:///'):
                    all_text.append(line)

print(f"Extraídas {len(all_text)} linhas")
for i, line in enumerate(all_text[:30]):
    print(f"{i}: {line}")
