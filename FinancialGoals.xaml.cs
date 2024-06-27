using System;
using System.Collections.Generic;
using System.Linq;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Dollar_Wise.Goals;
using Microsoft.Maui.Controls;

namespace Dollar_Wise
{
    public partial class FinancialGoals : ContentPage
    {
        private DataService _dataService;
        private List<Goal> _allGoals;

        public FinancialGoals()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            LoadGoals();
        }

        private async void LoadGoals()
        {
            _allGoals = await _dataService.GetGoals();

            var sortedGoals = _allGoals.OrderByDescending(g => g.Priority == "High")
                                       .ThenByDescending(g => g.Priority == "Medium")
                                       .ThenByDescending(g => g.Priority == "Low")
                                       .ToList();
            GoalsListView.ItemsSource = sortedGoals;
        }

        private async void AddGoal_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoalDialogPage());
            LoadGoals();
        }

        private async void AddMoney_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var goal = button?.BindingContext as Goal;

            if (goal != null)
            {
                string result = await DisplayPromptAsync("Add Money", "Enter the amount to add:", keyboard: Keyboard.Numeric);

                if (decimal.TryParse(result, out decimal amount))
                {
                    goal.AddMoney(amount);
                    await _dataService.UpdateGoal(goal);

                    // Update the specific goal in the list
                    var index = _allGoals.FindIndex(g => g.Id == goal.Id);
                    if (index >= 0)
                    {
                        _allGoals[index] = goal;
                        GoalsListView.ItemsSource = null;
                        GoalsListView.ItemsSource = _allGoals;
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid amount.", "OK");
                }
            }
        }


        private async void EditGoal_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var goal = button?.BindingContext as Goal;

            if (goal != null)
            {
                await Navigation.PushAsync(new GoalDialogPageEdit(goal));
                LoadGoals();
            }
        }

        protected override async void 
            
            
            OnAppearing()
        {
            base.OnAppearing();
            LoadGoals();
        }

        private async void DeleteGoal_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var goal = button?.BindingContext as Goal;

            if (goal != null)
            {
                var result = await DisplayAlert("Delete Goal", $"Are you sure you want to delete '{goal.GoalName}'?", "Yes", "No");
                if (result)
                {
                    await _dataService.DeleteGoal(goal);
                    LoadGoals();
                }
            }
        }
    }
}
