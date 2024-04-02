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
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check if the text animation has already been performed
            if (!isTextAnimated)
            {
                await StartTextAnimation();
                isTextAnimated = true;
            }
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
    }
}
