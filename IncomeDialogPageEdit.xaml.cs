using System;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class IncomeDialogPageEdit : ContentPage
    {
        private Income _incomeToUpdate;
        private DataService _dataService;


        public IncomeDialogPageEdit(Income incomeToUpdate)
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            CategoryPicker.ItemsSource = new List<string> { "Salary", "Freelance", "Investment", "Gift", "Other" };
            _incomeToUpdate = incomeToUpdate;

            // Populate fields with income details for editing
            if (_incomeToUpdate != null)
            {
                // Populate fields with income details for editing
                NameEntry.Text = _incomeToUpdate.Name;
                AmountEntry.Text = _incomeToUpdate.Amount.ToString();
                DatePicker.Date = _incomeToUpdate.Date;
                CategoryPicker.SelectedItem = _incomeToUpdate.Category;
            }
        }

        private async void SaveIncome_Clicked(object sender, EventArgs e)
        {
            // Retrieve user inputs from the page
            var name = NameEntry.Text;
            var amount = AmountEntry.Text;
            var date = DatePicker.Date;
            var category = CategoryPicker.SelectedItem as string;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Income name must be at least 3 characters long.", "OK");
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

            // Update Income object
            _incomeToUpdate.Name = name;
            _incomeToUpdate.Amount = amountValue;
            _incomeToUpdate.Date = date;
            _incomeToUpdate.Category = category;

            // Update the income in the database
            await _dataService.UpdateIncome(_incomeToUpdate);

            // Navigate back to the previous page
            await Navigation.PopAsync();
        }
    }
}
