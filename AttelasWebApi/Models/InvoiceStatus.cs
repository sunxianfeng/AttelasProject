using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Attelas.Models;

[JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
public enum InvoiceStatus
{   
    Pending = 0,
    
    Paid = 1,
    
    Overdue = 2,
    
}