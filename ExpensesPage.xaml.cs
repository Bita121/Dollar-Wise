using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class ExpensesPage : ContentPage, INotifyPropertyChanged
    {
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
            await Navigation.PushAsync(new ExpenseDialogPage());
            LoadExpenses();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Reload expenses when the ExpensesPage is shown again
            LoadExpenses();
        }

        private async void EditExpense_Clicked(object sender, EventArgs e)
        {
            // Get the selected expense from the ListView
            var button = sender as Button;
            var expense = button?.BindingContext as Expense;
            if (expense != null)
            {
                await Navigation.PushAsync(new ExpenseDialogPageEdit(expense));
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
