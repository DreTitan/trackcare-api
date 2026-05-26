const puppeteer = require('puppeteer');
const path = require('path');

const htmlPath = path.resolve(__dirname, 'Relatório Consolidado - Motivos de Recolhimento HealthGo Air (01-04 a 22-05-2026).html');
const outputPath = path.resolve(__dirname, 'Relatorio_Consolidado_Recolhimentos_Maio2026.pdf');

(async () => {
  const browser = await puppeteer.launch({
    headless: true,
    args: ['--no-sandbox', '--disable-setuid-sandbox']
  });

  const page = await browser.newPage();

  // Load the HTML file
  await page.goto(`file://${htmlPath}`, {
    waitUntil: 'networkidle0',
    timeout: 60000
  });

  // Wait for fonts to load
  await page.evaluate(() => document.fonts.ready);

  // Generate PDF
  await page.pdf({
    path: outputPath,
    format: 'A4',
    printBackground: true,
    margin: {
      top: '0',
      right: '0',
      bottom: '0',
      left: '0'
    }
  });

  console.log(`PDF gerado: ${outputPath}`);

  await browser.close();
})();
