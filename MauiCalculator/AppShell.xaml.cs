namespace MauiCalculator;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Ayarlar sayfası rotasını Shell'e kaydediyoruz
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // Menü çekmecesini kapat ve Ayarlar sayfasına geç
        FlyoutIsPresented = false;
        await Navigation.PushAsync(new SettingsPage());
    }
}