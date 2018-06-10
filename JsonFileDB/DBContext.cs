using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JsonFileDB
{
    public class DBContext : IDBContext
    {
        protected JObject _database;
        private string _jsonFilePath;

        public DBContext(string jsonFilePath)
        {
            //initialize the database => fetch
            _jsonFilePath = jsonFilePath;
            _database = Fetch(jsonFilePath).GetAwaiter().GetResult();
        }

        private async Task<JObject> Fetch(string jsonFilePath)
        {
            JObject database;
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    //TODO initialize a database file with all attributes of Dataset object
                    File.WriteAllText(jsonFilePath, "{'system':{'rows':[],'index':0}}");
                }
                using (StreamReader reader = File.OpenText(jsonFilePath))
                {
                    database = (JObject)await JToken.ReadFromAsync(new JsonTextReader(reader));
                }
            }
            catch (Exception)
            {
                database = JObject.Parse("{'system':{'rows':[],'index':0}}");
            }
            return database;
        }

        public void SaveChanges()
        {
            //save changes to json file on disk
            JsonSerializer serializer = new JsonSerializer();

            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(_jsonFilePath))

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _database);
            }
        }
    }
}
