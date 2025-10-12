# 🧠 WikiApi - API em .NET 9 e Angular com Arquitetura Limpa e Swagger

![Language](https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet)
![Database](https://img.shields.io/badge/PostgreSQL-17-blue?logo=postgresql)
![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)

---

## 📘 Descrição Geral

O Wiki API é um projeto desenvolvido com o objetivo de criar uma plataforma pessoal para armazenar e organizar pesquisas, tutoriais e soluções técnicas encontradas durante o desenvolvimento de software.

Este projeto segue o modelo de arquitetura limpa (Clean Architecture) e está sendo construído passo a passo de forma didática, integrando o back-end em .NET 9, o banco de dados PostgreSQL e o front-end em Angular.

---

## 📘 Descrição do Projeto

Este projeto tem como objetivo criar uma **API de gerenciamento de artigos (Wiki)** utilizando:

- **.NET 9**
- **Arquitetura Limpa (Clean Architecture)**
- **Entity Framework Core**
- **PostgreSQL**
- **Swagger para documentação**
- **Testes unitários com xUnit**

---

# 🚀 Status Atual

## ✅ Back-end (API em .NET 9)

- **Estrutura de projeto criada com Clean Architecture, dividida em:**

  - **WikiApi.Domain → Entidades e regras de domínio**

  - **WikiApi.Application → Interfaces, DTOs e serviços de aplicação**

  - **WikiApi.Infrastructure → Repositórios e acesso a dados (Entity Framework Core + PostgreSQL)**

  - **WikiApi.Api → Camada de apresentação da API (Controllers, Swagger, Startup)**

- **Banco de dados PostgreSQL configurado localmente**

- **Conexão feita via Entity Framework Core 9**

- **Migrações criadas e aplicadas (InitialCreate)**

- **Swagger configurado para documentação e testes de endpoints**

- **CRUD completo para a entidade Article:**

  - **GET /api/articles → listar todos os artigos**
  
  - **GET /api/articles/{id} → buscar artigo por ID**
  
  - **POST /api/articles → criar novo artigo**
  
  - **PUT /api/articles/{id} → atualizar artigo existente**
  
  - **DELETE /api/articles/{id} → excluir artigo**

- **Configuração de CORS liberada para permitir acesso do front-end**

- **Testes de unidade configurados com xUnit**

- **Teste local com banco de dados funcional ✅**

---

## 📸 Prévia - Swagger UI

<p align="center">
  <img src="docs/swagger.png" alt="Swagger UI" width="800">
</p>

---

## 🏗️ Estrutura do Projeto

```bash
WikiApi/
├── WikiApi.Api/              # Camada de apresentação (Controllers, Swagger, Program.cs)
├── WikiApi.Application/      # Regras de negócio e serviços
├── WikiApi.Domain/           # Entidades e interfaces base
├── WikiApi.Infrastructure/   # Persistência de dados, repositórios e EF Core
├── WikiApi.Tests/            # Testes unitários com xUnit
└── README.md                 # Este arquivo
```
---

## 🧩 Tecnologias Utilizadas
## Back-end

  - **.NET 9 SDK**
  - **Entity Framework Core 9**
  - **PostgreSQL 17**
  - **Swagger / Swashbuckle**
  - **xUnit**
  - **Docker**
    
---

## 📍 Próximos Passos
**🔸Validações e DTOs**

- Implementar validação de entrada de dados (campos obrigatórios, tamanhos, formatos)
  
- Introduzir DTOs para criação e atualização de artigos

**🔸 Autenticação e Autorização**

- Implementar autenticação via JWT
  
- Restringir endpoints sensíveis (ex: DELETE, PUT)

**🔸 Front-end em Angular**

- Criar interface visual com Angular 18
  
- Consumo da API via HttpClient
  
- Tela para listar, criar, editar e excluir artigos
  
- Organização por tags e categorias

**🔸 Deploy**

- Dockerizar aplicação completa (API + PostgreSQL)

- Hospedar API no Render.com

- Hospedar front-end no Netlify
---

## 🧭 Objetivo Final

Ao final, o projeto será uma Wiki pessoal completa, com:

  - API documentada e segura
  
  - Banco de dados persistente
  
  - Interface moderna com Angular
  
  - Deploy em nuvem totalmente funcional 🌐
---
