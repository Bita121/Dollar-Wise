using SQLite;
using System;

namespace Dollar_Wise.Models
{
    public class Expense
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
            // get currency based on set preferences
            string currencySymbol = GetCurrencySymbol(Preferences.Get("SelectedCurrency", "USD"));


            switch (Preferences.Get("SelectedCurrency", "USD"))
            {
                case "EUR":
                    return $"{Amount} €";
                case "RON":
                    return $"{Amount} RON";
                default: 
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
                default: 
                    return "$";
            }
        }
    }
}
