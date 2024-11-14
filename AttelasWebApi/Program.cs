using System.Text;
using Attelas.DbContex;
using Attelas.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
            ValidIssuer = configuration["Jwt:Issuer"], 
            ValidAudience = configuration["Jwt:Audience"], 
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

builder.Host.ConfigureServices(services =>
{
    // services.AddSingleton(new JwtHelper(configuration));
    services.AddDbContext<AttelasDbContext>();
    services.TryAddScoped<IInvoiceService, InvoiceService>();
    services.TryAddScoped<IClientService, ClientService>();
    services.TryAddScoped<ISqlParserService, SqlParserService>();
    services.TryAddScoped<ILLmGenerateSqlService, LLmGenerateSqlService>();
    services.TryAddScoped<ITriggerWorkflowsService, TriggerWorkflowsService>();
});
builder.Services.AddSwaggerGen();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


 
