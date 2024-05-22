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

            // sort goals by priority: High, Medium, Low
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
                // Handle adding money to the selected goal here
                // Example: Show a dialog to enter the amount to add and update the goal
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reload goals when the page is shown again
            LoadGoals();
        }
        private async void DeleteGoal_Clicked(object sender, EventArgs e)
        {
            // Get the selected goal from the ListView
            var button = sender as Button;
            var goal = button?.BindingContext as Goal;

            if (goal != null)
            {
                // Confirm deletion with an alert dialog
                var result = await DisplayAlert("Delete Goal", $"Are you sure you want to delete '{goal.GoalName}'?", "Yes", "No");
                if (result)
                {
                    // Delete the goal from the database
                    await _dataService.DeleteGoal(goal);
                    LoadGoals();
                }
            }
        }
    }
}
