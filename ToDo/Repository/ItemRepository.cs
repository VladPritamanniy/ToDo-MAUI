using ToDo.Models;
using ToDo.Repository.Base;

namespace ToDo.Repository;

public class ItemRepository : IRepository<ItemModel>
{
    private readonly List<ItemModel> _items = new()
    {
        new() { SortOrder = 1, Title = "Bug fix", Description = "Resolve null reference error" },
        new() { SortOrder = 2, Title = "Refactor code", Description = "Improve service layer design" },
        new() { SortOrder = 3, Title = "Unit tests", Description = "Cover key business logic" },
        new() { SortOrder = 4, Title = "Update packages", Description = "Refresh NuGet dependencies" },
        new() { SortOrder = 5, Title = "Optimize SQL", Description = "Reduce query execution time" },
        new() { SortOrder = 6, Title = "API docs", Description = "Add Swagger documentation" },
        new() { SortOrder = 7, Title = "Code review", Description = "Check naming and standards" },
        new() { SortOrder = 8, Title = "Add caching", Description = "Cache frequently used data" }
    };

    public List<ItemModel> GetAllItems()
        => _items;

    public void AddItem(ItemModel item)
        => _items.Add(item);

    public async Task DeleteItemById(Guid id)
    {
        var existing = _items.FirstOrDefault(item => item.Id == id);

        if(existing is not null)
        {
            await Task.Delay(2000);
            _items.RemoveAll(item => item.Id == id);
        }
    }

    public ItemModel? GetItemById(Guid id)
        => _items.FirstOrDefault(item => item.Id == id);

    public void UpdateItem(ItemModel item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var oldItem = GetItemById(item.Id);

        if (oldItem is null)
            item.SortOrder = _items.Max(p => p.SortOrder) + 1;
        else
            _items.Remove(oldItem);

        _items.Add(item);
    }
}
