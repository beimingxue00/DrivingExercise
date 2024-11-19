using System;
using dpa.Library.Services;
using dpa.Library.ViewModels;

namespace dpa.Services;

public class MenuNavigationService : IMenuNavigationService {
   public void NavigateTo(string view)
    {
        ViewModelBase viewModel=null;
        switch (view)
        {
            case MenuNavigationConstant.AnswerView:
                ServiceLocator.Current.AnswerViewModel.AdviseInitial();
                viewModel = ServiceLocator.Current
                    .AnswerViewModel;
                break;
            case MenuNavigationConstant.WrongView:
                viewModel = ServiceLocator.Current
                    .WrongViewModel;
                break;
            case MenuNavigationConstant.ProgressView:
                viewModel = ServiceLocator.Current
                    .ProgressViewModel;
                break;
        }
        if (viewModel != null)
        {
            ServiceLocator.Current.MainViewModel.SetMenuAndContent(view, viewModel);
        }
    }
}
