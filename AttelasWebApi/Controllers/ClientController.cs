using System.Net;
using Attelas.Services;
using Attelas.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attelas.Controllers;

[ApiController]
[Route("/v1/clients")]
public class ClientController: ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    private readonly IClientService _clientService;
    
    public ClientController(
        ILogger<ClientController> logger,
        IClientService clientService)
    {
        this._logger = logger;
        this._clientService = clientService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("{clientId}")]
    public async Task<IActionResult> EnquiryClientInfoAsync([FromRoute] string clientId)
    {
        ArgumentValidator.RequireNotNullOrEmpty(clientId, nameof(clientId));
        this._logger.LogInformation("Start to enquiry client info in ClientController.");
        try
        {
            var infoList = await this._clientService.EnquiryClientsInfoAsync(new List<string> { clientId });
            if (infoList.Count() < 1)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            var info = infoList.First();
            return this.Ok(info);
        }
        catch (Exception e)
        {
            this._logger.LogError($"Failed to get client info for client: {clientId}");
            return this.StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
}