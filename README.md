# VitaLog

**VitaLog** é uma plataforma fullstack para controle diário de pontos alimentares e gerenciamento de exames laboratoriais, desenvolvida com as melhores práticas de arquitetura de software.

🌐 **Produção:** [vitalog.app.br](https://vitalog.app.br)

---

## Índice

- [Visão Geral](#visão-geral)
- [Stack Tecnológica](#stack-tecnológica)
- [Arquitetura](#arquitetura)
- [Modelagem de Dados](#modelagem-de-dados)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Padrões e Princípios](#padrões-e-princípios)
- [Camadas da API](#camadas-da-api)
- [Frontend](#frontend)
- [Testes](#testes)
- [Deploy e CI/CD](#deploy-e-cicd)
- [Como Executar](#como-executar)
- [Decisões Arquiteturais](#decisões-arquiteturais)

---

## Visão Geral

O **VitaLog** é uma plataforma que permite:

- **Tabela de Pontos**: controle diário do consumo alimentar baseado em um sistema de pontos (máx. 300 pts/dia), com mais de 200 alimentos categorizados.
- **Gestão de Exames**: solicitação, acompanhamento e registro de resultados de exames laboratoriais organizados em 12 categorias clínicas.
- **Análise por Foto (IA)**: identificação automática de alimentos via foto usando Google Gemini Vision — suporta câmera e galeria do dispositivo.
- **Dashboard Inteligente**: painel com dados reais de pontos do dia, itens registrados e exames pendentes.
- **Autenticação JWT**: cadastro e login de usuários com tokens seguros.

---

## Stack Tecnológica

### Backend

| Tecnologia | Versão | Uso |
|---|---|---|
| **.NET** | 9.0 | Plataforma principal |
| **C#** | 13 | Linguagem |
| **ASP.NET Core Web API** | 9.0 | Framework HTTP |
| **Entity Framework Core** | 9.0 | ORM |
| **SQL Server** | 2019+ | Banco de dados |
| **MediatR** | 12.4 | Mediador CQRS |
| **FluentValidation** | 11 | Validação de comandos |
| **BCrypt.Net-Next** | 4.0 | Hash de senhas |
| **JWT Bearer** | 9.0 | Autenticação |
| **Serilog** | 9.0 | Logging estruturado |
| **Swashbuckle (Swagger)** | 7.3 | Documentação da API |
| **Google Gemini Vision** | 2.0 Flash | Análise de fotos de alimentos |

### Testes Backend (.NET)

| Tecnologia | Versão | Uso |
|---|---|---|
| **xUnit** | 2.9 | Framework de testes |
| **FluentAssertions** | 6.12 | Asserções expressivas |
| **NSubstitute** | 5.3 | Mocks e stubs |
| **NetArchTest** | 1.3 | Testes de arquitetura |
| **Bogus** | 35.6 | Geração de dados falsos |
| **Testcontainers (MsSql)** | 4.4 | SQL Server em Docker para integração |
| **Microsoft.AspNetCore.Mvc.Testing** | 9.0 | WebApplicationFactory |
| **Reqnroll** | 3.0 | BDD / Gherkin (SpecFlow successor) |
| **Microsoft.Playwright** | 1.52 | Testes E2E (.NET) |

### Testes E2E (Playwright TypeScript)

| Tecnologia | Versão | Uso |
|---|---|---|
| **Playwright** | 1.52 | Framework E2E cross-browser |
| **TypeScript** | 5+ | Linguagem dos testes |
| **@axe-core/playwright** | 4.x | Auditoria de acessibilidade WCAG 2.1 AA |
| **Page Object Model** | — | Padrão de organização dos testes |

### Frontend

| Tecnologia | Versão | Uso |
|---|---|---|
| **React** | 19 | Framework de UI |
| **TypeScript** | 6 (strict) | Tipagem estática, zero `any` |
| **Vite** | 8 | Build tool + HMR |
| **React Router** | 7 | Roteamento SPA com layout routes |
| **TanStack Query** | 5 | Server state, cache, mutations |
| **Zustand** | 5 | Client state (auth persisted) |
| **React Hook Form** | 7 | Formulários performáticos |
| **Zod** | 4 | Validação de schemas |
| **Axios** | 1 | HTTP client + interceptors JWT |
| **clsx** | 2 | Composição condicional de classes |
| **CSS Modules** | — | Estilos escopados por componente |

### Base de Dados (SQL)

| Arquivo | Conteúdo |
|---|---|
| `01_DDL_Create_Tables.sql` | Criação de todas as tabelas, constraints e índices |
| `02_DML_FoodCategory_and_FoodItem.sql` | Seed de categorias e alimentos (200+ itens) |
| `03_DML_ExamCategory_and_Exam.sql` | Seed de categorias e exames (~100 exames) |
| `04_DML_Sample_Users.sql` | Dados de exemplo com logs e solicitações |
| `05_Views_and_Useful_Queries.sql` | Views e queries reutilizáveis |

---

## Arquitetura

O projeto segue **Clean Architecture** com separação estrita de responsabilidades em 4 camadas concêntricas. As dependências fluem de fora para dentro — **nunca** o contrário.

```
┌─────────────────────────────────────────────┐
│                    API                      │  ← Apresentação (Controllers, Middleware)
├─────────────────────────────────────────────┤
│              Infrastructure                 │  ← Dados (EF Core, JWT, BCrypt, Serilog)
├─────────────────────────────────────────────┤
│               Application                  │  ← Casos de uso (CQRS, Validação, Pipeline)
├─────────────────────────────────────────────┤
│                 Domain                      │  ← Núcleo do negócio (Entidades, Regras)
└─────────────────────────────────────────────┘

Fluxo de dependência:
  API → Infrastructure → Application → Domain
  Domain não conhece nenhuma outra camada.
```

### Fluxo de uma Requisição

```
HTTP Request
    │
    ▼
Controller (Api)
    │  new Command/Query
    ▼
MediatR Pipeline
    ├── LoggingBehavior   (log entrada/saída)
    └── ValidationBehavior (FluentValidation)
         │
         ▼
    CommandHandler / QueryHandler (Application)
         │  IRepository / IUnitOfWork
         ▼
    Repository (Infrastructure / EF Core)
         │
         ▼
    SQL Server
```

---

## Modelagem de Dados

### Diagrama de Entidades

```
Users
├── Id (PK, Guid)
├── FullName          NVARCHAR(150)
├── Email             NVARCHAR(150) UNIQUE   ← Value Object
├── PhoneNumber       NVARCHAR(20)
├── BirthDate         DATE
├── Gender            CHAR(1)  [M|F|O]
├── Username          NVARCHAR(80) UNIQUE
├── PasswordHash      NVARCHAR(256)
├── IsActive          BIT
├── CreatedAt         DATETIME2
└── UpdatedAt         DATETIME2

FoodCategory ──< FoodItem
├── Id                Guid (PK)
├── Name              NVARCHAR(100)
├── DefaultQuotaPoints SMALLINT         ← ex: 25 pts para Carnes
├── ServingUnit       NVARCHAR(100)     ← ex: "2 colheres de sopa"
└── SortOrder         TINYINT

FoodItem
├── Id                Guid (PK)
├── FoodCategoryId    Guid (FK)
├── Name              NVARCHAR(150)
├── ServingSize       NVARCHAR(100)
├── Points            SMALLINT          ← Value Object
└── IsActive          BIT

DailyLog ──< DailyLogItem
├── Id                Guid (PK)         ← Aggregate Root
├── UserId            Guid (FK)
├── LogDate           DATE
├── TotalPoints       SMALLINT          ← recalculado automaticamente
└── Notes             NVARCHAR(500)

DailyLogItem
├── Id                Guid (PK)
├── DailyLogId        Guid (FK)
├── FoodItemId        Guid (FK)
├── Quantity          DECIMAL(5,2)
├── PointsComputed    SMALLINT
└── MealTime          TIME

ExamCategory ──< Exam
├── Id                Guid (PK)
├── Name              NVARCHAR(100)     ← Bioquímica, Imunologia, etc.
└── SortOrder         TINYINT

Exam
├── Id                Guid (PK)
├── ExamCategoryId    Guid (FK)
├── Name              NVARCHAR(150)
├── Abbreviation      NVARCHAR(50)
└── Description       NVARCHAR(300)

ExamRequest ──< ExamRequestItem
├── Id                Guid (PK)         ← Aggregate Root
├── UserId            Guid (FK)
├── RequestDate       DATE
├── DoctorName        NVARCHAR(150)
└── Notes             NVARCHAR(500)

ExamRequestItem
├── Id                Guid (PK)
├── ExamRequestId     Guid (FK)
├── ExamId            Guid (FK)
├── IsCompleted       BIT
├── CompletedDate     DATE
├── Result            NVARCHAR(500)
└── Laboratory        NVARCHAR(150)
```

### Categorias de Alimentos (13 categorias, 200+ itens)

| Categoria | Pontos/Quota | Exemplo de Porção |
|---|---|---|
| Vegetables | 0 (livre) | À vontade |
| Legumes | 10 | 2 colheres de sopa |
| Meats | 25 | 1 pires de chá |
| Cheeses | 25 | 1 fatia fina |
| Grains and Starches | 20 | 2 colheres de sopa |
| Fruits | 15 | 1 unidade |
| Fast Food / Snacks | variável | por porção |
| Fat-Free Sweets | variável | por porção |
| High-Fat Sweets | variável | por porção |
| Condiments | variável | por porção |
| Others | variável | por porção |
| Beverages | variável | por copo/dose |
| Soups | variável | por concha |

### Categorias de Exames (12 categorias, ~100 exames)

| Categoria | Exemplos |
|---|---|
| Biochemistry (Blood) | Glicose, HbA1c, Colesterol, TGO/TGP, Creatinina... |
| Immunology | Anti-CCP, Anti-GAD, HLA-B27, Sorologia CMV... |
| Hepatitis Serology | HAV, HBV, HCV |
| HIV Serology | HIV1 + HIV2 (4ª geração) |
| Antinuclear Antibodies | ANA, Anti-dsDNA, ENA, Fator Reumatóide... |
| C-Reactive Protein | CRP quantitativo, hs-CRP cardiovascular |
| Tumor Markers | AFP, CA 15-3, CA 19-9, CEA, PSA... |
| Stool | Parasitológico, Sangue Oculto |
| Urine | EAS, Urocultura, Cortisol 24h... |
| Hematology | Hemograma, Ferritina, Coagulograma... |
| Hormones | TSH, FSH, LH, Insulina, GH, IGF-1, AMH... |
| Thyroid | TSH, T3, T4 livre, Anti-TPO, Anti-Tg... |

---

## Estrutura de Pastas

```
PointsTableAndExams/
│
├── 1-Sql/                                    ← Scripts SQL Server
│   ├── 01_DDL_Create_Tables.sql
│   ├── 02_DML_FoodCategory_and_FoodItem.sql
│   ├── 03_DML_ExamCategory_and_Exam.sql
│   ├── 04_DML_Sample_Users.sql
│   └── 05_Views_and_Useful_Queries.sql
│
├── 2-Document/                               ← Documentos fonte (PDFs)
│   ├── TabelaExames.pdf
│   ├── TabelaPontos_Pagina1.pdf
│   ├── TabelaPontos_Pagina2.pdf
│   └── TabelaPontos_Pagina3.pdf
│
├── 2-FrontEnd/                               ← SPA React + TypeScript
│   ├── index.html                            ← title: VitaLog
│   ├── vite.config.ts                        ← Proxy /api → backend :7001
│   └── src/
│       ├── design-system/                    ← Tokens CSS + componentes
│       ├── core/                             ← API client, Auth store, Router
│       ├── features/
│       │   ├── auth/                         ← Login + Register
│       │   ├── dashboard/                    ← Dashboard com dados reais
│       │   ├── food-log/                     ← Log diário + Histórico + IA foto
│       │   └── exams/                        ← Solicitações de exames
│       └── shared/                           ← Spinner, ErrorBoundary
│
├── 3-BackEnd/                                ← Solução .NET 9
│   ├── PointsTableAndExams.sln
│   ├── src/
│   │   ├── PointsTableAndExams.Domain/
│   │   │   ├── Common/                       ← AggregateRoot, Entity, Result, ValueObject
│   │   │   ├── Entities/                     ← User, DailyLog, ExamRequest, FoodItem, Exam...
│   │   │   ├── ValueObjects/                 ← Email, PhoneNumber, Points
│   │   │   ├── DomainEvents/                 ← UserCreated, DailyLogCreated, ExamCompleted...
│   │   │   ├── Enums/                        ← Gender
│   │   │   ├── Exceptions/                   ← DomainException
│   │   │   └── Interfaces/Repositories/      ← IRepository<T>, IUnitOfWork, ...
│   │   │
│   │   ├── PointsTableAndExams.Application/
│   │   │   ├── Common/Behaviors/             ← LoggingBehavior, ValidationBehavior
│   │   │   ├── Users/                        ← CreateUser, Login, GetUserById
│   │   │   ├── DailyLogs/                    ← CreateDailyLog, AddLogItem, GetDailyLogByDate
│   │   │   ├── ExamRequests/                 ← CreateExamRequest, MarkExamCompleted, GetAll
│   │   │   ├── FoodItems/                    ← GetFoodItems, AnalyzeFoodPhoto (Gemini)
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   ├── PointsTableAndExams.Infrastructure/
│   │   │   ├── Data/                         ← AppDbContext, Configurations, Repositories
│   │   │   ├── Services/                     ← PasswordHasher, JwtTokenService, GeminiVisionService
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   └── PointsTableAndExams.Api/
│   │       ├── Controllers/
│   │       │   ├── BaseApiController.cs       ← FromResult<T> helper
│   │       │   ├── AuthController.cs          ← POST /api/auth/register, POST /api/auth/login
│   │       │   ├── UsersController.cs         ← GET /api/users/{id}
│   │       │   ├── DailyLogsController.cs     ← CRUD /api/daily-logs
│   │       │   ├── FoodItemsController.cs     ← GET /api/food-items, POST /api/food-items/analyze-photo
│   │       │   ├── ExamCategoriesController.cs← GET /api/exam-categories
│   │       │   ├── ExamsController.cs         ← GET /api/exams
│   │       │   └── ExamRequestsController.cs  ← CRUD /api/exam-requests
│   │       ├── Middlewares/
│   │       │   └── ExceptionHandlingMiddleware.cs
│   │       ├── Program.cs
│   │       └── appsettings.json
│   │
│   └── tests/
│       ├── PointsTableAndExams.UnitTests/     ← xUnit, NSubstitute, NetArchTest
│       ├── PointsTableAndExams.IntegrationTests/ ← Testcontainers SQL Server
│       ├── PointsTableAndExams.BddTests/      ← Reqnroll / Gherkin
│       └── PointsTableAndExams.E2ETests/      ← Playwright (.NET)
│
├── 4-E2E/                                    ← Testes E2E Playwright TypeScript
│   ├── playwright.config.ts                  ← 2 projects: chromium + non-functional
│   ├── global-setup.ts                       ← Warmup do backend (cold-start Azure)
│   ├── fixtures/
│   │   └── auth.fixture.ts                   ← authenticatedPage fixture (login automático)
│   ├── pages/                                ← Page Object Models
│   │   ├── LoginPage.ts
│   │   ├── RegisterPage.ts
│   │   └── FoodLogPage.ts
│   └── tests/
│       ├── functional/
│       │   ├── auth/
│       │   │   ├── login.spec.ts             ← 10 testes
│       │   │   ├── logout.spec.ts            ← 2 testes
│       │   │   └── register.spec.ts          ← 12 testes
│       │   ├── dashboard/
│       │   │   └── dashboard.spec.ts         ← 11 testes
│       │   ├── exams/
│       │   │   └── exams.spec.ts             ← 7 testes
│       │   ├── food-log/
│       │   │   ├── create-entry.spec.ts      ← 6 testes
│       │   │   ├── history.spec.ts           ← 5 testes
│       │   │   ├── add-item-flow.spec.ts     ← 13 testes (incl. Gallery mode)
│       │   │   └── photo-analysis.spec.ts    ← 3 testes
│       │   └── navigation/
│       │       ├── authenticated-nav.spec.ts ← 14 testes (incl. branding VitaLog)
│       │       └── protected-routes.spec.ts  ← 7 testes
│       └── non-functional/
│           ├── accessibility/
│           │   └── a11y.spec.ts              ← 6 testes (WCAG 2.1 AA + axe-core)
│           └── performance/
│               └── page-load.spec.ts         ← 5 testes (FCP, LCP, DCL, erros JS)
│
├── .github/workflows/
│   ├── backend.yml                           ← CI/CD backend → Azure App Service
│   ├── frontend.yml                          ← Build → Azure Static Web Apps → E2E gate
│   └── e2e.yml                               ← Run manual full cross-browser E2E
│
└── README.md                                 ← Este arquivo
```

---

## Padrões e Princípios

### Domain-Driven Design (DDD)
- **Aggregate Roots**: `User`, `DailyLog`, `ExamRequest` — únicos pontos de entrada para mutação de estado.
- **Value Objects**: `Email`, `PhoneNumber`, `Points` — imutáveis, sem identidade, igualdade por valor.
- **Domain Events**: disparados dentro do agregado, publicados via MediatR no `CommitAsync`.
- **Rich Domain Model**: toda regra de negócio vive dentro das entidades, com setters privados.

### CQRS com MediatR
```
Command → altera estado       → retorna Result<T>
Query   → apenas lê dados     → retorna Result<T> com DTO
```
Cada caso de uso tem seu próprio par `Command/Query + Handler + Validator` em pasta isolada.

### Result Pattern
Sem exceções no fluxo de controle da aplicação:
```csharp
// Em vez de throw:
return Result.Failure<Guid>(new Error("User.EmailTaken", "Email já está em uso."));

// No controller:
if (result.IsFailure) return BadRequest(result.Error);
```

### Repository + Unit of Work
- `IRepository<T>` genérico com operações básicas.
- Repositórios específicos adicionam queries de negócio.
- `IUnitOfWork.CommitAsync()` persiste tudo e despacha Domain Events atomicamente.

### Object Calisthenics (principais regras aplicadas)
- Um nível de indentação por método.
- Não usar `else` — fail fast com `return`.
- Todas as entidades encapsulam seus dados (setters privados/internos).
- Classes pequenas com responsabilidade única.

### SOLID
| Princípio | Aplicação |
|---|---|
| **S**ingle Responsibility | Cada handler faz uma única coisa |
| **O**pen/Closed | Novos comportamentos via novos handlers, não alterando os existentes |
| **L**iskov Substitution | `BaseRepository<T>` substituível por qualquer repositório concreto |
| **I**nterface Segregation | `IRepository<T>`, `IUnitOfWork` e interfaces de domínio separados |
| **D**ependency Inversion | Application depende de interfaces; Infrastructure implementa |

---

## Camadas da API

### Autenticação
```
POST /api/auth/register   → Cadastro de novo usuário
POST /api/auth/login      → Login, retorna JWT Bearer token
```

### Usuários (protegido)
```
GET  /api/users/{id}      → Busca usuário por ID
```

### Logs Diários (protegido)
```
GET  /api/daily-logs/{userId}/{date}         → Log do dia
POST /api/daily-logs                         → Cria log do dia
POST /api/daily-logs/{logId}/items           → Adiciona alimento ao log
```

### Alimentos
```
GET  /api/food-items?search=...              → Busca alimentos por nome
POST /api/food-items/analyze-photo           → Analisa foto via Gemini Vision (multipart/form-data)
```

### Solicitações de Exames (protegido)
```
GET   /api/exam-requests?userId=...                          → Lista solicitações do usuário
GET   /api/exam-requests/{id}                               → Busca solicitação por ID
POST  /api/exam-requests                                    → Cria solicitação com exames
PATCH /api/exam-requests/{requestId}/items/{itemId}/complete → Marca exame como realizado
```

### Catálogo de Exames
```
GET  /api/exam-categories   → Lista categorias de exames
GET  /api/exams             → Lista todos os exames disponíveis
```

---

## Frontend

O frontend é uma **SPA React** que consome a API REST via JWT, construída com **Feature-Based Clean Architecture** — organizado em fatias verticais por funcionalidade.

### Arquitetura Frontend

```
┌──────────────────────────────────────────────────────┐
│                   design-system/                     │  ← Tokens CSS + Componentes reutilizáveis
├──────────────────────────────────────────────────────┤
│                      core/                           │  ← API client, Auth store, Router
├──────────────────────────────────────────────────────┤
│                    features/                         │  ← Fatias verticais por domínio
│   ┌──────────┬─────────────┬───────────┬──────────┐  │
│   │   auth   │  dashboard  │ food-log  │  exams   │  │
│   │  api/    │   hooks/    │   api/    │   api/   │  │
│   │  hooks/  │   pages/    │   hooks/  │   hooks/ │  │
│   │  pages/  │             │   pages/  │   pages/ │  │
│   │  types/  │             │   types/  │   types/ │  │
│   └──────────┴─────────────┴───────────┴──────────┘  │
├──────────────────────────────────────────────────────┤
│                    shared/                           │  ← Spinner, ErrorBoundary
└──────────────────────────────────────────────────────┘
```

**Regra de dependência:** `features/` importa de `design-system/`, `core/` e `shared/`. Nunca entre features.

### Branding — VitaLog

O logotipo **Vita**`Log` usa tipografia `--font-display` peso 700 com a palavra *Log* em verde esmeralda `#34d399` para reforçar o contexto de saúde. O mesmo estilo aparece na nav global (44px preta) e na página de login.

### Design System — Estilo Apple

O design system implementa fielmente o sistema visual da Apple: token único de cor de ação (`#0066cc`), tipografia SF Pro / system-ui com tracking negativo nas headlines, tiles alternados claro/escuro como divisores de seção, nav preta de 44px + sub-nav frosted-glass de 52px, e o único `box-shadow` do sistema reservado exclusivamente para imagens de produto.

| Token | Valor | Uso |
|---|---|---|
| `--color-primary` | `#0066cc` | Único acento — todo elemento interativo |
| `--color-primary-on-dark` | `#2997ff` | Links em tiles escuros |
| `--color-canvas` | `#ffffff` | Canvas principal |
| `--color-canvas-parchment` | `#f5f5f7` | Tiles alternados, footer |
| `--color-surface-tile-1` | `#272729` | Tile escuro primário |
| `--color-surface-black` | `#000000` | Barra de nav global |
| `--color-ink` | `#1d1d1f` | Texto em superfícies claras |
| `--radius-pill` | `9999px` | CTAs primários, search input |
| `--radius-lg` | `18px` | Cards utilitários |
| `--shadow-product` | `rgba(0,0,0,0.22) 3px 5px 30px` | Apenas em renders de produto |

### Funcionalidades por Feature

#### Dashboard
Painel principal com dados reais carregados via React Query:
- **Today's Points** — total de pontos do dia (máx. 300 pts) com barra de progresso
- **Food items today** — quantidade de itens registrados no dia
- **Pending exams** — exames solicitados ainda não realizados
- Quick actions: atalhos para Food Log, Exams e History

#### Food Log — 3 modos de entrada
| Modo | Ícone | Comportamento |
|---|---|---|
| **Select** | 📋 | Busca por nome + dropdown + quantidade + horário da refeição |
| **Camera** | 📷 | Abre câmera do dispositivo → Gemini analisa → confirma e registra |
| **Gallery** | 🖼️ | Abre seletor de arquivos do dispositivo → mesmo fluxo de análise da câmera |

O fluxo de análise por IA (Camera e Gallery) usa `POST /api/food-items/analyze-photo` com Gemini Vision: identifica o alimento, estima porção em gramas, calcula calorias e pontos, e pode criar automaticamente novos itens no catálogo quando não encontrado.

#### Exams
Gerenciamento de solicitações de exames organizadas por categoria clínica, com marcação de conclusão e registro de resultado e laboratório.

### Fluxo de Autenticação Frontend

```
Usuário acessa rota protegida
        │
        ▼
  ProtectedRoute
        │ isAuthenticated? (Zustand)
        ├── NÃO → <Navigate to="/login" state={{ from }} />
        └── SIM → <Outlet /> (renderiza a page)

Login bem-sucedido
        │
        ▼
  useLogin (useMutation)
        │ authApi.login(credentials)
        ▼
  API retorna { token, user }
        │
        ▼
  authStore.setAuth(token, user)  ← persiste no localStorage
        │
        ▼
  navigate(from || '/dashboard')

Toda requisição Axios
        │
        ▼
  request interceptor
        │ injeta: Authorization: Bearer <token>
        ▼
  response interceptor
        │ status 401? → authStore.logout() + redirect /login
```

### Responsividade

| Breakpoint | Largura | Comportamento |
|---|---|---|
| Small phone | ≤ 419px | Hero title: 28px; tiles single-column; padding: 48px |
| Phone | 420–640px | Hero title: 34px; grids: 1 coluna |
| Tablet | 640–834px | Nav global colapsa; grids: 2 colunas |
| Desktop sm | 834–1068px | Layout completo; grids: 3 colunas |
| Desktop | > 1068px | Máx. conteúdo 980px; grids: 4–5 colunas |

---

## Testes

### Resumo

| Suite | Tipo | Testes | Status |
|---|---|---|---|
| `4-E2E` functional | E2E Playwright TypeScript | **89** | ✅ todos passando |
| `4-E2E` non-functional | Acessibilidade + Performance | **11** | ✅ todos passando |
| `UnitTests` | xUnit + NSubstitute | 31 | ✅ |
| `IntegrationTests` | Testcontainers SQL Server | — | ✅ |
| `BddTests` | Reqnroll / Gherkin | — | ✅ |

### Testes Funcionais E2E — 89 testes

#### Auth — Login (10 testes)
- Logs in with valid email and redirects to app
- Logs in with username and redirects to app
- Shows error for wrong password
- Shows error for non-existent user
- Shows Zod error when fields are empty
- Has link to create account (register)
- Password field is masked
- Login page shows VitaLog logo
- Login page shows tagline "Your health. Tracked."
- Page title is VitaLog

#### Auth — Logout (2 testes)
- Logs out and redirects to login page
- Cannot access protected route after logout

#### Auth — Register (12 testes)
- Registers a new user successfully and redirects
- Shows error when full name is too short
- Shows error for invalid email
- Shows error when username has uppercase letters
- Shows error when username has special characters
- Shows error when password has no uppercase letter
- Shows error when password has no digit
- Shows error when password is too short
- Shows error when gender is not selected
- Shows error when phone number is too short
- Shows error when username is too short
- Shows error when birth date is missing
- Shows error when email is already registered

#### Dashboard (11 testes)
- Shows "Your Health Dashboard" heading
- Shows personalised greeting with user first name
- Shows tagline about tracking food and exams
- Shows "Today's Points" stat card
- Shows "Food items today" stat card
- Shows "Pending exams" stat card
- Shows "What would you like to do?" section
- "Open Food Log" card is visible
- "Open Food Log" navigates to /food-log
- "Go to Exams" navigates to /exams
- "View History" navigates to /food-log/history

#### Exams (7 testes)
- Loads with "Exam Requests" heading
- Shows pending count or "All exams completed" subtitle
- Shows "My requests" section heading
- Shows "+ New request" button
- "+ New request" button navigates to /exams/requests
- SubNav "New request" link navigates to /exams/requests
- SubNav "My requests" link is active on /exams

#### Food Log — Page access & UI (6 testes)
- Food log page loads after login and shows today's log
- Shows today's items header
- Select mode is active by default
- Shows Add item button in select mode
- Shows error when submitting without selecting a food item
- Shows points badge when today's log is loaded

#### Food Log — History (5 testes)
- Loads with "Points History" heading
- Shows subtitle about daily point consumption
- Shows history list or "No history yet" empty state
- SubNav "Today" link navigates back to /food-log
- SubNav "History" link is active (stays on /food-log/history)

#### Food Log — Add item flow (13 testes)
- Search input is visible in select mode
- Typing in search input updates dropdown options
- Quantity below minimum (0) shows validation error
- Quantity above maximum (21) shows validation error
- Selects a food item and adds it to the log
- Switching from select to camera mode hides the food select
- Switching back to select mode shows the food select again
- "Gallery" tab button is visible on food log page
- Switching to Gallery mode hides the food select
- Gallery mode shows device file picker hint text
- Switching from Gallery back to Select shows the food select
- Gallery mode: uploading a photo triggers analysis
- Meal time dropdown has multiple options

#### Food Log — Photo mode / Camera (3 testes)
- 📷 Camera button is visible on food log page
- Switching to photo mode shows upload area
- Uploading a photo triggers analysis and shows result or loading state

#### Navigation — Authenticated (14 testes)
- Global nav is visible after login
- "Sign Out" button is visible in nav
- User name span is present in the nav
- "Dashboard" nav link is visible
- "Food Log" nav link is visible
- "Exams" nav link is visible
- "Dashboard" nav link navigates to /dashboard
- "Food Log" nav link navigates to /food-log
- "Exams" nav link navigates to /exams
- VitaLog logo link is visible in nav
- VitaLog logo navigates to root (dashboard)
- Page title is VitaLog

#### Navigation — Protected Routes (7 testes)
- Redirects /food-log to /login when not authenticated
- Redirects /dashboard to /login when not authenticated
- Redirects /exams to /login when not authenticated
- Redirects /food-log/history to /login when not authenticated
- Home / redirects unauthenticated user to login
- /login page loads and shows the form
- /register page loads and shows the form

### Testes Não Funcionais — 11 testes

#### Acessibilidade WCAG 2.1 AA — axe-core (6 testes)
- Login page has no critical a11y violations
- Register page has no critical a11y violations
- Login form fields have accessible labels
- Register form fields have accessible labels
- Login page is keyboard navigable
- Color contrast — page has sufficient contrast

#### Performance — Core Web Vitals (5 testes)
- Login page: FCP < 1800ms and LCP < 2500ms
- Register page: FCP < 1800ms
- domContentLoaded < 3000ms on public pages
- No unhandled JS errors on login page
- No unhandled JS errors on register page

### Executar Testes E2E (Playwright TypeScript)

```bash
cd 4-E2E
npm install
npx playwright install chromium

# Testes funcionais (89 testes)
npx playwright test tests/functional --project=chromium

# Testes não funcionais (11 testes)
npx playwright test tests/non-functional --project=non-functional

# Todos com relatório HTML
npx playwright test --reporter=list,html
npx playwright show-report
```

Variáveis de ambiente (arquivo `4-E2E/.env`):
```env
BASE_URL=https://vitalog.app.br
TEST_USER_EMAIL=emailusuario@teste.com
TEST_USER_PASSWORD=Password123
TEST_USER_USERNAME=emailusuarioteste
```

### Executar Testes Backend (.NET)

```bash
cd 3-BackEnd

# Unitários (31 testes, sem dependências externas)
dotnet test tests/PointsTableAndExams.UnitTests

# Integração (requer Docker — Testcontainers sobe SQL Server)
dotnet test tests/PointsTableAndExams.IntegrationTests

# BDD / Gherkin
dotnet test tests/PointsTableAndExams.BddTests

# Todos
dotnet test PointsTableAndExams.sln
```

---

## Deploy e CI/CD

### Infraestrutura Azure

| Componente | Serviço Azure | URL |
|---|---|---|
| **Frontend** | Azure Static Web Apps | [vitalog.app.br](https://vitalog.app.br) |
| **Backend API** | Azure App Service (.NET 9) | API interna |
| **Banco de dados** | Azure SQL Database | SQL Server 2019+ |

### GitHub Actions

#### `backend.yml` — Deploy Backend
Disparado em push na `main` (mudanças em `3-BackEnd/`):
1. Build + testes unitários
2. Publish `PointsTableAndExams.Api`
3. Deploy para Azure App Service via publish profile

#### `frontend.yml` — Deploy Frontend + Gate E2E
Disparado em push na `main` (mudanças em `2-FrontEnd/` ou `4-E2E/`):
1. `npm ci` + `npm run build`
2. Deploy para Azure Static Web Apps
3. Warmup do backend (aguarda cold-start)
4. Executa 89 testes funcionais Playwright (chromium)
5. Upload do relatório HTML como artefato

#### `e2e.yml` — Full E2E Manual
Disparo manual via `workflow_dispatch`:
- Executa todos os 100 testes (functional + non-functional)
- Projetos: chromium, firefox, webkit
- Upload do relatório consolidado

### Secrets necessários (GitHub → Settings → Secrets → Actions)

| Secret | Descrição |
|---|---|
| `AZURE_STATIC_WEB_APPS_API_TOKEN` | Token de deploy do Azure Static Web Apps |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Publish profile do Azure App Service |
| `TEST_USER_EMAIL` | Email do usuário de teste E2E |
| `TEST_USER_PASSWORD` | Senha do usuário de teste E2E |
| `TEST_USER_USERNAME` | Username do usuário de teste E2E |

---

## Como Executar

### Pré-requisitos

| Ferramenta | Versão mínima | Link |
|---|---|---|
| .NET SDK | 9.0 | https://dotnet.microsoft.com/download |
| Node.js | 18+ | https://nodejs.org |
| SQL Server | 2019+ | ou Docker |
| Docker | qualquer | para Testcontainers |

---

### Backend

#### 1. Configurar banco de dados
Execute os scripts na ordem em `1-Sql/`:
```sql
-- No SQL Server Management Studio ou Azure Data Studio:
-- 1. 01_DDL_Create_Tables.sql
-- 2. 02_DML_FoodCategory_and_FoodItem.sql
-- 3. 03_DML_ExamCategory_and_Exam.sql
-- 4. 04_DML_Sample_Users.sql
-- 5. 05_Views_and_Useful_Queries.sql
```

#### 2. Configurar `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PointsTableAndExams;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "SUA_CHAVE_SECRETA_AQUI_MINIMO_32_CARACTERES",
    "Issuer": "PointsTableAndExams.Api",
    "Audience": "PointsTableAndExams.Client",
    "ExpirationHours": "8"
  },
  "GeminiSettings": {
    "ApiKey": "SUA_GEMINI_API_KEY"
  }
}
```

#### 3. Executar a API
```bash
cd 3-BackEnd
dotnet run --project src/PointsTableAndExams.Api
# API disponível em: https://localhost:7001
```

#### 4. Acessar o Swagger
```
https://localhost:7001/swagger
```

---

### Frontend

#### 1. Instalar dependências
```bash
cd 2-FrontEnd
npm install
```

#### 2. Configurar variáveis de ambiente
```bash
cp .env.example .env.local
```

Editar `.env.local`:
```env
VITE_API_URL=https://localhost:7001/api
```

#### 3. Executar em desenvolvimento
```bash
npm run dev
# Disponível em: http://localhost:5173
```
O dev server faz proxy de `/api` para `https://localhost:7001` automaticamente (configurado no `vite.config.ts`), então o frontend pode rodar em HTTP enquanto a API roda em HTTPS sem problemas de CORS.

#### 4. Build para produção
```bash
npm run build
# Artefatos em: 2-FrontEnd/dist/
```

---

### Usuários de exemplo (após executar `04_DML_Sample_Users.sql`)

| Username | Senha | Nome |
|---|---|---|
| `ana.souza` | `password` | Ana Paula Souza |
| `carlos.lima` | `password` | Carlos Henrique Lima |
| `fernanda.c` | `password` | Fernanda Costa |

> **Nota:** Os hashes no seed são ilustrativos. Em produção, gere hashes reais com BCrypt antes de inserir.

---

## Decisões Arquiteturais

### Backend

| Decisão | Justificativa |
|---|---|
| **Guid como PK** | Evita dependência de sequência do banco, facilita sharding futuro |
| **Result Pattern** | Evita exceções para controle de fluxo, deixa erros explícitos no tipo de retorno |
| **Value Objects para Email e Points** | Centraliza validação, evita dados inválidos no domínio |
| **AsNoTracking** por padrão em reads | Performance — EF Core não rastreia entidades que não serão alteradas |
| **AsSplitQuery** em includes | Evita produto cartesiano em queries com múltiplos `Include` |
| **BCrypt workFactor 12** | Equilíbrio entre segurança e performance em hardware moderno |
| **Domain Events via MediatR** | Desacopla efeitos colaterais (ex: notificações) dos agregados |
| **Reqnroll** para BDD | SpecFlow successor oficial com suporte nativo ao .NET 9 |
| **Testcontainers** para integração | Banco real em Docker, sem mocks frágeis de infra |
| **Gemini Vision como ACL** | Anti-Corruption Layer — o domínio não conhece detalhes da API do Google |
| **Rotas kebab-case explícitas** | `[Route("api/exam-requests")]` — evita divergência entre nome da classe e URL esperada pelo frontend |

### Frontend

| Decisão | Justificativa |
|---|---|
| **Feature-Based Architecture** | Cada feature é uma fatia vertical autossuficiente — fácil de escalar, mover ou remover sem tocar outras features |
| **TanStack Query para server state** | Cache automático, stale-while-revalidate, deduplificação de requests — elimina gerenciamento manual de loading/error |
| **Zustand para client state** | API mínima sem boilerplate, colocação do estado próximo de quem usa, persisted middleware nativo |
| **CSS Modules + CSS Custom Properties** | Zero runtime, tokens compartilhados via variáveis nativas do browser, estilos escopados sem conflito |
| **React Router v7 com lazy()** | Code splitting automático por página, bundle inicial mínimo, layout routes evitam duplicação de UI |
| **Axios com interceptors** | Injeção de Bearer token centralizada, logout automático no 401 sem lógica espalhada pelos hooks |
| **React Hook Form + Zod** | Formulários uncontrolled (sem re-render por keystroke), validação com inferência de tipos end-to-end |
| **Design System próprio (Apple-style)** | Tokens CSS como fonte única de verdade, componentes agnósticos de negócio |
| **Vite como build tool** | HMR nativo em ESM, build com esbuild (10-100× mais rápido que webpack), proxy de API integrado |
| **TypeScript strict sem `any`** | Segurança de tipos do contrato da API até a UI, erros capturados em tempo de compilação |
| **Hooks reutilizados no Dashboard** | `useTodayLog()` e `useMyExamRequests()` compartilham cache com Food Log e Exams — zero chamadas de API duplicadas |

### Testes E2E

| Decisão | Justificativa |
|---|---|
| **Page Object Model** | Encapsulamento de seletores e ações — testes legíveis e fáceis de manter |
| **`authenticatedPage` fixture** | Login automático antes de cada teste autenticado — sem repetição de código |
| **Scope nav assertions** | Sempre escopar ao `nav[aria-label="Global navigation"]` — footer repete os links e causa strict-mode violations |
| **`toBeAttached()` para spans vazios** | Spans de 0×0 px são "hidden" no Playwright — `toBeAttached()` verifica presença no DOM |
| **`\|\|` em vez de `??` para env vars** | GitHub Secrets não definidos viram `''` (string vazia), não `undefined` — `\|\|` faz fallback para ambos |
| **Global setup de warmup** | Azure App Service entra em cold-start — warmup automático antes da suite evita timeouts falsos |
| **Projeto `non-functional` separado** | Testes de performance e a11y têm configurações diferentes (sem autenticação, timeouts maiores) |

---

*Projeto fullstack desenvolvido com foco em qualidade de código, testabilidade e aderência às melhores práticas dos ecossistemas .NET e React. Publicado em produção em [vitalog.app.br](https://vitalog.app.br).*
