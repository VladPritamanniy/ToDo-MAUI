using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using ToDo.Pages;

namespace ToDo.ViewModels;

public partial class YesNoPopupViewModel : ObservableObject
{
    [ObservableProperty]
    int opacity = 100;
    [ObservableProperty]
    bool isLoading = false;
    [ObservableProperty]
    bool isEnabledBtn = true;

    public IAsyncRelayCommand YesCommand { get; }
    public IAsyncRelayCommand NoCommand { get; }
    public Func<Task> OnYesDelegate { get; set; }
    public Func<bool, Task> ClosePopupDelegate { get; set; }

    public YesNoPopupViewModel()
    {
        YesCommand = new AsyncRelayCommand(async () =>
        {
            try
            {
                Opacity = 50;
                IsLoading = true;
                IsEnabledBtn = false;
                if (OnYesDelegate is not null)
                    await OnYesDelegate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                if (ClosePopupDelegate is not null)
                {
                    await ClosePopupDelegate(true);
                    await Shell.Current.GoToAsync($"//{nameof(ListPage)}");
                }
            }
        });

        NoCommand = new AsyncRelayCommand(async () =>
        {
            if (ClosePopupDelegate is not null)
                await ClosePopupDelegate(false);
        });
    }
}
