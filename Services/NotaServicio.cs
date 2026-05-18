using ProyectoFinal.Events;
using ProyectoFinal.Models;
using ProyectoFinal.Repositories;

namespace ProyectoFinal.Services
{
    // SOLID - SRP: lógica de notas + disparo de eventos del dominio
    // PROGRAMACIÓN ORIENTADA A EVENTOS: es el Emisor (Publisher) de los eventos
    public class NotaServicio : INotaServicio
    {
        private readonly CsvRepository<Nota> _repo;

        // EVENTOS - palabra clave 'event' protege el delegado:
        // solo esta clase puede dispararlo, otros solo se suscriben/desuscriben
        public event EventHandler<NotaRegistradaEventArgs>? NotaRegistrada;
        public event EventHandler<PromedioActualizadoEventArgs>? PromedioActualizado;

        public NotaServicio()
        {
            _repo = new CsvRepository<Nota>("Notas");
        }

        public void Crear(Nota nota, string nombreEstudiante, string nombreMateria)
        {
            // Capturar promedio ANTES de guardar para el evento PromedioActualizado
            decimal promedioAnterior = CalcularPromedio(nota.EstudianteId);

            _repo.Create(nota);

            // EVENTO 1: NotaRegistrada — cambio de estado significativo del dominio
            NotaRegistrada?.Invoke(this, new NotaRegistradaEventArgs(
                nota.EstudianteId,
                nombreEstudiante,
                nota.MateriaId,
                nombreMateria,
                nota.ValorNota
            ));

            // EVENTO 2: PromedioActualizado — el promedio del estudiante cambió
            decimal promedioNuevo = CalcularPromedio(nota.EstudianteId);
            PromedioActualizado?.Invoke(this, new PromedioActualizadoEventArgs(
                nota.EstudianteId,
                nombreEstudiante,
                promedioAnterior,
                promedioNuevo
            ));
        }

        public Nota? ObtenerPorId(int id) => _repo.Read(id);

        public List<Nota> ObtenerTodos() => _repo.GetAll();

        public List<Nota> ObtenerPorEstudiante(int estudianteId) =>
            _repo.GetAll().Where(n => n.EstudianteId == estudianteId).ToList();

        public void Actualizar(Nota nota) => _repo.Update(nota);

        public void Eliminar(int id) => _repo.Delete(id);

        public decimal CalcularPromedio(int estudianteId)
        {
            var notas = ObtenerPorEstudiante(estudianteId);
            if (!notas.Any()) return 0;
            return notas.Average(n => n.ValorNota);
        }
    }
}
