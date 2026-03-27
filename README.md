# Sandbox Counter (React + ASP.NET Core + PostgreSQL)

Sandbox minimal pour tester Pidhex avec:
- frontend React + TypeScript + Vite
- backend ASP.NET Core 8 Web API + EF Core + Npgsql
- PostgreSQL
- persistance d'un compteur (`counter_state`)

## Structure

- `frontend`: UI React
- `backend`: API + EF Core migration + seed
- `docker-compose.yml`: stack locale frontend + backend + postgres
- `scripts`: scripts simples de demarrage

## Variables d'environnement

### Backend
- `ConnectionStrings__Default` (prioritaire si `DATABASE_URL` absent)
- `DATABASE_URL` (optionnel, converti vers Npgsql)

Exemple: `backend/.env.example`

### Frontend
- `VITE_API_BASE_URL` (URL du backend)

Exemple: `frontend/.env.example`

## Run local (sans Docker)

Pre-requis:
- .NET SDK 8
- Node.js 22+
- PostgreSQL local

1) Copier les exemples d'env:
- `backend/.env.example` -> variables shell ou profil local
- `frontend/.env.example` -> `frontend/.env`

2) Installer les dependances frontend:
- `npm --prefix frontend install`

3) Lancer le backend:
- `.\scripts\start-backend.ps1`

4) Lancer le frontend:
- `.\scripts\start-frontend.ps1`

5) Ouvrir:
- Frontend: `http://localhost:5173`
- Backend: `http://localhost:8080`

## Run via Docker Compose

Depuis la racine:

- `docker compose up --build`

Services exposes:
- frontend: `http://localhost:4001`
- backend: `http://localhost:4002`
- postgres: `localhost:5432`

## Endpoints

- `GET /counter` -> `{ "value": number }`
- `POST /counter/increment` -> `{ "value": number }`

## Notes backend

- Au demarrage, le backend applique les migrations EF (`Database.Migrate()`).
- Si la ligne `{ id: 1 }` est absente, elle est creee avec `value = 0`.
- Table cible: `counter_state`:
  - `id` (PK int)
  - `value` (int not null)
  - `updated_at` (timestamp)

## Configuration Pidhex

### Backend project
- `repo`: votre repo Git
- `branch`: `main`
- `dockerfilePath`: `backend/Dockerfile`
- `contextPath`: `backend`
- `targetPort`: `4002`
- `databaseEnabled`: `true`
- `databaseEnvVarName`: `ConnectionStrings__Default`

### Frontend project
- `repo`: votre repo Git
- `branch`: `main`
- `dockerfilePath`: `frontend/Dockerfile`
- `contextPath`: `frontend`
- `targetPort`: `4001`
- `databaseEnabled`: `false`
- `databaseEnvVarName`: *(vide)*

