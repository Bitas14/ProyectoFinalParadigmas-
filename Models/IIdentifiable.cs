namespace ProyectoFinal.Models
{
    // SOLID - ISP: interfaz pequeña y específica para identificación
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
}
