using System.Threading.Tasks;

namespace JsonFileDB.Interfeces;

public interface IJsonFileService
{
    public Task<T> ReadAsync<T>(string path);
    public Task WriteAsync<T>(string path, T database);
}
