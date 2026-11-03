using System.Globalization;

namespace MauiCalculator;

public partial class ScientificPage : ContentPage
{
    private double _firstNumber = 0;
    private double _secondNumber = 0;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private double _memoryValue = 0;
    private bool _isDegreeMode = true;

    public ScientificPage()
    {
        InitializeComponent();
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            string digit = button.Text;

            if (_isNewEntry || SciDisplayLabel.Text == "0" || SciDisplayLabel.Text == "Geçersiz giriş" || SciDisplayLabel.Text == "Sıfıra bölünemez")
            {
                SciDisplayLabel.Text = digit;
                _isNewEntry = false;
            }
            else
            {
                SciDisplayLabel.Text += digit;
            }
        }
    }

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (_isNewEntry)
        {
            SciDisplayLabel.Text = "0,";
            _isNewEntry = false;
        }
        else if (!SciDisplayLabel.Text.Contains(","))
        {
            SciDisplayLabel.Text += ",";
        }
    }

    private void OnNegateClicked(object sender, EventArgs e)
    {
        if (double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            val = -val;
            SciDisplayLabel.Text = val.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
        }
    }

    private void OnAngleModeClicked(object sender, EventArgs e)
    {
        _isDegreeMode = !_isDegreeMode;
        AngleModeButton.Text = _isDegreeMode ? "DEG" : "RAD";
    }

    private async void OnTrigMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Trigonometrik Fonksiyon", "İptal", null, "sin", "cos", "tan");
        if (action == "İptal" || string.IsNullOrEmpty(action)) return;

        if (double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            double angle = _isDegreeMode ? (val * Math.PI / 180.0) : val;
            double res = 0;

            switch (action)
            {
                case "sin":
                    res = Math.Sin(angle);
                    SciHistoryLabel.Text = $"sin({val})";
                    break;
                case "cos":
                    res = Math.Cos(angle);
                    SciHistoryLabel.Text = $"cos({val})";
                    break;
                case "tan":
                    res = Math.Tan(angle);
                    SciHistoryLabel.Text = $"tan({val})";
                    break;
            }

            SciDisplayLabel.Text = res.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _isNewEntry = true;
        }
    }

    private void OnConstantClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            double val = btn.Text == "π" ? Math.PI : Math.E;
            SciDisplayLabel.Text = val.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _isNewEntry = true;
        }
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _firstNumber = value;
                _currentOperator = button.Text;
                SciHistoryLabel.Text = $"{SciDisplayLabel.Text} {_currentOperator}";
                _isNewEntry = true;
            }
        }
    }

    private void OnEqualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentOperator))
            return;

        if (double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double secondVal))
        {
            _secondNumber = secondVal;
            SciHistoryLabel.Text = $"{_firstNumber.ToString().Replace(".", ",")} {_currentOperator} {_secondNumber.ToString().Replace(".", ",")} =";

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
                    if (_secondNumber == 0)
                    {
                        SciDisplayLabel.Text = "Sıfıra bölünemez";
                        _currentOperator = string.Empty;
                        _isNewEntry = true;
                        return;
                    }
                    result = _firstNumber / _secondNumber;
                    break;
                case "mod":
                    if (_secondNumber == 0)
                    {
                        SciDisplayLabel.Text = "Sıfıra bölünemez";
                        _currentOperator = string.Empty;
                        _isNewEntry = true;
                        return;
                    }
                    result = _firstNumber % _secondNumber;
                    break;
                case "xʸ":
                    result = Math.Pow(_firstNumber, _secondNumber);
                    break;
            }

            SciDisplayLabel.Text = result.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _firstNumber = result;
            _currentOperator = string.Empty;
            _isNewEntry = true;
        }
    }

    private void OnSpecialOpClicked(object sender, EventArgs e)
    {
        if (sender is Button button && double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            double res = 0;
            string op = button.Text;

            switch (op)
            {
                case "log":
                    if (val <= 0)
                    {
                        SciDisplayLabel.Text = "Geçersiz giriş";
                        _isNewEntry = true;
                        return;
                    }
                    res = Math.Log10(val);
                    SciHistoryLabel.Text = $"log({val})";
                    break;

                case "ln":
                    if (val <= 0)
                    {
                        SciDisplayLabel.Text = "Geçersiz giriş";
                        _isNewEntry = true;
                        return;
                    }
                    res = Math.Log(val);
                    SciHistoryLabel.Text = $"ln({val})";
                    break;

                case "√x":
                    if (val < 0)
                    {
                        SciDisplayLabel.Text = "Geçersiz giriş";
                        _isNewEntry = true;
                        return;
                    }
                    res = Math.Sqrt(val);
                    SciHistoryLabel.Text = $"√({val})";
                    break;

                case "x²":
                    res = Math.Pow(val, 2);
                    SciHistoryLabel.Text = $"sqr({val})";
                    break;

                case "1/x":
                    if (val == 0)
                    {
                        SciDisplayLabel.Text = "Sıfıra bölünemez";
                        _isNewEntry = true;
                        return;
                    }
                    res = 1 / val;
                    SciHistoryLabel.Text = $"1/({val})";
                    break;

                case "|x|":
                    res = Math.Abs(val);
                    SciHistoryLabel.Text = $"abs({val})";
                    break;

                case "10ˣ":
                    res = Math.Pow(10, val);
                    SciHistoryLabel.Text = $"10^({val})";
                    break;

                case "n!":
                    if (val < 0 || val != Math.Floor(val) || val > 170)
                    {
                        SciDisplayLabel.Text = "Geçersiz giriş";
                        _isNewEntry = true;
                        return;
                    }
                    res = CalculateFactorial((int)val);
                    SciHistoryLabel.Text = $"fact({val})";
                    break;
            }

            SciDisplayLabel.Text = res.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
            _isNewEntry = true;
        }
    }

    private double CalculateFactorial(int n)
    {
        if (n == 0 || n == 1) return 1;
        double fact = 1;
        for (int i = 2; i <= n; i++)
        {
            fact *= i;
        }
        return fact;
    }

    private void OnParenthesisClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            SciHistoryLabel.Text += btn.Text;
        }
    }

    private void OnFunctionClicked(object sender, EventArgs e)
    {
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _firstNumber = 0;
        _secondNumber = 0;
        _currentOperator = string.Empty;
        SciHistoryLabel.Text = "";
        SciDisplayLabel.Text = "0";
        _isNewEntry = true;
    }

    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (_isNewEntry || SciDisplayLabel.Text.Length <= 1 || SciDisplayLabel.Text == "Sıfıra bölünemez" || SciDisplayLabel.Text == "Geçersiz giriş")
        {
            SciDisplayLabel.Text = "0";
            _isNewEntry = true;
        }
        else
        {
            SciDisplayLabel.Text = SciDisplayLabel.Text.Substring(0, SciDisplayLabel.Text.Length - 1);
        }
    }

    private void OnMemoryClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && double.TryParse(SciDisplayLabel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            switch (btn.Text)
            {
                case "MC":
                    _memoryValue = 0;
                    break;
                case "MR":
                    SciDisplayLabel.Text = _memoryValue.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
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