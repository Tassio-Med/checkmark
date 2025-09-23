# API Checkmark

Esta é a documentação para a API Checkmark.

## Visão Geral

A API Checkmark é uma API RESTful para gerenciar itens de uma lista de tarefas. Ela permite criar, ler, atualizar, excluir e filtrar itens.

## Endpoints da API

A URL base da API é `/api/Checkmark`.

### `GET /api/Checkmark`

Retorna todos os itens da lista de tarefas.

**Exemplo de Resposta:**
```json
[
  {
    "id": 1,
    "description": "Criar o frontend",
    "isCompleted": false,
    "priority": "High"
  },
  {
    "id": 2,
    "description": "Criar o backend",
    "isCompleted": true,
    "priority": "High"
  }
]
```

### `GET /api/Checkmark/{id}`

Retorna um item específico pelo seu ID.

**Parâmetros:**
- `id` (int): O ID do item.

### `POST /api/Checkmark`

Cria um novo item.

**Corpo da Requisição:**
```json
{
  "description": "Nova tarefa",
  "isCompleted": false,
  "priority": "Medium"
}
```

### `PUT /api/Checkmark/{id}`

Atualiza um item existente.

**Parâmetros:**
- `id` (int): O ID do item a ser atualizado.

**Corpo da Requisição:**
```json
{
  "id": 1,
  "description": "Descrição atualizada",
  "isCompleted": true,
  "priority": "Low"
}
```

### `DELETE /api/Checkmark/{id}`

Exclui um item.

**Parâmetros:**
- `id` (int): O ID do item a ser excluído.

### Endpoints de Filtragem

-   `GET /api/Checkmark/completed`: Retorna todos os itens concluídos.
-   `GET /api/Checkmark/pending`: Retorna todos os itens pendentes.
-   `GET /api/Checkmark/priority/{priority}`: Retorna todos os itens com uma prioridade específica (`Low`, `Medium`, `High`).
