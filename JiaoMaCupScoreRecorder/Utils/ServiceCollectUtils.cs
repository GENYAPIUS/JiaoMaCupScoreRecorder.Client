namespace JiaoMaCupScoreRecorder.Utils;

public static class StaticServiceCollectUtils
{
    public static readonly IServiceCollection StaticService = new ServiceCollection();

    public static T GetStaticService<T>() where T : class
    {
        var serviceProvider = StaticService.BuildServiceProvider();
        var result = serviceProvider.GetRequiredService<T>();

        return result;
    }
}