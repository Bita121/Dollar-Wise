
using Microsoft.Maui;

namespace Dollar_Wise
{
    public partial class UsernameInputPage : ContentPage
    {
        public UsernameInputPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void NextButton_Click(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                ErrorLabel.Text = "Please enter a username with at least 3 characters.";
                ErrorLabel.IsVisible = true;


                return;
            }

            // Navigate to the CurrencyInputPage
            await Navigation.PushAsync(new CurrencyInputPage(username));
        }
    }
}
