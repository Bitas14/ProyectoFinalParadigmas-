namespace ProyectoFinal.Events
{
    // PROGRAMACIÓN ORIENTADA A EVENTOS
    // Evento semánticamente significativo: el promedio del estudiante cambió.
    // Esto es un cambio de estado del dominio, no solo un evento técnico.
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
