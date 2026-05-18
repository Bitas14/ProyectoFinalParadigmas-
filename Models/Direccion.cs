namespace ProyectoFinal.Models
{
    // POO - Composición: Direccion vive y muere con Estudiante
    // SOLID - SRP: solo responsabilidad de datos de ubicación
    public class Direccion
    {
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = "Colombia";

        public override string ToString() => $"{Calle}, {Ciudad}, {Pais}";
    }
}
