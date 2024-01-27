namespace Auth0Maui.MAUI.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
