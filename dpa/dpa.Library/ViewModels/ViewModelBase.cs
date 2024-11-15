using CommunityToolkit.Mvvm.ComponentModel;

namespace dpa.Library.ViewModels;

public abstract class ViewModelBase : ObservableObject {
    public virtual void SetParameter(object parameter) { }
}