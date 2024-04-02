using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Dollar_Wise
{
    public partial class ChooseUsernameInputPage : ContentPage
    {
        private readonly string _selectedCurrency;
        private bool _isTextAnimated = false;

        public ChooseUsernameInputPage(string selectedCurrency)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _selectedCurrency = selectedCurrency;
        }
        private async void OpenEntryImage_Tapped(object sender, EventArgs e)
        {
            // Hide the entry image
            EntryImage.Opacity = 0;
            EntryImage.IsVisible = false;

            // Prompt the user to enter their username
            string username = await DisplayPromptAsync("Enter Username", "Please enter your username:");

            // Check if the username is provided
            if (!string.IsNullOrWhiteSpace(username))
            {
                EntryUsername.Text = username;

                // Show the entry and button
                EntryUsername.IsVisible = true;
                EntryUsername.Opacity = 1;
                ButtonNext.Opacity = 1;
            }
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check if the text animation has already been performed
            if (!_isTextAnimated)
            {
                await StartTextAnimation();
                _isTextAnimated = true;
            }
        }

        private async Task StartTextAnimation()
        {
            // Text animation for "Choose Username"
            string welcomeText = "Choose Your Username";
            foreach (char letter in welcomeText)
            {
                LabelChooseUsername.Text += letter;
                await Task.Delay(100); // Adjust the delay as needed
            }

            // Show the entry image after text animation completes
            EntryImage.IsVisible = true;

            // Fade in animation for entry image
            await Task.WhenAll(
                EntryImage.FadeTo(1, 1000),
                ButtonNext.FadeTo(1, 1000)
            );
        }

        private async void NextButton_Click(object sender, EventArgs e)
        {
            string username = EntryUsername.Text;

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                ErrorLabel.Text = "Please enter a username with at least 3 characters.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Proceed with navigation to MainPage with both username and selected currency
            await Navigation.PushAsync(new MainPage(username, _selectedCurrency));
        }
    }
}
