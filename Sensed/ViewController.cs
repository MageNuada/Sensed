using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Sensed.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Sensed;

public interface IQueuedView : IViewControlled
{
    public void GetOnPreviousView()
    {
        if (Design.IsDesignMode) return;

        ViewController.ReturnPrevious();
    }
}

public class ViewController
{
    internal ViewController() { }

    internal ViewController(MainViewModel mainViewModel)
    {
        MainViewModel = mainViewModel;
    }

    internal void SetMainView(MainViewModel mainViewModel)
    {
        MainViewModel = mainViewModel;
    }

    public void OpenView(ViewModelBase? viewModel, bool addHistory = false)
    {
        if (addHistory)
            OpenedViewModels.Add(MainViewModel.ActiveViewModel);

        if (viewModel != null && !ExistedViewModels.Exists(y => y.GetType() == viewModel.GetType()))
        {
            ExistedViewModels.Add(viewModel);
        }

        viewModel = ExistedViewModels.FirstOrDefault(x => x.GetType() == viewModel?.GetType()) ?? viewModel;

        MainViewModel.ActiveViewModel = viewModel;
    }

    public bool ReturnPrevious()
    {
        if (OpenedViewModels.Any())
        {
            var viewModel = OpenedViewModels[^1];
            OpenedViewModels.RemoveAt(OpenedViewModels.Count - 1);
            MainViewModel.ActiveViewModel = viewModel;
            return true;
        }
        else if (MainViewModel.ActiveViewModel?.GetType() != typeof(PeopleViewModel))
        {
            var viewModel = ExistedViewModels.Find(x => x.GetType() == typeof(PeopleViewModel));
            MainViewModel.ActiveViewModel = viewModel;
            return true;
        }

        return false;
    }

    private List<ViewModelBase> ExistedViewModels { get; } = new();

    private List<ViewModelBase?> OpenedViewModels { get; } = new();

    private MainViewModel MainViewModel { get; set; }

    public IStorageProvider? StorageProvider => MainViewModel?.StorageProvider;
}
