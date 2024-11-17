using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace dpa.Views;

public partial class WrongView : UserControl {
    public WrongView() {
        InitializeComponent();
        // 创建并绑定 ViewModel
            IPreferenceStorage preferenceStorage = new FilePreferenceStorage();
            this.DataContext = new WrongViewModel(new PoetryStorage(preferenceStorage));
    }
}
