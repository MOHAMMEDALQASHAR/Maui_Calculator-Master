namespace MauiCalculator;

public partial class App : Application
{
    private const string ThemePreferenceKey = "AppThemeSetting";

    public App()
    {
        InitializeComponent();
        LoadSavedTheme();
        MainPage = new AppShell();
    }

    // Uygulama pencere boyutunu hocanın örneğindeki hesap makinesi boyutuna ayarlar
    // Uygulama pencere boyutunu hocanın örneğindeki hesap makinesi boyutuna ayarlar
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Width = 420;
        window.Height = 650;
        window.MinimumWidth = 360;
        window.MinimumHeight = 550;
        window.MaximumWidth = 480; // Pencerenin yana doğru devasa yayılmasını engeller

        return window;
    }

    private void LoadSavedTheme()
    {
        string savedTheme = Preferences.Get(ThemePreferenceKey, "System");

        switch (savedTheme)
        {
            case "Light":
                UserAppTheme = AppTheme.Light;
                break;
            case "Dark":
                UserAppTheme = AppTheme.Dark;
                break;
            default:
                UserAppTheme = AppTheme.Unspecified;
                break;
        }
    }
}