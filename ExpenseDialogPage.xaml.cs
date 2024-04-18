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

            // Set the ItemsSource property of the Picker
            CategoryPicker.ItemsSource = new List<string> { "Food", "Transportation", "Entertainment", "Utilities", "Investments", "Other" };
        }

        private async void SaveExpense_Clicked(object sender, EventArgs e)
        {
            // Retrieve user inputs from the page
            var name = NameEntry.Text;
            var amount = AmountEntry.Text;
            var date = DatePicker.Date;
            var category = CategoryPicker.SelectedItem as string;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Expense name must be at least 3 characters long.", "OK");
                return;
            }

            decimal amountValue;
            if (string.IsNullOrWhiteSpace(amount) || !decimal.TryParse(amount, out amountValue))
            {
                await DisplayAlert("Error", "Invalid amount format.", "OK");
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

            // Create Expense object
            var expense = new Expense
            {
                Name = name,
                Amount = amountValue,
                Date = date,
                Category = category
            };

            // Save the expense to the database
            await _dataService.AddExpense(expense);
            // Navigate back to the previous page
            await Navigation.PopAsync();
        }
    }
}
