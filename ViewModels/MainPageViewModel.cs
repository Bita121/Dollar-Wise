namespace Dollar_Wise
{
    public class MainPageViewModel
    {
        public string Username { get; }

        public string SelectedCurrency { get; }

        public MainPageViewModel(string username, string selectedCurrency)
        {
            // Initialize with the username and selectedCurrency provided
            Username = username;
            SelectedCurrency = selectedCurrency;
        }
    }
}
