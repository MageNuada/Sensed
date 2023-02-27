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

    /// <summary>
    /// Открытие заданной вьюмодели
    /// </summary>
    /// <param name="viewModel">Вьюмодель для отображения</param>
    /// <param name="addPreviousToHistory">Добавлять ли предыдущую вьюмодель в историю,
    /// чтобы при нажатии кнопки возврата открывалась снова предыдущая вьюмодель</param>
    public void OpenView(ViewModelBase? viewModel, bool addPreviousToHistory = false)
    {
        if (addPreviousToHistory)
            OpenedViewModels.Add(MainViewModel.ActiveViewModel);

        if (viewModel != null && !ExistedViewModels.Exists(y => y.GetType() == viewModel.GetType()))
        {
            ExistedViewModels.Add(viewModel);
        }

        viewModel = ExistedViewModels.FirstOrDefault(x => x.GetType() == viewModel?.GetType()) ?? viewModel;

        MainViewModel.SetViewModel(viewModel);
    }

    public bool ReturnPrevious()
    {
        if (OpenedViewModels.Any())
        {
            var viewModel = OpenedViewModels[^1];
            OpenedViewModels.RemoveAt(OpenedViewModels.Count - 1);
            MainViewModel.SetViewModel(viewModel);
            return true;
        }
        else if (MainViewModel.ActiveViewModel?.GetType() != typeof(PeopleViewModel))
        {
            var viewModel = ExistedViewModels.Find(x => x.GetType() == typeof(PeopleViewModel));
            MainViewModel.SetViewModel(viewModel);
            return true;
        }

        return false;
    }

    public void Close()
    {
        System.Console.WriteLine("Closing all views.");
        foreach (var viewModel in OpenedViewModels)
            viewModel.Close();
    }

    private List<ViewModelBase> ExistedViewModels { get; } = new();

    private List<ViewModelBase> OpenedViewModels { get; } = new();

    private MainViewModel MainViewModel { get; set; }

    public IStorageProvider? StorageProvider => MainViewModel?.StorageProvider;
}
