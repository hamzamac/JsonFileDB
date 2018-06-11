using System;
using System.Collections.Generic;
using System.Text;

namespace JsonFileDB
{
    public interface IDataset<E>
    {
        IEnumerable<E> GetAll();
        E Find(int id);
        void Add(E value);
        void Update(E value);
        void Remove(int id);
    }
}
