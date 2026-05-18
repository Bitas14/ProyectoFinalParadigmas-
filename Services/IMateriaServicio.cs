using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    // SOLID - DIP + PARADIGMA DE ASPECTOS: interceptable por Castle Windsor
    public interface IMateriaServicio
    {
        void Crear(Materia materia);
        Materia? ObtenerPorId(int id);
        List<Materia> ObtenerTodos();
        void Actualizar(Materia materia);
        void Eliminar(int id);
    }
}
