using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dollar_Wise.Models
{
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string GoalName { get; set; }

        public decimal TargetAmount { get; set; }

        private decimal _currentAmount;
        public decimal CurrentAmount
        {
            get { return _currentAmount; }
            // Remove the setter to prevent direct modification of CurrentAmount
        }

        public DateTime TargetDate { get; set; }

        public string Priority { get; set; }

        [Ignore]
        public string AmountFormatted
        {
            get
            {
                return GetFormattedAmount();
            }
        }

        [Ignore]
        public double Progress
        {
            get
            {
                return CalculateProgress();
            }
        }

        public Goal()
        {
            _currentAmount = 0;
        }

        private string GetFormattedAmount()
        {
            // get currenct based on set preferences
            string currencySymbol = GetCurrencySymbol(Preferences.Get("SelectedCurrency", "USD"));

            switch (Preferences.Get("SelectedCurrency", "USD"))
            {
                case "EUR":
                    return $"{TargetAmount} €";
                case "RON":
                    return $"{TargetAmount} RON";
                default: 
                    return $"${TargetAmount}";
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

        private double CalculateProgress()
        {
            if (TargetAmount == 0)
                return 0;

            return Math.Min(1, (double)(_currentAmount / TargetAmount));
        }

        // add money to goal
        public void AddMoney(decimal amount)
        {
            _currentAmount += amount;
        }
    }
}
