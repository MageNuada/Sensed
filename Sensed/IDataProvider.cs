using Sensed.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sensed;

public enum OperationResult
{
    Success,
    Fail,
    Undefined,
}

public interface IDataProvider
{
    /// <summary>
    /// ??? По идее при запуске приложения попытка законнектиться по номеру телефона и узнать,
    /// что всё в порядке - аккаунт существует
    /// </summary>
    /// <param name="id"></param>
    /// <returns>id в случае успеха, null в случае неудачи</returns>
    Task<string?> Login(string phone);

    /// <summary>
    /// Регистрация нового аккаунта
    /// </summary>
    /// <param name="phone">Номер телефона без пробелов и скобочек с кодом страны в начале</param>
    /// <returns>Если телефон уже зарегистрирован - выдаст неудачу</returns>
    Task<OperationResult> CreateAccount(string phone);

    /// <summary>
    /// Подтверждение аккаунта кодом из смс
    /// </summary>
    /// <param name="id"></param>
    /// <param name="smsCode"></param>
    /// <returns>id в случае успеха, null в случае неудачи</returns>
    Task<string?> VerifyAccount(string phone, string smsCode);

    Task<OperationResult> ModifyAccount(string description, IEnumerable<object> photos, IEnumerable<object> tags);

    Task<OperationResult> DeleteAccount(string id);

    Task<IEnumerable<Account>> GetAccounts(IEnumerable<string> ids, IEnumerable<SearchFilter>? filters = null);

    Task<IEnumerable<Account>> SearchAccounts(IEnumerable<SearchFilter> filters);

    /// <summary>
    /// Лайк/дизлайк аккаунта
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OperationResult> MarkAccount(string id, AccountMark mark, string? description = null);

    /// <summary>
    /// Получить список совпавших аккаунтов. Вызывается каждые Х секунд + должна вызываться после лайка для проверки?
    /// </summary>
    /// <returns></returns>
    Task<Account> GetMatchedAccounts();

    /// <summary>
    /// Старт чата между людьми - видимо, должно возвращать провайдер, отвечающий за чат???
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<object> StartChat(string id);

    Task<byte[]> GetPhoto(string photoId);
}