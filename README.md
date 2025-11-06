
# 📞 Agenda Telefônica - Fullstack

## Tecnologias Necessárias para Rodar o Projeto

Antes de iniciar, instale:

| Tecnologia | Versão sugerida | Para quê? |
|-----------|----------------|-----------|
| .NET SDK | 8.x | Rodar a API |
| PostgreSQL | 14+ | Banco de dados |
| Node.js | 18+ | Rodar o frontend |

---

## Clonar o Repositório

```bash
git clone https://github.com/dev-LBAM/davinti-teste-pratico.git
cd davinti-teste-pratico
```

---

## ⚙️ Configurar o Backend (API)

### 1️⃣ Criar banco no PostgreSQL

Conecte‑se e crie um banco de dados no PostgreSQL para obter sua connection string

### 2️⃣ Criar o arquivo `.env` dentro da pasta da API

Arquivo: `/api/telephonediary/telephonediary/.env`

Conteúdo do exemplo:

```env
CONNECTION_STRING=Host=localhost;Port=5432;Database=suadb;Username=seuuser;Password=suasenha
FRONTEND_URL=http://localhost:SUA_PORTA_DO_FRONTEND
```

📌 Modifique usuário e senha do banco conforme o seu PostgreSQL.

### 3️⃣ Aplicar as migrations

```bash
cd api/telephonediary/telephonediary
dotnet ef database update
```

### 4️⃣ Rodar a API

```bash
dotnet run
```

A API iniciará em:

➡️ http://localhost:SUA_PORTA_DO_BACKEND/api/contatos

---

## 💻 Configurar o Frontend

```bash
cd frontend/telephonediary
npm install
```

### 1️⃣ Criar arquivo `.env` no frontend

Arquivo: `/frontend/telephonediary/.env`

```env
VITE_API_URL=http://localhost:SUA_PORTA_DO_BACKEND/api/contatos
```

### 2️⃣ Rodar o frontend

```bash
npm run dev
```

Aplicação iniciará em:

➡️ http://localhost:SUA_PORTA_DO_FRONTEND

---
