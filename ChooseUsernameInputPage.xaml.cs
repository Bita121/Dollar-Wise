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

        public ChooseUsernameInputPage()
        {
            InitializeComponent();
        }
        private async void OpenEntryImage_Tapped(object sender, EventArgs e)
        {
            EntryImage.Opacity = 0;
            EntryImage.IsVisible = false;

            string username = await DisplayPromptAsync("Enter Username", "Please enter your username:");

            if (!string.IsNullOrWhiteSpace(username))
            {
                EntryUsername.Text = username;
                EntryUsername.IsVisible = true;
                EntryUsername.Opacity = 1;
                ButtonNext.Opacity = 1;
            }
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_isTextAnimated)
            {
                await StartTextAnimation();
                _isTextAnimated = true;
            }
        }

        private async Task StartTextAnimation()
        {
            // animation for text
            string welcomeText = "Choose Your Username";
            foreach (char letter in welcomeText)
            {
                LabelChooseUsername.Text += letter;
                await Task.Delay(100);
            }

   
            EntryImage.IsVisible = true;

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
            Preferences.Set("Username", username);

            await Shell.Current.GoToAsync("//MainPage");
        }

    }
}
