using ToDo.Models.Base;

namespace ToDo.Models;

public class ItemModel : BaseModel<Guid>
{
    public int SortOrder { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public ItemModel()
    {
        Id = Guid.NewGuid();
    }
}
