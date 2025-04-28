using System.Threading.Tasks;
using Entities;
using LabsoftAPI;

namespace Interfaces
{
    public interface IAccountsService
    {
        Task<ExternalResponse<MyLIMSResponseBase<Account>, ErrorResponse>> GetAccounts(int? accountType);
    }
}
