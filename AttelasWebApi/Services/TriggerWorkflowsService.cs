using Attelas.Utility;

namespace Attelas.Services;

public class TriggerWorkflowsService : ITriggerWorkflowsService
{
    public const string InvoiceTableName = "t_invoices";
    public const string ClientTableName = "t_clients";
    private readonly ILogger<TriggerWorkflowsService> _logger;
    private readonly ISqlParserService _sqlParserService;
    private readonly ILLmGenerateSqlService _llmGenerateSqlService;
    private readonly IInvoiceService _invoiceService;
    private readonly IClientService _clientService;
    
    public TriggerWorkflowsService(
        ILogger<TriggerWorkflowsService> logger, 
        ISqlParserService sqlParserService, 
        ILLmGenerateSqlService llmGenerateSqlService,
        IInvoiceService invoiceService, 
        IClientService clientService)
    {
        this._logger = logger;
        this._sqlParserService = sqlParserService;
        this._llmGenerateSqlService = llmGenerateSqlService;
        this._invoiceService = invoiceService;
        this._clientService = clientService;
    }

    public async Task<object> Run(string text)
    {
        try
        {
            string sqlStatement = await this._llmGenerateSqlService.GenerateSqlAsync(text);
    
            this._sqlParserService.Parse(sqlStatement);
            string tableName = this._sqlParserService.GetTableName();
            bool isQuery = this._sqlParserService.IsQuery();
            bool isCreate = this._sqlParserService.IsCreate();
            string sqlStatement2 = this._sqlParserService.GetSqlStatement();
            if (StringUtility.CompareIgnoreCase(tableName, InvoiceTableName) == 0)
            {
                if (isQuery)
                {
                    return this._invoiceService.EnquiryInvoiceFromRawSql(sqlStatement2).ToList();
                }
                else if (isCreate)
                {
                    return this._invoiceService.SubmitInvoiceFromRawSql(sqlStatement2);
                }
                else
                {
                    throw new NotSupportedException("Not supported this workflow now.");
                }
            }
            else if (StringUtility.CompareIgnoreCase(tableName, ClientTableName) == 0)
            {
                if (isQuery)
                {
                    return this._clientService.EnquiryClientsInfoFromRawSql(sqlStatement2);
                }
                else
                {
                    throw new NotSupportedException("Not supported this workflow now.");
                }
            }
            else
            {
                throw new ApplicationException("TableName not found.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to trigger workflows, got error: {e.Message}");
            throw;
        }
        
    }
}