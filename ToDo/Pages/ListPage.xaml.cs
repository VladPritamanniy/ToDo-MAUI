using ToDo.ViewModels;

namespace ToDo.Pages;

public partial class ListPage : ContentPage
{
    private readonly ListViewModel _viewModel;

    public ListPage(ListViewModel listViewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = listViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}
