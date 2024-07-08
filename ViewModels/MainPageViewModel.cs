using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Graphics;

namespace Dollar_Wise
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public string Username { get; }
        public string SelectedCurrency { get; }

        private DataService _dataService;

        private Color _expenseButtonColor;
        public Color ExpenseButtonColor
        {
            get { return _expenseButtonColor; }
            set { SetProperty(ref _expenseButtonColor, value); }
        }

        private Color _incomeButtonColor;
        public Color IncomeButtonColor
        {
            get { return _incomeButtonColor; }
            set { SetProperty(ref _incomeButtonColor, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            backingStore = value;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel(string username, string selectedCurrency)
        {
            Username = username;
            SelectedCurrency = selectedCurrency;
            _dataService = new DataService(new DatabaseService());

            ExpenseButtonColor = Color.FromArgb("#2196F3");
            IncomeButtonColor = Color.FromArgb("#A9A9A9");
        }

        public async Task UpdatePieChartWithExpensesAsync(Action<List<ChartEntry>> updateChart)
        {
            try
            {
                var categoryTotals = await CalculateExpenseCategoryTotalsAsync();
                var totalExpense = categoryTotals.Sum(kvp => kvp.Value);

                var entries = new List<ChartEntry>();
                foreach (var kvp in categoryTotals)
                {
                    double percentage = (double)(kvp.Value / totalExpense) * 100;
                    entries.Add(new ChartEntry((float)percentage)
                    {
                        Label = kvp.Key,
                        ValueLabel = percentage.ToString("0.##") + "%",
                        Color = SKColor.Parse(GetExpenseCategoryColor(kvp.Key))
                    });
                }

                updateChart(entries);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ExpenseButtonColor = Color.FromArgb("#2196F3");
            IncomeButtonColor = Color.FromArgb("#A9A9A9");
        }

        public async Task UpdatePieChartWithIncomesAsync(Action<List<ChartEntry>> updateChart)
        {
            try
            {
                var categoryTotals = await CalculateIncomeCategoryTotalsAsync();
                var totalIncome = categoryTotals.Sum(kvp => kvp.Value);

                var entries = new List<ChartEntry>();
                foreach (var kvp in categoryTotals)
                {
                    double percentage = (double)(kvp.Value / totalIncome) * 100;
                    entries.Add(new ChartEntry((float)percentage)
                    {
                        Label = kvp.Key,
                        ValueLabel = percentage.ToString("0.##") + "%",
                        Color = SKColor.Parse(GetIncomeCategoryColor(kvp.Key))
                    });
                }

                updateChart(entries);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            IncomeButtonColor = Color.FromArgb("#2196F3");
            ExpenseButtonColor = Color.FromArgb("#A9A9A9");
        }

        private async Task<Dictionary<string, decimal>> CalculateExpenseCategoryTotalsAsync()
        {
            var expenses = await GetExpensesAsync();
            var categoryTotals = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            return categoryTotals;
        }

        private async Task<Dictionary<string, decimal>> CalculateIncomeCategoryTotalsAsync()
        {
            var incomes = await GetIncomesAsync();
            var categoryTotals = incomes
                .GroupBy(i => i.Category)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Amount));

            return categoryTotals;
        }

        private async Task<List<Expense>> GetExpensesAsync()
        {
            return await _dataService.GetExpenses();
        }

        private async Task<List<Income>> GetIncomesAsync()
        {
            return await _dataService.GetIncomes();
        }

        private string GetExpenseCategoryColor(string category)
        {
            var expenseColors = new Dictionary<string, string>
            {
                { "Food", "#FF6347" },
                { "Transportation", "#4682B4" },
                { "Entertainment", "#9370DB" },
                { "Utilities", "#FFD700" },
                { "Investments", "#00FA9A" },
                { "Other", "#8A2BE2" }
            };

            return expenseColors.TryGetValue(category, out var color) ? color : "#808080";
        }

        private string GetIncomeCategoryColor(string category)
        {
            var incomeColors = new Dictionary<string, string>
            {
                { "Salary", "#00FF7F" },
                { "Freelance", "#1E90FF" },
                { "Investment", "#8A2BE2" },
                { "Gift", "#FF69B4" },
                { "Other", "#FFD700" }
            };

            return incomeColors.TryGetValue(category, out var color) ? color : "#808080"; //default color if category not found
        }
    }
}
