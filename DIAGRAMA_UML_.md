# Guía para crear el Diagrama UML en Draw.io

## Clases a incluir en el diagrama:

### Clases Base (POO - Jerarquía)
- **Persona** (ABSTRACTA)
  - Propiedades: id, nombre, email
  - Método abstracto: ObtenerRol()
  - Interfaces: IIdentifiable, IReportable

- **Estudiante** (extiende Persona)
  - Propiedad adicional: direccion (Direccion)
  - Propiedades: materias (List<int>), notas (List<Nota>)

- **Profesor** (extiende Persona)
  - Propiedades: titulo, materias (List<int>)

### Clases de Dominio
- **Materia**
  - Propiedades: id, nombre, profesor_id (referencia a Profesor)
  - Interfaz: IIdentifiable, IReportable

- **Nota**
  - Propiedades: id, estudiante_id, materia_id, valor
  - Relación: Asociación a Estudiante y Materia

- **Direccion**
  - Propiedades: calle, numero, ciudad, pais
  - Relación: Composición con Estudiante (mismo ciclo de vida)

### Interfaces
- **IIdentifiable**
  - Propiedad: Id
  - Método: ObtenerRol()

- **IReportable**
  - Método: ObtenerResumen()

## Relaciones a mostrar:

1. **Herencia**: Estudiante y Profesor heredan de Persona
2. **Composición**: Estudiante contiene Direccion (la línea debe ser sólida con rombo lleno)
3. **Agregación**: Materia referencia Profesor por Id (línea con rombo vacío)
4. **Asociación**: Nota se asocia con Estudiante y Materia (línea sencilla)
5. **Implementación de Interfaz**: Persona, Materia, Nota implementan IIdentifiable e IReportable (línea punteada)

```  classDiagram

      %% ── Interfaces (Models) ──────────────────────────────────
      class IIdentifiable {
          <<interface>>
          +int Id
      }
      class IReportable {
          <<interface>>
          +ObtenerResumen() string
      }

      %% ── Domain Models ────────────────────────────────────────
      class Persona {
          <<abstract>>
          +int Id
          +string Nombres
          +string Apellidos
          +string NombreCompleto
          +ObtenerRol()* string
          +ObtenerResumen()* string
          +ToString() string
      }
      class Estudiante {
          +string Matricula
          +Direccion DireccionResidencia
          +ObtenerRol() string
          +ObtenerResumen() string
      }
      class Profesor {
          +string Especialidad
          +ObtenerRol() string
          +ObtenerResumen() string
      }
      class Direccion {
          +string Calle
          +string Ciudad
          +string Pais
          +ToString() string
      }
      class Materia {
          +int Id
          +string Nombre
          +int Creditos
          +int ProfesorTitularId
          +ToString() string
      }
      class Nota {
          +int Id
          +int EstudianteId
          +int MateriaId
          +decimal ValorNota
          +string Comentarios
          +bool Aprobada
          +ToString() string
      }
      class ResumenEstudiante {
          <<record>>
          +int EstudianteId
          +string NombreCompleto
          +decimal Promedio
          +decimal NotaMaxima
          +decimal NotaMinima
          +int TotalNotas
          +int NotasAprobadas
          +decimal PorcentajeAprobacion
          +ToString() string
      }
  
      %% ── Service Interfaces ───────────────────────────────────
      class IEstudianteServicio {
          <<interface>>
          +Crear(Estudiante) void
          +ObtenerPorId(int) Estudiante
          +ObtenerTodos() List~Estudiante~
          +Actualizar(Estudiante) void
          +Eliminar(int) void
      }
      class IMateriaServicio {
          <<interface>>
          +Crear(Materia) void
          +ObtenerPorId(int) Materia
          +ObtenerTodos() List~Materia~
          +Actualizar(Materia) void
          +Eliminar(int) void
      }
      class INotaServicio {
          <<interface>>
          +NotaRegistrada EventHandler
          +PromedioActualizado EventHandler
          +Crear(Nota, string, string) void
          +ObtenerPorId(int) Nota
          +ObtenerTodos() List~Nota~
          +ObtenerPorEstudiante(int) List~Nota~
          +Actualizar(Nota) void
          +Eliminar(int) void
          +CalcularPromedio(int) decimal
      }

      %% ── Service Implementations ──────────────────────────────
      class EstudianteServicio {
          -CsvRepository~Estudiante~ _repo
          +Crear(Estudiante) void
          +ObtenerPorId(int) Estudiante
          +ObtenerTodos() List~Estudiante~
          +Actualizar(Estudiante) void
          +Eliminar(int) void
      }
      class MateriaServicio {
          -CsvRepository~Materia~ _repo
          +Crear(Materia) void
          +ObtenerPorId(int) Materia
          +ObtenerTodos() List~Materia~
          +Actualizar(Materia) void
          +Eliminar(int) void
      }
      class NotaServicio {
          -CsvRepository~Nota~ _repo
          +NotaRegistrada EventHandler
          +PromedioActualizado EventHandler
          +Crear(Nota, string, string) void
          +ObtenerPorId(int) Nota
          +ObtenerTodos() List~Nota~
          +ObtenerPorEstudiante(int) List~Nota~
          +Actualizar(Nota) void
          +Eliminar(int) void
          +CalcularPromedio(int) decimal
      }

      %% ── Repository ───────────────────────────────────────────
      class CsvRepository~T~ {
          -string _filePath
          +GetAll() List~T~
          +SaveAll(List~T~) void
          +Create(T) void
          +Read(int) T
          +Update(T) void
          +Delete(int) void
      }

      %% ── AOP – Interceptors ───────────────────────────────────
      class IInterceptor {
          <<interface>>
          +Intercept(IInvocation) void
      }
      class LoggingInterceptor {
          +Intercept(IInvocation) void
      }
      class ValidationInterceptor {
          +Intercept(IInvocation) void
      }
      class ContenedorWindsor {
          <<static>>
          -IWindsorContainer _contenedor
          +Inicializar() IWindsorContainer
          +Resolver~T~() T
      }

      %% ── Events ───────────────────────────────────────────────
      class NotaRegistradaEventArgs {
          +int EstudianteId
          +string NombreEstudiante
          +int MateriaId
          +string NombreMateria
          +decimal ValorNota
          +DateTime Momento
      }
      class PromedioActualizadoEventArgs {
          +int EstudianteId
          +string NombreEstudiante
          +decimal PromedioAnterior
          +decimal PromedioNuevo
      }

      %% ── Functional ───────────────────────────────────────────
      class ReporteNotas {
          <<static>>
          +FiltrarNotas(List~Nota~, Func) List~Nota~
          +GenerarResumenes(List~Nota~, List~Estudiante~) List~ResumenEstudiante~
          +CalcularPromedioGeneral(List~Nota~) decimal
          +ProcesarReporte(List~ResumenEstudiante~, Action) void
          +ObtenerTopEstudiantes(List~ResumenEstudiante~, int) List~ResumenEstudiante~
          +ObtenerEstudiantesEnRiesgo(List~ResumenEstudiante~, decimal) List~string~
      }

      %% ── UI ───────────────────────────────────────────────────
      class Menu {
          -IEstudianteServicio _estudianteServicio
          -IMateriaServicio _materiaServicio
          -INotaServicio _notaServicio
          +Ejecutar() void
      }

      %% ── Relationships ────────────────────────────────────────
  
      %% Interfaces → Persona
      IIdentifiable <|.. Persona : implements
      IReportable   <|.. Persona : implements
      IIdentifiable <|.. Materia : implements
      IIdentifiable <|.. Nota    : implements
  
      %% Herencia
      Persona <|-- Estudiante
      Persona <|-- Profesor
  
      %% Composición / Agregación / Asociación
      Estudiante *-- Direccion         : composición
      Materia    ..> Profesor          : agregación (ProfesorTitularId)
      Nota       ..> Estudiante        : asociación (EstudianteId)
      Nota       ..> Materia           : asociación (MateriaId)

      %% Services
      IEstudianteServicio <|.. EstudianteServicio : implements
      IMateriaServicio    <|.. MateriaServicio    : implements
      INotaServicio       <|.. NotaServicio       : implements

      EstudianteServicio --> CsvRepository~Estudiante~ : instancia
      MateriaServicio    --> CsvRepository~Materia~    : instancia
      NotaServicio       --> CsvRepository~Nota~       : instancia

      %% AOP
      IInterceptor <|.. LoggingInterceptor    : implements
      IInterceptor <|.. ValidationInterceptor : implements
      ContenedorWindsor --> LoggingInterceptor    : registra
      ContenedorWindsor --> ValidationInterceptor : registra
      ContenedorWindsor ..> IEstudianteServicio   : resuelve
      ContenedorWindsor ..> IMateriaServicio      : resuelve
      ContenedorWindsor ..> INotaServicio         : resuelve

      %% Events
      NotaServicio ..> NotaRegistradaEventArgs      : dispara
      NotaServicio ..> PromedioActualizadoEventArgs  : dispara

      %% Functional
      ReporteNotas ..> Nota             : usa
      ReporteNotas ..> Estudiante       : usa
      ReporteNotas ..> ResumenEstudiante : produce

      %% UI
      Menu --> IEstudianteServicio : usa
      Menu --> IMateriaServicio    : usa
      Menu --> INotaServicio       : usa
      Menu ..> ReporteNotas        : llama
```
