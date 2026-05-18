using ProyectoFinal.Models;
using ProyectoFinal.Repositories;

namespace ProyectoFinal.Services
{
    // SOLID - SRP: solo lógica de negocio de materias
    public class MateriaServicio : IMateriaServicio
    {
        private readonly CsvRepository<Materia> _repo;

        public MateriaServicio()
        {
            _repo = new CsvRepository<Materia>("Materias");
        }

        public void Crear(Materia materia) => _repo.Create(materia);

        public Materia? ObtenerPorId(int id) => _repo.Read(id);

        public List<Materia> ObtenerTodos() => _repo.GetAll();

        public void Actualizar(Materia materia) => _repo.Update(materia);

        public void Eliminar(int id) => _repo.Delete(id);
    }
}
