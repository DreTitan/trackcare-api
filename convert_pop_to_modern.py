import pdfplumber
import re

pdf_path = r"C:\Users\HealthGo\Desktop\HealthGo\Ferramentas internas\POP_HealthGo_Suporte_Tecnico.pdf"

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
                if line and not line.startswith('file://') and not line.startswith('11/02/2026'):
                    all_text.append(line)

with open('C:\\Users\\HealthGo\\Desktop\\HealthGo\\pop_content.txt', 'w', encoding='utf-8') as f:
    f.write('\n'.join(all_text))

print("Conteúdo extraído com sucesso!")
print(f"Total de linhas: {len(all_text)}")
