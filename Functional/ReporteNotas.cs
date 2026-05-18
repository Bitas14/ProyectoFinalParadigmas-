using ProyectoFinal.Models;

namespace ProyectoFinal.Functional
{
    // PROGRAMACIÓN FUNCIONAL
    // Todos los métodos son funciones puras: dado el mismo input, siempre el mismo output.
    // No modifican estado externo (sin efectos secundarios).
    // Usan LINQ (Where, Select, Aggregate) y Func<>/Action<> como parámetros de alto orden.
    public static class ReporteNotas
    {
        // FUNCIONAL - Función pura con Func<> como parámetro de alto orden
        // El filtro es configurable sin modificar este método (OCP funcional)
        public static List<Nota> FiltrarNotas(List<Nota> notas, Func<Nota, bool> criterio) =>
            notas.Where(criterio).ToList();

        // FUNCIONAL - Función pura con Select (proyección/transformación)
        // Transforma una lista de notas en resumenes por estudiante
        public static List<ResumenEstudiante> GenerarResumenes(
            List<Nota> notas,
            List<Estudiante> estudiantes)
        {
            // LINQ - Where + GroupBy + Select + Aggregate encadenados
            return estudiantes
                .Select(est =>
                {
                    var notasEst = notas.Where(n => n.EstudianteId == est.Id).ToList();

                    if (!notasEst.Any())
                        return new ResumenEstudiante(est.Id, est.NombreCompleto, 0, 0, 0, 0, 0);

                    // LINQ - Aggregate para calcular promedio acumulado (reduce funcional)
                    decimal promedio = notasEst.Aggregate(0m, (acc, n) => acc + n.ValorNota) / notasEst.Count;

                    return new ResumenEstudiante(
                        EstudianteId: est.Id,
                        NombreCompleto: est.NombreCompleto,
                        Promedio: Math.Round(promedio, 2),
                        NotaMaxima: notasEst.Max(n => n.ValorNota),
                        NotaMinima: notasEst.Min(n => n.ValorNota),
                        TotalNotas: notasEst.Count,
                        NotasAprobadas: notasEst.Count(n => n.Aprobada)
                    );
                })
                .Where(r => r.TotalNotas > 0)
                .OrderByDescending(r => r.Promedio)
                .ToList();
        }

        // FUNCIONAL - Función pura + Aggregate como acumulador (fold)
        public static decimal CalcularPromedioGeneral(List<Nota> notas)
        {
            if (!notas.Any()) return 0;
            // Aggregate: equivalente funcional al fold/reduce
            return Math.Round(notas.Aggregate(0m, (acc, n) => acc + n.ValorNota) / notas.Count, 2);
        }

        // FUNCIONAL - Función de alto orden: recibe Action<> para procesar cada resultado
        // Separa la generación del reporte de su presentación (SRP funcional)
        public static void ProcesarReporte(
            List<ResumenEstudiante> resumenes,
            Action<ResumenEstudiante> accionPorEstudiante)
        {
            // Select + ForEach usando Action<> como parámetro de alto orden
            resumenes.ForEach(accionPorEstudiante);
        }

        // FUNCIONAL - Función pura: top N estudiantes sin modificar la lista original
        public static List<ResumenEstudiante> ObtenerTopEstudiantes(
            List<ResumenEstudiante> resumenes,
            int cantidad) =>
            resumenes
                .OrderByDescending(r => r.Promedio)
                .Take(cantidad)
                .ToList();

        // FUNCIONAL - Función pura con Select para proyectar solo los datos necesarios
        public static List<string> ObtenerEstudiantesEnRiesgo(
            List<ResumenEstudiante> resumenes,
            decimal umbral = 3.0m) =>
            resumenes
                .Where(r => r.Promedio < umbral)
                .Select(r => $"{r.NombreCompleto} (Promedio: {r.Promedio:F2})")
                .ToList();
    }
}
