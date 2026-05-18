using ProyectoFinal.Models;
using ProyectoFinal.Repositories;

namespace ProyectoFinal.Services
{
    // SOLID - SRP: solo lógica de negocio de estudiantes
    // SOLID - DIP: recibe el repositorio como abstracción (aunque aquí instanciamos directo
    //              porque Castle Windsor gestiona la inyección del servicio completo)
    public class EstudianteServicio : IEstudianteServicio
    {
        private readonly CsvRepository<Estudiante> _repo;

        public EstudianteServicio()
        {
            _repo = new CsvRepository<Estudiante>("Estudiantes");
        }

        public void Crear(Estudiante estudiante) => _repo.Create(estudiante);

        public Estudiante? ObtenerPorId(int id) => _repo.Read(id);

        public List<Estudiante> ObtenerTodos() => _repo.GetAll();

        public void Actualizar(Estudiante estudiante) => _repo.Update(estudiante);

        public void Eliminar(int id) => _repo.Delete(id);
    }
}
