using Attelas.Models;

namespace Attelas.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceStatus>> EnquiryInvoiceByIdsAsync(IEnumerable<string> invoiceIds);
    
    Task<IEnumerable<InvoiceModel>> EnquiryInvoiceByClientIdsAsync(IEnumerable<string> clientIds);
    
    Task<int> SubmitInvoiceAsync(InvoiceModel invoices);
    
    IList<InvoiceModel> EnquiryInvoiceFromRawSql(string sql);
    
    int SubmitInvoiceFromRawSql(string sql);
}