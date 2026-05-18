using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repositories
{
    // SOLID - SRP: solo responsabilidad de persistencia en CSV
    // SOLID - OCP: genérico, funciona para cualquier IIdentifiable sin modificarse
    // SOLID - DIP: las capas de servicio dependen de IRepository<T>, no de esta clase
    public class CsvRepository<T> where T : IIdentifiable
    {
        private readonly string _filePath;
        private readonly CsvConfiguration _config;

        public CsvRepository(string entityName)
        {
            var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, $"{entityName}.csv");

            _config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null
            };

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, string.Empty);
        }

        public List<T> GetAll()
        {
            if (new FileInfo(_filePath).Length == 0)
                return new List<T>();

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, _config);
            return csv.GetRecords<T>().ToList();
        }

        public void SaveAll(List<T> records)
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, _config);
            csv.WriteRecords(records);
        }

        public void Create(T entity)
        {
            var all = GetAll();
            if (entity.Id == 0)
                entity.Id = all.Count > 0 ? all.Max(x => x.Id) + 1 : 1;
            all.Add(entity);
            SaveAll(all);
        }

        public T? Read(int id) => GetAll().FirstOrDefault(x => x.Id == id);

        public void Update(T entity)
        {
            var all = GetAll();
            var index = all.FindIndex(x => x.Id == entity.Id);
            if (index >= 0)
            {
                all[index] = entity;
                SaveAll(all);
            }
        }

        public void Delete(int id)
        {
            var all = GetAll();
            all.RemoveAll(x => x.Id == id);
            SaveAll(all);
        }
    }
}
