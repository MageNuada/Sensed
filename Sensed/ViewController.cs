using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Sensed.Data;
using Sensed.ViewModels;
using System;
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

    internal ViewController(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }

    internal ViewController(MainViewModel mainViewModel, IDataProvider dataProvider)
    {
        MainViewModel = mainViewModel;
        DataProvider = dataProvider;
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
        if (viewModel == null) return;

        if (addPreviousToHistory && MainViewModel.ActiveViewModel != null)
            OpenedViewModels.Add(MainViewModel.ActiveViewModel);

        if (!ExistedViewModels.Exists(y => y.GetType() == viewModel.GetType()))
        {
            ExistedViewModels.Add(viewModel);
        }

        //не нужно, потому что мы хотим открыть конкретную вьюмодель
        //viewModel = ExistedViewModels.FirstOrDefault(x => x.GetType() == viewModel.GetType()) ?? viewModel;

        MainViewModel.SetViewModel(viewModel);
    }

    /// <summary>
    /// Открытие заданной вьюмодели
    /// </summary>
    /// <typeparam name="T"><see cref="ControlledViewModelBase"></typeparam>
    /// <param name="addPreviousToHistory"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public T OpenView<T>(bool addPreviousToHistory, params object[] args) where T : ControlledViewModelBase
    {
        if (addPreviousToHistory && MainViewModel.ActiveViewModel != null)
            OpenedViewModels.Add(MainViewModel.ActiveViewModel);

        var viewModel = GetOrCreateView<T>(args);

        MainViewModel.SetViewModel(viewModel);

        return viewModel;
    }

    /// <summary>
    /// Открытие заданной вьюмодели
    /// </summary>
    /// <typeparam name="T"><see cref="ControlledViewModelBase"></typeparam>
    /// <param name="addPreviousToHistory"></param>
    /// <returns></returns>
    public T OpenView<T>(bool addPreviousToHistory = false) where T : ControlledViewModelBase
    {
        if (addPreviousToHistory && MainViewModel.ActiveViewModel != null)
            OpenedViewModels.Add(MainViewModel.ActiveViewModel);

        var viewModel = GetOrCreateView<T>();

        MainViewModel.SetViewModel(viewModel);

        return viewModel;
    }

    public T CreateView<T>(params object[] args) where T : ControlledViewModelBase
    {
        if (args == null || args.Length == 0) args = new[] { this };
        var viewModel = (T)Activator.CreateInstance(typeof(T), args);

        return viewModel;
    }

    public T GetOrCreateView<T>(params object[] args) where T : ControlledViewModelBase
    {
        ViewModelBase? viewModel = ExistedViewModels.FirstOrDefault(x => x.GetType() == typeof(T));
        if (viewModel == null)
        {
            viewModel = CreateView<T>(args);
            ExistedViewModels.Add(viewModel);
        }

        return (T)viewModel;
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
            var viewModel = ExistedViewModels.FirstOrDefault(x => x.GetType() == typeof(PeopleViewModel));
            if (viewModel != null)
            {
                MainViewModel.SetViewModel(viewModel);
                return true;
            }
        }

        return false;
    }

    public void Close()
    {
        Console.WriteLine("Closing all views.");
        foreach (var viewModel in ExistedViewModels)
            viewModel.Close();
        MainViewModel?.Close();
    }

    private List<ViewModelBase> ExistedViewModels { get; } = new();

    private List<ViewModelBase> OpenedViewModels { get; } = new();

    internal MainViewModel MainViewModel { get; set; }

    public IStorageProvider? StorageProvider => MainViewModel?.StorageProvider;

    public IDataProvider DataProvider { get; }
}
