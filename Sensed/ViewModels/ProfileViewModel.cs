using ReactiveUI.Fody.Helpers;
using Sensed.Models;

namespace Sensed.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel()
        {
            Account = new Account(new AccountDTO(), null);
        }


        public ProfileViewModel(Account account)
        {
            Account = account;
        }

        [Reactive] public Account Account { get; set; }
    }
}
