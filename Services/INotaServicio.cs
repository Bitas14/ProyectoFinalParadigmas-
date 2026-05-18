using ProyectoFinal.Events;
using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    // SOLID - DIP + PARADIGMA DE ASPECTOS: interceptable por Castle Windsor
    // PROGRAMACIÓN ORIENTADA A EVENTOS: define los eventos del dominio
    public interface INotaServicio
    {
        // EVENTOS DE DOMINIO: semánticamente significativos, no solo técnicos
        event EventHandler<NotaRegistradaEventArgs> NotaRegistrada;
        event EventHandler<PromedioActualizadoEventArgs> PromedioActualizado;

        void Crear(Nota nota, string nombreEstudiante, string nombreMateria);
        Nota? ObtenerPorId(int id);
        List<Nota> ObtenerTodos();
        List<Nota> ObtenerPorEstudiante(int estudianteId);
        void Actualizar(Nota nota);
        void Eliminar(int id);
        decimal CalcularPromedio(int estudianteId);
    }
}
