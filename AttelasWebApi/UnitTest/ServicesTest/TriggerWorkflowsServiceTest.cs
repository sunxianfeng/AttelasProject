using Attelas.Models;
using Attelas.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace Attelas.UnitTest.ServicesTest;

[TestClass]
public class TriggerWorkflowsServiceTest
{
    private Mock<ILogger<TriggerWorkflowsService>> _logger;
    private Mock<ISqlParserService> _sqlParserService;
    private Mock<ILLmGenerateSqlService> _llmGenerateSqlService;
    private Mock<IInvoiceService> _invoiceService;
    private Mock<IClientService> _clientService;
    private ITriggerWorkflowsService _triggerWorkflowsService;

    [SetUp]
    public void Init()
    {
        this._logger = new Mock<ILogger<TriggerWorkflowsService>>();
        this._sqlParserService = new Mock<ISqlParserService>();
        this._llmGenerateSqlService = new Mock<ILLmGenerateSqlService>();
        this._invoiceService = new Mock<IInvoiceService>();
        this._clientService = new Mock<IClientService>();
        this._triggerWorkflowsService = new TriggerWorkflowsService(
            this._logger.Object,
            this._sqlParserService.Object,
            this._llmGenerateSqlService.Object,
            this._invoiceService.Object,
            this._clientService.Object);
    }

    [Test]
    public async Task TestTriggerQueryInvoiceWorkflowAsync()
    {
        this._llmGenerateSqlService.Setup(item => item.GenerateSqlAsync(It.IsAny<string>()))
            .ReturnsAsync("select * from t_invoices;");
        this._sqlParserService.Setup(item => item.Parse(It.IsAny<string>()));
        this._sqlParserService.Setup(item => item.GetTableName()).Returns("t_invoices");
        this._sqlParserService.Setup(item => item.IsQuery()).Returns(true);
        this._invoiceService.Setup(item => item.EnquiryInvoiceFromRawSql(It.IsAny<string>()))
            .Returns(new List<InvoiceModel>
            {
                new InvoiceModel { ClientId = "C002", InvoiceNumber = "INV-10", Status = InvoiceStatus.Overdue, DueDate = DateTime.Now }
            });

        var res = await this._triggerWorkflowsService.Run("Could you check if INV-1020 has been processed?");
        
        Assert.NotNull(res);
        Assert.IsAssignableFrom<List<InvoiceModel>>(res);
        this._llmGenerateSqlService.Verify(item => item.GenerateSqlAsync(It.IsAny<string>()), Times.Once);
        this._sqlParserService.Verify(item => item.Parse(It.IsAny<string>()), Times.Once);
        this._sqlParserService.Verify(item => item.IsQuery(), Times.Once);
        this._sqlParserService.Verify(item => item.GetTableName(), Times.Once);
        this._invoiceService.Verify(item => item.EnquiryInvoiceFromRawSql(It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public async Task TestTriggerSubmitInvoiceWorkflowAsync()
    {
        this._llmGenerateSqlService.Setup(item => item.GenerateSqlAsync(It.IsAny<string>()))
            .ReturnsAsync("insert into t_invoices(invoice_number, client_id, status, due_date) values ('test-01', 'C001', 1, now());");
        this._sqlParserService.Setup(item => item.Parse(It.IsAny<string>()));
        this._sqlParserService.Setup(item => item.GetTableName()).Returns("t_invoices");
        this._sqlParserService.Setup(item => item.IsCreate()).Returns(true);
        this._invoiceService.Setup(item => item.SubmitInvoiceFromRawSql(It.IsAny<string>()))
            .Returns(1);

        var res = await this._triggerWorkflowsService.Run("Could you check if INV-1020 has been processed?");
        
        Assert.NotNull(res);
        Assert.True(1 == (int)res);
        this._llmGenerateSqlService.Verify(item => item.GenerateSqlAsync(It.IsAny<string>()), Times.Once);
        this._sqlParserService.Verify(item => item.Parse(It.IsAny<string>()), Times.Once);
        this._sqlParserService.Verify(item => item.IsCreate(), Times.Once);
        this._sqlParserService.Verify(item => item.GetTableName(), Times.Once);
        this._invoiceService.Verify(item => item.SubmitInvoiceFromRawSql(It.IsAny<string>()), Times.Once);
    }
    
    
}