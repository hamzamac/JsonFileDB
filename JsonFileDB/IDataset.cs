using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JsonFileDB
{
    public interface IDataset<E>
    {
        IEnumerable<E> GetAll();
        Task<IEnumerable<E>> GetAllAsync();

        E Find(object id);
        Task<E> FindAsync(object id);

        void Add(E value);
        void Update(E value);
        void Remove(object id);
    }
}
