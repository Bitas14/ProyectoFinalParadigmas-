namespace ProyectoFinal.Models
{
    // POO - Agregación: referencia al Profesor por Id (el Profesor existe independientemente)
    // SOLID - SRP: solo datos de la materia
    public class Materia : IIdentifiable
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }

        // POO - Agregación: el Profesor vive sin la Materia
        public int ProfesorTitularId { get; set; }

        public override string ToString() => $"{Nombre} ({Creditos} créditos)";
    }
}
