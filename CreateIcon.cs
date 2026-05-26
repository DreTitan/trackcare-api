using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

class Program
{
    static void Main()
    {
        // Criar imagem 256x256
        using (var bmp = new Bitmap(256, 256))
        {
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                // Fundo azul gradiente
                var rect = new Rectangle(0, 0, 256, 256);
                using (var brush = new LinearGradientBrush(rect, Color.FromArgb(59, 130, 246), Color.FromArgb(30, 64, 175), 45f))
                {
                    g.FillEllipse(brush, 10, 10, 236, 236);
                }

                // Letter H
                using (var font = new Font("Segoe UI", 140, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString("H", font, brush, new RectangleF(0, 0, 256, 256), sf);
                }

                // Círculos decorativos
                using (var pen = new Pen(Color.White, 8))
                {
                    g.DrawEllipse(pen, 15, 15, 226, 226);
                }

                // Simbolo de + (representando医疗/saúde)
                using (var pen = new Pen(Color.FromArgb(16, 185, 129), 20))
                {
                    // Linha horizontal
                    g.DrawLine(pen, 175, 70, 205, 100);
                    g.DrawLine(pen, 175, 100, 205, 70);
                }
            }

            // Salvar como PNG temporário
            var pngPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "icon.png");
            bmp.Save(pngPath, System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine($"PNG salvo em: {pngPath}");
        }

        // Converter PNG para ICO
        ConvertToIcon("icon.png", "icon.ico");
        Console.WriteLine("Ícone criado com sucesso!");
    }

    static void ConvertToIcon(string pngPath, string icoPath)
    {
        using (var img = Image.FromFile(pngPath))
        {
            var ms = new MemoryStream();
            // Salvar como ICO
            var icon = Icon.FromHandle(((Bitmap)img).GetHicon());
            var fs = new FileStream(icoPath, FileMode.Create);
            icon.Save(fs);
            fs.Close();
        }
    }
}
