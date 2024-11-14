using Attelas.Models;

namespace Attelas.Services;

public interface IClientService
{
    Task<IEnumerable<ClientModel>> EnquiryClientsInfoAsync(IEnumerable<string> clientIds);

    IEnumerable<ClientModel> EnquiryClientsInfoFromRawSql(string sql);
}