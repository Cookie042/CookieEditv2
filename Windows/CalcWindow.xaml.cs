using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using LoreSoft.MathExpressions;

namespace CookieEdit2.Windows
{
    /// <summary>
    /// Interaction logic for CalcWindow.xaml
    /// </summary>
    public partial class CalcWindow : Window
    {

        public MathEvaluator mathEval = new MathEvaluator();
        public CalcWindow()
        {
            InitializeComponent();
        }

        private void OutputFunctions()
        {
            foreach (var item in mathEval.Functions)
            {
                Debug.WriteLine(item.ToString());
            }
        }

        private void Evaluate()
        {
            if (mathEval == null || tb_Input == null)
                return;

            string _input = tb_Input.Text;

            if (tb_Input.Text.Trim() == "")
            {
                tb_Output.Text = "0";
                statusBar.Text = "";
                return;
            }

            _input = _input.Replace('[', '(');
            _input = _input.Replace(']', ')');

            Regex regex = new Regex(@"#(\d{1,3})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(tb_Input.Text);

            foreach (Match match in matches)
                _input = _input.Replace(match.Value, MainWindow.instance.macroVariableManager.variables[int.Parse(match.Groups[1].Value)].value.ToString());

            try
            {

                double evalResult = mathEval.Evaluate(_input);

                tb_Output.Text = string.Format("{0:0.#########}", evalResult);
                statusBar.Text = "";
            }
            catch (LoreSoft.MathExpressions.ParseException ex)
            {
                tb_Output.Text = "Error";
                statusBar.Text = ex.Message.ToString();
            }
            catch (System.ArgumentNullException ex)
            {
                tb_Output.Text = "Error";
                statusBar.Text = ex.Message.ToString();
            }
            catch (System.InvalidOperationException ex)
            {
                tb_Output.Text = "Error";
                statusBar.Text = ex.Message.ToString();
            }

        }

        private void PlaceStringAtInputSelection(string s)
        {
            tb_Input.SelectedText = s;
            tb_Input.Select(tb_Input.SelectionStart + tb_Input.SelectionLength, 0);
        }

        private void btn_Eval_Click(object sender, RoutedEventArgs e)
        {
            Evaluate();
            listBox_history.Items.Add(tb_Input.Text + " = " + tb_Output.Text);
        }

        private void tb_Input_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Evaluate();
        }

        private void btn_0_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("0");
            Evaluate();
        }
        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("1");
            Evaluate();
        }
        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("2");
            Evaluate();
        }
        private void btn_3_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("3");
            Evaluate();
        }
        private void btn_4_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("4");
            Evaluate();
        }
        private void btn_5_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("5");
            Evaluate();
        }
        private void btn_6_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("6");
            Evaluate();
        }
        private void btn_7_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("7");
            Evaluate();
        }
        private void btn_8_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("8");
            Evaluate();
        }
        private void btn_9_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("9");
            Evaluate();
        }

        private void btn_pi_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("pi");
            Evaluate();
        }

        private void btn_multiply_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("*");
            Evaluate();
        }
        private void btn_divide_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("/");
            Evaluate();
        }
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("+");
            Evaluate();
        }
        private void btn_minus_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection("-");
            Evaluate();
        }
        private void btn_period_Click(object sender, RoutedEventArgs e)
        {
            PlaceStringAtInputSelection(".");
            Evaluate();
        }
        private void btn_braces_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.SelectedText = "(" + tb_Input.SelectedText + ")";
            Evaluate();
        }

        private void btn_sin_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.SelectedText = "sin(" + tb_Input.SelectedText + ")";
            Evaluate();
        }

        private void btn_cos_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.SelectedText = "cos(" + tb_Input.SelectedText + ")";
            Evaluate();
        }

        private void btn_tan_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.SelectedText = "tan(" + tb_Input.SelectedText + ")";
            Evaluate();
        }
        private void btn_Sqrt_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.SelectedText = "sqrt(" + tb_Input.SelectedText + ")";
            Evaluate();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!tb_Input.IsFocused && !tb_Output.IsFocused)
            {
                Convert.ToChar(e.Key.GetHashCode());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OutputFunctions();
        }

        private void btn_clr_Click(object sender, RoutedEventArgs e)
        {
            tb_Input.Clear();
            tb_Output.Clear();
            statusBar.Text = "";
        }
    }
}
