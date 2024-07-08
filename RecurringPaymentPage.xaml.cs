using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Dollar_Wise.RecurringPayments;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Dollar_Wise
{
    public partial class RecurringPaymentPage : ContentPage
    {
        private DataService _dataService;

        public RecurringPaymentPage()
        {
            InitializeComponent();
            _dataService = new DataService(App.Database);
            LoadRecurringPayments();
        }

        private async void LoadRecurringPayments()
        {
            var recurringPayments = await _dataService.GetRecurringPayments();
            RecurringPaymentsListView.ItemsSource = recurringPayments;

            //check for and create any needed expenses
            foreach (var recurringPayment in recurringPayments)
            {
                await CheckAndCreateExpense(recurringPayment);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRecurringPayments();
        }

        private async void AddRecurringPayment_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecurringPaymentDialogPage());
            LoadRecurringPayments();
        }

        private async void EditRecurringPayment_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var recurringPayment = button?.BindingContext as RecurringPayment;
            if (recurringPayment != null)
            {
                await Navigation.PushAsync(new RecurringPaymentDialogPageEdit(recurringPayment));
                LoadRecurringPayments();
            }
        }

        private async void DeleteRecurringPayment_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var recurringPayment = button?.BindingContext as RecurringPayment;

            if (recurringPayment != null)
            {
                var result = await DisplayAlert("Delete Recurring Payment", $"Are you sure you want to delete '{recurringPayment.Name}'?", "Yes", "No");
                if (result)
                {
                    await _dataService.DeleteRecurringPayment(recurringPayment);
                    LoadRecurringPayments();
                }
            }
        }

        private async Task CheckAndCreateExpense(RecurringPayment recurringPayment)
        {
            DateTime today = DateTime.Today;

            if (!recurringPayment.LastExpenseDate.HasValue || recurringPayment.LastExpenseDate.Value < today)
            {
                DateTime nextExpenseDate = recurringPayment.LastExpenseDate.HasValue
                    ? GetNextExpenseDate(recurringPayment.LastExpenseDate.Value, recurringPayment.Frequency)
                    : recurringPayment.StartingDate;

                while (nextExpenseDate <= today)
                {
                    var expense = new Expense
                    {
                        Name = recurringPayment.Name,
                        Amount = recurringPayment.Amount,
                        Date = nextExpenseDate,
                        Category = recurringPayment.Category
                    };

                    await _dataService.AddExpense(expense);

                    recurringPayment.LastExpenseDate = nextExpenseDate;
                    nextExpenseDate = GetNextExpenseDate(nextExpenseDate, recurringPayment.Frequency);
                }

                await _dataService.UpdateRecurringPayment(recurringPayment);
            }
        }

        private DateTime GetNextExpenseDate(DateTime lastExpenseDate, RecurrenceFrequency frequency)
        {
            return frequency switch
            {
                RecurrenceFrequency.Daily => lastExpenseDate.AddDays(1),
                RecurrenceFrequency.Weekly => lastExpenseDate.AddDays(7),
                RecurrenceFrequency.Monthly => lastExpenseDate.AddMonths(1),
                RecurrenceFrequency.Quarterly => lastExpenseDate.AddMonths(3),
                RecurrenceFrequency.SemiAnnually => lastExpenseDate.AddMonths(6),
                RecurrenceFrequency.Annually => lastExpenseDate.AddYears(1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
