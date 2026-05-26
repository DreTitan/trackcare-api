"""
Script: extract_may.py
Extrai texto de todos os PDFs da pasta Relatorio N3 e filtra os relatorios
emitidos no periodo de 04/05/2026 a 08/05/2026.
Gera: may_reports.json com os dados encontrados.
"""

import os
import json
import re
import sys

# Forca UTF-8 no stdout do Windows
if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
if hasattr(sys.stderr, "reconfigure"):
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

try:
    import pdfplumber
except ImportError:
    print("Instalando pdfplumber...")
    os.system(f"{sys.executable} -m pip install pdfplumber")
    import pdfplumber

PDF_DIR = os.path.join(os.path.dirname(__file__), "..")
OUTPUT_JSON = os.path.join(os.path.dirname(__file__), "may_reports.json")
OUTPUT_TXT  = os.path.join(os.path.dirname(__file__), "may_reports.txt")

# Datas alvo: 04/05 a 08/05 (qualquer ano, mas esperamos 2026)
DATE_PATTERN = re.compile(r"\b(0[4-8]/05/20\d{2})\b")

def extract_field(text, label_variants, next_label_variants=None):
    """Tenta extrair o conteúdo de um campo pelo rótulo."""
    for label in label_variants:
        idx = text.find(label)
        if idx != -1:
            start = idx + len(label)
            # Avança espaços/dois-pontos
            while start < len(text) and text[start] in (' ', ':', '\n', '\r'):
                start += 1
            # Vai até o próximo rótulo ou fim
            end = len(text)
            if next_label_variants:
                for nl in next_label_variants:
                    ni = text.find(nl, start)
                    if ni != -1 and ni < end:
                        end = ni
            return text[start:end].strip()
    return ""

def parse_report(text, filename):
    """Extrai campos estruturados de um relatório FRGH.049."""

    # Normaliza espaços múltiplos
    clean = re.sub(r'[ \t]{2,}', ' ', text)

    data_match = re.search(r'Data[:\s]*(\d{2}/\d{2}/\d{4})', clean)
    data_val = data_match.group(1) if data_match else ""

    serie_match = re.search(r'Número\s+de\s+Série\s*/\s*Lote[:\s]*([\w]+)', clean)
    serie_val = serie_match.group(1) if serie_match else ""

    hgid_match = re.search(r'HGID[:\s]*([\w]+)', clean)
    hgid_val = hgid_match.group(1) if hgid_match else ""

    cliente_match = re.search(r'Cliente\s+\(caso\s+aplicável\)[:\s]*(.+?)(?=\d\.|\n{2}|PROBLEMA)', clean, re.DOTALL)
    cliente_val = cliente_match.group(1).strip() if cliente_match else ""
    cliente_val = re.sub(r'\s+', ' ', cliente_val)

    problema_cliente_match = re.search(
        r'PROBLEMA\s+RELATADO\s+PELO\s+CLIENTE[:\s]*(.+?)(?=PROBLEMA\s+CONSTATADO|3\.)', clean, re.DOTALL
    )
    prob_cliente = problema_cliente_match.group(1).strip() if problema_cliente_match else ""
    prob_cliente = re.sub(r'\s+', ' ', prob_cliente)

    problema_suporte_match = re.search(
        r'PROBLEMA\s+CONSTATADO\s+PELO\s+SUPORTE[:\s]*(.+?)(?=3\.|ANÁLISE|RESULTADO)', clean, re.DOTALL
    )
    prob_suporte = problema_suporte_match.group(1).strip() if problema_suporte_match else ""
    prob_suporte = re.sub(r'\s+', ' ', prob_suporte)

    investigacao_match = re.search(
        r'RESULTADO\s+DA\s+INVESTIGAÇÃO[:\s]*(.+?)(?=OBSERVAÇÕES|4\.|EVIDÊNCIA)', clean, re.DOTALL
    )
    investigacao = investigacao_match.group(1).strip() if investigacao_match else ""
    investigacao = re.sub(r'\s+', ' ', investigacao)

    obs_match = re.search(
        r'OBSERVAÇÕES[:\s]*(.+?)(?=4\.|EVIDÊNCIA|5\.|DISPOSIÇÃO)', clean, re.DOTALL
    )
    obs = obs_match.group(1).strip() if obs_match else ""
    obs = re.sub(r'\s+', ' ', obs)

    return {
        "arquivo": filename,
        "data": data_val,
        "numero_serie": serie_val,
        "hgid": hgid_val,
        "cliente": cliente_val,
        "problema_cliente": prob_cliente,
        "problema_suporte": prob_suporte,
        "investigacao": investigacao,
        "observacoes": obs,
    }

def classify_failure(report):
    """Classifica o motivo de recolhimento."""
    text = (report["problema_suporte"] + " " + report["investigacao"] + " " + report["problema_cliente"]).lower()

    if any(k in text for k in ["htu", "autenticação do htu", "autenticação htu"]):
        return "Falha de Autenticação do HTU (LED vermelho)"
    if any(k in text for k in ["bomba", "válvula", "pneumática"]):
        return "Falha nas Bombas Pneumáticas / Válvulas"
    if any(k in text for k in ["sensor", "calibr", "tgs", "mcp3564", "offset", "descalibr", "h2", "ch4"]):
        return "Falha / Descalibração de Sensores"
    if any(k in text for k in ["preventiva", "250 exames", "manutenção preventiva"]):
        return "Manutenção Preventiva (250 exames)"
    if any(k in text for k in ["não liga", "doa", "não acende", "sem energia", "sem ligar", "nao liga"]):
        return "Aparelho Não Liga (DOA)"
    if any(k in text for k in ["conexão", "software", "porta com", "driver"]):
        return "Falha de Conexão / Hardware Geral"
    if any(k in text for k in ["firmware"]):
        return "Falha de Firmware"
    return "Outros / Investigação"

def main():
    pdf_files = [f for f in os.listdir(PDF_DIR) if f.lower().endswith(".pdf")]
    print(f"Total de PDFs encontrados: {len(pdf_files)}")

    all_reports = []
    may_reports = []
    errors = []

    for fname in sorted(pdf_files):
        fpath = os.path.join(PDF_DIR, fname)
        print(f"  Processando: {fname}...")
        try:
            full_text = ""
            with pdfplumber.open(fpath) as pdf:
                for page in pdf.pages:
                    t = page.extract_text()
                    if t:
                        full_text += t + "\n"

            # Divide em relatórios individuais pelo marcador FRGH.049
            # Alguns PDFs contêm múltiplos relatórios
            segments = re.split(r'(?=SISTEMA DE GESTÃO DA QUALIDADE)', full_text)
            segments = [s for s in segments if len(s.strip()) > 100]

            # Agrupa segmentos que pertencem ao mesmo relatório
            # (um relatório começa quando há campo "Data:" com uma data)
            report_texts = []
            current = ""
            for seg in segments:
                if re.search(r'Data[:\s]*\d{2}/\d{2}/\d{4}', seg):
                    if current:
                        report_texts.append(current)
                    current = seg
                else:
                    current += "\n" + seg
            if current:
                report_texts.append(current)

            if not report_texts:
                report_texts = [full_text]

            for i, rtext in enumerate(report_texts):
                date_match = re.search(r'Data[:\s]*(\d{2}/\d{2}/\d{4})', rtext)
                if not date_match:
                    continue

                date_str = date_match.group(1)
                report = parse_report(rtext, fname if len(report_texts) == 1 else f"{fname} [parte {i+1}]")
                report["categoria"] = classify_failure(report)
                all_reports.append(report)

                # Filtra período 04/05 a 08/05/2026
                date_match2 = re.match(r'(\d{2})/(\d{2})/(\d{4})', date_str)
                if date_match2:
                    day = int(date_match2.group(1))
                    month = int(date_match2.group(2))
                    year = int(date_match2.group(3))
                    if month == 5 and 4 <= day <= 8:
                        may_reports.append(report)
                        print(f"    [OK] PERIODO 04-08/05: {date_str} - {report['cliente']}")

        except Exception as e:
            errors.append({"arquivo": fname, "erro": str(e)})
            print(f"    [ERRO]: {e}")

    print(f"\n{'='*60}")
    print(f"Total de relatórios processados: {len(all_reports)}")
    print(f"Relatórios do período 04-08/05: {len(may_reports)}")
    print(f"Erros: {len(errors)}")

    # Salva JSON completo
    output = {
        "periodo": "04/05/2026 a 08/05/2026",
        "total_periodo": len(may_reports),
        "relatorios": may_reports,
        "todos_relatorios": all_reports,
        "erros": errors
    }

    with open(OUTPUT_JSON, "w", encoding="utf-8") as f:
        json.dump(output, f, ensure_ascii=False, indent=2)
    print(f"\nSalvo em: {OUTPUT_JSON}")

    # Gera txt legível
    with open(OUTPUT_TXT, "w", encoding="utf-8") as f:
        f.write(f"RELATÓRIOS DO PERÍODO 04/05 A 08/05/2026\n")
        f.write("="*60 + "\n\n")
        if not may_reports:
            f.write("Nenhum relatório encontrado neste período.\n")
            f.write("Relatórios encontrados em outros períodos:\n\n")
            for r in all_reports:
                f.write(f"  - {r['data']} | {r['cliente']} | {r['categoria']}\n")
        else:
            for r in may_reports:
                f.write(f"Data: {r['data']}\n")
                f.write(f"Cliente: {r['cliente']}\n")
                f.write(f"Série: {r['numero_serie']} | HGID: {r['hgid']}\n")
                f.write(f"Categoria: {r['categoria']}\n")
                f.write(f"Problema cliente: {r['problema_cliente']}\n")
                f.write(f"Constatado suporte: {r['problema_suporte']}\n")
                f.write("-"*60 + "\n")

    print(f"Texto salvo em: {OUTPUT_TXT}")

    # Resumo por categoria
    if may_reports:
        print("\n=== RESUMO POR CATEGORIA (04-08/05) ===")
        cats = {}
        for r in may_reports:
            cats[r["categoria"]] = cats.get(r["categoria"], 0) + 1
        for cat, cnt in sorted(cats.items(), key=lambda x: -x[1]):
            pct = cnt / len(may_reports) * 100
            print(f"  [{cnt}] {pct:.1f}% — {cat}")
    else:
        print("\n[AVISO] Nenhum relatorio do periodo 04-08/05 foi encontrado.")
        print("   Datas encontradas nos PDFs:")
        dates = sorted(set(r["data"] for r in all_reports if r["data"]))
        for d in dates:
            print(f"   - {d}")

if __name__ == "__main__":
    main()
