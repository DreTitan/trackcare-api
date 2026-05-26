const fs = require('fs');
const path = require('path');
const { PDFParse } = require('pdf-parse');

const pdfDir = path.resolve(__dirname, '..');

async function extractAll() {
  const files = fs.readdirSync(pdfDir).filter(f => f.endsWith('.pdf'));
  const results = {};

  for (const file of files) {
    const filePath = path.join(pdfDir, file);
    try {
      const dataBuffer = fs.readFileSync(filePath);
      const parser = new PDFParse({ data: new Uint8Array(dataBuffer) });
      await parser.load();
      // Try to get text from pages
      let text = '';
      if (parser.doc) {
        const numPages = parser.doc.numPages;
        for (let i = 1; i <= numPages; i++) {
          const page = await parser.doc.getPage(i);
          const content = await page.getTextContent();
          const strings = content.items.map(item => item.str);
          text += strings.join(' ') + '\n';
        }
      }
      results[file] = text;
      console.log(`=== EXTRACTED: ${file} (${text.length} chars) ===`);
    } catch (err) {
      results[file] = `ERROR: ${err.message}`;
      console.error(`ERROR reading ${file}: ${err.message}`);
    }
  }

  // Write all extracted text to a JSON file
  const outputPath = path.join(__dirname, 'extracted_texts.json');
  fs.writeFileSync(outputPath, JSON.stringify(results, null, 2), 'utf-8');
  console.log(`\nAll texts saved to: ${outputPath}`);

  // Also write a readable text version
  const txtPath = path.join(__dirname, 'extracted_texts.txt');
  let txt = '';
  for (const [file, text] of Object.entries(results)) {
    txt += `\n${'='.repeat(80)}\n`;
    txt += `ARQUIVO: ${file}\n`;
    txt += `${'='.repeat(80)}\n`;
    txt += text + '\n';
  }
  fs.writeFileSync(txtPath, txt, 'utf-8');
  console.log(`Readable version saved to: ${txtPath}`);
}

extractAll().catch(console.error);
