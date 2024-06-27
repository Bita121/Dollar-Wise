using System;
using System.Collections.Generic;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise.RecurringPayments
{
    public partial class RecurringPaymentDialogPage : ContentPage
    {
        private DataService _dataService;
        private string _selectedCategory;

        public RecurringPaymentDialogPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            FrequencyPicker.ItemsSource = Enum.GetValues(typeof(RecurrenceFrequency));

            GenerateCategoryGrid();
        }

        private void GenerateCategoryGrid()
        {
            // clear existing grid
            CategoryGrid.Children.Clear();

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

                // set the Grid.Row and Grid.Column properties to position the image
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
            var amountText = AmountEntry.Text;
            var startingDate = DatePicker.Date;
            var frequency = FrequencyPicker.SelectedItem as RecurrenceFrequency?;

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Recurring payment name must be at least 3 characters long.", "OK");
                return;
            }

            decimal amountValue;
            if (string.IsNullOrWhiteSpace(amountText) || !decimal.TryParse(amountText, out amountValue))
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

            // create recurringPayment object
            var recurringPayment = new RecurringPayment
            {
                Name = name,
                Amount = amountValue,
                StartingDate = startingDate,
                Frequency = frequency.Value,
                Category = _selectedCategory
            };

            await _dataService.AddRecurringPayment(recurringPayment);

            await Navigation.PopAsync();
        }
    }
}
