"""
Gerador de HTML Autocontido (CSS Inline)
Para envio facil por email/WhatsApp sem perder o design
"""
import re

# Ler o HTML original
with open(r"C:\Users\HealthGo\Desktop\Ferramentas internas\documento_sistemas_healthgo.html", "r", encoding="utf-8") as f:
    html_content = f.read()

# Remover referencias externas (fonts do Google)
html_content = html_content.replace('<link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet">', '')

# Adicionar CSS da fonte Inter diretamente
font_css = """
<style>
    @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap');
</style>
"""

# Inserir o import de fonte no head
head_end = html_content.find('</head>')
html_content = html_content[:head_end] + font_css + html_content[head_end:]

# Salvar como versao para envio
output_path = r"C:\Users\HealthGo\Desktop\Ferramentas internas\documento_para_envio.html"
with open(output_path, "w", encoding="utf-8") as f:
    f.write(html_content)

print(f"[OK] HTML para envio gerado!")
print(f"[FILE] {output_path}")
print(f"")
print("=" * 50)
print("OPCOES DE COMPARTILHAMENTO:")
print("=" * 50)
print("")
print("1. EMAIL: Anexe o arquivo documento_para_envio.html")
print("2. WHATSAPP: Envie o arquivo diretamente")
print("3. GOOGLE DRIVE: Faça upload e compartilhe o link")
print("4. ONEDRIVE/DROPBOX: Faça upload e compartilhe o link")
print("5. TELEGRAM: Envie como arquivo")
print("")
print("O arquivo mantem o design completo e funciona offline!")
