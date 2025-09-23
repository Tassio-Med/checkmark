# Projeto Checkmark

Este é um projeto full-stack que consiste em um frontend em React e um backend em .NET.

## Visão Geral

O projeto é uma aplicação de lista de tarefas (to-do list) chamada "Checkmark". O frontend é construído com React e o backend é uma API RESTful construída com .NET e Entity Framework Core, usando um banco de dados SQLite.

## Como Executar o Projeto

Para executar este projeto, você precisará clonar o repositório e executar o frontend e o backend separadamente.

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js e npm](https://nodejs.org/en/)

### Backend

1.  Navegue até a pasta `backend`:
    ```bash
    cd backend
    ```
2.  Execute a API:
    ```bash
    dotnet run --project Checkmark.API
    ```
    A API estará disponível em `http://localhost:5000` (ou outra porta, verifique o console).

### Frontend

1.  Em um novo terminal, navegue até a pasta `frontend`:
    ```bash
    cd frontend
    ```
2.  Instale as dependências:
    ```bash
    npm install
    ```
3.  Inicie o servidor de desenvolvimento:
    ```bash
    npm start
    ```
    A aplicação React estará disponível em `http://localhost:3000`.
