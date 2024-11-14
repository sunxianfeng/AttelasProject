using System.Text;
using Attelas.Utility;
using Newtonsoft.Json;
using Python.Runtime;


namespace Attelas.Services;

public class LLmGenerateSqlService : ILLmGenerateSqlService
{
     
    private readonly ILogger<LLmGenerateSqlService> _logger;
     
    public LLmGenerateSqlService(ILogger<LLmGenerateSqlService> logger)
    {
        this._logger = logger;
    }
    
    public async Task<string> GenerateSqlAsync(string text)
    {
        var buildPostContent = new { Text = text };
        var pythonServiceUrl = GetConfigurations.GetConfiguration("CallPython:PythonServiceUrl");
        var content = new StringContent(JsonConvert.SerializeObject(buildPostContent), Encoding.UTF8, "application/json");
        var sqlStatement = string.Empty;
        try
        {
            this._logger.LogInformation("Generating SQL...");
            using var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(pythonServiceUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                sqlStatement = await response.Content.ReadAsStringAsync();
                this._logger.LogInformation($"Got SQL: {sqlStatement}");
            }
        }
        catch (Exception e)
        {
            this._logger.LogError($"Failed to generate sql statement from python service, error message:{e.Message}.");
        }
        return sqlStatement;
    }
}

public interface ILLmGenerateSqlService
{
    Task<string> GenerateSqlAsync(string text);
}