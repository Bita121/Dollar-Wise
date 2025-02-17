using System;
using System.Collections.Generic;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class ExpenseDialogPage : ContentPage
    {
        private DataService _dataService;

        public ExpenseDialogPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            CategoryPicker.ItemsSource = new List<string> { "Food", "Transportation", "Entertainment", "Utilities", "Investments", "Other" };
        }

        private async void SaveExpense_Clicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var amount = AmountEntry.Text;
            var date = DatePicker.Date;
            var category = CategoryPicker.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Expense name must be at least 3 characters long.", "OK");
                return;
            }

            decimal amountValue;
            if (string.IsNullOrWhiteSpace(amount) || !decimal.TryParse(amount, out amountValue) || amountValue <= 0)
            {
                await DisplayAlert("Error", "Invalid amount format or amount is negative.", "OK");
                return;
            }

            if (date == default)
            {
                await DisplayAlert("Error", "Please select a valid date.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(category))
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            var expense = new Expense
            {
                Name = name,
                Amount = amountValue,
                Date = date,
                Category = category
            };
            await _dataService.AddExpense(expense);
            await Navigation.PopAsync();
        }
    }
}
