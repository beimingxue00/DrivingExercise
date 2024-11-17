using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class WrongViewModel : ViewModelBase
{
    private readonly IPoetryStorage _poetryStorage;

    public WrongViewModel(IPoetryStorage poetryStorage)
    {
        _poetryStorage = poetryStorage;
        LoadExerciseQuestions();
    }

    // ObservableCollection 用于绑定到 UI
    private ObservableCollection<string> _exerciseQuestions;
    public ObservableCollection<string> ExerciseQuestions
    {
        get => _exerciseQuestions;
        set => SetProperty(ref _exerciseQuestions, value);
    }

    private string _status;
    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    // 状态常量
    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更多结果";

    // 分页大小
    public const int PageSize = 20;

    // 加载问题列表的方法
    private async void LoadExerciseQuestions()
    {
        Status = Loading;

        // 这里我们用一个条件表达式来过滤数据，筛选 `question` 字段为 "example" 的数据
        var filter = Expression.Lambda<Func<Exercise, bool>>(
            Expression.Equal(
                Expression.Property(Expression.Parameter(typeof(Exercise), "e"), "Question"),
                Expression.Constant("example", typeof(string))
            ),
            Expression.Parameter(typeof(Exercise), "e")
        );

        // 获取问题列表
        var questions = await _poetryStorage.GetExerciseQuestionsAsync(filter, 0, PageSize);

        // 更新问题列表
        if (questions.Count == 0)
        {
            Status = NoResult;
        }
        else
        {
            ExerciseQuestions = new ObservableCollection<string>(questions);
            Status = string.Empty;
        }
    }
}
