using Auth0Maui.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.ViewModels
{
    public partial class ListDetailViewModel : BaseViewModel
    {
        readonly SampleDataService dataService;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        ObservableCollection<SampleItem> items;

        public ListDetailViewModel(SampleDataService service)
        {
            dataService = service;
        }

        [RelayCommand]
        private async Task OnRefreshing()
        {
            IsRefreshing = true;

            try
            {
                await LoadDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task LoadMore()
        {
            var items = await dataService.GetItems();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async Task LoadDataAsync()
        {
            Items = new ObservableCollection<SampleItem>(await dataService.GetItems());
        }

        [RelayCommand]
        private async Task GoToDetails(SampleItem item)
        {
            await Shell.Current.GoToAsync(nameof(ListDetailDetailPage), true, new Dictionary<string, object>
        {
            { "Item", item }
        });
        }
    }
}
