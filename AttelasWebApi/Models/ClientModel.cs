using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Attelas.Models;

public class ClientModel
{
    [JsonProperty("clientId")]
    [Column("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("name")]
    [Column("name")]
    public string Name { get; set; }
    
    [JsonProperty("email")]
    [Column("email")]
    public string Email { get; set; }
    
    [JsonProperty("address")]
    [Column("address")]
    public string Address { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}