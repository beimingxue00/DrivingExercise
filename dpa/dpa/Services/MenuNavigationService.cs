using System;
using dpa.Library.Services;
using dpa.Library.ViewModels;

namespace dpa.Services;

public class MenuNavigationService : IMenuNavigationService {
   public void NavigateTo(string view) {
        ViewModelBase viewModel = view switch {
            MenuNavigationConstant.AnswerView => ServiceLocator.Current
                .AnswerViewModel,
            MenuNavigationConstant.WrongView => ServiceLocator.Current
                .WrongViewModel,
            MenuNavigationConstant.ProgressView => ServiceLocator.Current
                .ProgressViewModel,
            _ => throw new Exception("未知的视图。")
        };
        ServiceLocator.Current.MainViewModel.SetMenuAndContent(view, viewModel);
   }
}
