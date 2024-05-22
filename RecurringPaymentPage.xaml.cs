using Dollar_Wise.Models;
using Dollar_Wise.Services;
using Dollar_Wise.RecurringPayments;
using Microsoft.Maui.Controls;
using System;

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
    }
}
