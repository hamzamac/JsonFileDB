using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonFileDB
{
    /// <summary>
    /// Represents a table intsance belonging to the database.
    /// Contains methods for performing basic CRUD operation to table entities.
    /// </summary>
    public class Dataset<E> : IDataset<E> where E : ITable
    {
        private JsonDocument _table;
        private JArray _rows;
        private int _rowId;

        /// <summary>
        /// Initiaalized the a Dataset instance and makes it a table of the database.
        /// </summary>
        /// <param name="database">A JsonDocument object.</param>
        public Dataset(JsonDocument database)
        {
            //extract corresponding table to E
            var tableName = typeof(E).Name.ToLower();
            _table = (JsonDocument)database[tableName];

            //create tableof type E if not exists
            if (_table == null)
            {
                database.Add(tableName, JToken.Parse("{'rows':[],'index':0}"));
            }
            _table = (JsonDocument)database[tableName];
            _rows = (JArray)_table["rows"];

            //set the currect row id
            _rowId = (int)(JValue)_table["index"];
        }

        private int NextId()
        {
            _rowId += 1;
            _table["index"] = _rowId;
            return _rowId;
        }

        /// <summary>
        /// Gets all entities of the Dataset.
        /// </summary>
        /// <returns>
        /// A collection of all entities from the dataset.
        /// </returns>
        /// See <see cref="Dataset.GetAllAsync()"/> to getAll asyncronously.

        public IEnumerable<E> GetAll()
        {
            var entities = _rows.ToObject<IList<E>>();
            return entities;
        }

        /// <summary>
        /// Gets all entities of the Dataset asyncronously.
        /// </summary>
        public async Task<IEnumerable<E>> GetAllAsync()
        {
            var entities = await Task.Run(() => _rows.ToObject<IList<E>>());
            return entities;
        }

        /// <summary>
        /// Adds a new entitiy to the Datase.
        /// </summary>
        /// <param name="entity">An E object.</param>
        /// <typeparam name="E">A type that inherits from the ITable interface.</typeparam>
        public void Add(E entity)
        {
            entity.Id = NextId();
            JsonDocument entityJson = (JsonDocument)JToken.FromObject(entity);
            _rows.Add(entityJson);
        }

        /// <summary>
        /// Finds an entity with a specified id.
        /// </summary>
        /// <returns>
        /// An entity with a specified id or defaut if not found.
        /// </returns>
        /// <param name="id">An interger number.</param>
        /// See <see cref="Dataset.FindAsync()"/> to getAll asyncronously.
        public E Find(int id)
        {
            var entity = _rows.FirstOrDefault(e => e.ToObject<E>().Id == id);
            if (entity == null)
            {
                return default(E);
            }
            JsonSerializer serializer = new JsonSerializer();
            E firstEntity = (E)serializer.Deserialize(new JTokenReader(entity), typeof(E));
            return firstEntity;
        }

        /// <summary>
        /// Finds an entity with a specified id asyncronously.
        /// </summary>
        /// <returns>
        /// An entity with a specified id or defaut if not found.
        /// </returns>
        /// <param name="id">A integer number.</param>
        public async Task<E> FindAsync(int id)
        {
            var entity = await Task.Run(() => _rows.FirstOrDefault(e => e.ToObject<E>().Id == id));
            if (entity == null)
            {
                return default(E);
            }
            JsonSerializer serializer = new JsonSerializer();
            E firstEntity = (E)serializer.Deserialize(new JTokenReader(entity), typeof(E));
            return firstEntity;
        }

        /// <summary>
        /// Removes an entity from the dataset.
        /// </summary>
        /// <param name="id">A interger precision number.</param>
        public void Remove(int id)
        {
            _rows.Remove(_rows.FirstOrDefault(e => e.ToObject<E>().Id == id));
        }
        /// <summary>
        /// Updates an entity in the dataset.
        /// </summary>
        /// <param name="entity">An E object.</param>
        /// <typeparam name="E">A type that inherits from the ITable interface.</typeparam>
        public void Update(E entity)
        {
            _rows.FirstOrDefault(e => e.ToObject<E>().Id == entity.Id).Replace(JToken.FromObject(entity));
        }

    }
}
