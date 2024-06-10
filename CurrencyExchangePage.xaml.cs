using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Maui.Controls;
using Dollar_Wise.Services;

namespace Dollar_Wise
{
    public partial class CurrencyExchangePage : ContentPage
    {
        private const string ExchangeRateApiUrl = "https://www.bnr.ro/nbrfxrates.xml";
        private Dictionary<string, decimal> _exchangeRates;
        private Dictionary<string, string> _currencyToCountry;
        private Dictionary<string, string> _countryFlags;
        private const string BaseCurrency = "RON";

        public CurrencyExchangePage()
        {
            InitializeComponent();
            LoadCurrencies();
        }

        private async void LoadCurrencies()
        {
            _exchangeRates = await FetchExchangeRates();
            _currencyToCountry = await FetchCurrencyToCountryMapping();
            _countryFlags = await FetchCountryFlags();
            var currencies = _exchangeRates.Keys.ToList();
            
            FromCurrencyPicker.ItemsSource = currencies;
            ToCurrencyPicker.ItemsSource = currencies;
        }

        private async Task<Dictionary<string, decimal>> FetchExchangeRates()
        {
            Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>
            {
                { BaseCurrency, 1 } // Base currency rate is 1
            };

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(ExchangeRateApiUrl);
                var xml = XDocument.Parse(response);

                var rates = xml.Descendants()
                               .Where(node => node.Name.LocalName == "Rate")
                               .Select(rate => new
                               {
                                   Currency = rate.Attribute("currency").Value,
                                   Value = decimal.Parse(rate.Value),
                                   Multiplier = rate.Attribute("multiplier") != null ? int.Parse(rate.Attribute("multiplier").Value) : 1
                               });

                foreach (var rate in rates)
                {
                    exchangeRates[rate.Currency] = rate.Value / rate.Multiplier;
                }
            }
            return exchangeRates;
        }

        private async Task<Dictionary<string, string>> FetchCurrencyToCountryMapping()
        {
            return new Dictionary<string, string>
            {
                { "USD", "us" }, { "EUR", "eu" }, { "GBP", "gb" }, { "RON", "ro" },
                // Add other mappings as needed
            };
        }

        private async Task<Dictionary<string, string>> FetchCountryFlags()
        {
            CountryFlagService flagService = new CountryFlagService();
            return await flagService.FetchCountryFlags();
        }

        private async void OnConvertClicked(object sender, EventArgs e)
        {
            if (FromCurrencyPicker.SelectedItem == null || ToCurrencyPicker.SelectedItem == null || string.IsNullOrEmpty(AmountEntry.Text))
            {
                await DisplayAlert("Error", "Please select both currencies and enter an amount.", "OK");
                return;
            }

            string fromCurrency = FromCurrencyPicker.SelectedItem.ToString();
            string toCurrency = ToCurrencyPicker.SelectedItem.ToString();
            if (decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                decimal result = ConvertCurrency(fromCurrency, toCurrency, amount);
                ResultLabel.Text = $"{amount} {fromCurrency} = {result:F2} {toCurrency}";
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK");
            }
        }

        private decimal ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            if (!_exchangeRates.ContainsKey(fromCurrency) || !_exchangeRates.ContainsKey(toCurrency))
            {
                return 0;
            }

            decimal fromRate = _exchangeRates[fromCurrency];
            decimal toRate = _exchangeRates[toCurrency];

            // Convert amount to RON first
            decimal amountInRON = amount * fromRate;

            // Convert from RON to target currency
            return amountInRON / toRate;
        }

        private void FromCurrencyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FromCurrencyPicker.SelectedItem != null && _currencyToCountry != null && _countryFlags != null)
            {
                string selectedCurrency = FromCurrencyPicker.SelectedItem.ToString();
                if (_currencyToCountry.ContainsKey(selectedCurrency) && _countryFlags.ContainsKey(_currencyToCountry[selectedCurrency]))
                {
                    FromCurrencyFlag.Source = _countryFlags[_currencyToCountry[selectedCurrency]];
                }
                else
                {
                    FromCurrencyFlag.Source = null;
                }
            }
        }

        private void ToCurrencyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ToCurrencyPicker.SelectedItem != null && _currencyToCountry != null && _countryFlags != null)
            {
                string selectedCurrency = ToCurrencyPicker.SelectedItem.ToString();
                if (_currencyToCountry.ContainsKey(selectedCurrency) && _countryFlags.ContainsKey(_currencyToCountry[selectedCurrency]))
                {
                    ToCurrencyFlag.Source = _countryFlags[_currencyToCountry[selectedCurrency]];
                }
                else
                {
                    ToCurrencyFlag.Source = null;
                }
            }
        }
    }
}
