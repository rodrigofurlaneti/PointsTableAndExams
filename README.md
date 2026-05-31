# PointsTableAndExams

Sistema completo para controle diário de pontos alimentares e gerenciamento de exames laboratoriais, desenvolvido com as melhores práticas de arquitetura de software.

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
- [Como Executar](#como-executar)
- [Decisões Arquiteturais](#decisões-arquiteturais)

---

## Visão Geral

O **PointsTableAndExams** é uma plataforma que permite:

- **Tabela de Pontos**: controle diário do consumo alimentar baseado em um sistema de pontos (máx. 300 pts/dia), com mais de 200 alimentos categorizados.
- **Gestão de Exames**: solicitação, acompanhamento e registro de resultados de exames laboratoriais organizados em 12 categorias clínicas.
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

### Testes

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
| **Microsoft.Playwright** | 1.52 | Testes E2E |

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
├── 3-BackEnd/                                ← Solução .NET 9
│   ├── PointsTableAndExams.sln
│   │
│   ├── src/
│   │   ├── PointsTableAndExams.Domain/
│   │   │   ├── Common/
│   │   │   │   ├── AggregateRoot.cs          ← Dispara Domain Events
│   │   │   │   ├── Entity.cs                 ← Id, CreatedAt, UpdatedAt
│   │   │   │   ├── IDomainEvent.cs
│   │   │   │   ├── Result.cs                 ← Result Pattern (sem exceções no fluxo)
│   │   │   │   └── ValueObject.cs
│   │   │   ├── Entities/
│   │   │   │   ├── User.cs                   ← Aggregate Root
│   │   │   │   ├── DailyLog.cs               ← Aggregate Root
│   │   │   │   ├── DailyLogItem.cs
│   │   │   │   ├── ExamRequest.cs            ← Aggregate Root
│   │   │   │   ├── ExamRequestItem.cs
│   │   │   │   ├── FoodCategory.cs
│   │   │   │   ├── FoodItem.cs
│   │   │   │   ├── ExamCategory.cs
│   │   │   │   └── Exam.cs
│   │   │   ├── ValueObjects/
│   │   │   │   ├── Email.cs                  ← Validação + normalização
│   │   │   │   ├── PhoneNumber.cs
│   │   │   │   └── Points.cs                 ← Imutável, nunca negativo
│   │   │   ├── DomainEvents/
│   │   │   │   ├── UserCreatedEvent.cs
│   │   │   │   ├── DailyLogCreatedEvent.cs
│   │   │   │   ├── ExamRequestCreatedEvent.cs
│   │   │   │   └── ExamCompletedEvent.cs
│   │   │   ├── Enums/
│   │   │   │   └── Gender.cs
│   │   │   ├── Exceptions/
│   │   │   │   └── DomainException.cs
│   │   │   └── Interfaces/Repositories/
│   │   │       ├── IRepository.cs            ← Interface genérica
│   │   │       ├── IUnitOfWork.cs
│   │   │       ├── IUserRepository.cs
│   │   │       ├── IDailyLogRepository.cs
│   │   │       ├── IExamRequestRepository.cs
│   │   │       ├── IFoodCategoryRepository.cs
│   │   │       ├── IFoodItemRepository.cs
│   │   │       ├── IExamCategoryRepository.cs
│   │   │       └── IExamRepository.cs
│   │   │
│   │   ├── PointsTableAndExams.Application/
│   │   │   ├── Common/
│   │   │   │   ├── Behaviors/
│   │   │   │   │   ├── LoggingBehavior.cs    ← MediatR Pipeline
│   │   │   │   │   └── ValidationBehavior.cs ← MediatR Pipeline
│   │   │   │   └── Interfaces/
│   │   │   │       ├── ICurrentUser.cs
│   │   │   │       ├── IPasswordHasher.cs
│   │   │   │       └── ITokenService.cs
│   │   │   ├── Users/
│   │   │   │   ├── Commands/CreateUser/      ← Command + Handler + Validator
│   │   │   │   ├── Commands/Login/
│   │   │   │   └── Queries/GetUserById/      ← Query + Handler + Response DTO
│   │   │   ├── DailyLogs/
│   │   │   │   ├── Commands/CreateDailyLog/
│   │   │   │   ├── Commands/AddLogItem/
│   │   │   │   └── Queries/GetDailyLogByDate/
│   │   │   ├── ExamRequests/
│   │   │   │   ├── Commands/CreateExamRequest/
│   │   │   │   ├── Commands/MarkExamCompleted/
│   │   │   │   └── Queries/GetExamRequestById/
│   │   │   └── DependencyInjection.cs        ← AddApplication()
│   │   │
│   │   ├── PointsTableAndExams.Infrastructure/
│   │   │   ├── Data/
│   │   │   │   ├── AppDbContext.cs            ← Despacha Domain Events no CommitAsync
│   │   │   │   ├── Configurations/            ← IEntityTypeConfiguration<T> (Fluent API)
│   │   │   │   │   ├── UserConfiguration.cs
│   │   │   │   │   ├── FoodItemConfiguration.cs
│   │   │   │   │   ├── DailyLogConfiguration.cs
│   │   │   │   │   └── ExamRequestConfiguration.cs
│   │   │   │   └── Repositories/
│   │   │   │       ├── BaseRepository.cs      ← AsNoTracking por padrão
│   │   │   │       ├── UserRepository.cs
│   │   │   │       ├── DailyLogRepository.cs  ← AsSplitQuery para includes
│   │   │   │       └── ExamRequestRepository.cs
│   │   │   ├── Services/
│   │   │   │   ├── PasswordHasher.cs          ← BCrypt workFactor 12
│   │   │   │   ├── JwtTokenService.cs         ← HS256, claims padrão
│   │   │   │   └── CurrentUserService.cs      ← IHttpContextAccessor
│   │   │   └── DependencyInjection.cs         ← AddInfrastructure()
│   │   │
│   │   └── PointsTableAndExams.Api/
│   │       ├── Controllers/
│   │       │   ├── BaseApiController.cs       ← FromResult<T> helper
│   │       │   ├── AuthController.cs          ← POST /register, POST /login
│   │       │   ├── UsersController.cs         ← GET /users/{id}
│   │       │   ├── DailyLogsController.cs     ← CRUD logs diários
│   │       │   └── ExamRequestsController.cs  ← CRUD solicitações de exames
│   │       ├── Middlewares/
│   │       │   └── ExceptionHandlingMiddleware.cs
│   │       ├── Program.cs
│   │       └── appsettings.json
│   │
│   └── tests/
│       ├── PointsTableAndExams.UnitTests/
│       │   ├── Domain/Entities/               ← UserTests, DailyLogTests
│       │   ├── Domain/ValueObjects/           ← EmailTests, PointsTests
│       │   ├── Application/Users/             ← CreateUserCommandHandlerTests
│       │   ├── Application/DailyLogs/         ← CreateDailyLogCommandHandlerTests
│       │   └── Architecture/
│       │       └── ArchitectureTests.cs       ← NetArchTest (limites de camada)
│       │
│       ├── PointsTableAndExams.IntegrationTests/
│       │   ├── Common/
│       │   │   ├── IntegrationTestWebAppFactory.cs  ← Testcontainers SQL Server
│       │   │   └── BaseIntegrationTest.cs
│       │   └── Controllers/
│       │       └── AuthControllerTests.cs
│       │
│       ├── PointsTableAndExams.BddTests/
│       │   ├── Features/
│       │   │   ├── UserRegistration.feature   ← Cenários em Gherkin
│       │   │   └── DailyLog.feature
│       │   ├── StepDefinitions/
│       │   │   ├── UserRegistrationSteps.cs
│       │   │   └── DailyLogSteps.cs
│       │   └── reqnroll.json
│       │
│       └── PointsTableAndExams.E2ETests/
│           ├── Support/
│           │   └── PlaywrightFixture.cs       ← IAsyncLifetime, IBrowser
│           └── Tests/
│               └── AuthE2ETests.cs            ← Register → Login → Endpoint protegido
│
└── README.md                                  ← Este arquivo
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

### Solicitações de Exames (protegido)
```
GET   /api/exam-requests/{id}                        → Busca solicitação
POST  /api/exam-requests                             → Cria solicitação com exames
PATCH /api/exam-requests/{requestId}/items/{itemId}/complete  → Marca exame como realizado
```

---

## Frontend

O frontend é uma **SPA React** que consome a API REST via JWT, construída com **Feature-Based Clean Architecture** — o equivalente visual do Clean Architecture do backend, organizado em fatias verticais por funcionalidade em vez de camadas horizontais.

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
│   │  api/    │   api/      │   api/    │   api/   │  │
│   │  hooks/  │   hooks/    │   hooks/  │   hooks/ │  │
│   │  pages/  │   pages/    │   pages/  │   pages/ │  │
│   │  types/  │             │   types/  │   types/ │  │
│   └──────────┴─────────────┴───────────┴──────────┘  │
├──────────────────────────────────────────────────────┤
│                    shared/                           │  ← Spinner, ErrorBoundary, hooks globais
└──────────────────────────────────────────────────────┘
```

**Regra de dependência:** `features/` importa de `design-system/`, `core/` e `shared/`. Nunca entre features.

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

**Escala tipográfica:**

| Token CSS | Tamanho | Peso | Uso |
|---|---|---|---|
| `--text-hero-display` | 56px | 600 | Hero headline com tracking −0.28px |
| `--text-display-lg` | 40px | 600 | Título de tile de produto |
| `--text-display-md` | 34px | 600 | Cabeçalho de seção |
| `--text-lead` | 28px | 400 | Subcopy de tile |
| `--text-tagline` | 21px | 600 | Sub-nav, eyebrow |
| `--text-body` | 17px | 400 | Parágrafo — 17px, não 16px |
| `--text-caption` | 14px | 400 | Labels, botões utilitários |
| `--text-nav-link` | 12px | 400 | Links da nav global |

### Componentes do Design System

| Componente | Variantes | Descrição |
|---|---|---|
| `Button` | `primary`, `secondary`, `dark`, `hero`, `pearl`, `icon` | Todos com `transform: scale(0.95)` no active |
| `GlobalNav` | — | Barra preta 44px, sticky, colapsa em mobile |
| `SubNav` | — | Frosted glass 52px abaixo da nav global |
| `ProductTile` | `light`, `parchment`, `dark`, `dark2`, `dark3` | Tiles full-bleed sem border-radius |
| `StoreCard` | — | Cards com border `hairline` + `radius-lg` |
| `Input` | `pill`, `rect` | Input com label acessível + estado de erro |
| `Footer` | — | Colunas de links com `line-height: 2.41` |

### Estrutura de Pastas Frontend

```
2-FrontEnd/
├── .env.example                          ← Copiar para .env.local
├── vite.config.ts                        ← Proxy /api → backend :7001
├── tsconfig.app.json                     ← strict: true, zero any
└── src/
    ├── main.tsx                          ← Monta tokens.css + global.css + App
    ├── App.tsx                           ← QueryClientProvider + AppRouter
    │
    ├── design-system/
    │   ├── tokens.css                    ← Todas as variáveis CSS (cores, tipo, espaço, raio)
    │   ├── global.css                    ← Reset + base body
    │   ├── tokens.ts                     ← Mirror TypeScript dos tokens
    │   ├── index.ts                      ← Barrel export
    │   └── components/
    │       ├── Button/                   ← Button.tsx + Button.module.css
    │       ├── Nav/                      ← GlobalNav + SubNav
    │       ├── ProductTile/              ← ProductTile.tsx + module.css
    │       ├── Card/                     ← StoreCard.tsx + module.css
    │       ├── Input/                    ← Input.tsx + module.css
    │       └── Footer/                  ← Footer.tsx + module.css
    │
    ├── core/
    │   ├── api/
    │   │   └── client.ts                ← Axios + interceptor Bearer + logout no 401
    │   ├── auth/
    │   │   ├── authStore.ts             ← Zustand com persist (localStorage)
    │   │   └── ProtectedRoute.tsx       ← Redireciona para /login se não autenticado
    │   ├── router/
    │   │   └── AppRouter.tsx            ← Layout routes + lazy() em todas as pages
    │   └── queryClient.ts              ← TanStack Query — staleTime 2min, retry 1
    │
    ├── features/
    │   ├── auth/
    │   │   ├── types/auth.types.ts      ← LoginRequest, RegisterRequest, AuthResponse
    │   │   ├── api/authApi.ts           ← POST /auth/login, POST /auth/register
    │   │   ├── hooks/useLogin.ts        ← useMutation → setAuth → redirect
    │   │   ├── hooks/useRegister.ts     ← useMutation → setAuth → /dashboard
    │   │   ├── pages/LoginPage.tsx      ← Card centralizado, RHF + Zod
    │   │   └── pages/RegisterPage.tsx   ← Formulário completo com seleção de gênero
    │   │
    │   ├── dashboard/
    │   │   ├── api/dashboardApi.ts      ← GET /users/me/summary
    │   │   ├── hooks/useDashboard.ts    ← useQuery com queryKey
    │   │   └── pages/DashboardPage.tsx  ← Hero escuro + stats cards + quick actions
    │   │
    │   ├── food-log/
    │   │   ├── types/foodLog.types.ts   ← DailyLog, DailyLogItem, FoodItem, etc.
    │   │   ├── api/foodLogApi.ts        ← GET today, POST item, DELETE item, history
    │   │   ├── hooks/useFoodLog.ts      ← useTodayLog, useAddLogItem, useRemoveLogItem
    │   │   ├── pages/FoodLogPage.tsx    ← Painel de adição + lista de itens do dia
    │   │   └── pages/FoodLogHistoryPage.tsx ← Histórico de pontos por dia
    │   │
    │   └── exams/
    │       ├── types/exams.types.ts     ← ExamRequest, ExamRequestItem, Exam, etc.
    │       ├── api/examsApi.ts          ← GET requests, POST request, PATCH item
    │       ├── hooks/useExams.ts        ← useMyExamRequests, useCreateExamRequest, etc.
    │       ├── pages/ExamsPage.tsx      ← Lista de solicitações + marcar como feito
    │       └── pages/ExamRequestPage.tsx ← Seleção de exames por categoria + criar request
    │
    └── shared/
        └── components/
            ├── Spinner.tsx              ← Spinner acessível com role="status"
            └── Spinner.module.css
```

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

### Executar Testes Unitários
```bash
dotnet test tests/PointsTableAndExams.UnitTests
# 31 testes — sem dependências externas
```

### Executar Testes de Integração
> Requer Docker instalado (Testcontainers sobe SQL Server automaticamente)
```bash
dotnet test tests/PointsTableAndExams.IntegrationTests
```

### Executar Testes BDD (Reqnroll)
```bash
dotnet test tests/PointsTableAndExams.BddTests
```

### Executar Testes E2E (Playwright)
> Requer a API em execução na porta 7001
```bash
dotnet run --project src/PointsTableAndExams.Api
dotnet test tests/PointsTableAndExams.E2ETests
```

### Executar Todos
```bash
dotnet test PointsTableAndExams.sln
```

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

#### 5. Preview do build
```bash
npm run preview
# Preview em: http://localhost:4173
```

---

### Usuários de exemplo (após executar `04_DML_Sample_Users.sql`)

| Username | Senha | Nome |
|---|---|---|
| `ana.souza` | `password` | Ana Paula Souza |
| `carlos.lima` | `password` | Carlos Henrique Lima |
| `fernanda.c` | `password` | Fernanda Costa |

> **Nota:** Os hashes no seed são ilustrativos (SHA-256 da string `"password"`). Em produção, gere hashes reais com BCrypt antes de inserir.

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
| **Design System próprio (Apple-style)** | Tokens CSS como fonte única de verdade, componentes agnósticos de negócio, sistema de design escalável e documentado |
| **Vite como build tool** | HMR nativo em ESM, build com esbuild (10-100× mais rápido que webpack), proxy de API integrado |
| **TypeScript strict sem `any`** | Segurança de tipos do contrato da API até a UI, erros capturados em tempo de compilação |

---

*Projeto fullstack desenvolvido com foco em qualidade de código, testabilidade e aderência às melhores práticas dos ecossistemas .NET e React.*
