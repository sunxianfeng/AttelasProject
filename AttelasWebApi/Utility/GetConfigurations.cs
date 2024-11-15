namespace Attelas.Utility;

public static class GetConfigurations
{
    private static string? _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    private static IConfiguration ConfigRoot { get; } = GetConfigRoot();

    private static IConfiguration GetConfigRoot()
    {
        ConfigurationBuilder configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        configBuilder.AddJsonFile($"appsettings.{_env}.json", optional: true, reloadOnChange: false);
        return configBuilder.Build();
    }
    
    public static string? GetConfiguration(string name)
    {
        return ConfigRoot.GetSection(name).Value;
    }
    
}