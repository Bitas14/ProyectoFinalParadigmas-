using ProyectoFinal.Events;
using ProyectoFinal.Functional;
using ProyectoFinal.Models;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    // SOLID - SRP: solo responsabilidad de presentación e interacción con el usuario
    // El menú no contiene lógica de negocio, solo orquesta los servicios
    public class Menu
    {
        private readonly IEstudianteServicio _estudianteServicio;
        private readonly IMateriaServicio _materiaServicio;
        private readonly INotaServicio _notaServicio;

        // SOLID - DIP: recibe abstracciones (interfaces), no implementaciones concretas
        public Menu(
            IEstudianteServicio estudianteServicio,
            IMateriaServicio materiaServicio,
            INotaServicio notaServicio)
        {
            _estudianteServicio = estudianteServicio;
            _materiaServicio = materiaServicio;
            _notaServicio = notaServicio;

            // PROGRAMACIÓN ORIENTADA A EVENTOS - Suscripción a eventos del dominio
            // El Menu es el Oyente (Subscriber) de los eventos que dispara NotaServicio
            _notaServicio.NotaRegistrada += OnNotaRegistrada;
            _notaServicio.PromedioActualizado += OnPromedioActualizado;
        }

        // MANEJADORES DE EVENTOS - Reaccionan cuando el dominio notifica cambios
        private void OnNotaRegistrada(object? sender, NotaRegistradaEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✓ [EVENTO: NotaRegistrada]");
            Console.WriteLine($"    Estudiante: {e.NombreEstudiante}");
            Console.WriteLine($"    Materia:    {e.NombreMateria}");
            Console.WriteLine($"    Nota:       {e.ValorNota:F1}  |  {(e.ValorNota >= 3.0m ? "APROBADA" : "REPROBADA")}");
            Console.WriteLine($"    Momento:    {e.Momento:HH:mm:ss}");
            Console.ResetColor();
        }

        private void OnPromedioActualizado(object? sender, PromedioActualizadoEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  ↺ [EVENTO: PromedioActualizado]");
            Console.WriteLine($"    Estudiante:         {e.NombreEstudiante}");
            Console.WriteLine($"    Promedio anterior:  {e.PromedioAnterior:F2}");
            Console.WriteLine($"    Promedio nuevo:     {e.PromedioNuevo:F2}  {(e.Mejoro ? "▲ Mejoró" : "▼ Bajó")}");
            Console.ResetColor();
        }

        public void Ejecutar()
        {
            Console.Clear();
            bool salir = false;
            while (!salir)
            {
                MostrarMenuPrincipal();
                var opcion = Console.ReadLine();
                Console.Clear();
                switch (opcion)
                {
                    case "1": MenuEstudiantes(); break;
                    case "2": MenuProfesores(); break;
                    case "3": MenuMaterias(); break;
                    case "4": MenuNotas(); break;
                    case "5": MenuReportes(); break;
                    case "6": DemoPolimorfismo(); break;
                    case "0": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        Pausar();
                        break;
                }
            }
            Console.WriteLine("\nHasta luego.");
        }

        private void MostrarMenuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      SISTEMA DE NOTAS — PROYECTO FINAL       ║");
            Console.WriteLine("║   POO · AOP · Funcional · Eventos · SOLID    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Gestión de Estudiantes                   ║");
            Console.WriteLine("║  2. Gestión de Profesores                    ║");
            Console.WriteLine("║  3. Gestión de Materias                      ║");
            Console.WriteLine("║  4. Gestión de Notas                         ║");
            Console.WriteLine("║  5. Reportes Funcionales (LINQ)              ║");
            Console.WriteLine("║  6. Demo Polimorfismo (POO)                  ║");
            Console.WriteLine("║  0. Salir                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("Opción: ");
        }

        // ─── ESTUDIANTES ──────────────────────────────────────────────────────────

        private void MenuEstudiantes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("── ESTUDIANTES ──────────────────────────────");
                Console.WriteLine("  1. Crear estudiante");
                Console.WriteLine("  2. Listar estudiantes");
                Console.WriteLine("  3. Modificar estudiante");
                Console.WriteLine("  4. Eliminar estudiante");
                Console.WriteLine("  0. Volver");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1": CrearEstudiante(); break;
                    case "2": ListarEstudiantes(); break;
                    case "3": ModificarEstudiante(); break;
                    case "4": EliminarEstudiante(); break;
                    case "0": volver = true; break;
                }
                Console.Clear();
            }
        }

        private void CrearEstudiante()
        {
            Console.WriteLine("\n── Crear Estudiante ─────────────────────────");
            var est = new Estudiante
            {
                Nombres = LeerTexto("Nombres: "),
                Apellidos = LeerTexto("Apellidos: "),
                Matricula = LeerTexto("Matrícula: "),
                DireccionResidencia = new Direccion
                {
                    Calle = LeerTexto("Calle: "),
                    Ciudad = LeerTexto("Ciudad: ")
                }
            };
            _estudianteServicio.Crear(est);
            Console.WriteLine("\nEstudiante creado exitosamente.");
            Pausar();
        }

        private void ListarEstudiantes()
        {
            Console.WriteLine("\n── Estudiantes Registrados ──────────────────");
            var lista = _estudianteServicio.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("Sin registros."); Pausar(); return; }
            foreach (var e in lista)
                Console.WriteLine($"  [{e.Id}] {e.NombreCompleto} | {e.Matricula} | {e.DireccionResidencia.Ciudad}");
            Pausar();
        }

        private void ModificarEstudiante()
        {
            ListarEstudiantes();
            Console.Write("ID a modificar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            var est = _estudianteServicio.ObtenerPorId(id);
            if (est == null) { Console.WriteLine("No encontrado."); Pausar(); return; }

            est.Nombres = LeerTexto($"Nombres [{est.Nombres}]: ", est.Nombres);
            est.Apellidos = LeerTexto($"Apellidos [{est.Apellidos}]: ", est.Apellidos);
            est.Matricula = LeerTexto($"Matrícula [{est.Matricula}]: ", est.Matricula);
            _estudianteServicio.Actualizar(est);
            Console.WriteLine("Actualizado.");
            Pausar();
        }

        private void EliminarEstudiante()
        {
            ListarEstudiantes();
            Console.Write("ID a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            _estudianteServicio.Eliminar(id);
            Console.WriteLine("Eliminado.");
            Pausar();
        }

        // ─── PROFESORES ───────────────────────────────────────────────────────────

        private void MenuProfesores()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("── PROFESORES ───────────────────────────────");
                Console.WriteLine("  1. Crear profesor");
                Console.WriteLine("  2. Listar profesores");
                Console.WriteLine("  0. Volver");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1": CrearProfesor(); break;
                    case "2": ListarProfesores(); break;
                    case "0": volver = true; break;
                }
                Console.Clear();
            }
        }

        private void CrearProfesor()
        {
            Console.WriteLine("\n── Crear Profesor ───────────────────────────");
            var prof = new Profesor
            {
                Nombres = LeerTexto("Nombres: "),
                Apellidos = LeerTexto("Apellidos: "),
                Especialidad = LeerTexto("Especialidad: ")
            };
            // Los Profesores se guardan con EstudianteServicio no aplica;
            // usamos el repositorio directo por simplicidad.
            // En este sistema el Profesor se gestiona a través del repo genérico.
            var repo = new Repositories.CsvRepository<Profesor>("Profesores");
            repo.Create(prof);
            Console.WriteLine("\nProfesor creado exitosamente.");
            Pausar();
        }

        private void ListarProfesores()
        {
            Console.WriteLine("\n── Profesores Registrados ───────────────────");
            var repo = new Repositories.CsvRepository<Profesor>("Profesores");
            var lista = repo.GetAll();
            if (!lista.Any()) { Console.WriteLine("Sin registros."); Pausar(); return; }
            foreach (var p in lista)
                Console.WriteLine($"  [{p.Id}] {p.NombreCompleto} | {p.Especialidad}");
            Pausar();
        }

        // ─── MATERIAS ─────────────────────────────────────────────────────────────

        private void MenuMaterias()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("── MATERIAS ─────────────────────────────────");
                Console.WriteLine("  1. Crear materia");
                Console.WriteLine("  2. Listar materias");
                Console.WriteLine("  0. Volver");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1": CrearMateria(); break;
                    case "2": ListarMaterias(); break;
                    case "0": volver = true; break;
                }
                Console.Clear();
            }
        }

        private void CrearMateria()
        {
            Console.WriteLine("\n── Crear Materia ────────────────────────────");
            ListarProfesores();
            Console.Write("ID del profesor titular: ");
            int.TryParse(Console.ReadLine(), out int profId);

            var mat = new Materia
            {
                Nombre = LeerTexto("Nombre: "),
                Creditos = LeerEntero("Créditos: "),
                ProfesorTitularId = profId
            };
            _materiaServicio.Crear(mat);
            Console.WriteLine("\nMateria creada exitosamente.");
            Pausar();
        }

        private void ListarMaterias()
        {
            Console.WriteLine("\n── Materias Registradas ─────────────────────");
            var lista = _materiaServicio.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("Sin registros."); Pausar(); return; }
            foreach (var m in lista)
                Console.WriteLine($"  [{m.Id}] {m.Nombre} | {m.Creditos} créditos | Prof ID: {m.ProfesorTitularId}");
            Pausar();
        }

        // ─── NOTAS ────────────────────────────────────────────────────────────────

        private void MenuNotas()
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("── NOTAS ────────────────────────────────────");
                Console.WriteLine("  1. Registrar nota (dispara eventos)");
                Console.WriteLine("  2. Listar notas");
                Console.WriteLine("  3. Ver notas por estudiante");
                Console.WriteLine("  0. Volver");
                Console.Write("Opción: ");

                switch (Console.ReadLine())
                {
                    case "1": RegistrarNota(); break;
                    case "2": ListarNotas(); break;
                    case "3": NotasPorEstudiante(); break;
                    case "0": volver = true; break;
                }
                Console.Clear();
            }
        }

        private void RegistrarNota()
        {
            Console.WriteLine("\n── Registrar Nota ───────────────────────────");
            ListarEstudiantes();
            Console.Write("ID del estudiante: ");
            int.TryParse(Console.ReadLine(), out int estId);
            var est = _estudianteServicio.ObtenerPorId(estId);
            if (est == null) { Console.WriteLine("Estudiante no encontrado."); Pausar(); return; }

            ListarMaterias();
            Console.Write("ID de la materia: ");
            int.TryParse(Console.ReadLine(), out int matId);
            var mat = _materiaServicio.ObtenerPorId(matId);
            if (mat == null) { Console.WriteLine("Materia no encontrada."); Pausar(); return; }

            Console.Write("Valor de la nota (0.0 - 5.0): ");
            decimal.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal valor);

            var nota = new Nota
            {
                EstudianteId = estId,
                MateriaId = matId,
                ValorNota = valor,
                Comentarios = LeerTexto("Comentarios (opcional): ", "-")
            };

            Console.WriteLine("\n  [Interceptores activos: Logging + Validación]");
            // Al llamar Crear, Castle Windsor intercepta primero con Logging y Validación
            // Luego NotaServicio dispara los eventos NotaRegistrada y PromedioActualizado
            _notaServicio.Crear(nota, est.NombreCompleto, mat.Nombre);
            Pausar();
        }

        private void ListarNotas()
        {
            Console.WriteLine("\n── Notas Registradas ────────────────────────");
            var lista = _notaServicio.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("Sin registros."); Pausar(); return; }
            foreach (var n in lista)
            {
                var est = _estudianteServicio.ObtenerPorId(n.EstudianteId);
                var mat = _materiaServicio.ObtenerPorId(n.MateriaId);
                Console.WriteLine($"  [{n.Id}] {est?.NombreCompleto ?? "?"} | {mat?.Nombre ?? "?"} | {n.ValorNota:F1} | {(n.Aprobada ? "✓" : "✗")}");
            }
            Pausar();
        }

        private void NotasPorEstudiante()
        {
            ListarEstudiantes();
            Console.Write("ID del estudiante: ");
            int.TryParse(Console.ReadLine(), out int estId);
            var est = _estudianteServicio.ObtenerPorId(estId);
            if (est == null) { Console.WriteLine("No encontrado."); Pausar(); return; }

            var notas = _notaServicio.ObtenerPorEstudiante(estId);
            Console.WriteLine($"\nNotas de {est.NombreCompleto}:");
            foreach (var n in notas)
            {
                var mat = _materiaServicio.ObtenerPorId(n.MateriaId);
                Console.WriteLine($"  {mat?.Nombre ?? "?"}: {n.ValorNota:F1} {(n.Aprobada ? "✓" : "✗")}");
            }
            decimal promedio = _notaServicio.CalcularPromedio(estId);
            Console.WriteLine($"\n  Promedio: {promedio:F2}");
            Pausar();
        }

        // ─── REPORTES FUNCIONALES ─────────────────────────────────────────────────

        private void MenuReportes()
        {
            Console.WriteLine("── REPORTES FUNCIONALES (LINQ) ──────────────\n");

            var todasLasNotas = _notaServicio.ObtenerTodos();
            var todosLosEstudiantes = _estudianteServicio.ObtenerTodos();

            if (!todasLasNotas.Any() || !todosLosEstudiantes.Any())
            {
                Console.WriteLine("No hay suficientes datos para generar reportes.");
                Pausar();
                return;
            }

            // FUNCIONAL - GenerarResumenes usa Select + Aggregate internamente
            var resumenes = ReporteNotas.GenerarResumenes(todasLasNotas, todosLosEstudiantes);

            // Promedio general con Aggregate
            decimal promedioGeneral = ReporteNotas.CalcularPromedioGeneral(todasLasNotas);

            Console.WriteLine($"  Promedio general del sistema: {promedioGeneral:F2}\n");

            // FUNCIONAL - ProcesarReporte recibe Action<> como parámetro de alto orden
            Console.WriteLine("  ── Resumen por estudiante ──────────────────");
            ReporteNotas.ProcesarReporte(resumenes, r =>
            {
                Console.ForegroundColor = r.Promedio >= 3.0m ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  {r}");
                Console.ResetColor();
            });

            // FUNCIONAL - FiltrarNotas con Func<> predefinido
            Console.WriteLine("\n  ── Notas aprobadas (Where) ─────────────────");
            var aprobadas = ReporteNotas.FiltrarNotas(todasLasNotas, n => n.Aprobada);
            Console.WriteLine($"  Total aprobadas: {aprobadas.Count} / {todasLasNotas.Count}");

            // FUNCIONAL - Top 3 estudiantes
            Console.WriteLine("\n  ── Top 3 estudiantes ───────────────────────");
            var top = ReporteNotas.ObtenerTopEstudiantes(resumenes, 3);
            top.ForEach(r => Console.WriteLine($"  ★ {r.NombreCompleto}: {r.Promedio:F2}"));

            // FUNCIONAL - Estudiantes en riesgo
            Console.WriteLine("\n  ── Estudiantes en riesgo (Promedio < 3.0) ──");
            var enRiesgo = ReporteNotas.ObtenerEstudiantesEnRiesgo(resumenes);
            if (enRiesgo.Any())
                enRiesgo.ForEach(e => Console.WriteLine($"  ⚠ {e}"));
            else
                Console.WriteLine("  Ninguno. ¡Todos aprobando!");

            Pausar();
        }

        // ─── DEMO POLIMORFISMO ────────────────────────────────────────────────────

        private void DemoPolimorfismo()
        {
            Console.WriteLine("── DEMO POLIMORFISMO EN TIEMPO DE EJECUCIÓN ─\n");
            Console.WriteLine("  Lista de Persona[] con Estudiantes y Profesores mezclados:");
            Console.WriteLine("  El mismo método ToString() produce salidas diferentes según el tipo real.\n");

            var repoEstudiantes = _estudianteServicio.ObtenerTodos();
            var repoProfesores = new Repositories.CsvRepository<Profesor>("Profesores").GetAll();

            // POO - Lista de tipo base Persona que contiene subtipos diferentes
            // Polimorfismo: el runtime decide qué ToString()/ObtenerRol() ejecutar
            var personas = new List<Persona>();
            personas.AddRange(repoEstudiantes);
            personas.AddRange(repoProfesores);

            if (!personas.Any())
            {
                Console.WriteLine("  Crea algunos estudiantes y profesores primero para ver el polimorfismo.");
                Pausar();
                return;
            }

            foreach (var p in personas)
            {
                // Una sola llamada a ToString(), pero el comportamiento varía según el tipo real
                Console.ForegroundColor = p is Estudiante ? ConsoleColor.Cyan : ConsoleColor.Magenta;
                Console.WriteLine($"  {p}");          // Llama a ToString() polimórfico
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"    Resumen: {p.ObtenerResumen()}");  // IReportable polimórfico
                Console.ResetColor();
            }

            Console.WriteLine($"\n  Tipo en tiempo de ejecución de cada objeto:");
            foreach (var p in personas)
                Console.WriteLine($"  {p.NombreCompleto} → {p.GetType().Name}");

            Pausar();
        }

        // ─── UTILIDADES ───────────────────────────────────────────────────────────

        private static string LeerTexto(string prompt, string valorPorDefecto = "")
        {
            Console.Write(prompt);
            var input = Console.ReadLine() ?? string.Empty;
            return string.IsNullOrWhiteSpace(input) ? valorPorDefecto : input;
        }

        private static int LeerEntero(string prompt)
        {
            Console.Write(prompt);
            int.TryParse(Console.ReadLine(), out int result);
            return result;
        }

        private static void Pausar()
        {
            Console.WriteLine("\nPresiona Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
