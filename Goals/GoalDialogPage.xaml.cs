
using Dollar_Wise.Models;
using Dollar_Wise.Services;


namespace Dollar_Wise.Goals;

public partial class GoalDialogPage : ContentPage
{
    private DataService _dataService;

    public GoalDialogPage()
    {
        InitializeComponent();
        _dataService = new DataService(App.Database);

        PriorityPicker.ItemsSource = new[] { "High", "Medium", "Low" };
    }

    private async void SaveGoal_Clicked(object sender, EventArgs e)
    {
        //prefill data
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

        var goal = new Goal
        {
            GoalName = name,
            TargetAmount = amountValue,
            TargetDate = targetDate,
            Priority = priority
        };

        await _dataService.AddGoal(goal);
        await Navigation.PopAsync();
    }
}
