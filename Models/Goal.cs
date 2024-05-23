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

        private decimal _currentAmount;
        public decimal CurrentAmount
        {
            get { return _currentAmount; }
            private set
            {
                if (_currentAmount != value)
                {
                    _currentAmount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentAmount));
                    OnPropertyChanged(nameof(AmountFormatted));
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

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

        public Goal()
        {
            _currentAmount = 0;
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

            return Math.Min(1, (double)(_currentAmount / TargetAmount));
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
