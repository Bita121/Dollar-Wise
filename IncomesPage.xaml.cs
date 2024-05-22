using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class IncomesPage : ContentPage, INotifyPropertyChanged
    {
        private DataService _dataService;
        private List<Income> _allIncomes;
        private bool _isDateFilterVisible = false;
        private bool _isCategoryFilterVisible = false;

        public IncomesPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            LoadIncomes();
            FilterPicker.ItemsSource = new List<string> { "Date", "Category" };
            CategoryPicker.ItemsSource = new List<string> { "Salary", "Freelance", "Investment", "Gift", "Other" };
        }

        private async void LoadIncomes()
        {
            _allIncomes = await _dataService.GetIncomes();
            if (_isCategoryFilterVisible && CategoryPicker.SelectedItem is string selectedCategory)
            {
                _allIncomes = _allIncomes.Where(income => income.Category == selectedCategory).ToList();
            }
            if (_isDateFilterVisible)
            {
                DateTime startDate = StartDatePicker.Date;
                DateTime endDate = EndDatePicker.Date;
                _allIncomes = _allIncomes.Where(income => income.Date >= startDate && income.Date <= endDate).ToList();
            }
            IncomesListView.ItemsSource = _allIncomes;
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
            LoadIncomes();
        }

        private async void ApplyFilter_Clicked(object sender, EventArgs e)
        {
            string selectedCategory = CategoryPicker.SelectedItem as string;
            DateTime startDate = StartDatePicker.Date;
            DateTime endDate = EndDatePicker.Date;

            string previousSelectedCategory = selectedCategory;
            DateTime previousStartDate = startDate;
            DateTime previousEndDate = endDate;

            IEnumerable<Income> filteredIncomes = _allIncomes;
            if (_isCategoryFilterVisible && selectedCategory != null)
            {
                filteredIncomes = filteredIncomes.Where(income => income.Category == selectedCategory);
            }
            if (_isDateFilterVisible)
            {
                filteredIncomes = filteredIncomes.Where(income => income.Date >= startDate && income.Date <= endDate);
            }

            IncomesListView.ItemsSource = filteredIncomes;

            // store applied filter parameters for reapplication after editing or deleting
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
            LoadIncomes();
        }

        private async void AddIncome_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncomeDialogPage());
            LoadIncomes();
        }

        private async void EditIncome_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var income = button?.BindingContext as Income;
            if (income != null)
            {
                await Navigation.PushAsync(new IncomeDialogPageEdit(income));
                LoadIncomes();
                if (ApplyFilterButton.CommandParameter is (string selectedCategory, DateTime startDate, DateTime endDate))
                {
                    CategoryPicker.SelectedItem = selectedCategory;
                    StartDatePicker.Date = startDate;
                    EndDatePicker.Date = endDate;
                    ApplyFilter_Clicked(null, null);
                }
            }
        }

        private async void DeleteIncome_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var income = button?.BindingContext as Income;

            if (income != null)
            {
                var result = await DisplayAlert("Delete Income", $"Are you sure you want to delete '{income.Name}'?", "Yes", "No");
                if (result)
                {
                    await _dataService.DeleteIncome(income);
                    LoadIncomes();
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
