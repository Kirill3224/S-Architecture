using System.Text.Json;
using SL.Domain.Common;
using SL.Domain.Interfaces;

namespace SL.Infra.Repositories;

public class JsonRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly string _filePath;
    private List<T> _items;

    public JsonRepository(string fileName)
    {
        _filePath = fileName;
        _items = LoadData();
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public T? GetById(Guid id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public Guid Add(T item)
    {
        _items.Add(item);
        SaveData();
        return item.Id;
    }

    public void Update(T item)
    {
        var index = _items.FindIndex(x => x.Id == item.Id);
        if (index != -1)
        {
            _items[index] = item;
            SaveData();
        }
    }

    public void Delete(Guid id)
    {
        var item = GetById(id);
        if (item != null)
        {
            _items.Remove(item);
            SaveData();
        }
    }

    private List<T> LoadData()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    private void SaveData()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_items, options);
        File.WriteAllText(_filePath, json);
    }
}