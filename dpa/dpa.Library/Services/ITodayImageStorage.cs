using dpa.Library.Models;

namespace dpa.Library.Services;

public interface ITodayImageStorage {
    Task<TodayImage> GetTodayImageAsync(bool isIncludingImageStream);

    Task SaveTodayImageAsync(TodayImage todayImage, bool isSavingExpiresAtOnly);
}