using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollar_Wise.Models
{
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }

        [Ignore]
        public string AmountFormatted
        {
            get
            {
                return GetFormattedAmount();
            }
        }

        private string GetFormattedAmount()
        {
            // Get the currency symbol based on the selected currency
            string currencySymbol = GetCurrencySymbol(Preferences.Get("SelectedCurrency", "USD"));

            // Format the amount with the currency symbol
            switch (Preferences.Get("SelectedCurrency", "USD"))
            {
                case "EUR":
                    return $"{Amount} €";
                case "RON":
                    return $"{Amount} RON";
                default: // USD or other default to $
                    return $"${Amount}";
            }
        }

        private string GetCurrencySymbol(string currency)
        {
            switch (currency)
            {
                case "EUR":
                    return "€";
                case "RON":
                    return "RON";
                default: // USD or other default to $
                    return "$";
            }
        }
    }
}
