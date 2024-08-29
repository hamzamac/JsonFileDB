using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonFileDB
{
    /// <example>
    /// <code>
    /// public class AppDBContext: DBContext
    /// {
    ///     public AppDBContext() : base($".\\database.json")
    /// {
    ///     Persons = new Dataset<Person>(_database);
    /// }
    /// public Dataset<Person> Persons { get; set; }
    ///}
    /// </code>
    /// </example>
    public class DBContext : IDBContext
    {
        /// <value>Gets and Sets the Database instance.</value>
        protected JsonDocument _database { get; set; }

        private string _jsonFilePath;

        /// <summary>
        /// Initiaalizes the context of a Database from a given json file.
        /// </summary>
        public DBContext(string jsonFilePath)
        {
            //initialize the database => fetch
            _jsonFilePath = jsonFilePath;
            _database = Fetch(jsonFilePath).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Fetches data from json file to the database context in memory.
        /// </summary>
        private async Task<JsonDocument> Fetch(string jsonFilePath)
        {
            JsonDocument database;
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    //TODO initialize a database file with all attributes of Dataset object
                    File.WriteAllText(jsonFilePath, "{'system':{'rows':[],'index':0}}");
                }
                using (StreamReader reader = File.OpenText(jsonFilePath))
                {
                    Utf8JsonReader(reader)
                    database = (JsonDocument)await );
                }
            }
            catch (Exception)
            {
                database = JsonDocument.Parse("{'system':{'rows':[],'index':0}}");
            }
            return database;
        }

        /// <summary>
        /// Persists the data of the DBcontaxt in memory to a json file on drive.
        /// </summary>
        public void SaveChanges()
        {
            //save changes to json file on disk
            using (StreamWriter sw = new StreamWriter(_jsonFilePath))
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(sw.BaseStream))
                {
                    JsonSerializer.Serialize(writer, _database);
                }
            }
        }
    }
}
