using Avalonia.Threading;
using ReactiveUI.Fody.Helpers;
using Sensed.Data;
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
            CurrentId = await MainDataProvider.Login("+79991234567");
            ActiveViewModel?.Deactivate();

            var resultTask = Task.CompletedTask;
            if (string.IsNullOrEmpty(CurrentId))
            {
                //Света, алло, тут вызов передача твоей вьюмодели контрола регистрации
            }
            else
            {
                Dispatcher.UIThread.Post(() => ActiveViewModel = new PeopleViewModel(MainDataProvider));
            }

            return resultTask.ContinueWith(x => base.OnInit());
        });
    }

    #endregion

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] public ViewModelBase? ActiveViewModel { get; set; }

    private IDataProvider? MainDataProvider { get; set; }

    public string? CurrentId { get; private set; }
}
