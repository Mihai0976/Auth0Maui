using Auth0Maui.ViewModels.UserManagement;

namespace Auth0Maui.Views;

public partial class GreetingsPage : ContentPage
{
    private readonly GreetingsViewModel _viewModel;

    public GreetingsPage(GreetingsViewModel viewModel)
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}