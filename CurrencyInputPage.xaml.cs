
namespace Dollar_Wise
{
    public partial class CurrencyInputPage : ContentPage
    {
        private readonly string _username;

        public CurrencyInputPage(string username)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _username = username;
        }

        private async void FinishButton_Click(object sender, EventArgs e)
        {
            if (CurrencyPicker.SelectedIndex == -1)
            {
                ErrorLabel.Text = "Please select a currency.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Proceed with navigation
            string selectedCurrency = CurrencyPicker.SelectedItem?.ToString();
            await Navigation.PushAsync(new MainPage(_username, selectedCurrency));
        }

    }
}
