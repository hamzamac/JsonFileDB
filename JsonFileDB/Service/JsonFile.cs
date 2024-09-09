using JsonFileDB.Interfeces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonFileDB;

internal class JsonFile : IJsonFileService
{
    public async Task<T> ReadAsync<T>(string path)
    {
        if (!File.Exists(path))
        {
            //TODO initialize a database file with all attributes of Dataset object
            File.WriteAllText(path, """{"system":{"rows":[],"index":0}}""");
        }
        using FileStream stream = File.OpenRead(path);
        var data = await JsonSerializer.DeserializeAsync<T>(stream);
        await stream.DisposeAsync();
        return data;
    }

    public async Task WriteAsync<T>(string path, T database)
    {
        using FileStream stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, database);
        await stream.DisposeAsync();
        await Task.CompletedTask;
    }
}
