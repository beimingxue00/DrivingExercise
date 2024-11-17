using System.Linq.Expressions;
using dpa.Library.Models;

namespace dpa.Library.Services;

public interface IPoetryStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Poetry> GetPoetryAsync(int id);

    Task<IList<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);
    
    // 这是修改后的用于返回问题字段的方法
    Task<List<string>> GetExerciseQuestionsAsync(Expression<Func<Exercise, bool>> where, int skip, int take);
}
