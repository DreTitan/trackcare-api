const { chromium } = require('playwright');
const path = require('path');

const htmlPath = path.resolve(__dirname, 'Relatorio_Motivos_Chamadores_Maio2026.html');
const outputPath = path.resolve(__dirname, 'Relatorio_Motivos_Chamadores_Maio2026.pdf');

(async () => {
  const browser = await chromium.launch({ headless: true });
  const page = await browser.newPage();

  await page.goto(`file://${htmlPath}`, { waitUntil: 'networkidle' });
  await page.evaluate(() => document.fonts.ready);

  await page.pdf({
    path: outputPath,
    format: 'A4',
    printBackground: true,
    margin: { top: '0', right: '0', bottom: '0', left: '0' },
    scale: 1
  });

  console.log(`PDF gerado: ${outputPath}`);
  await browser.close();
})();
