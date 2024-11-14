using System.Net;
using Attelas.Controllers;
using Attelas.Models;
using Attelas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace Attelas.UnitTest.ControllersTest;

[TestClass]
public class InvoiceControllerTest
{
    private Mock<ILogger<InvoiceController>> _logger;
    private Mock<IInvoiceService> _invoiceService;
    private InvoiceController _invoiceController;
   
    [SetUp]
    public void Init()
    {
        this._logger = new Mock<ILogger<InvoiceController>>();
        this._invoiceService = new Mock<IInvoiceService>();
        this._invoiceController = new InvoiceController(
            this._logger.Object,
            this._invoiceService.Object);
    }

    [Test]
    public async Task TestEnquiryInvoiceStatus()
    {
        this._invoiceService.Setup(item => item.EnquiryInvoiceByIdsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<InvoiceStatus>{InvoiceStatus.Pending});
        
        IActionResult result = await this._invoiceController.EnquiryInvoiceStatusAsync(It.IsAny<string>());
        
        var okResult = result as OkObjectResult;
        Assert.Equal(InvoiceStatus.Pending, okResult?.Value);
        this._invoiceService.Verify(item => item.EnquiryInvoiceByIdsAsync(It.IsAny<List<string>>()), Times.Once);
    }

    [Test]
    public async Task TestEnquiryInvoiceStatus_NotFound()
    {
        this._invoiceService.Setup(item => item.EnquiryInvoiceByIdsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<InvoiceStatus>());
        
        IActionResult result = await this._invoiceController.EnquiryInvoiceStatusAsync(It.IsAny<string>());
        
        var notFoundResult = result as StatusCodeResult;
        Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult?.StatusCode);
        this._invoiceService.Verify(item => item.EnquiryInvoiceByIdsAsync(It.IsAny<List<string>>()), Times.Once);
    }

    // [Test]
    // public async Task TestSubmitInvoice()
    // {
    //     this._invoiceService.Setup(item => item.SubmitInvoiceAsync(It.IsAny<InvoiceModel>()))
    //         .ReturnsAsync(1);
    //     
    //     IActionResult result = await this._invoiceController.SubmitInvoiceAsync(It.IsAny<InvoiceModel>());
    //     
    //     var okResult = result as OkObjectResult;
    //     Assert.Equal(1, okResult?.Value);
    //     this._invoiceService.Verify(item => item.SubmitInvoiceAsync(It.IsAny<InvoiceModel>()), Times.Once);
    // }
    
}

