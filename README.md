# Sistema de Cadastro de Beneficiários

Este projeto é uma solução completa para o gerenciamento de Planos de Saúde e Beneficiários, desenvolvida como parte de um desafio técnico Full Stack.

A aplicação implementa um CRUD completo, validações de regras de negócio e um sistema de exclusão assíncrona (Soft Delete), processado por um Background Service (Worker) com fila de prioridade.

---

## Tecnologias Utilizadas

### Backend (.NET 9)
- ASP.NET Core Web API.
- Entity Framework Core.
- SQL Server.
- Hosted Services (Worker) - Processamento em segundo plano para exclusão física.
- xUnit.
- Swagger.

### Frontend (Angular 20)
- Angular Standalone Components.
- NG-ZORRO.
- RxJS.
- Jasmine/Karma.

---

## Arquitetura e Decisões Técnicas

### 1. Monorepo Organizado
O projeto mantém Backend e Frontend no mesmo repositório para facilitar o versionamento e a avaliação, mantendo separação clara de responsabilidades:
- Desafio_Tecnico...: Contém toda a lógica do Backend.
- frontend: Contém a aplicação Angular (SPA).

### 2. DDD Lógico (Logical Layering)
Para evitar complexidade desnecessária (over-engineering), optou-se por uma arquitetura em camadas lógicas dentro de um único projeto WebAPI, mantendo a organização do Clean Architecture:
- Domain: Entidades, Enums e Interfaces.
- Application: Services, DTOs e Validadores.
- Infrastructure: Contexto do Banco de Dados (Data) e Migrations.
- Controllers : Controllers e Workers.

### 3. Worker Service & Soft Delete
A exclusão de beneficiários segue um fluxo assíncrono para garantir performance e integridade:
1. O usuário solicita a exclusão e define a Prioridade (Alta, Média, Baixa).
2. O registro é marcado como PendenteExclusao = true no banco (Soft Delete).
3. Um Worker Service executa a cada 1 minuto, processa os itens pendentes ordenados por prioridade e realiza a exclusão física.

---

## Pré-requisitos

- .NET SDK (versão 9.0)
- Node.js (versão 18 ou 20 LTS)
- SQL Server (LocalDB ou SQL Express)
- Angular CLI (npm install -g @angular/cli)

---

## Como Rodar o Projeto

### 1. Configurar o Banco de Dados

Navegue até a pasta do backend, verifique a ConnectionString no arquivo appsettings.json e aplique as migrations:

cd Desafio_Tecnico_Cadastro_de_Beneficiarios
dotnet ef database update

### 2. Iniciar o Backend:
dotnet run
A API estará disponível em https://localhost:7185. Acesse a documentação em /swagger.

### 3. Iniciar o Frontend
Abra um novo terminal, navegue até a pasta do frontend, instale as dependências e inicie o servidor de desenvolvimento:
npm install
ng serve
Acesse a aplicação no navegador em http://localhost:4200.