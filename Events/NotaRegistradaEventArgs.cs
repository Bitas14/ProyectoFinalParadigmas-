namespace ProyectoFinal.Events
{
    /// <summary>
    /// PARADIGMA DE EVENTOS - Parte del Reactivo
    /// Encapsula información de dominio cuando se registra una nota.
    /// Permite que múltiples suscriptores reaccionen al evento de forma desacoplada.
    /// </summary>
    public class NotaRegistradaEventArgs : EventArgs
    {
        public int EstudianteId { get; }
        public string NombreEstudiante { get; }
        public int MateriaId { get; }
        public string NombreMateria { get; }
        public decimal ValorNota { get; }
        public DateTime Momento { get; }

        public NotaRegistradaEventArgs(
            int estudianteId,
            string nombreEstudiante,
            int materiaId,
            string nombreMateria,
            decimal valorNota)
        {
            EstudianteId = estudianteId;
            NombreEstudiante = nombreEstudiante;
            MateriaId = materiaId;
            NombreMateria = nombreMateria;
            ValorNota = valorNota;
            Momento = DateTime.Now;
        }
    }
}
