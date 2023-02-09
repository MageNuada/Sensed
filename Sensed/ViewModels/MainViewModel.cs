using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace Sensed.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
    }
    
    #region Overriden

    protected async override Task OnActivate()
    {
        await base.OnActivate();

        MainDataProvider = new StubDataProvider();

        CurrentId = await MainDataProvider.Login("+79991234567");

        ActiveViewModel?.Deactivate();

        if (string.IsNullOrEmpty(CurrentId))
        {
            //Света, алло, тут вызов передача твоей вьюмодели контрола регистрации
        }
        else
        {
            ActiveViewModel = new PeopleViewModel(MainDataProvider);

            await ActiveViewModel.Activate();
        }
    }

    #endregion

    /// <summary>
    /// Активная в данный момент вьюмодель для приложения
    /// </summary>
    [Reactive] public ConnectedViewModelBase? ActiveViewModel { get; set; }

    private IDataProvider? MainDataProvider { get; set; }

    public string? CurrentId { get; private set; }
}
