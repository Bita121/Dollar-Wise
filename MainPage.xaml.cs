using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class MainPage : ContentPage
    {
        public MainPage(string username, string selectedCurrency)
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel(username, selectedCurrency);
        }
    }

}
