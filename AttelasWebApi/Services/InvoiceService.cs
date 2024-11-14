using Attelas.DbContex;
using Attelas.Models;
using Microsoft.EntityFrameworkCore;

namespace Attelas.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ILogger<InvoiceService> _logger;
    private readonly AttelasDbContext _context;

    public InvoiceService(ILogger<InvoiceService> logger, AttelasDbContext context)
    {
        this._logger = logger;
        this._context = context;
    }
    
    public async Task<IEnumerable<InvoiceStatus>> EnquiryInvoiceByIdsAsync(IEnumerable<string> invoiceIds)
    {
        var invoiceIdsSet = invoiceIds.ToHashSet();
        this._logger.LogInformation($"Getting invoice status for invoice {invoiceIdsSet} in InvoiceService.");

        var res = await this._context.Invoices.Where(invoice => invoiceIdsSet.Contains(invoice.InvoiceNumber))
                                                            .Select(invoice => invoice.Status).ToListAsync();
        this._logger.LogInformation($"Got invoice status: {res} successfully");
        return res;
    }
    
    public async Task<IEnumerable<InvoiceModel>> EnquiryInvoiceByClientIdsAsync(IEnumerable<string> clientIds)
    {
        var clientIdsSet = clientIds.ToHashSet();
        this._logger.LogInformation($"Getting invoice status for invoice {clientIdsSet} in InvoiceService.");

        var res = await this._context.Invoices.Where(invoice => clientIdsSet.Contains(invoice.ClientId)).ToListAsync();
        this._logger.LogInformation($"Got invoice model: {res} successfully");
        return res;
    }

    public async Task<int> SubmitInvoiceAsync(InvoiceModel invoice)
    {
        this._logger.LogInformation($"Adding invoice new {invoice} in InvoiceService.");
        this._context.Invoices.Add(invoice);
        return await this._context.SaveChangesAsync();
    }
    
    
    public IList<InvoiceModel> EnquiryInvoiceFromRawSql(string sql)
    {
        this._logger.LogInformation($"Getting invoice status from raw sql: {sql} in InvoiceService.");
        
        var res = this._context.Invoices.FromSqlRaw(sql).ToList();
        
        this._logger.LogInformation($"Got invoice result: {res} successfully");
        return res;
    }
    
    public int SubmitInvoiceFromRawSql(string sql)
    {
        this._logger.LogInformation($"Adding invoice from raw sql: {sql} in InvoiceService.");
        
        var count = this._context.Database.ExecuteSqlRaw(sql);
        
        this._logger.LogInformation($"Added invoice count: {count} successfully");
        return count;
    }
}