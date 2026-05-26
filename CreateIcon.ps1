Add-Type -AssemblyName System.Drawing

$size = 256
$bitmap = New-Object System.Drawing.Bitmap($size, $size)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAliasGridFit

# Fundo azul gradiente (círculocircular)
$brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
    (New-Object System.Drawing.Point(0, 0)),
    (New-Object System.Drawing.Point($size, $size)),
    [System.Drawing.Color]::FromArgb(59, 130, 246),
    [System.Drawing.Color]::FromArgb(30, 64, 175)
)
$graphics.FillEllipse($brush, 8, 8, 240, 240)

# Letra H branca
$font = New-Object System.Drawing.Font("Segoe UI", 130, [System.Drawing.FontStyle]::Bold)
$sf = New-Object System.Drawing.StringFormat
$sf.Alignment = [System.Drawing.StringAlignment]::Center
$sf.LineAlignment = [System.Drawing.StringAlignment]::Center
$brushWhite = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
$graphics.DrawString("H", $font, $brushWhite, (New-Object System.Drawing.RectangleF(0, 0, $size, $size)), $sf)

# Contorno circular branco
$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, 6)
$graphics.DrawEllipse($pen, 12, 12, 232, 232)

# Símbolo de + (saúde)
$penGreen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(16, 185, 129), 18)
$graphics.DrawLine($penGreen, 170, 60, 200, 90)
$graphics.DrawLine($penGreen, 170, 90, 200, 60)

# Salvar como PNG
$pngPath = "C:\Users\HealthGo\Desktop\HealthGo\HealthGoWPF\icon.png"
$bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
Write-Host "PNG criado: $pngPath"

# Converter para ICO
$icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())
$icoPath = "C:\Users\HealthGo\Desktop\HealthGo\HealthGoWPF\icon.ico"
$fs = [System.IO.FileStream]::new($icoPath, [System.IO.FileMode]::Create)
$icon.Save($fs)
$fs.Close()

Write-Host "Ícone ICO criado: $icoPath"

# Limpar
$graphics.Dispose()
$bitmap.Dispose()
$brush.Dispose()
$brushWhite.Dispose()
$font.Dispose()
$pen.Dispose()
$penGreen.Dispose()

Write-Host "Pronto!"
