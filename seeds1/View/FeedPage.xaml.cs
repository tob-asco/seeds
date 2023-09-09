using seeds1.Factories;
using seeds1.Interfaces;
using System.Runtime.CompilerServices;

namespace seeds1.View;

public partial class FeedPage : ContentPage
{
    private readonly FeedViewModel vm;
    private readonly INavigationService navigationService;
    public FeedPage(
        FeedViewModel vm,
        INavigationService navigationService)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.navigationService = navigationService;
    }
}