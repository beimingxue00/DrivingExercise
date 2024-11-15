using System.Drawing;
using System.Net;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;
using Webiat;

namespace dpa.Library.ViewModels;

public class AnswerViewModel : ViewModelBase
{
    /// ////////////////////////////////////////////////////////////////////////////
    private AIReplyService _aiReplyService;//ai封装服务
    private AdviseService _adviseService;//交通新闻服务
    
    private string _answer_exercise;//习题答案
    public string answer_exercise {
        get => _answer_exercise;
        set => SetProperty(ref _answer_exercise, value);
    }
    private string _advise_content;//新闻内容
    public string advise_content {
        get => _advise_content;
        set => SetProperty(ref _advise_content, value);
    }
    private string _advise_picture;//新闻图片
    public string advise_picture {
        get => _advise_picture;
        set => SetProperty(ref _advise_picture, value);
    }
    private string _advise_href;//新闻链接
    public string advise_href {
        get => _advise_href;
        set => SetProperty(ref _advise_href, value);
    }
 
    private string _answer_ai;//ai回答
    public string answer_ai {
        get => _answer_ai;
        set => SetProperty(ref _answer_ai, value);
    }
    private string _question_user;//用户问题
    public string question_user {
        get => _question_user;
        set => SetProperty(ref _question_user, value);
    }
    private Boolean _isFocused;//文本框焦点
    public Boolean isFocused {
        get => _isFocused;
        set => SetProperty(ref _isFocused, value);
    }
    private Boolean _isPaneOpened;//ai页面显示
    public Boolean isPaneOpened {
        get => _isPaneOpened;
        set => SetProperty(ref _isPaneOpened, value);
    }
    public ICommand SubmitCommand { get;}//提交问题command
    public ICommand AIPaneCommand { get;}//ai页面显示开关
    public async void Submit()//提交问题函数
    {
        answer_ai = "AI分析中，请稍候......";
        string i = question_user;
        question_user = String.Empty;
        isFocused = false;
        answer_ai=await _aiReplyService.reply(i,500);
        isFocused = true;
    }

    public async void AIPane() //ai页面函数
    {
        isPaneOpened = !isPaneOpened;
    }
    //初始化
    public AnswerViewModel(IContentNavigationService contentNavigationService) {
        _contentNavigationService = contentNavigationService;
        _aiReplyService = new AIReplyService();
        _adviseService = new AdviseService();

        SubmitCommand = new RelayCommand(Submit);
        AIPaneCommand = new RelayCommand(AIPane);
        isFocused = true;
        isPaneOpened = false;
        AdviseInitial();//新闻内容拉取
    }
    //新闻内容初始化
    public void AdviseInitial()
    {
        string[] str = _adviseService.getadvise();
        _advise_content = str[0];
        _advise_picture = str[1];
        _advise_href = str[2];
    }
  
    
    
    
    
    
    
    
    
  
    /// /////////////////////////////////////////////////////////////////////////
    private readonly ITodayImageService _todayImageService;
    private readonly ITodayPoetryService _todayPoetryService;
    private readonly IContentNavigationService _contentNavigationService;

    private TodayPoetry _todayPoetry;

    public void OnInitialized() {
        //
        Task.Run(async () => {
            IsLoading = true;
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });
        // Task.Run(async () => {
        //     TodayImage = await _todayImageService.GetTodayImageAsync();
        //     
        //     var updateResult = await _todayImageService.CheckUpdateAsync();
        //     if (updateResult.HasUpdate) {
        //         TodayImage = updateResult.TodayImage;
        //     }
        // });
    }
    public TodayPoetry TodayPoetry {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }

    private TodayImage _todayImage;

    public TodayImage TodayImage {
        get => _todayImage;
        private set => SetProperty(ref _todayImage, value);
    }

    private bool _isLoading;

    public bool IsLoading {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand OnInitializedCommand { get; }
    
    public ICommand ShowDetailCommand { get; }

    public void ShowDetail() {
        _contentNavigationService.NavigateTo(
            ContentNavigationConstant.TodayDetailView, TodayPoetry);
    }
}