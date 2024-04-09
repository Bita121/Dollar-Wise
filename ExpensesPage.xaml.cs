using System;
using System.ComponentModel;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class ExpensesPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DataService _dataService;

        public ExpensesPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            LoadExpenses();
        }

        private async void LoadExpenses()
        {
            // Await the asynchronous operation to get expenses
            var expenses = await _dataService.GetExpenses();

            // Set the ItemsSource property with the result
            ExpensesListView.ItemsSource = expenses;
        }

        private async void AddExpense_Tapped(object sender, EventArgs e)
        {
            // Create a new expense object
            var expense = new Expense();

            // Display an alert dialog with input fields for adding expense details
            var name = await DisplayPromptAsync("Add Expense", "Enter expense name:", maxLength: 50);
            if (string.IsNullOrEmpty(name))
                return; // Cancelled

            expense.Name = name;

            var amount = await DisplayPromptAsync("Add Expense", "Enter expense amount:", "OK", "Cancel", keyboard: Keyboard.Numeric);
            if (string.IsNullOrEmpty(amount))
                return; // Cancelled

            if (!decimal.TryParse(amount, out decimal amountValue))
            {
                await DisplayAlert("Error", "Invalid amount format.", "OK");
                return;
            }

            expense.Amount = amountValue;

            var date = await DisplayPromptAsync("Add Expense", "Enter expense date (YYYY-MM-DD):", "OK", "Cancel");
            if (string.IsNullOrEmpty(date))
                return; // Cancelled

            if (!DateTime.TryParse(date, out DateTime dateValue))
            {
                await DisplayAlert("Error", "Invalid date format.", "OK");
                return;
            }

            expense.Date = dateValue;

            var category = await DisplayPromptAsync("Add Expense", "Enter expense category:", "OK", "Cancel", maxLength: 50);
            if (string.IsNullOrEmpty(category))
                return; // Cancelled

            expense.Category = category;

            // Save the expense to the database
            await _dataService.AddExpense(expense);

            // Refresh the expenses list
            LoadExpenses();
        }

        private async void EditExpense_Clicked(object sender, EventArgs e)
        {
            // Get the selected expense from the ListView
            var button = sender as Button;
            var expense = button?.BindingContext as Expense;

            if (expense != null)
            {
                // Display an alert dialog with input fields prefilled with expense details for editing
                var name = await DisplayPromptAsync("Edit Expense", "Enter expense name:", "OK", "Cancel", expense.Name, maxLength: 50);
                if (string.IsNullOrEmpty(name))
                    return; // Cancelled

                var amount = await DisplayPromptAsync("Edit Expense", "Enter expense amount:", "OK", "Cancel", expense.Amount.ToString(), keyboard: Keyboard.Numeric);
                if (string.IsNullOrEmpty(amount))
                    return; // Cancelled

                if (!decimal.TryParse(amount, out decimal amountValue))
                {
                    await DisplayAlert("Error", "Invalid amount format.", "OK");
                    return;
                }

                var date = await DisplayPromptAsync("Edit Expense", "Enter expense date (YYYY-MM-DD):", "OK", "Cancel", expense.Date.ToString("yyyy-MM-dd"));
                if (string.IsNullOrEmpty(date))
                    return; // Cancelled

                if (!DateTime.TryParse(date, out DateTime dateValue))
                {
                    await DisplayAlert("Error", "Invalid date format.", "OK");
                    return;
                }

                var category = await DisplayPromptAsync("Edit Expense", "Enter expense category:", "OK", "Cancel", expense.Category, maxLength: 50);
                if (string.IsNullOrEmpty(category))
                    return; // Cancelled

                // Update the expense with new details
                expense.Name = name;
                expense.Amount = amountValue;
                expense.Date = dateValue;
                expense.Category = category;

                // Save the updated expense to the database
                await _dataService.UpdateExpense(expense);

                // Refresh the expenses list
                LoadExpenses();
            }
        }

        private async void DeleteExpense_Clicked(object sender, EventArgs e)
        {
            // Get the selected expense from the ListView
            var button = sender as Button;
            var expense = button?.BindingContext as Expense;

            if (expense != null)
            {
                // Confirm deletion with an alert dialog
                var result = await DisplayAlert("Delete Expense", $"Are you sure you want to delete '{expense.Name}'?", "Yes", "No");
                if (result)
                {
                    // Delete the expense from the database
                    await _dataService.DeleteExpense(expense);

                    // Refresh the expenses list
                    LoadExpenses();
                }
            }
        }
    }
}
