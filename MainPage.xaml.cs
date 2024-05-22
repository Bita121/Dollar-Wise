using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microcharts;
using System.Linq;
using Microcharts.Maui;
using SkiaSharp;

namespace Dollar_Wise
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel ViewModel;

        public MainPage(string username, string selectedCurrency)
        {
            InitializeComponent();

            ViewModel = new MainPageViewModel(username, selectedCurrency);
            BindingContext = ViewModel;

            ViewModel.ExpenseButtonColor = Color.FromHex("#2196F3");
            ViewModel.IncomeButtonColor = Color.FromHex("#A9A9A9");

            // Initially display expenses chart
            ViewModel.UpdatePieChartWithExpensesAsync(UpdatePieChart);
        }

        public MainPage() : this(Preferences.Get("Username", null), Preferences.Get("SelectedCurrency", null))
        {
            if (string.IsNullOrEmpty(ViewModel.Username) || string.IsNullOrEmpty(ViewModel.SelectedCurrency))
            {
                Navigation.PushAsync(new UsernameInputPage());
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.UpdatePieChartWithExpensesAsync(UpdatePieChart);
        }

        private async void OnExpensesClicked(object sender, EventArgs e)
        {
            await ViewModel.UpdatePieChartWithExpensesAsync(UpdatePieChart);
        }

        private async void OnIncomesClicked(object sender, EventArgs e)
        {
            await ViewModel.UpdatePieChartWithIncomesAsync(UpdatePieChart);
        }

        private void UpdatePieChart(List<ChartEntry> entries)
        {
            var chart = new DonutChart { Entries = entries, BackgroundColor = SKColors.DarkGray, LabelMode = LabelMode.LeftAndRight, LabelTextSize=40f};
            PieChartView.Chart = chart;

        }
    }
}
