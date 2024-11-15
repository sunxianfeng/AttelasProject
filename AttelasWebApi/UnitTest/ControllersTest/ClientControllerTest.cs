using Attelas.Controllers;
using Attelas.Models;
using Attelas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace Attelas.UnitTest.ControllersTest;

[TestClass]
public class ClientControllerTest
{
    private Mock<ILogger<ClientController>> _logger;
    private Mock<IClientService> _clientService;
    private ClientController _clientController;
    
    [SetUp]
    public void Setup()
    {
        this._logger = new Mock<ILogger<ClientController>>();
        this._clientService = new Mock<IClientService>();
        this._clientController = new ClientController(
            this._logger.Object,
            this._clientService.Object);
    }

    [Test]
    public async Task EnquiryClientInfo()
    {
        var clientModel = new ClientModel()
            { ClientId = Guid.NewGuid().ToString(), Name = "testName", Email = "xxx@xxx.com" };
        this._clientService.Setup(item => item.EnquiryClientsInfoAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<ClientModel>{ clientModel });
        
        IActionResult result = await this._clientController.EnquiryClientInfoAsync(It.IsAny<string>());
        
        Assert.IsType<OkObjectResult>(result);
        this._clientService.Verify(item => item.EnquiryClientsInfoAsync(It.IsAny<List<string>>()), Times.Once);

    }
    
}