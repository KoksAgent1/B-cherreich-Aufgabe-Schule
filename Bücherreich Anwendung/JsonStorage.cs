using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Bücherreich_Anwendung
{
    public class JsonStorage<T> where T : class
    {
        private string filePath;

        // Konstruktor nimmt den Pfad zur Datei
        public JsonStorage(string path)
        {
            filePath = path;
        }

        // Speichern einer Liste von Objekten in die JSON-Datei
        public void SaveToFile(List<T> items)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,  // Schleifen ignorieren
                Formatting = Formatting.Indented // Zum besseren Lesen
            };

            var json = JsonConvert.SerializeObject(items, settings);
            File.WriteAllText(filePath, json);
        }

        // Laden einer Liste von Objekten aus der JSON-Datei
        public List<T> LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            return new List<T>(); // Falls die Datei nicht existiert, geben wir eine leere Liste zurück
        }
    }
}