using System;
using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Microsoft.Maui.Controls;

namespace Dollar_Wise.Goals
{
    public partial class GoalDialogPageEdit : ContentPage
    {
        private Goal _goalToUpdate;
        private DataService _dataService;

        public GoalDialogPageEdit(Goal goalToUpdate)
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            PriorityPicker.ItemsSource = new[] { "High", "Medium", "Low" };
            _goalToUpdate = goalToUpdate;

            // Prefill with details before editing
            if (_goalToUpdate != null)
            {
                NameEntry.Text = _goalToUpdate.GoalName;
                TargetAmountEntry.Text = _goalToUpdate.TargetAmount.ToString();
                DatePicker.Date = _goalToUpdate.TargetDate;
                PriorityPicker.SelectedItem = _goalToUpdate.Priority;
            }
        }

        private async void SaveGoal_Clicked(object sender, EventArgs e)
        {
            // Prefill with existing data
            var name = NameEntry.Text;
            var targetAmount = TargetAmountEntry.Text;
            var targetDate = DatePicker.Date;
            var priority = PriorityPicker.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                await DisplayAlert("Error", "Goal name must be at least 3 characters long.", "OK");
                return;
            }

            decimal amountValue;
            if (string.IsNullOrWhiteSpace(targetAmount) || !decimal.TryParse(targetAmount, out amountValue) || amountValue <= 0)
            {
                await DisplayAlert("Error", "Invalid target amount format or amount is negative.", "OK");
                return;
            }

            if (targetDate == default || targetDate <= DateTime.Today)
            {
                await DisplayAlert("Error", "Please select a valid target date in the future.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(priority))
            {
                await DisplayAlert("Error", "Please select a priority.", "OK");
                return;
            }

            _goalToUpdate.GoalName = name;
            _goalToUpdate.TargetAmount = amountValue;
            _goalToUpdate.TargetDate = targetDate;
            _goalToUpdate.Priority = priority;

            await _dataService.UpdateGoal(_goalToUpdate);
            await Navigation.PopAsync();
        }
    }
}