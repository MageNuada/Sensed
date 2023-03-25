using Avalonia.Media.Imaging;
using Sensed.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sensed.Data;

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
    /// Регистрация нового аккаунта, в случае успеха надо вызвать <see cref="VerifyAccount(string, string)"/>
    /// для отправки полученного на указанный номер кода в смс
    /// </summary>
    /// <param name="phone">Номер телефона (Y) без пробелов и скобочек с кодом страны (X) в начале вида +XXYYYYYYY</param>
    /// <returns>Если телефон уже зарегистрирован - выдаст неудачу</returns>
    Task<OperationResult> CreateAccount(string phone);

    /// <summary>
    /// Подтверждение аккаунта кодом из смс
    /// </summary>
    /// <param name="id"></param>
    /// <param name="smsCode"></param>
    /// <returns>id в случае успеха, null в случае неудачи</returns>
    Task<string?> VerifyAccount(string phone, string smsCode);

    /// <summary>
    /// Изменение информации аккаунта текущего пользователя
    /// </summary>
    /// <param name="account">Аккаунт</param>
    /// <returns></returns>
    Task<OperationResult> ModifyAccount(AccountDTO account);

    Task<OperationResult> DeleteAccount();

    Task<IEnumerable<AccountDTO>> GetAccounts(IEnumerable<string> ids, IEnumerable<SearchFilter>? filters = null);

    Task<IEnumerable<AccountDTO>> SearchAccounts(IEnumerable<SearchFilter> filters);

    /// <summary>
    /// Получение списка гендеров, сексуальных ориентаций и прочих выбираемых параметров для профиля с сервера
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<(string parameter, InfoType type)>> GetSGParams();

    /// <summary>
    /// Лайк/дизлайк аккаунта
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OperationResult> MarkAccount(string id, AccountMark mark, string? description = null);

    /// <summary>
    /// Получить список совпавших аккаунтов. Вызывается каждые Х секунд + должна вызываться после лайка для проверки?
    /// </summary>
    /// <returns>список аккаунтов, mark - тип взаимодействия поставленный, whos - кто поставил
    /// (0 - текущий пользователь, 1 - другие пользователи, 2 - оба лайкнули друг друга)</returns>
    Task<IEnumerable<(AccountDTO account, AccountMark mark, int whos)>> GetMatchedAccounts();

    /// <summary>
    /// Получить статус аккаунта в плане оплаченности
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountStatus> GetAccountStatus();

    /// <summary>
    /// Запрос на смену(оплату) статуса, после выполнения надо вызвать <see cref="GetAccountStatus(string)" />
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns>Ссылка (? скорее всего) на оплату</returns>
    Task<string> SetAccountStatus(AccountStatus status);

    /// <summary>
    /// Старт чата между людьми - видимо, должно возвращать провайдер, отвечающий за чат???
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<object> StartChat(string id);

    /// <summary>
    /// Получение изображения по его айди
    /// </summary>
    /// <param name="photoId"></param>
    /// <returns>Изображение</returns>
    Task<Bitmap> GetPhoto(string photoId);

    /// <summary>
    /// Загрузка изображения на сервер
    /// </summary>
    /// <param name="photo">Изображение</param>
    /// <returns>ID изображения</returns>
    public Task<string> UploadPhoto(Bitmap photo);

    /// <summary>
    /// Удаление изображения с сервера по его ID
    /// </summary>
    /// <param name="photoId">ID удаляемого изображения</param>
    /// <returns></returns>
    public Task<OperationResult> DeletePhoto(string photoId);
}