using System.Net;
using Attelas.Services;
using Attelas.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attelas.Controllers;

[ApiController]
[Route("/v1/interaction")]
public class LLmInteractionController : ControllerBase
{
    private readonly ILogger<LLmInteractionController> _logger;
    private readonly ITriggerWorkflowsService _triggerWorkflowsService;
    

    public LLmInteractionController(
        ILogger<LLmInteractionController> logger,
        ITriggerWorkflowsService triggerWorkflowsService)
    {
        this._logger = logger;
        this._triggerWorkflowsService = triggerWorkflowsService;
    }

    [HttpPost]
    [Route("{text}")]
    [Authorize]
    public async Task<IActionResult> LLmTextInteraction([FromRoute] string text)
    {
        ArgumentValidator.RequireNotNullOrEmpty(text, nameof(text));
        this._logger.LogInformation("Start to trigger workflows in  LLmInteractionController.");
        try
        {
            var res = await this._triggerWorkflowsService.Run(text);
            return Ok(res);
        }
        catch (Exception e)
        {
            this._logger.LogError($"Got an exception while triggering LLmInteraction: {e.Message}");
            return this.StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
}