using System;
using System.Collections.Generic;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise.RecurringPayments
{
    public partial class RecurringPaymentDialogPageEdit : ContentPage
    {
        private RecurringPayment _recurringPaymentToUpdate;
        private DataService _dataService;
        private string _selectedCategory;

        public RecurringPaymentDialogPageEdit(RecurringPayment recurringPaymentToUpdate)
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            FrequencyPicker.ItemsSource = Enum.GetValues(typeof(RecurrenceFrequency));
            _recurringPaymentToUpdate = recurringPaymentToUpdate;

            if (_recurringPaymentToUpdate != null)
            {
                NameEntry.Text = _recurringPaymentToUpdate.Name;
                AmountEntry.Text = _recurringPaymentToUpdate.Amount.ToString();
                DatePicker.Date = _recurringPaymentToUpdate.StartingDate;
                FrequencyPicker.SelectedItem = _recurringPaymentToUpdate.Frequency;
                _selectedCategory = _recurringPaymentToUpdate.Category;
            }

            GenerateCategoryGrid();
        }

        private void GenerateCategoryGrid()
        {
            // clear existing grid
            CategoryGrid.Children.Clear();

            // define categories with photos
            var categories = new Dictionary<string, string>
            {
                { "Food", "food_icon.png" },
                { "Transportation", "transportation_icon.png" },
                { "Entertainment", "entertainment_icon.png" },
                { "Utilities", "utilities_icon.png" },
                { "Investments", "investments_icon.png" },
                { "Other", "other_icon.png" }
            };

            // add category icons to the grid
            int column = 0;
            int row = 0;
            foreach (var category in categories)
            {
                var image = new Image
                {
                    Source = category.Value,
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    _selectedCategory = category.Key;
                };
                image.GestureRecognizers.Add(tapGestureRecognizer);

                CategoryGrid.Children.Add(image);

                Grid.SetRow(image, row);
                Grid.SetColumn(image, column);

                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
            }
        }

        private async void SaveRecurringPayment_Clicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var amount = AmountEntry.Text;
            var startingDate = DatePicker.Date;
            var frequency = FrequencyPicker.SelectedItem as RecurrenceFrequency?;

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Recurring payment name must be at least 3 characters long.", "OK");
                return;
            }

            decimal amountValue;
            if (string.IsNullOrWhiteSpace(amount) || !decimal.TryParse(amount, out amountValue))
            {
                await DisplayAlert("Error", "Invalid amount format.", "OK");
                return;
            }

            if (startingDate == default)
            {
                await DisplayAlert("Error", "Please select a valid starting date.", "OK");
                return;
            }

            if (frequency == null)
            {
                await DisplayAlert("Error", "Please select a frequency.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(_selectedCategory))
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            _recurringPaymentToUpdate.Name = name;
            _recurringPaymentToUpdate.Amount = amountValue;
            _recurringPaymentToUpdate.StartingDate = startingDate;
            _recurringPaymentToUpdate.Frequency = frequency.Value;
            _recurringPaymentToUpdate.Category = _selectedCategory;

            await _dataService.UpdateRecurringPayment(_recurringPaymentToUpdate);

            await Navigation.PopAsync();
        }
    }
}
