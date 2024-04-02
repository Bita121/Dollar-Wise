using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Dollar_Wise
{
    public partial class CurrencyInputPage : ContentPage
    {
        private bool isTextAnimated = false;

        public CurrencyInputPage()
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
            // Text animation for "Select Your Currency"
            string welcomeText = "Select Your Currency";
            foreach (char letter in welcomeText)
            {
                LabelSelectCurrency.Text += letter;
                await Task.Delay(100); // Adjust the delay as needed
            }

            // Show the image and button after text animation completes
            pozabuton.IsVisible = true;
            NextButton.IsVisible = true;

            // Fade in animation for image and button
            await Task.WhenAll(
                pozabuton.FadeTo(1, 1000),
                NextButton.FadeTo(1, 1000)
            );
        }

        private async void FinishButton_Click(object sender, EventArgs e)
        {
            if (CurrencyPicker.SelectedIndex == -1)
            {
                ErrorLabel.Text = "Please select a currency.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Proceed with navigation to ChooseUsernameInputPage with username
            string selectedCurrency = CurrencyPicker.SelectedItem?.ToString();
            await Navigation.PushAsync(new ChooseUsernameInputPage(selectedCurrency));
        }

        private async void OpenCurrencyPicker_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Select Currency", "Cancel", null, "RON", "USD", "EUR");
            if (result != "Cancel" && !string.IsNullOrEmpty(result))
            {
                // Update the picker value based on the selected currency
                CurrencyPicker.SelectedItem = result;
            }
        }

        private void CurrencyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide the image once a value is selected in the picker
            pozabuton.IsVisible = false;

            // Make the picker visible with the selected value
            CurrencyPicker.IsVisible = true;
        }
    }
}
