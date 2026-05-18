namespace ProyectoFinal.Models
{
    // POO - Clase abstracta base: herencia + abstracción
    // SOLID - SRP: solo datos de identidad personal
    public abstract class Persona : IIdentifiable, IReportable
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;

        public string NombreCompleto => $"{Nombres} {Apellidos}";

        // POO - Método abstracto: polimorfismo en tiempo de ejecución
        public abstract string ObtenerRol();

        // POO - Override para polimorfismo demostrable
        public override string ToString() => $"[{ObtenerRol()}] {NombreCompleto} (ID: {Id})";

        // IReportable - cada subclase define su propio resumen
        public abstract string ObtenerResumen();
    }
}
