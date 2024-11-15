using System.Threading.Tasks;
using dpa.Library.Services;
using Ursa.Controls;

namespace dpa.Services;

public class AlertService : IAlertService {
    public async Task AlertAsync(string title, string message) =>
        await MessageBox.ShowAsync(message, title, button: MessageBoxButton.OK);
}