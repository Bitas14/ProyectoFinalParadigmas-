# ProyectoFinal — Sistema de Notas

Aplicación de consola en **.NET 8 / C#** que integra cuatro paradigmas de programación sobre un dominio de gestión académica.

## 📋 Índice de Contenidos

- [Dominio](#dominio)
- [Paradigmas Implementados](#paradigmas-implementados)
  - [1. Orientado a Objetos (POO)](#1-orientado-a-objetos-poo--15)
  - [2. Paradigma de Aspectos (AOP)](#2-paradigma-de-aspectos-aop--20)
  - [3. Programación Funcional](#3-programación-funcional--20)
  - [4. Programación Reactiva / Eventos](#4-programación-reactiva--eventos--15)
- [Decisiones Arquitectónicas](#decisiones-arquitectónicas)

---

## Dominio

Sistema de notas académicas: gestión de **Estudiantes**, **Profesores**, **Materias** y **Notas**, con persistencia en archivos CSV.

---

## Paradigmas implementados

### 1. Orientado a Objetos (POO) — 15%

| Concepto | Dónde se aplica |
|---|---|
| Herencia | `Persona` → `Estudiante`, `Profesor` |
| Composición | `Estudiante` contiene `Direccion` (mismo ciclo de vida) |
| Agregación | `Materia` referencia `Profesor` por Id (vive independientemente) |
| Asociación | `Nota` une `Estudiante` con `Materia` |
| Interfaz | `IIdentifiable`, `IReportable` |
| Polimorfismo | Método abstracto `ObtenerRol()` y `ObtenerResumen()` en `Persona`, demostrable en runtime |
| Abstracción | `Persona` es abstracta, no se instancia directamente |

**Decisión de diseño:** Se agrega `IReportable` como segunda interfaz (ISP) para separar la capacidad de reporte de la identidad. El polimorfismo se demuestra explícitamente en el menú "Demo Polimorfismo" con una lista `List<Persona>` que mezcla `Estudiante` y `Profesor`, llamando al mismo método y obteniendo comportamientos diferentes en tiempo de ejecución.

---

### 2. Paradigma de Aspectos (AOP) — 20%

Implementado con **Castle Windsor** como contenedor de Inyección de Dependencias y **Castle DynamicProxy** para los interceptores.

| Componente | Rol |
|---|---|
| `ContenedorWindsor` | Registra y resuelve servicios como interfaces |
| `LoggingInterceptor` | Registra entrada/salida de cada método automáticamente |
| `ValidationInterceptor` | Valida parámetros (nulos, rango de notas, nombres vacíos) centralmente |

**Decisión de diseño:** Los servicios se registran como su interfaz (`IEstudianteServicio`, `IMateriaServicio`, `INotaServicio`), lo que permite a Castle interceptar sin necesidad de métodos `virtual`. Los interceptores se aplican en cadena: primero `LoggingInterceptor`, luego `ValidationInterceptor`. Esto separa completamente las funciones transversales de la lógica de negocio.

---

### 3. Programación Funcional — 20%

Implementado en el módulo `ReporteNotas` (clase estática).

| Elemento funcional | Cómo se aplica |
|---|---|
| Funciones puras | Todos los métodos son sin efectos secundarios |
| LINQ `Where` | `FiltrarNotas(notas, criterio)` con `Func<Nota, bool>` configurable |
| LINQ `Select` | `GenerarResumenes()` proyecta `Nota` → `ResumenEstudiante` |
| LINQ `Aggregate` | Calcula el promedio como un fold funcional acumulativo |
| `Func<>` de alto orden | `FiltrarNotas` recibe el criterio como función |
| `Action<>` de alto orden | `ProcesarReporte` recibe la acción de presentación |
| Tipo inmutable `record` | `ResumenEstudiante` — inmutable por diseño |

**Decisión de diseño:** Se separa completamente la generación de reportes (`ReporteNotas`) de su presentación (`Menu`). Los métodos no reciben referencias a servicios, solo los datos necesarios: esto garantiza que sean puras y testables.

---

### 4. Programación Orientada a Eventos — 20%

| Componente | Rol |
|---|---|
| `NotaRegistradaEventArgs` | Datos del evento "se registró una nota" (dominio) |
| `PromedioActualizadoEventArgs` | Datos del evento "cambió el promedio del estudiante" |
| `NotaServicio` | Emisor (Publisher): dispara ambos eventos |
| `Menu` | Oyente (Subscriber): reacciona mostrando info en consola |

**Decisión de diseño:** Los eventos reflejan **cambios de estado significativos del dominio**, no solo acciones técnicas. `NotaRegistrada` y `PromedioActualizado` son sucesos de negocio reales. El `Menu` no conoce la implementación de `NotaServicio`; solo se suscribe a su evento a través de la interfaz `INotaServicio`.

---

### 5. Principios SOLID — 15%

| Principio | Aplicación |
|---|---|
| **SRP** | `Menu` solo presenta, `*Servicio` solo lógica de negocio, `CsvRepository` solo persistencia |
| **OCP** | `CsvRepository<T>` soporta cualquier entidad sin modificarse; `ReporteNotas` extensible con nuevos filtros vía `Func<>` |
| **LSP** | `Estudiante` y `Profesor` reemplazan a `Persona` en cualquier contexto sin romper el contrato |
| **ISP** | `IIdentifiable` e `IReportable` son contratos pequeños y específicos |
| **DIP** | `Menu` recibe `IEstudianteServicio`, `IMateriaServicio`, `INotaServicio` — nunca implementaciones concretas |

---

## Estructura del proyecto

```
ProyectoFinal/
├── Models/
│   ├── IIdentifiable.cs
│   ├── IReportable.cs
│   ├── Persona.cs           (abstracta)
│   ├── Direccion.cs
│   ├── Estudiante.cs
│   ├── Profesor.cs
│   ├── Materia.cs
│   ├── Nota.cs
│   └── ResumenEstudiante.cs (record inmutable)
├── Repositories/
│   └── CsvRepository.cs
├── Services/
│   ├── IEstudianteServicio.cs
│   ├── IMateriaServicio.cs
│   ├── INotaServicio.cs
│   ├── EstudianteServicio.cs
│   ├── MateriaServicio.cs
│   └── NotaServicio.cs
├── Interceptors/
│   ├── LoggingInterceptor.cs
│   └── ValidationInterceptor.cs
├── Events/
│   ├── NotaRegistradaEventArgs.cs
│   └── PromedioActualizadoEventArgs.cs
├── Functional/
│   └── ReporteNotas.cs
├── DI/
│   └── ContenedorWindsor.cs
├── Menu.cs
├── Program.cs
└── ProyectoFinal.csproj
```

## Paquetes NuGet

- `CsvHelper` — persistencia en CSV
- `Castle.Windsor` — contenedor de DI para AOP
- `Castle.Core` — DynamicProxy para interceptores

## Cómo ejecutar

Abrir `ProyectoFinal.sln` en Visual Studio, restaurar paquetes NuGet y presionar F5.
