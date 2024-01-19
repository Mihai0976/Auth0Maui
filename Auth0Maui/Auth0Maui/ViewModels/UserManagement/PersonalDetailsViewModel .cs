using Auth0Maui.Domain.Models.DTOs;
using Auth0Maui.ViewModels.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Auth0Maui.ViewModels.UserManagement;

public class PersonalDetailsViewModel : BaseViewModel
{
    private readonly ApiExternServices _apiExternServices;
    private readonly ApiService _apiService;
    private readonly ILogger _logger;

    private readonly INavigationService _navigationService;
    private readonly UserSessionService _userSessionService;
    private string _address;
    private string _countryFlagUrl;

    private ObservableCollection<CountryPickerObj> _countryPickerObj;
    private string _email;
    private string _name;
    private string _phone;

    private CountryPickerObj _selectedCountry;
    private string _selectedCountryFlagUrl;
    private string _surname;
    private string _userPictureUrl;

    public PersonalDetailsViewModel(
        INavigationService navigationService,
        ILogger<PersonalDetailsViewModel> logger,
        ApiService apiService,
        ApiExternServices apiExternServices,
        UserSessionService userSessionService)
    {
        _navigationService = navigationService;
        _logger = logger;
        _apiService = apiService;
        _apiExternServices = apiExternServices;
        _userSessionService = userSessionService;


        UpdateCommand = new Command(OnUpdate);
        BackArrowTapped = new Command(OnBackArrowTapped);
        CountryPickerObj = new ObservableCollection<CountryPickerObj>();
    }

    public ICommand UpdateCommand { get; }
    public ICommand BackArrowTapped { get; }
    public ICommand ChangePictureCommand => new Command(Execute);

    //private ObservableCollection<CountryPickerObj> _countryPickerObj;
    //public ObservableCollection<CountryPickerObj> CountryPickerObj
    //{
    //    get => _countryPickerObj;
    //    set => SetProperty(ref _countryPickerObj, value);
    //}
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Surname
    {
        get => _surname;
        set => SetProperty(ref _surname, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string UserPictureUrl
    {
        get => _userPictureUrl;
        set => SetProperty(ref _userPictureUrl, value);
    }

    public string CountryFlagUrl
    {
        get => _countryFlagUrl;
        set
        {
            _countryFlagUrl = value;
            OnPropertyChanged(nameof(CountryFlagUrl));
        }
    }

    public string SelectedCountryFlagUrl
    {
        get => _selectedCountryFlagUrl;
        set => SetProperty(ref _selectedCountryFlagUrl, value);
    }

    public ObservableCollection<CountryPickerObj> CountryPickerObj
    {
        get => _countryPickerObj;
        set
        {
            _countryPickerObj = value;
            OnPropertyChanged(nameof(CountryPickerObj));
        }
    }

    public CountryPickerObj SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            if (_selectedCountry != value)
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                CountryFlagUrl = _selectedCountry?.CountryFlag;
            }
        }
    }

    private async void Execute()
    {
        await ChangePictureAsync();
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Assuming you have a method to load user details
            await LoadUserDetailsAsync();

            // Load country-specific data
            await LoadCountryDataAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Initialization error: {ex.Message}");
            _logger.LogError(ex, "Error occurred during initialization.");
        }
    }

    public void ResetInitialization()
    {
        // Reset any necessary state here
    }

    private async Task LoadUserDetailsAsync()
    {
        try
        {
            var userId = _userSessionService.CurrentUser.Id;
            var userDetails = await _apiService.GetUserById(userId.ToString());
            if (userDetails != null)
            {
                Name = userDetails.Name;
                Surname = userDetails.Surname;
                Email = userDetails.Email;
                UserPictureUrl = userDetails.UserPictureUrl;
                CountryFlagUrl = userDetails.CountryFlagUrl;
                Address = userDetails.Address;
                Phone = userDetails.Phone;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            // Handle exception
        }
    }

    private async void OnUpdate()
    {
        try
        {
            {
                var updateUserDto = new UpdateUserModel
                {
                    Id = _userSessionService.CurrentUser.Id,
                    Name = Name,
                    Surname = Surname,
                    Email = Email,
                    PhoneNumber = Phone,
                    Address = Address
                };

                // Make the API call
                var response = await _apiService.UpdateUser(updateUserDto);

                // Handle the response from the API
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    Debug.WriteLine("User updated successfully.");
                    if (Application.Current != null)
                        if (Application.Current.MainPage != null)
                            await Application.Current.MainPage.DisplayAlert("", "User updated successfully.", "OK");
                }
                else
                {
                    if (Application.Current != null)
                        if (Application.Current.MainPage != null)
                            await Application.Current.MainPage.DisplayAlert("Fail!",
                                $"Update failed: {await response.Content.ReadAsStringAsync()}", "ok");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user update.");
        }
    }

    private async void OnBackArrowTapped()
    {
        try
        {
            await _navigationService.NavigateToAsync("//accountTab");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation error: {ex.Message}");
            _logger.LogError(ex, "Error occurred during navigation to homeTab.");
        }
    }

    private async Task LoadCountryDataAsync()
    {
        try
        {
            var jsonData = await _apiExternServices.GetCountryDataAsync();

            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                var countriesData = JsonConvert.DeserializeObject<List<CountryPickerObj>>(jsonData);

                // Clear existing items
                CountryPickerObj.Clear();

                if (countriesData != null)
                    foreach (var countryData in countriesData)
                        CountryPickerObj.Add(countryData);

                OnPropertyChanged(nameof(CountryPickerObj));
            }
            else
            {
                Debug.WriteLine("Failed to retrieve country data from the API.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoadCountryDataAsync: {ex.Message}");
        }
    }

    private async Task ChangePictureAsync()
    {
        if (Application.Current != null)
            if (Application.Current.MainPage != null)
            {
                var action = await Application.Current.MainPage.DisplayActionSheet(
                    "Change Picture",
                    "Cancel",
                    null,
                    "Choose from Gallery",
                    "Take a Photo"
                );

                switch (action)
                {
                    case "Choose from Gallery":
                        var galleryStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();
                        if (galleryStatus != PermissionStatus.Granted)
                        {
                            galleryStatus = await Permissions.RequestAsync<Permissions.Photos>();
                            if (galleryStatus != PermissionStatus.Granted)
                            {
                                // Permission denied, alert the user and return
                                await Application.Current.MainPage.DisplayAlert("Permissions Denied",
                                    "Unable to access the photo gallery.", "OK");
                                return;
                            }
                        }

                        try
                        {
                            var result = await MediaPicker.PickPhotoAsync();
                            UserPictureUrl =
                                result.FullPath;
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Unable to access gallery.", "OK");
                            throw new Exception(ex.Message);
                        }

                        break;

                    case "Take a Photo":
                        var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                        if (cameraStatus != PermissionStatus.Granted)
                        {
                            cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                            if (cameraStatus != PermissionStatus.Granted)
                            {
                                // Permission denied, alert the user and return
                                await Application.Current.MainPage.DisplayAlert("Permissions Denied",
                                    "Unable to access the camera.", "OK");
                                return;
                            }
                        }

                        try
                        {
                            var photo = await MediaPicker.CapturePhotoAsync();
                            UserPictureUrl =
                                photo.FullPath; // assuming UserPictureUrl is a property that binds to your Image.Source
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Unable to access the camera.",
                                "OK");
                            throw new Exception(ex.Message);
                        }

                        break;
                }
            }
    }
}

