# TodoItems DDD 🚀

[![TodoItems .NET CI](https://github.com/kitpymes/net-ddd-todo-items/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kitpymes/net-ddd-todo-items/actions/workflows/dotnet.yml)

Este proyecto es una implementación de referencia de una arquitectura **Domain-Driven Design (DDD)** robusta y moderna. El sistema está diseñado para gestionar listas de tareas complejas, aplicando un dominio enriquecido, patrones de persistencia avanzados y una estrategia de pruebas multinivel.

## 🏗️ Arquitectura del Sistema

El proyecto sigue una estructura de **Arquitectura Limpia (Clean Architecture)**, asegurando que el núcleo del negocio (Dominio) sea independiente de marcos externos, bases de datos o interfaces de usuario.

### 1. Capa de Presentación (`1-Presentacion`)
*   **TodoItems.Presentation.API**: Punto de entrada del sistema desarrollado en .NET 8. Implementa **Swagger/OpenAPI** y **ApiExplorer** para una documentación interactiva y estandarizada.
*   **TodoItems.Presentation.API.E2E.Tests**: Pruebas de extremo a extremo que validan el comportamiento del sistema desde la petición HTTP hasta la base de datos, asegurando que todos los componentes integrados funcionen correctamente.

### 2. Capa de Aplicación (`2-Application`)
*   **TodoItems.Application**: Orquesta el flujo del sistema mediante el patrón **CQRS** con **MediatR**. 
    *   **AutoMapper**: Utilizado para la transformación transparente entre entidades de dominio y DTOs de respuesta.
    *   **FluentValidation**: Reglas de validación declarativas para los Comandos (Commands) y Consultas (Queries).
    *   **ValidationMiddleware**: Middleware global que intercepta errores de validación y devuelve respuestas estandarizadas al cliente.
*   **TodoItems.Application.Tests**: Pruebas unitarias que aseguran que los Handlers orquesten correctamente el dominio y los servicios.

### 3. Capa de Dominio (`3-Domain`)
*   **TodoItems.Domain**: El corazón del sistema. Contiene los Agregados, Entidades y Objetos de Valor (**Value Objects**). 
    *   **Dominio Enriquecido**: El agregado `TodoList` protege sus propias invariantes (reglas de negocio) y gestiona colecciones privadas para evitar estados inconsistentes.
    *   **Eventos de Dominio**: Comunicación desacoplada mediante eventos internos.
*   **TodoItems.Domain.Tests**: Suite de pruebas unitarias críticas que validan las reglas de negocio estrictas.

### 4. Capa de Infraestructura (`4-Infrastructure`)
*   **TodoItems.Infrastructure**: Implementa la persistencia utilizando **Entity Framework Core** y **SQLite**.
    *   Mapeo avanzado de colecciones privadas y Value Objects.
    *   Configuración de despacho automático de eventos de dominio durante el `SaveChangesAsync`.
*   **TodoItems.Infrastructure.IntegrationTests**: Pruebas de integración que garantizan que el esquema de base de datos relacional refleje fielmente el modelo de dominio.

---

## ⚙️ Integración Continua (CI/CD)

El proyecto utiliza **GitHub Actions** para garantizar la calidad del código mediante un flujo de trabajo automatizado que se dispara en cada `push` o `pull_request` a la rama principal:

*   **Build Automático**: Compilación de toda la solución en entornos Windows ['6.0.x', '7.0.x', '8.0.x'].
*   **Tests Automatizados**: Ejecución secuencial de todas las capas de pruebas:
    *   Unit Tests (Domain & Application)
    *   Integration Tests (Infrastructure con SQLite)
    *   E2E Tests (Presentation)
*   **Workflow**: Definido en `.github/workflows/dotnet.yml`.

## 🛠️ Stack Tecnológico

*   **.NET 8:**	Framework de ejecución.
*   **MediatR:** Implementación de CQRS y mediación de eventos.
*   **EF Core + SQLite:** ORM y base de datos relacional ligera y portátil.
*   **FluentValidation:** Validación de lógica de entrada en la capa de aplicación.
*   **GitHub Actions:** Automatización de Build y Testing (CI).
*   **AutoMapper:** Mapeo de objetos entre capas.
*   **Swagger / OpenAPI:** Documentación y exploración de la API.
*   **xUnit + FluentAssertions:** Framework de pruebas y aserciones de lenguaje natural.

## 📂 Estructura de Carpetas

```text
├── 1-Presentacion
│   ├── TodoItems.Presentation.API
│   └── TodoItems.Presentation.API.E2E.Tests
├── 2-Application
│   ├── TodoItems.Application
│   └── TodoItems.Application.Tests
├── 3-Domain
│   ├── TodoItems.Domain
│   └── TodoItems.Domain.Tests
└── 4-Infrastructure
    ├── TodoItems.Infrastructure
    └── TodoItems.Infrastructure.IntegrationTests
```


## 🛡️ Reglas de Negocio (Invariantes del Dominio)

### El sistema garantiza por diseño las siguientes restricciones:
**Identidad Única:** No se permiten ítems con IDs duplicados dentro de la misma lista de tareas.
**Protección de Progreso:** No se permite editar (descripción) ni eliminar un ítem si este ya ha superado el 50% de su progreso total.
**Lógica de Progresión:**
- El porcentaje de cada registro debe ser mayor a 0 y la suma total no puede exceder el 100%.
- Las fechas de las nuevas progresiones deben ser estrictamente posteriores a las ya existentes.
**Consistencia:** Todas las validaciones residen en el agregado TodoList, garantizando que el dominio siempre sea el "único origen de la verdad".