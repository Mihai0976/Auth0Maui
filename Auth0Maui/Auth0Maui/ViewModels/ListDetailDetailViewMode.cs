using Auth0Maui.ViewModels.Base;

namespace Auth0Maui.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class ListDetailDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    SampleItem item;
}
