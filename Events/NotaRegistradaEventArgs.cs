namespace ProyectoFinal.Events
{
    // PROGRAMACIÓN ORIENTADA A EVENTOS
    // EventArgs propio que transporta información semánticamente significativa del dominio:
    // "Una nota fue registrada" — no solo un evento técnico, sino un suceso de negocio
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
