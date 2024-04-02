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

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            // Handle menu button click
        }
    }
}
