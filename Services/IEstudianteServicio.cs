using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    // SOLID - DIP: los consumidores dependen de esta abstracción, no de la implementación
    // PARADIGMA DE ASPECTOS: Castle Windsor intercepta a través de interfaces
    public interface IEstudianteServicio
    {
        void Crear(Estudiante estudiante);
        Estudiante? ObtenerPorId(int id);
        List<Estudiante> ObtenerTodos();
        void Actualizar(Estudiante estudiante);
        void Eliminar(int id);
    }
}
