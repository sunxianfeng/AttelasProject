using Attelas.DbContex;
using Attelas.Models;
using Attelas.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;


namespace Attelas.UnitTest.ServicesTest;


[TestClass]
public class InvoiceServiceTest : IDisposable
{
    private Mock<ILogger<InvoiceService>> _logger;
    private AttelasDbContext _context;
    private IInvoiceService _invoiceService;
    
    [SetUp]
    public void Init()
    {
        this._logger = new Mock<ILogger<InvoiceService>>();
        this.InitContext();
        this._invoiceService = new InvoiceService(
            this._logger.Object,
            this._context
        );
    }

    [Test]
    public async Task TestEnquiryInvoiceByIds()
    {
        string invoiceNumber = "test_id";
        var res = await this._invoiceService.EnquiryInvoiceByIdsAsync(new List<string>{ invoiceNumber });
        Assert.NotNull(res);
        Assert.True(1 == res.Count());
        Assert.True(InvoiceStatus.Pending == res.First());
    }
    
    [Test]
    public async Task TestEnquiryInvoiceByClientIds()
    {
        string clientId = "C001";
        var res = await this._invoiceService.EnquiryInvoiceByClientIdsAsync(new List<string>{ clientId });
        Assert.NotNull(res);
        Assert.True(1 == res.Count());
        Assert.True(clientId == res.First().ClientId);
    }

    [Test]
    public async Task TestSubmitInvoice()
    {
        var newInvoice = new InvoiceModel
        {
            InvoiceNumber = "test_id2",
            ClientId = "C002",
            Status = InvoiceStatus.Paid,
            DueDate = new DateTime().AddDays(7),
        };
        var res = await this._invoiceService.SubmitInvoiceAsync(newInvoice);
        Assert.True(1 == res);
    }
    
    public void InitContext()
    {
        // this._contextOptions = new DbContextOptionsBuilder<AttelasDbContext>()
        //     .UseInMemoryDatabase("AttelasDBTest")
        //     .Options;
        this._context = new AttelasDbContext();
    
        this._context.Database.EnsureCreated();
        this._context.Database.EnsureDeleted();
        
        var invoice = new InvoiceModel
        {
            InvoiceNumber = "test_id",
            ClientId = "C001",
            Status = InvoiceStatus.Pending,
            DueDate = new DateTime().AddDays(7),
        };
        this._context.Invoices.Add(invoice);
        this._context.SaveChanges();
    }
    
    public void Dispose()
    {
        this._context.Dispose();
    }
}

