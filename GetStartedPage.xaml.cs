using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Dollar_Wise
{
    public partial class UsernameInputPage : ContentPage
    {
        private bool isTextAnimated = false;

        public UsernameInputPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            

        }

        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            // Check if the text animation has already been performed
            if (!isTextAnimated)
            {
                await StartTextAnimation();
                isTextAnimated = true;
            }

            // Check if username and currency are stored
            
        }

        private async Task StartTextAnimation()
        {
            // Text animation
            string welcomeText = "Welcome to Dollar Wise!";
            foreach (char letter in welcomeText)
            {
                WelcomeLabel.Text += letter;
                await Task.Delay(100); // Adjust the delay as needed
            }

            // Fade in animation for image and label
            await Task.WhenAll(
                DollarImage.FadeTo(1, 1000),
                TapLabel.FadeTo(1, 1000)
            );
        }

        private async void NextButton_Click(object sender, EventArgs e)
        {
            // Navigate to the CurrencyInputPage
            
            await Navigation.PushAsync(new CurrencyInputPage());
            
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent back button functionality
            return true;
        }

    }
}
