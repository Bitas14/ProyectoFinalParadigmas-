namespace ProyectoFinal.Models
{
    // POO - Herencia de Persona + Composición con Direccion
    // SOLID - LSP: puede reemplazar a Persona sin romper el contrato
    public class Estudiante : Persona
    {
        public string Matricula { get; set; } = string.Empty;

        // POO - Composición: Direccion no existe sin Estudiante
        public Direccion DireccionResidencia { get; set; } = new Direccion();

        // POO - Polimorfismo: implementación concreta del método abstracto
        public override string ObtenerRol() => "Estudiante";

        public override string ObtenerResumen() =>
            $"Estudiante: {NombreCompleto} | Matrícula: {Matricula} | Ciudad: {DireccionResidencia.Ciudad}";
    }
}
