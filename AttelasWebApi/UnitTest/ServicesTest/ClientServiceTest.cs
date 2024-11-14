using Attelas.DbContex;
using Attelas.Models;
using Attelas.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;


namespace Attelas.UnitTest.ServicesTest;

public class ClientServiceTest : IDisposable
{
    private Mock<ILogger<ClientService>> _logger;
    private AttelasDbContext _context;
    private IClientService _clientService;
    
    [SetUp]
    public void Init()
    {
        this._logger = new Mock<ILogger<ClientService>>();
        this.InitContext();
        this._clientService = new ClientService(
            this._logger.Object,
            this._context
        );
    }

    [Test]
    public async Task TestEnquiryClientInfo()
    {
        string clientId = "C001";
        var res = await this._clientService.EnquiryClientsInfoAsync(new List<string>{ clientId });
        Assert.NotNull(res);
        Assert.True(1 == res.Count());
        Assert.True(clientId == res.First().ClientId);
    }
    
    public void InitContext()
    {
        // this._contextOptions = new DbContextOptionsBuilder<AttelasDbContext>()
        //     .UseInMemoryDatabase("AttelasDBTest")
        //     .Options;
        this._context = new AttelasDbContext();
    
        this._context.Database.EnsureCreated();
        this._context.Database.EnsureDeleted();
        
        var client = new ClientModel
        {
            ClientId = "C001",
            Name = "Client Name",
            Email = "email@email.com",
            Address = "address",
        };
        this._context.Clients.Add(client);
        this._context.SaveChanges();
    }
    
    public void Dispose()
    {
        this._context.Dispose();
    }
}