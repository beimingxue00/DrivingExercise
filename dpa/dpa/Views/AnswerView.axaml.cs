using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using dpa.Library.ViewModels;

namespace dpa.Views;

public partial class AnswerView : UserControl {
    public AnswerView() {
        InitializeComponent();
    }

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var vm = this.DataContext as AnswerViewModel;
            vm.Submit();
        }
    }
}