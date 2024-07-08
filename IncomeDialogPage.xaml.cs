using System;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class IncomeDialogPage : ContentPage
    {
        private DataService _dataService;

        public IncomeDialogPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            CategoryPicker.ItemsSource = new List<string> { "Salary", "Freelance", "Investment", "Gift", "Other" };
        }

        private async void SaveIncome_Clicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var amount = AmountEntry.Text;
            var date = DatePicker.Date;
            var category = CategoryPicker.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Income name must be at least 3 characters long.", "OK");
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
            var income = new Income
            {
                Name = name,
                Amount = amountValue,
                Date = date,
                Category = category
            };

            await _dataService.AddIncome(income);
            await Navigation.PopAsync();
        }
    }
}
