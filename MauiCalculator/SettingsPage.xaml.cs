using Microsoft.Maui.Controls;

namespace MauiCalculator;

public partial class SettingsPage : ContentPage
{
    private const string ThemePreferenceKey = "AppThemeSetting";
    private bool _isInitializing = true;

    public SettingsPage()
    {
        InitializeComponent();
        LoadCurrentThemeSetting();
    }

    // Tercihlerden mevcut temayı oku ve RadioButton'u işaretle
    private void LoadCurrentThemeSetting()
    {
        _isInitializing = true;
        string savedTheme = Preferences.Get(ThemePreferenceKey, "System");

        if (savedTheme == "Light")
        {
            RadioLight.IsChecked = true;
        }
        else if (savedTheme == "Dark")
        {
            RadioDark.IsChecked = true;
        }
        else
        {
            RadioSystem.IsChecked = true;
        }

        _isInitializing = false;
    }

    // RadioButton seçildiğinde anında çalışır ve temayı uygular
    private void OnThemeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_isInitializing || !e.Value)
            return;

        if (sender == RadioLight)
        {
            ApplyAndSaveTheme(AppTheme.Light, "Light");
        }
        else if (sender == RadioDark)
        {
            ApplyAndSaveTheme(AppTheme.Dark, "Dark");
        }
        else if (sender == RadioSystem)
        {
            ApplyAndSaveTheme(AppTheme.Unspecified, "System");
        }
    }

    private void ApplyAndSaveTheme(AppTheme theme, string themeName)
    {
        if (Application.Current != null)
        {
            // Temayı anında uygula
            Application.Current.UserAppTheme = theme;
        }

        // Seçimi yerel hafızaya kaydet
        Preferences.Set(ThemePreferenceKey, themeName);
    }
}