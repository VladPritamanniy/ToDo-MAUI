using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Input;
using ToDo.ViewModels;

namespace ToDo.Pages;

public partial class EditPage : ContentPage
{
    private readonly EditViewModel _viewModel;

    public EditPage(EditViewModel editViewModel)
	{
		InitializeComponent();
        editViewModel.DisplayPopupCommand = new AsyncRelayCommand(DisplayPopup);
        BindingContext = _viewModel = editViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }

    private async Task DisplayPopup()
    {
        var popup = new YesNoPopup(new YesNoPopupViewModel()
        {
            OnYesDelegate = async () => await _viewModel.DeleteCommand.ExecuteAsync(null)
        });

        IPopupResult<bool> popupResult = await this.ShowPopupAsync<bool>(popup, new PopupOptions
        {
            CanBeDismissedByTappingOutsideOfPopup = false,
            PageOverlayColor = Colors.Grey.WithAlpha(0.5f),
            Shadow = null
        }, CancellationToken.None);

        if (popupResult.WasDismissedByTappingOutsideOfPopup)
        {
            return;
        }

        if (popupResult.Result is true)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
