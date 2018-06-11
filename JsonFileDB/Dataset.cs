using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFileDB
{
    public class Dataset<E> : IDataset<E> where E : ITable
    {
        private JObject _table;
        private JArray _rows;
        private int _rowId;

        public Dataset(JObject database)
        {
            //extract corresponding table to E
            var tableName = typeof(E).Name.ToLower();
            _table = (JObject)database[tableName];

            //create tableof type E if not exists
            if (_table == null)
            {
                database.Add(tableName, JToken.Parse("{'rows':[],'index':0}"));
            }
            _table = (JObject)database[tableName];
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

        public IEnumerable<E> GetAll()
        {
            IList<E> entities = _rows.ToObject<IList<E>>();
            return entities;
        }

        public void Add(E entity)
        {
            entity.Id = NextId();
            JObject entityJson = (JObject)JToken.FromObject(entity);
            _rows.Add(entityJson);
        }

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

        public void Remove(int id)
        {
            _rows.Remove(_rows.FirstOrDefault(e => e.ToObject<E>().Id == id));
        }

        public void Update(E entity)
        {
            _rows.FirstOrDefault(e => e.ToObject<E>().Id == entity.Id).Replace(JToken.FromObject(entity));
        }
    }
}
