namespace ProyectoFinal.Models
{
    // PROGRAMACIÓN FUNCIONAL - Tipo inmutable (record)
    // Un record en C# es inmutable por defecto: una vez creado no se modifica,
    // se crea uno nuevo si se necesitan cambios (estilo funcional puro)
    public record ResumenEstudiante(
        int EstudianteId,
        string NombreCompleto,
        decimal Promedio,
        decimal NotaMaxima,
        decimal NotaMinima,
        int TotalNotas,
        int NotasAprobadas
    )
    {
        public decimal PorcentajeAprobacion =>
            TotalNotas == 0 ? 0 : Math.Round((decimal)NotasAprobadas / TotalNotas * 100, 1);

        public override string ToString() =>
            $"{NombreCompleto} | Promedio: {Promedio:F2} | Aprobación: {PorcentajeAprobacion}% ({NotasAprobadas}/{TotalNotas})";
    }
}
