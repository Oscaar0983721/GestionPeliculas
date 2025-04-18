using System;
using System.IO;
using System.Text.Json;

namespace GestionPeliculas.Services
{
    public class JsonDataService
    {
        private readonly string _dataDirectory;

        public JsonDataService()
        {
            // Crear directorio de datos si no existe
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        public virtual T CargarDatos<T>(string nombreArchivo) where T : class
        {
            string rutaArchivo = Path.Combine(_dataDirectory, nombreArchivo);

            if (!File.Exists(rutaArchivo))
            {
                return null;
            }

            try
            {
                string jsonData = File.ReadAllText(rutaArchivo);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<T>(jsonData, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar datos: {ex.Message}");
                return null;
            }
        }

        public virtual bool GuardarDatos<T>(string nombreArchivo, T datos) where T : class
        {
            string rutaArchivo = Path.Combine(_dataDirectory, nombreArchivo);

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                string jsonData = JsonSerializer.Serialize(datos, options);
                File.WriteAllText(rutaArchivo, jsonData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar datos: {ex.Message}");
                return false;
            }
        }
    }
}
