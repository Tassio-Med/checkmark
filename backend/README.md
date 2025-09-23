# Backend da Aplicação Checkmark

Este diretório contém o código-fonte do backend da aplicação Checkmark, uma API RESTful construída com .NET 8.

## Visão Geral

A API é responsável por gerenciar os itens da lista de tarefas (Checkmark items). Ela fornece endpoints para criar, ler, atualizar e excluir itens, além de funcionalidades de filtragem.

A API utiliza o Entity Framework Core para interagir com um banco de dados SQLite.

## Estrutura do Projeto

-   `Checkmark.API/`: O projeto principal da API.
    -   `Controllers/`: Contém os controladores da API.
    -   `Data/`: Contém o `DbContext` do Entity Framework.
    -   `Models/`: Contém os modelos de dados.
    -   `Repositories/`: Contém os repositórios para acesso a dados.
    -   `Services/`: Contém a lógica de negócios.
    -   `Program.cs`: O ponto de entrada da aplicação.
-   `Checkmark.API.Tests/`: Contém os testes de unidade para a API.
-   `Checkmark.sln`: O arquivo de solução do Visual Studio.

## Como Executar

1.  Navegue até a pasta `backend`:
    ```bash
    cd backend
    ```
2.  Restaure as dependências do .NET:
    ```bash
    dotnet restore
    ```
3.  Execute a API:
    ```bash
    dotnet run --project Checkmark.API
    ```
    A API estará disponível em `http://localhost:5000` (ou outra porta, verifique o console). A documentação do Swagger estará disponível em `http://localhost:5000/swagger`.
