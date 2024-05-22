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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!isTextAnimated)
            {
                await StartTextAnimation();
                isTextAnimated = true;
            }
        }

        private async Task StartTextAnimation()
        {
            string welcomeText = "Select Your Currency";
            foreach (char letter in welcomeText)
            {
                LabelSelectCurrency.Text += letter;
                await Task.Delay(100);
            }
            pozabuton.IsVisible = true;
            NextButton.IsVisible = true;

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

            string selectedCurrency = CurrencyPicker.SelectedItem?.ToString();
            Preferences.Set("SelectedCurrency", selectedCurrency);
            await Navigation.PushAsync(new ChooseUsernameInputPage(selectedCurrency));
        }

        private async void OpenCurrencyPicker_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Select Currency", "Cancel", null, "RON", "USD", "EUR");
            if (result != "Cancel" && !string.IsNullOrEmpty(result))
            {
                CurrencyPicker.SelectedItem = result;
            }
        }

        private void CurrencyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            pozabuton.IsVisible = false;
            CurrencyPicker.IsVisible = true;
        }

    }
}
