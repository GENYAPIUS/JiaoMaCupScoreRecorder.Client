using JiaoMaCupScoreRecorder.Models;
using JiaoMaCupScoreRecorder.Services;
using JiaoMaCupScoreRecorder.Services.Interfaces;
using JiaoMaCupScoreRecorder.Utils;

namespace JiaoMaCupScoreRecorder.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IGoogleSheetService, GoogleSheetService>();
        services.AddSingleton<StateContainer>();

        return services;
    }

    public static IServiceCollection AddGoogleSheetsApiAuth(this IServiceCollection services,
                                                            IConfiguration configuration)
    {
        StaticServiceCollectUtils.StaticService.AddSingleton(serviceProvider =>
        {
            var result = new GoogleSheetsAuthModel();
            configuration.GetSection("GoogleSheetsApiAuthorization").Bind(result);

            return result;
        });

        return services;
    }

    public static IServiceCollection AddSpreadSheetData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider =>
        {
            var result = new SpreadsheetModel();
            configuration.GetSection("SpreadSheet").Bind(result);

            return result;
        });

        return services;
    }
}