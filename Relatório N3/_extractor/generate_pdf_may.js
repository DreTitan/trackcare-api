const puppeteer = require('puppeteer');
const path = require('path');

(async () => {
  const browser = await puppeteer.launch({ headless: "new" });
  const page = await browser.newPage();

  const htmlPath = path.resolve(__dirname, 'relatorio_consolidado_maio.html');
  const pdfPath = path.resolve(__dirname, '..', 'Relatorio_Consolidado_Recolhimentos_Maio_2026.pdf');

  console.log(`Carregando HTML de: ${htmlPath}`);
  await page.goto(`file:///${htmlPath.replace(/\\/g, '/')}`, { waitUntil: 'networkidle0', timeout: 30000 });

  // Espera carregar fontes e renderizar
  await page.evaluateHandle('document.fonts.ready');
  await new Promise(r => setTimeout(r, 2000));

  await page.pdf({
    path: pdfPath,
    format: 'A4',
    printBackground: true,
    margin: { top: '0', right: '0', bottom: '0', left: '0' },
    preferCSSPageSize: true
  });

  console.log(`PDF gerado com sucesso em: ${pdfPath}`);
  await browser.close();
})();
