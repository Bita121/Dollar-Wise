using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dollar_Wise.Models
{
    public class Goal : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string GoalName { get; set; }

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public DateTime TargetDate { get; set; }

        public string Priority { get; set; }

        [Ignore]
        public string AmountFormatted
        {
            get
            {
                return $"{CurrentAmount}/{TargetAmount} {GetCurrencySymbol(Preferences.Get("SelectedCurrency", "USD"))}";
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

        private string GetCurrencySymbol(string currency)
        {
            return currency switch
            {
                "EUR" => "€",
                "RON" => "RON",
                _ => "$"
            };
        }

        public double CalculateProgress()
        {
            if (TargetAmount == 0)
                return 0;

            return Math.Min(1, (double)(CurrentAmount / TargetAmount));
        }

        public void AddMoney(decimal amount)
        {
            CurrentAmount += amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
