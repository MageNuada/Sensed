using Sensed.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sensed;

internal class StubDataProvider : IDataProvider
{
    public Task<OperationResult> CreateAccount(string phone)
    {
        throw new System.NotImplementedException();
    }

    public Task<OperationResult> DeleteAccount(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Account>> GetAccounts(IEnumerable<string> ids, IEnumerable<SearchFilter>? filters = null)
    {
        throw new System.NotImplementedException();
    }

    public Task<AccountStatus> GetAccountStatus(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task<Account> GetMatchedAccounts()
    {
        throw new System.NotImplementedException();
    }

    public Task<byte[]> GetPhoto(string photoId)
    {
        throw new System.NotImplementedException();
    }

    public Task<string?> Login(string phone)
    {
        throw new System.NotImplementedException();
    }

    public Task<OperationResult> MarkAccount(string id, AccountMark mark, string? description = null)
    {
        throw new System.NotImplementedException();
    }

    public Task<OperationResult> ModifyAccount(string description, IEnumerable<object> photos, IEnumerable<object> tags)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Account>> SearchAccounts(IEnumerable<SearchFilter> filters)
    {
        throw new System.NotImplementedException();
    }

    public Task<OperationResult> SetAccountStatus(string id, AccountStatus status)
    {
        throw new System.NotImplementedException();
    }

    public Task<object> StartChat(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task<string?> VerifyAccount(string phone, string smsCode)
    {
        throw new System.NotImplementedException();
    }
}
