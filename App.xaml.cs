namespace Dollar_Wise;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Set the initial page to UsernameInputPage
        MainPage = new AppShell();
    }

}
