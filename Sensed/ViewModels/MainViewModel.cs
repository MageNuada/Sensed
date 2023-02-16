using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
using Sensed.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
    }

    #region Overriden

    protected override Task OnInit()
    {
        MainDataProvider = new StubDataProvider();

        return Task.Run(async () =>
        {
            var idTask = MainDataProvider.Login("+79991234567");
            var paramsTask = MainDataProvider.GetSGParams();
            CurrentId = await idTask;
            VariationsList = (await paramsTask).ToArray();

            //await Task.WhenAll(paramsTask, idTask);
            //CurrentId = idTask.Result;
            //VariationsList = paramsTask.Result.ToArray();

            ActiveViewModel?.Deactivate();
            var resultTask = Task.CompletedTask;
            if (string.IsNullOrEmpty(CurrentId))
            {
                //Света, алло, тут вызов передача твоей вьюмодели контрола регистрации
            }
            else
            {
                Dispatcher.UIThread.Post(() => ActiveViewModel = new PeopleViewModel(MainDataProvider));
                //Dispatcher.UIThread.Post(() => ActiveViewModel = 
                //new FillProfileViewModel(new Account(new AccountDTO(), MainDataProvider), MainDataProvider, StorageProvider));
            }

            return resultTask.ContinueWith(x => base.OnInit());
        });
    }
    
    #endregion

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] public ViewModelBase? ActiveViewModel { get; set; }

    internal IStorageProvider? StorageProvider { get; set; }

    private IDataProvider? MainDataProvider { get; set; }

    public string? CurrentId { get; private set; }

    public (string parameter, InfoType type)[] VariationsList { get; private set; } = Array.Empty<(string, InfoType)>();
}
