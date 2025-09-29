using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDo.Pages;

namespace ToDo.ViewModels;

public partial class ItemViewModel : ObservableObject
{
    [ObservableProperty]
    Guid id;
    [ObservableProperty]
    int sortOrder;
    [ObservableProperty]
    string title;
    [ObservableProperty]
    string description;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TextDecoration))]
    bool isCompleted;
    [ObservableProperty]
    bool isPlaceholder;

    public IRelayCommand<object> TapCommand { get; }
    public TextDecorations TextDecoration => IsCompleted ? TextDecorations.Strikethrough : TextDecorations.None;

    public ItemViewModel()
    {
        TapCommand = new RelayCommand<object>(OnTapGestureRecognizerTapped);
    }

    private async void OnTapGestureRecognizerTapped(object id)
    {
        if (id is Guid guid)
            await Shell.Current.GoToAsync($"{nameof(EditPage)}?{nameof(EditViewModel.Id)}={guid}");
    }
}
