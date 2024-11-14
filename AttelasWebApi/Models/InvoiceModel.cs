using System.ComponentModel.DataAnnotations.Schema;
using Attelas.Utility;
using Newtonsoft.Json;

namespace Attelas.Models;

public class InvoiceModel
{
    [JsonProperty("invoiceNumber")]
    [Column("invoice_number")]
    public string InvoiceNumber { get; set; }
    
    [JsonProperty("clientId")]
    [Column("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("dueDate")]
    [Column("due_date")]
    public DateTime DueDate { get; set; }
    
    [JsonProperty("invoiceStatus")]
    [Column("status")]
    public InvoiceStatus Status { get; set; }

    public static void Validate(InvoiceModel request)
    {
        ArgumentValidator.RequireNotNullOrEmpty(nameof(request), request);
        ArgumentValidator.RequireNotNullOrEmpty(nameof(request.InvoiceNumber), request.InvoiceNumber);
        ArgumentValidator.RequireNotNullOrEmpty(nameof(request.ClientId), request.ClientId);
        ArgumentValidator.RequireNotNullOrEmpty(nameof(request.Status), request.Status);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}