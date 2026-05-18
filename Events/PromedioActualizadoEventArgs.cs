namespace ProyectoFinal.Events
{
    /// <summary>
    /// PARADIGMA DE EVENTOS - Parte del Reactivo
    /// Captura cambios de estado en el dominio: cuando el promedio de un estudiante se actualiza.
    /// Permite desacoplamiento entre la lógica de negocio y la reacción a eventos.
    /// </summary>
    public class PromedioActualizadoEventArgs : EventArgs
    {
        public int EstudianteId { get; }
        public string NombreEstudiante { get; }
        public decimal PromedioAnterior { get; }
        public decimal PromedioNuevo { get; }
        public DateTime Momento { get; }

        public bool Mejoro => PromedioNuevo > PromedioAnterior;

        public PromedioActualizadoEventArgs(
            int estudianteId,
            string nombreEstudiante,
            decimal promedioAnterior,
            decimal promedioNuevo)
        {
            EstudianteId = estudianteId;
            NombreEstudiante = nombreEstudiante;
            PromedioAnterior = promedioAnterior;
            PromedioNuevo = promedioNuevo;
            Momento = DateTime.Now;
        }
    }
}
