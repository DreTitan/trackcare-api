import pdfplumber

pdf_path = r"C:\Users\HealthGo\Desktop\HealthGo\Ferramentas internas\POP_HealthGo_Suporte_Tecnico.pdf"

with pdfplumber.open(pdf_path) as pdf:
    print(f"Total de páginas: {len(pdf.pages)}")
    print("\n" + "="*50)

    for i, page in enumerate(pdf.pages[:3]):  # Primeiras 3 páginas
        print(f"\n--- PÁGINA {i+1} ---")
        text = page.extract_text()
        if text:
            print(text[:2000])  # Primeiros 2000 caracteres
        print("\n")
