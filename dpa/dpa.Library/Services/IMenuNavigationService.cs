namespace dpa.Library.Services;

public interface IMenuNavigationService {
    void NavigateTo(string view);
}

public static class MenuNavigationConstant {
    public const string AnswerView = nameof(AnswerView);

    public const string WrongView = nameof(WrongView);

    public const string ProgressView = nameof(ProgressView);
}