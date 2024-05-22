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
            if (!isTextAnimated)
            {
                await StartTextAnimation();
                isTextAnimated = true;
            }
            
        }

        private async Task StartTextAnimation()
        {
            // text animation
            string welcomeText = "Welcome to Dollar Wise!";
            foreach (char letter in welcomeText)
            {
                WelcomeLabel.Text += letter;
                await Task.Delay(100);
            }

            await Task.WhenAll(
                DollarImage.FadeTo(1, 1000),
                TapLabel.FadeTo(1, 1000)
            );
        }

        private async void NextButton_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CurrencyInputPage());
            
        }

        protected override bool OnBackButtonPressed()
        {
            // prevent back button functionality
            return true;
        }

    }
}
