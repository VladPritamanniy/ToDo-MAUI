namespace ToDo.Repository.Base;

public interface IRepository<T>
{
    public List<T> GetAllItems();
    public T? GetItemById(Guid id);
    public void AddItem(T item);
    public void UpdateItem(T item);
    public Task DeleteItemById(Guid id);
}
