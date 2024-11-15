using dpa.Library.Models;

namespace dpa.Library.ViewModels;

public class TodayDetailViewModel : ViewModelBase {
    private TodayPoetry _todayPoetry;

    public TodayPoetry TodayPoetry {
        get => _todayPoetry;
        private set => SetProperty(ref _todayPoetry, value);
    }

    public override void SetParameter(object parameter) {
        TodayPoetry = parameter as TodayPoetry;
    }
}