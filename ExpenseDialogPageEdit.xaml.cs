using System;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class ExpenseDialogPageEdit : ContentPage
    {
        private Expense _expenseToUpdate;
        private DataService _dataService;


        public ExpenseDialogPageEdit(Expense expenseToUpdate)
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            CategoryPicker.ItemsSource = new List<string> { "Food", "Transportation", "Entertainment", "Utilities", "Investments", "Other" };
            _expenseToUpdate = expenseToUpdate;

            // Populate fields with expense details for editing
            if (_expenseToUpdate != null)
            {
                // Populate fields with expense details for editing
                NameEntry.Text = _expenseToUpdate.Name;
                AmountEntry.Text = _expenseToUpdate.Amount.ToString();
                DatePicker.Date = _expenseToUpdate.Date;
                CategoryPicker.SelectedItem = _expenseToUpdate.Category;
            }
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
            // Update the properties of the existing expense
            _expenseToUpdate.Name = name;
            _expenseToUpdate.Amount = amountValue;
            _expenseToUpdate.Date = date;
            _expenseToUpdate.Category = category;

            // Save the updated expense to the database
            await _dataService.UpdateExpense(_expenseToUpdate);
            // Navigate back to the previous page
            await Navigation.PopAsync();
        }

    }
}
