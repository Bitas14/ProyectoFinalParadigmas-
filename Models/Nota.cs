namespace ProyectoFinal.Models
{
    // POO - Asociación: puente entre Estudiante y Materia
    // SOLID - SRP: solo representa la calificación de un estudiante en una materia
    public class Nota : IIdentifiable
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public decimal ValorNota { get; set; }
        public string Comentarios { get; set; } = string.Empty;

        public bool Aprobada => ValorNota >= 3.0m;

        public override string ToString() =>
            $"Nota {ValorNota:F1} | {(Aprobada ? "APROBADA" : "REPROBADA")} | {Comentarios}";
    }
}
