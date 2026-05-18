namespace ProyectoFinal.Models
{
    // SOLID - ISP: separada de IIdentifiable para no forzar a todos a implementar reporting
    public interface IReportable
    {
        string ObtenerResumen();
    }
}
