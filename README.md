# API Matheus - Projeto Final PI0924

## Sobre o projeto

Este projeto foi desenvolvido para o Projeto Final da UFCD PI0924.

O objetivo foi criar uma API REST em ASP.NET Core (.NET 8), permitindo gerir utilizadores e produtos, utilizando autenticação JWT, documentação com Swagger e integração com um serviço externo simulado através do Mountebank (Imposter).

Além disso, foram implementados mecanismos de cache com Redis e políticas de resiliência com Polly para melhorar o desempenho e o tratamento de falhas.

---

## Tecnologias utilizadas

- ASP.NET Core (.NET 8)
- JWT
- Swagger
- Redis
- Polly
- Mountebank (Imposter)
- Docker
- Postman

---

## Estrutura do projeto

```
ApiMatheusProjetoFinal/
│
├── Controllers
├── Models
├── Services
├── Resilience
├── imposter
│
├── Dockerfile
├── docker-compose.yml
├── README.md
└── ApiMatheusProjetoFinal.postman_collection.json
```

---

## Executar o projeto

### Sem Docker

1. Iniciar o Mountebank.

```bash
mb start --configfile ApiMatheusProjetoFinal/imposter/imposter.json
```

2. Executar a API.

```bash
cd ApiMatheusProjetoFinal
dotnet run
```

3. Abrir o Swagger.

```
https://localhost:7072/swagger
```

---

### Com Docker

Executar:

```bash
docker compose up --build
```

Serão iniciados três serviços:

- API
- Redis
- Mountebank

---

## Login

Para obter um token JWT utiliza:

```
POST /api/auth/login
```

```json
{
  "username": "matheus",
  "password": "12345"
}
```

O token devolvido deve ser utilizado no Swagger ou no Postman através do cabeçalho:

```
Authorization: Bearer <token>
```

---

## Endpoints

| Método | Endpoint | Descrição |
|---------|----------|-----------|
| POST | /api/auth/login | Efetuar login |
| GET | /api/users | Listar utilizadores |
| POST | /api/users | Criar utilizador |
| PUT | /api/users/{id} | Atualizar utilizador |
| DELETE | /api/users/{id} | Remover utilizador |
| GET | /api/products | Listar produtos |
| POST | /api/products | Criar produto |
| PUT | /api/products/{id} | Atualizar produto |
| DELETE | /api/products/{id} | Remover produto |
| GET | /api/imposter/inventory/{sku} | Consultar inventário |
| POST | /api/imposter/payments | Simular pagamento |

---

## Testes

Foi incluída uma coleção do Postman com os pedidos necessários para testar a API.

Antes de testar os restantes endpoints, é necessário executar o pedido de login para obter o token JWT.

---

## Funcionalidades implementadas

- Autenticação com JWT
- CRUD de Utilizadores
- CRUD de Produtos
- Integração com Mountebank
- Cache com Redis
- Retry e Circuit Breaker com Polly
- Documentação com Swagger
- Docker Compose

---

## Autor

Matheus Silva

Projeto desenvolvido para avaliação da UFCD PI0924.