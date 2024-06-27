using System.ComponentModel;
using Dollar_Wise.Models;
using Dollar_Wise.Services;

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
            try
            {
                _allExpenses = await _dataService.GetExpenses();

                // Apply filters if they were previously selected
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await DisplayAlert("Error", "Failed to load expenses. Please try again later.", "OK");
            }
        }

        private void FilterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterPicker.SelectedItem is string selectedFilter)
            {
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

        private void ApplyFilter_Clicked(object sender, EventArgs e)
        {
            try
            {
                string selectedCategory = CategoryPicker.SelectedItem as string;
                DateTime startDate = StartDatePicker.Date;
                DateTime endDate = EndDatePicker.Date;

                string previousSelectedCategory = selectedCategory;
                DateTime previousStartDate = startDate;
                DateTime previousEndDate = endDate;

                IEnumerable<Expense> filteredExpenses = _allExpenses;
                if (_isCategoryFilterVisible && !string.IsNullOrEmpty(selectedCategory))
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                DisplayAlert("Error", "Failed to apply filter. Please try again later.", "OK");
            }
        }

        private void ResetFilter_Clicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                DisplayAlert("Error", "Failed to reset filter. Please try again later.", "OK");
            }
        }

        private async void AddExpense_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ExpenseDialogPage());
                LoadExpenses();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await DisplayAlert("Error", "Failed to add expense. Please try again later.", "OK");
            }
        }

        private async void EditExpense_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var expense = button?.BindingContext as Expense;
                if (expense != null)
                {
                    await Navigation.PushAsync(new ExpenseDialogPageEdit(expense));
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await DisplayAlert("Error", "Failed to edit expense. Please try again later.", "OK");
            }
        }

        private async void DeleteExpense_Clicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await DisplayAlert("Error", "Failed to delete expense. Please try again later.", "OK");
            }
        }
    }
}
