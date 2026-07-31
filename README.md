# ApiMatheusProjetoFinal

Projeto Final: API REST em .NET 8 (ASP.NET Core Web API) com JWT, Swagger, cache e resiliência (Polly) e um serviço *imposter* (Mountebank) que simula sistemas externos de inventário e pagamentos.

## Arquitetura

- **Backend:** ASP.NET Core Web API (.NET 8)
- **Autenticação:** JWT (JSON Web Tokens)
- **Documentação:** Swagger / Swashbuckle
- **Cache e resiliência:** Polly (retry + circuit breaker) + Redis (`IDistributedCache`)
- **Mock externo (imposter):** Mountebank

## Estrutura

```
├── ApiMatheusProjetoFinal/
│   ├── Controllers/       # Auth, Users, Products, Imposter
│   ├── Models/
│   ├── Services/          # ImposterService (HttpClient + Polly)
│   ├── Resilience/        # Políticas Polly (retry, circuit breaker)
│   └── imposter/          # Configuração do Mountebank
├── Dockerfile
├── docker-compose.yml
└── ApiMatheusProjetoFinal.postman_collection.json
```

## Como correr localmente (sem Docker)

1. Arrancar o imposter (Mountebank) na porta `4545`:
   ```bash
   mb start --configfile ApiMatheusProjetoFinal/imposter/imposter.json
   ```
2. Arrancar a API:
   ```bash
   cd ApiMatheusProjetoFinal
   dotnet run
   ```
3. Abrir o Swagger em `https://localhost:7072/swagger`

## Como correr com Docker

```bash
docker compose up --build
```

Isto sobe três containers:
- **api** — a API .NET 8, disponível em `http://localhost:8080`
- **imposter** — o Mountebank, disponível em `http://localhost:4545` (mocks) e `http://localhost:2525` (admin)
- **redis** — cache Redis, disponível em `localhost:6379`

Para correr localmente sem Docker, precisas também de um Redis a correr em `localhost:6379` (ex.: `docker run -p 6379:6379 redis:7-alpine`).

## Autenticação

```
POST /api/auth/login
{
  "username": "matheus",
  "password": "12345"
}
```

Devolve um token JWT a usar no header `Authorization: Bearer <token>` nos restantes endpoints.

## Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/auth/login` | Autenticação e emissão do token JWT |
| GET/POST/PUT/DELETE | `/api/users` | CRUD de utilizadores |
| GET/POST/PUT/DELETE | `/api/products` | CRUD de produtos |
| GET | `/api/imposter/inventory/{sku}` | Consulta inventário via imposter (com cache) |
| POST | `/api/imposter/payments` | Simula um pagamento via imposter |

## Testes

Coleção Postman disponível em `ApiMatheusProjetoFinal.postman_collection.json` — importar no Postman, correr primeiro o `Login` (guarda o token automaticamente numa variável de coleção) e depois os restantes pedidos.

## Resiliência (Polly)

- **Retry:** tentativas automáticas em caso de falha temporária ao contactar o imposter.
- **Circuit breaker:** interrompe temporariamente as chamadas ao imposter após falhas consecutivas, evitando sobrecarga.
- **Cache:** respostas de inventário são guardadas no Redis durante 30 segundos.
