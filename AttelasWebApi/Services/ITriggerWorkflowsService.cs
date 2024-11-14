namespace Attelas.Services;

public interface ITriggerWorkflowsService
{
    Task<object> Run(string text);
}