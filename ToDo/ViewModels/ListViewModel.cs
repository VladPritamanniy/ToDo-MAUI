using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDo.Models;
using ToDo.Repository.Base;

namespace ToDo.ViewModels;

public class ListViewModel : ObservableObject
{
    private readonly IRepository<ItemModel> _repository;
    private ItemViewModel _draggedItem;
    private ItemViewModel _placeholderItem;

    public ObservableCollection<ItemViewModel> Items { get; } = new();
    public IRelayCommand<object> DragStartingCommand { get; }
    public IRelayCommand<object> DropCommand { get; }
    public IRelayCommand DropCompletedCommand { get; }
    public IRelayCommand DragOver { get; }

    public ListViewModel(IRepository<ItemModel> repository)
    {
        _repository = repository;
        DragStartingCommand = new RelayCommand<object>(OnDragStarting);
        DropCommand = new RelayCommand<object>(OnDrop);
        DropCompletedCommand = new RelayCommand(OnDropCompleted);
        DragOver = new RelayCommand<object>(OnDragOver);
    }

    public void OnAppearing()
    {
        var items = _repository.GetAllItems()
            .Select(i => new ItemViewModel()
            {
                Id = i.Id,
                SortOrder = i.SortOrder,
                Title = i.Title,
                Description = i.Description,
                IsCompleted = i.IsCompleted
            })
            .OrderBy(p => p.SortOrder)
            .ToList();

        Items.Clear();

        foreach (var item in items)
        {
            Items.Add(item);
        }
    }

    private void OnDragOver(object vm)
    {
        if (_draggedItem is null || vm is null) return;

        if (_placeholderItem is not null)
            _placeholderItem.IsPlaceholder = false;

        _placeholderItem = vm as ItemViewModel;
        _placeholderItem!.IsPlaceholder = true;
    }

    private void OnDragStarting(object vm)
    {
        if (vm is not null)
            _draggedItem = vm as ItemViewModel;
    }

    private void OnDrop(object vm)
    {
        if (_draggedItem is null || vm is null || _draggedItem == vm)
            return;

        var index1 = Items.IndexOf(_draggedItem);
        var index2 = Items.IndexOf(vm as ItemViewModel);

        if (index1 < 0 || index2 < 0) return;

        Items.Move(index1, index2);
        _draggedItem = null;

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].SortOrder = i + 1;
            var model = _repository.GetItemById(Items[i].Id);
            if (model != null)
            {
                model.SortOrder = Items[i].SortOrder;
                _repository.UpdateItem(model);
            }
        }
    }

    private void OnDropCompleted()
    {
        if (_placeholderItem is not null)
            _placeholderItem.IsPlaceholder = false;
    }
}
