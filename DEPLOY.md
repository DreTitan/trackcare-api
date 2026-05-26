# TrackCare - Deploy Manual no Azure (sem Docker)

## Passo 1: Criar Tabelas no Supabase (5 minutos)

1. Acesse: https://supabase.com/dashboard/project/ewcldebtsrjfdkckvqin/sql
2. Clique em **New Query**
3. Copie TODO o conteúdo do arquivo `migrations.sql` (raiz do projeto)
4. Cole no editor SQL
5. Clique **Run** (ou Ctrl+Enter)
6. Você verá "Success" se as tabelas forem criadas

---

## Passo 2: Criar App Service no Azure (10 minutos)

1. Acesse: https://portal.azure.com
2. Clique **Criar um recurso**
3. Pesquise **App Service** → **Criar**
4. Preencha:
   - **Nome do aplicativo**: `trackcare-api` (ou outro único)
   - **Publicar**: Código
   - **Runtime**: .NET 8
   - **Sistema Operacional**: Linux
   - **Região**: Brazil South
   - **Plano de Hospedagem**: clique em **Criar novo**
     - Nome: `trackcare-plan`
     - **SKU e tamanho**: clique em **Alterar tamanho** → selecione **Free (F1)**
5. Clique **Revisar + criar** → **Criar**
6. Aguarde ~2 minutos → clique em **Ir para o recurso**

---

## Passo 3: Configurar Variáveis de Ambiente

1. No App Service, menu lateral: **Configurações → Variáveis de aplicativo**
2. Clique em **+ Nova aplicação**
3. Adicione CADA UMA destas (5 no total):

| Nome | Valor |
|------|-------|
| `DB_CONNECTION_STRING` | `postgresql://postgres:ewcldebtsrjfdkckvqin@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres` |
| `SUPABASE_URL` | `https://ewcldebtsrjfdkckvqin.supabase.co` |
| `SUPABASE_KEY` | `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV3Y2xkZWJ0c3JqZmRrY2t2cWluIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Nzg4MTcxNzksImV4cCI6MjA5NDM5MzE3OX0.PhQPmhTrhNMGRLEtrOBAc-9heIf451rKkL0KzPu7r4s` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `FRONTEND_URL` | `*` |

4. Clique **Salvar** (ícone 💾)

---

## Passo 4: Deploy do Código

1. No App Service, menu lateral: **Implantação → Centro de Implantação**
2. Na seção **Configurações básicas**:
   - **Source**: Local Git ou ZIP
   - Selecione **ZIP**
3. Clique em **Salvar**
4. **Fazer Upload do ZIP**:
   - Arraste o arquivo `publish.zip` da raiz do projeto para a área de upload
   - Ou clique em **Procurar** e selecione `C:\Users\HealthGo\Desktop\HealthGo\publish.zip`
5. Aguarde a mensagem "Implantação bem-sucedida"

---

## Passo 5: Verificar

1. Acesse: `https://NOME-DO-SEU-APP.azurewebsites.net/api/recolhimentos`
   - Deve retornar `[]` (array vazio, sem dados ainda)
2. Swagger: `https://NOME-DO-SEU-APP.azurewebsites.net/swagger`

---

## Passo 6: Deploy do Frontend (opcional)

1. Edite o arquivo `src-react/trackcare-client/.env`:
   ```
   VITE_API_URL=https://NOME-DO-SEU-APP.azurewebsites.net/api
   ```
2. No terminal:
   ```powershell
   cd C:\Users\HealthGo\Desktop\HealthGo\src-react\trackcare-client
   npm install
   npm run build
   ```
3. Deploy no **Vercel** (https://vercel.com) — drag & drop da pasta `dist/`
   - Ou Azure Static Web Apps
