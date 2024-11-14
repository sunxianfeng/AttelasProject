using System.Net;
using Attelas.Models;
using Attelas.Services;
using Attelas.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attelas.Controllers;

[ApiController]
[Route("/v1/invoices")]
public class InvoiceController: ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;
    
    public InvoiceController(
        ILogger<InvoiceController> logger,
        IInvoiceService invoiceService)
    {
        this._logger = logger;
        this._invoiceService = invoiceService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("{invoiceId}/status")]
    public async Task<IActionResult> EnquiryInvoiceStatusAsync([FromRoute] string invoiceId)
    {
        ArgumentValidator.RequireNotNullOrEmpty(invoiceId, nameof(invoiceId));
        this._logger.LogInformation("Start to enquiry invoice status in InvoiceController.");
        try
        {
            var statusList = await this._invoiceService.EnquiryInvoiceByIdsAsync(new List<string> { invoiceId });
            if (statusList.Count() < 1)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            var status = statusList.First();
            return this.Ok(status);
        }
        catch (Exception e)
        {
            this._logger.LogError($"Failed to get invoice status for invoice");
            return this.StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
    
    [HttpPost]
    [Route("")]
    [Authorize]
    public async Task<IActionResult> SubmitInvoiceAsync([FromBody] InvoiceModel request)
    {
        ArgumentValidator.RequireNotNullOrEmpty(nameof(request), request);
        this._logger.LogInformation("Start to submit a new invoice in InvoiceController.");
        try
        {
            InvoiceModel.Validate(request);
            var count = await this._invoiceService.SubmitInvoiceAsync(request);
            return this.Ok(count);
        }
        catch (Exception e)
        {
            this._logger.LogError($"Failed to submit a new invoice, error: {e.Message}.");
            return this.StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
}