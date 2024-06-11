using System;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load saved settings
            var username = Preferences.Get("Username", "User");
            // Set other settings if necessary
        }

        private async void OnChangeUsernameClicked(object sender, EventArgs e)
        {
            string newUsername = await DisplayPromptAsync("Change Username", "Enter new username:");
            if (!string.IsNullOrEmpty(newUsername))
            {
                Preferences.Set("Username", newUsername);
                await DisplayAlert("Username Changed", $"Username changed to {newUsername}", "OK");
            }
        }

        private async void OnChangeBaseCurrencyClicked(object sender, EventArgs e)
        {
            string[] currencies = { "RON", "USD", "EUR" };
            string newCurrency = await DisplayActionSheet("Change Base Currency", "Cancel", null, currencies);
            if (!string.IsNullOrEmpty(newCurrency) && newCurrency != "Cancel")
            {
                Preferences.Set("SelectedCurrency", newCurrency);
                await DisplayAlert("Base Currency Changed", $"Base currency changed to {newCurrency}", "OK");
            }
        }

        private async void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Settings Saved", "Your settings have been saved successfully. Please restart the app for the changes to take effect.", "OK");
            // Optionally, force a restart of the app
            Application.Current.Quit();
        }
    }
}
