using CommunityToolkit.Maui.Views;
using seeds1.Factories;
using seeds1.Interfaces;

namespace seeds1.View;

public partial class PreferencesPage : ContentPage
{
    public PreferencesPage(PreferencesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}