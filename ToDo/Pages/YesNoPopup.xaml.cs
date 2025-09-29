using CommunityToolkit.Maui.Views;
using ToDo.ViewModels;

namespace ToDo.Pages;

public partial class YesNoPopup : Popup<bool>
{
    public YesNoPopup(YesNoPopupViewModel viewModel)
	{
        InitializeComponent();
        viewModel.ClosePopupDelegate = ClosePopup; // Set the ClosePopupDelegate here because it requires the current Popup instance
        BindingContext = viewModel;
    }

    private async Task ClosePopup(bool result)
    {
        await CloseAsync(result);
    }
}
