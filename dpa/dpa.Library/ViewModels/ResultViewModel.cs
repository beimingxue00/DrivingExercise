using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using AvaloniaInfiniteScrolling;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class ResultViewModel : ViewModelBase {
    private readonly IPoetryStorage _poetryStorage;

    public ResultViewModel(IPoetryStorage poetryStorage) {
        _poetryStorage = poetryStorage;

        PoetryCollection = new AvaloniaInfiniteScrollCollection<Poetry> {
            OnCanLoadMore = () => _canLoadMore,
            OnLoadMore = async () => {
                Status = Loading;
                var poetries = await poetryStorage.GetPoetriesAsync(Expression.Lambda<Func<Poetry, bool>>(
                        Expression.Constant(true),
                        Expression.Parameter(typeof(Poetry), "p")),
                    PoetryCollection.Count, PageSize);
                Status = string.Empty;

                if (poetries.Count < PageSize) {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (PoetryCollection.Count == 0 && poetries.Count == 0) {
                    Status = NoResult;
                }

                return poetries;
            }
        };
    }

    private bool _canLoadMore = true;

    private string _status;

    public string Status {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";


    public const int PageSize = 20;

    public AvaloniaInfiniteScrollCollection<Poetry> PoetryCollection { get; }
}