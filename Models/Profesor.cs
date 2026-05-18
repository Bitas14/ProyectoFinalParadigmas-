namespace ProyectoFinal.Models
{
    // POO - Herencia de Persona
    // SOLID - LSP: puede reemplazar a Persona sin romper el contrato
    public class Profesor : Persona
    {
        public string Especialidad { get; set; } = string.Empty;

        // POO - Polimorfismo: implementación concreta del método abstracto
        public override string ObtenerRol() => "Profesor";

        public override string ObtenerResumen() =>
            $"Profesor: {NombreCompleto} | Especialidad: {Especialidad}";
    }
}
