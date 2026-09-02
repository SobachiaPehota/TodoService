# TodoService API

Сервис для управления пользователями и задачами с интеграцией dummyjson.com. Данные кешируются в PostgreSQL, обновляются по расписанию и доступны через REST API.

## Стек

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core 8 + Npgsql
- PostgreSQL 16
- Docker + Docker Compose

## Функциональность

- Автоматическая синхронизация пользователей и задач с dummyjson.com
- Фоновая синхронизация с настраиваемым интервалом
- REST API с пагинацией, фильтрацией и сортировкой
- Документация через Swagger

## Запуск

```bash
docker compose up -d

Запускается на localhost:8080
