using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ToDo.Models;
using ToDo.Repository;
using ToDo.Repository.Base;
using ToDo.ViewModels;

namespace ToDo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IRepository<ItemModel>, ItemRepository>();
        builder.Services.AddTransient<ListViewModel>();
        builder.Services.AddTransient<EditViewModel>();
        return builder.Build();
    }
}
