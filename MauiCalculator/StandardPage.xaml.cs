using System.Globalization;

namespace MauiCalculator;

public partial class StandardPage : ContentPage
{
    private double _firstNumber = 0;
    private double _secondNumber = 0;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private double _memoryValue = 0;

    public StandardPage()
    {
        InitializeComponent();
    }

    // Rakam butonlarına tıklandığında (0-9)
    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            string digit = button.Text;

            if (_isNewEntry || DisplayLabel.Text == "0" || DisplayLabel.Text == "Sıfıra bölünemez")
            {
                DisplayLabel.Text = digit;
                _isNewEntry = false;
            }
            else
            {
                DisplayLabel.Text += digit;
            }
        }
    }

    // Ondalık virgül butonu (,)
    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (_isNewEntry)
        {
            DisplayLabel.Text = "0,";
            _isNewEntry = false;
        }
        else if (!DisplayLabel.Text.Contains(","))
        {
            DisplayLabel.Text += ",";
        }
    }

    // İşaret değiştirme butonu (+/-)
    private void OnNegateClicked(object sender, EventArgs e)
    {
        if (double.TryParse(DisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double currentVal))
        {
            currentVal = -currentVal;
            DisplayLabel.Text = currentVal.ToString().Replace(".", ",");
        }
    }

    // Dört işlem operatörleri (+, -, ×, ÷)
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (double.TryParse(DisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _firstNumber = value;
                _currentOperator = button.Text;
                HistoryLabel.Text = $"{DisplayLabel.Text} {_currentOperator}";
                _isNewEntry = true;
            }
        }
    }

    // Eşittir butonu (=)
    private void OnEqualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentOperator))
            return;

        if (double.TryParse(DisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double secondVal))
        {
            _secondNumber = secondVal;
            HistoryLabel.Text = $"{_firstNumber.ToString().Replace(".", ",")} {_currentOperator} {_secondNumber.ToString().Replace(".", ",")} =";

            double result = 0;
            switch (_currentOperator)
            {
                case "+":
                    result = _firstNumber + _secondNumber;
                    break;
                case "-":
                    result = _firstNumber - _secondNumber;
                    break;
                case "×":
                    result = _firstNumber * _secondNumber;
                    break;
                case "÷":
                    // Sıfıra bölme hatası kontrolü (Ödev Kriteri 7)
                    if (_secondNumber == 0)
                    {
                        DisplayLabel.Text = "Sıfıra bölünemez";
                        _currentOperator = string.Empty;
                        _isNewEntry = true;
                        return;
                    }
                    result = _firstNumber / _secondNumber;
                    break;
            }

            DisplayLabel.Text = result.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _firstNumber = result;
            _currentOperator = string.Empty;
            _isNewEntry = true;
        }
    }

    // Tekli işlemler: 1/x, x², √x, %
    private void OnSpecialOpClicked(object sender, EventArgs e)
    {
        if (sender is Button button && double.TryParse(DisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            double res = 0;
            string op = button.Text;

            switch (op)
            {
                case "1/x":
                    if (val == 0)
                    {
                        DisplayLabel.Text = "Sıfıra bölünemez";
                        _isNewEntry = true;
                        return;
                    }
                    res = 1 / val;
                    HistoryLabel.Text = $"1/({val})";
                    break;
                case "x²":
                    res = Math.Pow(val, 2);
                    HistoryLabel.Text = $"sqr({val})";
                    break;
                case "√x":
                    if (val < 0)
                    {
                        DisplayLabel.Text = "Geçersiz giriş";
                        _isNewEntry = true;
                        return;
                    }
                    res = Math.Sqrt(val);
                    HistoryLabel.Text = $"√({val})";
                    break;
                case "%":
                    res = (_firstNumber != 0) ? (_firstNumber * val / 100.0) : (val / 100.0);
                    HistoryLabel.Text = res.ToString();
                    break;
            }

            DisplayLabel.Text = res.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _isNewEntry = true;
        }
    }

    // CE (Girişi Temizle)
    private void OnClearEntryClicked(object sender, EventArgs e)
    {
        DisplayLabel.Text = "0";
        _isNewEntry = true;
    }

    // C (Tümünü Temizle)
    private void OnClearClicked(object sender, EventArgs e)
    {
        _firstNumber = 0;
        _secondNumber = 0;
        _currentOperator = string.Empty;
        HistoryLabel.Text = "";
        DisplayLabel.Text = "0";
        _isNewEntry = true;
    }

    // Silme tuşu (Backspace - ⌫)
    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (_isNewEntry || DisplayLabel.Text.Length <= 1 || DisplayLabel.Text == "Sıfıra bölünemez" || DisplayLabel.Text == "Geçersiz giriş")
        {
            DisplayLabel.Text = "0";
            _isNewEntry = true;
        }
        else
        {
            DisplayLabel.Text = DisplayLabel.Text.Substring(0, DisplayLabel.Text.Length - 1);
        }
    }

    // Hafıza işlemleri (MC, MR, M+, M-, MS)
    private void OnMemoryClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && double.TryParse(DisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            switch (btn.Text)
            {
                case "MC":
                    _memoryValue = 0;
                    break;
                case "MR":
                    DisplayLabel.Text = _memoryValue.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
                    _isNewEntry = true;
                    break;
                case "M+":
                    _memoryValue += val;
                    _isNewEntry = true;
                    break;
                case "M-":
                    _memoryValue -= val;
                    _isNewEntry = true;
                    break;
                case "MS":
                    _memoryValue = val;
                    _isNewEntry = true;
                    break;
            }
        }
    }
}