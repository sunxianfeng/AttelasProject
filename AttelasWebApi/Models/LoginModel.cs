using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Attelas.Models;

public class LoginModel
{
    [Required]
    [JsonProperty("user_name")]
    public string Username { get; set; }
    
    [Required]
    [JsonProperty("password")]
    public string Password { get; set; }
}