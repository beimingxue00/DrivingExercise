using System;
using Avalonia;
using dpa.Library.Services;
using dpa.Library.ViewModels;
using dpa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace dpa;

public class ServiceLocator {
    private readonly IServiceProvider _serviceProvider;

    private static ServiceLocator _current;

    public static ServiceLocator Current {
        get {
            if (_current is not null) {
                return _current;
            }

            if (Application.Current.TryGetResource(nameof(ServiceLocator), null,
                    out var resource) &&
                resource is ServiceLocator serviceLocator) {
                return _current = serviceLocator;
            }

            throw new Exception("理论上来讲不应该发生这种情况。");
        }
    }

    public ResultViewModel ResultViewModel =>
        _serviceProvider.GetRequiredService<ResultViewModel>();

    public AnswerViewModel AnswerViewModel =>
        _serviceProvider.GetRequiredService<AnswerViewModel>();

    public MainWindowViewModel MainWindowViewModel =>
        _serviceProvider.GetRequiredService<MainWindowViewModel>();

    // TODO Delete this
    public IRootNavigationService RootNavigationService =>
        _serviceProvider.GetRequiredService<IRootNavigationService>();

    public MainViewModel MainViewModel =>
        _serviceProvider.GetRequiredService<MainViewModel>();

    public WrongViewModel WrongViewModel =>
        _serviceProvider.GetRequiredService<WrongViewModel>();

    public InitializationViewModel InitializationViewModel =>
        _serviceProvider.GetRequiredService<InitializationViewModel>();

    public TodayDetailViewModel TodayDetailViewModel =>
        _serviceProvider.GetRequiredService<TodayDetailViewModel>();
    
    public ProgressViewModel ProgressViewModel =>
        _serviceProvider.GetRequiredService<ProgressViewModel>();

    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<AnswerViewModel>();
        serviceCollection
            .AddSingleton<ITodayPoetryService, JinrishiciService>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection
            .AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection
            .AddSingleton<IMenuNavigationService, MenuNavigationService>();
        serviceCollection.AddSingleton<WrongViewModel>();
        serviceCollection.AddSingleton<ProgressViewModel>();
        serviceCollection.AddSingleton<InitializationViewModel>();
        serviceCollection.AddSingleton<ITodayImageService, BingImageService>();
        serviceCollection.AddSingleton<ITodayImageStorage, TodayImageStorage>();
        serviceCollection.AddSingleton<TodayDetailViewModel>();
        serviceCollection
            .AddSingleton<IContentNavigationService,
                ContentNavigationService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}