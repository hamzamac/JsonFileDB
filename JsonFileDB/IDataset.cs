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

        E Find(int id);
        Task<E> FindAsync(int id);

        void Add(E value);
        void Update(E value);
        void Remove(int id);
    }
}
