using System;
using dpa.Library.Services;
using dpa.Library.ViewModels;

namespace dpa.Services;

public class RootNavigationService : IRootNavigationService {
    public void NavigateTo(string view) {
        ServiceLocator.Current.MainWindowViewModel.Content = view switch {
            RootNavigationConstant.InitializationView => ServiceLocator.Current
                .InitializationViewModel,
            RootNavigationConstant.MainView => ServiceLocator.Current
                .MainViewModel,
            _ => throw new Exception("未知的视图。")
        };
    }
}