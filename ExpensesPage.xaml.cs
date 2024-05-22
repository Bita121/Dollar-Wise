using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class ExpensesPage : ContentPage, INotifyPropertyChanged
    {
        private DataService _dataService;
        private List<Expense> _allExpenses;
        private bool _isDateFilterVisible = false;
        private bool _isCategoryFilterVisible = false;

        public ExpensesPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            LoadExpenses();
            FilterPicker.ItemsSource = new List<string> { "Date", "Category" };
            CategoryPicker.ItemsSource = new List<string> { "Food", "Transportation", "Entertainment", "Utilities", "Investments", "Other" };
        }

        private async void LoadExpenses()
        {
            _allExpenses = await _dataService.GetExpenses();

            // apply filters if they were previously selected
            if (_isCategoryFilterVisible && CategoryPicker.SelectedItem is string selectedCategory)
            {
                _allExpenses = _allExpenses.Where(expense => expense.Category == selectedCategory).ToList();
            }
            if (_isDateFilterVisible)
            {
                DateTime startDate = StartDatePicker.Date;
                DateTime endDate = EndDatePicker.Date;
                _allExpenses = _allExpenses.Where(expense => expense.Date >= startDate && expense.Date <= endDate).ToList();
            }
            ExpensesListView.ItemsSource = _allExpenses;
        }

        private async void FilterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = FilterPicker.SelectedItem as string;
            if (selectedFilter == "Date")
            {
                _isDateFilterVisible = true;
                _isCategoryFilterVisible = false;
            }
            else if (selectedFilter == "Category")
            {
                _isDateFilterVisible = false;
                _isCategoryFilterVisible = true;
            }
            else
            {
                _isDateFilterVisible = false;
                _isCategoryFilterVisible = false;
            }

            UpdateFilterVisibility();
        }

        private void UpdateFilterVisibility()
        {
            DateFilterLayout.IsVisible = _isDateFilterVisible;
            CategoryPicker.IsVisible = _isCategoryFilterVisible;
            ApplyFilterButton.IsVisible = _isDateFilterVisible || _isCategoryFilterVisible;
            ResetFilterButton.IsVisible = _isDateFilterVisible || _isCategoryFilterVisible;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadExpenses();
        }

        private async void ApplyFilter_Clicked(object sender, EventArgs e)
        {
            string selectedCategory = CategoryPicker.SelectedItem as string;
            DateTime startDate = StartDatePicker.Date;
            DateTime endDate = EndDatePicker.Date;

            string previousSelectedCategory = selectedCategory;
            DateTime previousStartDate = startDate;
            DateTime previousEndDate = endDate;

            IEnumerable<Expense> filteredExpenses = _allExpenses;
            if (_isCategoryFilterVisible && selectedCategory != null)
            {
                filteredExpenses = filteredExpenses.Where(expense => expense.Category == selectedCategory);
            }
            if (_isDateFilterVisible)
            {
                filteredExpenses = filteredExpenses.Where(expense => expense.Date >= startDate && expense.Date <= endDate);
            }

            ExpensesListView.ItemsSource = filteredExpenses;
            ApplyFilterButton.CommandParameter = (previousSelectedCategory, previousStartDate, previousEndDate);
        }

        private async void ResetFilter_Clicked(object sender, EventArgs e)
        {
            FilterPicker.SelectedItem = null;
            CategoryPicker.SelectedItem = null;
            StartDatePicker.Date = DateTime.Today;
            EndDatePicker.Date = DateTime.Today;
            _isDateFilterVisible = false;
            _isCategoryFilterVisible = false;
            UpdateFilterVisibility();
            LoadExpenses();
        }

        private async void AddExpense_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExpenseDialogPage());
            LoadExpenses();
        }

        private async void EditExpense_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expense = button?.BindingContext as Expense;
            if (expense != null)
            {
                await Navigation.PushAsync(new ExpenseDialogPageEdit(expense));
                LoadExpenses();

                // reapply filters if they were previously applied
                if (ApplyFilterButton.CommandParameter is (string selectedCategory, DateTime startDate, DateTime endDate))
                {
                    CategoryPicker.SelectedItem = selectedCategory;
                    StartDatePicker.Date = startDate;
                    EndDatePicker.Date = endDate;
                    ApplyFilter_Clicked(null, null);
                }
            }
        }

        private async void DeleteExpense_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expense = button?.BindingContext as Expense;

            if (expense != null)
            {
                var result = await DisplayAlert("Delete Expense", $"Are you sure you want to delete '{expense.Name}'?", "Yes", "No");
                if (result)
                {
                    await _dataService.DeleteExpense(expense);
                    LoadExpenses();

                    // Reapply filters if they were previously applied
                    if (ApplyFilterButton.CommandParameter is (string selectedCategory, DateTime startDate, DateTime endDate))
                    {
                        CategoryPicker.SelectedItem = selectedCategory;
                        StartDatePicker.Date = startDate;
                        EndDatePicker.Date = endDate;
                        ApplyFilter_Clicked(null, null);
                    }
                }
            }
        }
    }
}
