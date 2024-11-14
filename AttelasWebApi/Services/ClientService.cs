using Attelas.DbContex;
using Attelas.Models;
using Microsoft.EntityFrameworkCore;

namespace Attelas.Services;

public class ClientService : IClientService
{
    private readonly ILogger<ClientService> _logger;
    private readonly AttelasDbContext _context;

    public ClientService(ILogger<ClientService> logger, AttelasDbContext context)
    {
        this._logger = logger;
        this._context = context;
    }

    public async Task<IEnumerable<ClientModel>> EnquiryClientsInfoAsync(IEnumerable<string> clientIds)
    {
        var clientIdsSet = clientIds.ToHashSet();
        this._logger.LogInformation($"Getting clients info for clientIds: {clientIdsSet}");
        var res = await this._context.Clients.Where(client => clientIdsSet.Contains(client.ClientId)).ToListAsync();
        this._logger.LogInformation($"Got clients info result: {res} successfully.");
        return res;
    }
    
    public IEnumerable<ClientModel> EnquiryClientsInfoFromRawSql(string sql)
    {
        this._logger.LogInformation($"Getting clients info from raw sql: {sql} in ClientService.");
        var clientsInfo = this._context.Clients.FromSqlRaw(sql);
        this._logger.LogInformation($"Got clients info result: {clientsInfo} successfully.");
        return clientsInfo;
    }
}