# Deploy Azure — PointsTableAndExams

**Stack:** Azure SQL Serverless · Azure App Service (Linux .NET 9) · Azure Static Web Apps · GitHub Actions

---

## Pré-requisitos

- Conta Azure ativa → [portal.azure.com](https://portal.azure.com)
- Azure CLI instalada → `winget install Microsoft.AzureCLI`
- Repositório no GitHub com o código

---

## 1. Login Azure CLI

```bash
az login
az account show  # confirma a subscription ativa
```

---

## 2. Criar Resource Group

```bash
az group create \
  --name rg-pointstable-prod \
  --location brazilsouth
```

> **`brazilsouth`** = região São Paulo. Outras opções: `eastus`, `westeurope`.

---

## 3. Banco de Dados — Azure SQL Serverless

### 3.1 Criar o SQL Server (servidor lógico)

```bash
az sql server create \
  --resource-group rg-pointstable-prod \
  --name sql-pointstable-prod \
  --location brazilsouth \
  --admin-user sqladmin \
  --admin-password "SuaSenhaForte@2024!"
```

> Guarde o `--admin-password` — você vai usar na connection string.

### 3.2 Liberar acesso dos serviços Azure (App Service → SQL)

```bash
az sql server firewall-rule create \
  --resource-group rg-pointstable-prod \
  --server sql-pointstable-prod \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### 3.3 Criar o banco Serverless

```bash
az sql db create \
  --resource-group rg-pointstable-prod \
  --server sql-pointstable-prod \
  --name sqldb-pointstable-prod \
  --edition GeneralPurpose \
  --family Gen5 \
  --capacity 1 \
  --compute-model Serverless \
  --auto-pause-delay 60 \
  --min-capacity 0.5 \
  --backup-storage-redundancy Local
```

**Por que Serverless?**
- Pausa automaticamente após 60 min sem uso → cobra ~US$0 parado
- Escala de 0.5 a 1 vCore conforme demanda
- `--backup-storage-redundancy Local` = mais barato (sem geo-redundância)

### 3.4 Pegar a Connection String

```bash
az sql db show-connection-string \
  --server sql-pointstable-prod \
  --name sqldb-pointstable-prod \
  --client ado.net
```

Saída similar a:
```
Server=tcp:sql-pointstable-prod.database.windows.net,1433;Initial Catalog=sqldb-pointstable-prod;Persist Security Info=False;User ID=sqladmin;Password={your_password};...
```

Substitua `{your_password}` pela senha do passo 3.1. Guarde essa string — usada no passo 5.

---

## 4. Backend — Azure App Service

### 4.1 Criar App Service Plan (Linux B1)

```bash
az appservice plan create \
  --resource-group rg-pointstable-prod \
  --name asp-pointstable-prod \
  --sku B1 \
  --is-linux
```

> **B1** = ~US$13/mês. F1 (free) não suporta Always-On e reinicia após inatividade.

### 4.2 Criar Web App (.NET 9)

```bash
az webapp create \
  --resource-group rg-pointstable-prod \
  --plan asp-pointstable-prod \
  --name app-pointstable-api \
  --runtime "DOTNETCORE:9.0"
```

> O nome `app-pointstable-api` precisa ser único no Azure. Se falhar, tente `app-pointstable-api-prod` ou adicione um sufixo numérico.

### 4.3 Configurar variáveis de ambiente (App Settings)

```bash
az webapp config appsettings set \
  --resource-group rg-pointstable-prod \
  --name app-pointstable-api \
  --settings \
    ASPNETCORE_ENVIRONMENT="Production" \
    ConnectionStrings__DefaultConnection="<CONNECTION_STRING_DO_PASSO_3.4>" \
    JwtSettings__Secret="<CHAVE_JWT_FORTE_MIN_32_CHARS>" \
    JwtSettings__Issuer="PointsTableAndExams.Api" \
    JwtSettings__Audience="PointsTableAndExams.Client" \
    JwtSettings__ExpirationHours="8" \
    Gemini__ApiKey="<SUA_CHAVE_AIzaSy_DO_GOOGLE_CLOUD>" \
    Gemini__BaseUrl="https://generativelanguage.googleapis.com" \
    Gemini__ApiVersion="v1beta" \
    Gemini__Model="gemini-flash-latest" \
    AllowedOrigins__0="https://<seu-app>.azurestaticapps.net"
```

> **IMPORTANTE:** A chave Gemini do tipo `AQ.` expira a cada hora. Use uma chave permanente `AIzaSy...` gerada em:
> [console.cloud.google.com](https://console.cloud.google.com) → APIs & Services → Credentials → Create API Key

### 4.4 Baixar o Publish Profile (para o GitHub Actions)

No portal Azure:
1. Abra o App Service `app-pointstable-api`
2. Menu lateral → **"Get publish profile"**
3. Baixa um arquivo `.PublishSettings`
4. Copie o conteúdo inteiro desse arquivo → GitHub Secret `AZURE_WEBAPP_PUBLISH_PROFILE`

Ou via CLI:
```bash
az webapp deployment list-publishing-profiles \
  --resource-group rg-pointstable-prod \
  --name app-pointstable-api \
  --xml
```

---

## 5. Frontend — Azure Static Web Apps

### 5.1 Criar o Static Web App vinculado ao GitHub

```bash
az staticwebapp create \
  --resource-group rg-pointstable-prod \
  --name stapp-pointstable \
  --source https://github.com/<SEU_USUARIO>/<SEU_REPO> \
  --branch main \
  --app-location "2-FrontEnd" \
  --output-location "dist" \
  --login-with-github
```

Isso vai:
- Pedir autenticação GitHub
- Criar o Static Web App
- Adicionar automaticamente o secret `AZURE_STATIC_WEB_APPS_API_TOKEN` no seu repositório GitHub

> **Anote a URL gerada**: algo como `https://proud-stone-0a1b2c3d4.azurestaticapps.net`
> Essa URL vai no `AllowedOrigins__0` do App Service (passo 4.3) e no GitHub Secret `VITE_API_URL`.

---

## 6. Configurar GitHub Secrets

Vá em: **GitHub Repo → Settings → Secrets and variables → Actions → New repository secret**

| Secret | Valor |
|--------|-------|
| `AZURE_WEBAPP_NAME` | `app-pointstable-api` |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Conteúdo do arquivo `.PublishSettings` do passo 4.4 |
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Gerado automaticamente no passo 5.1 (verifique se já existe) |
| `VITE_API_URL` | `https://app-pointstable-api.azurewebsites.net/api` |

---

## 7. Primeiro Deploy

Depois de configurar todos os secrets, faça um push para `main`:

```bash
git add .
git commit -m "chore: add CI/CD workflows and Azure configuration"
git push origin main
```

Acompanhe em: **GitHub → Actions** — dois workflows vão rodar:
- `Backend — Build & Deploy` → deploya a API no App Service
- `Frontend — Build & Deploy` → deploya o React no Static Web App

O App Service executa `MigrateDatabaseAsync()` no startup, criando todas as tabelas automaticamente no Azure SQL.

---

## 8. Verificar

```bash
# Checar se a API está respondendo
curl https://app-pointstable-api.azurewebsites.net/api/health

# Ver logs em tempo real
az webapp log tail \
  --resource-group rg-pointstable-prod \
  --name app-pointstable-api
```

---

## Custos Estimados (por mês)

| Recurso | Tier | Custo estimado |
|---------|------|----------------|
| Azure SQL Serverless | GP Gen5 1vCore, autopause | US$0–5 (uso leve) |
| App Service | B1 Linux | ~US$13 |
| Static Web Apps | Free | US$0 |
| **Total** | | **~US$13–18/mês** |

---

## Chave Gemini Permanente (importante!)

A chave `AQ.` do AI Studio expira a cada hora. Para produção:

1. Acesse [console.cloud.google.com](https://console.cloud.google.com)
2. Selecione ou crie um projeto
3. Vá em **APIs & Services → Library**
4. Ative a **"Generative Language API"**
5. Vá em **APIs & Services → Credentials → Create credentials → API Key**
6. Copie a chave `AIzaSy...` e coloque no App Setting `Gemini__ApiKey`
