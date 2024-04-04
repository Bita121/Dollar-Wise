using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class MainPage : ContentPage
    {

        public MainPage(string username, string selectedCurrency)
        {
            InitializeComponent();

            // Set the binding context to the view model
            BindingContext = new MainPageViewModel(username, selectedCurrency);
        }
        public MainPage()
        {
            InitializeComponent();
            string username = Preferences.Get("Username", null);
            string selectedCurrency = Preferences.Get("SelectedCurrency", null);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(selectedCurrency))
            {
                // Navigate to the UsernameInputPage if either username or selected currency is null
                Navigation.PushAsync(new UsernameInputPage());
            }
            else
            {
                // Set the binding context to the view model
                BindingContext = new MainPageViewModel(username, selectedCurrency);
            }

        }

    }
}
