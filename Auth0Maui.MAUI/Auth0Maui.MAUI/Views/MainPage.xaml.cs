using Auth0Maui.MAUI.Auth0;
using Auth0Maui.MAUI.Models;
using Auth0Maui.MAUI.Services;

namespace Auth0Maui.MAUI.Views;

public partial class MainPage : ContentPage
{
    public MainPage(Auth0Client client, HttpClient httpClient, AccountViewModel accountViewModel, ApiService apiService)
    {
        InitializeComponent();
        var viewModel = new MainViewModel(client, httpClient, apiService, accountViewModel );
        BindingContext = viewModel;

        MessagingCenter.Subscribe<MainViewModel, AlertMessage>(this, "DisplayAlert", async (sender, arg) =>
        {
            await DisplayAlert(arg.Title, arg.Message, arg.Cancel);
        });
    }

    // Don't forget to unsubscribe when the page is disposed
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<MainViewModel, AlertMessage>(this, "DisplayAlert");
    }

}
