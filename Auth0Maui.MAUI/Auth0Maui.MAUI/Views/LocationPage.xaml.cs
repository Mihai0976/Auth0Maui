namespace Auth0Maui.MAUI.Views;

public partial class LocationPage : ContentPage
{
	public LocationPage(LocationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
