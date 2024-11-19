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
    
    public ICommand OnQuestionClickedCommand => new RelayCommand<int>((id) => OnQuestionClicked(id));

    public WrongViewModel(IPoetryStorage poetryStorage)
    {
        _poetryStorage = poetryStorage;
        LoadExerciseQuestions();
    }

    // ObservableCollection 用于绑定到 UI
    private ObservableCollection<Exercise> _exerciseQuestions;
    public ObservableCollection<Exercise> ExerciseQuestions
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
    public const int PageSize = 1000;

    // 加载问题列表的方法
    private async void LoadExerciseQuestions()
    {
        Status = Loading;

        // 获取所有数据，不使用筛选条件
        var exercises = await _poetryStorage.GetExerciseQuestionsAsync(null, 0, PageSize);

        // 更新问题列表
        if (exercises.Count == 0)
        {
            Status = NoResult;
        }
        else
        {
            // 遍历 Exercise 列表并为每个问题添加序号
            for (int i = 0; i < exercises.Count; i++)
            {
                var exercise = exercises[i];
                // 检查问题内容长度是否超过 8 个字符，若超过则截取
                if (exercise.question.Length > 8)
                {
                    exercise.question = exercise.question.Substring(0, 8) + "...";
                }
                // 为每个问题添加前缀（序号）
                exercise.question = $"（{(i + 1)}）{exercise.question}";
            }

            // 使用 ObservableCollection 存储 Exercise 对象列表
            ExerciseQuestions = new ObservableCollection<Exercise>(exercises);
            Status = string.Empty;
        }
    }

    
    private void OnQuestionClicked(int id)
    {
        // 在这里编写跳转题目的逻辑
        Console.WriteLine($"题目 ID: {id} 被点击");
    }


}
