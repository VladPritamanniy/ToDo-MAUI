using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDo.Models;
using ToDo.Pages;
using ToDo.Repository.Base;

namespace ToDo.ViewModels;

public partial class EditViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRepository<ItemModel> _repository;

    [ObservableProperty]
    Guid id;
    [ObservableProperty]
    int sortOrder;
    [ObservableProperty]
    string title;
    [ObservableProperty]
    string description;
    [ObservableProperty]
    bool isCompleted;

    public IRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand DisplayPopupCommand { get; set; }

    public EditViewModel(IRepository<ItemModel> repository)
    {
        _repository = repository;
        SaveCommand = new RelayCommand(OnSaveItem, ValidateFields);
        DeleteCommand = new AsyncRelayCommand(OnDeleteItem);
    }

    public void OnAppearing()
    {
        if (Id == Guid.Empty)
            return;

        var item = _repository.GetItemById(Id);
        if (item is not null)
        {
            SortOrder = item.SortOrder;
            Title = item.Title;
            Description = item.Description;
            IsCompleted = item.IsCompleted;
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Id), out var idObj) && Guid.TryParse(idObj?.ToString(), out Guid id))
        {
            Id = id;
        }
    }

    partial void OnTitleChanged(string value)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    private async void OnSaveItem()
    {
        if(Id == Guid.Empty)
            Id = Guid.NewGuid();

        _repository.UpdateItem(new ItemModel()
        {
            Id = Id,
            SortOrder = SortOrder,
            Title = Title,
            Description = Description,
            IsCompleted = IsCompleted
        });

        Id = Guid.Empty;
        SortOrder = 0;
        Title = string.Empty;
        Description = string.Empty;
        IsCompleted = false;

        await Shell.Current.GoToAsync($"//{nameof(ListPage)}");
    }

    private async Task OnDeleteItem()
    {
        await _repository.DeleteItemById(Id);
    }

    private bool ValidateFields()
    {
        return !string.IsNullOrWhiteSpace(Title);
    }
}
