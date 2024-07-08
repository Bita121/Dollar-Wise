using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollar_Wise.Models
{
    public class RecurringPayment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartingDate { get; set; }
        public RecurrenceFrequency Frequency { get; set; }
        public string Category { get; set; }
        public DateTime? LastExpenseDate { get; set; }   
        public bool IsActive { get; set; } 

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


    public enum RecurrenceFrequency
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        SemiAnnually,
        Annually
    }
}
