using System.Windows;
using System.Text.RegularExpressions;

using LoreSoft.MathExpressions;

namespace CookieEdit2
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        public Calculator()
        {
            InitializeComponent();
        }

        private void EvalButton_Click(object sender, RoutedEventArgs e)
        {
            var evaluator = new MathEvaluator();

            string inTxt = inputTextBox.Text;

            var matches = Regex.Matches(inTxt, @"#+\d{1,3}");
            
            for (int i = 0; i < matches.Count; i++)
            {
                string key = matches[i].Value;
                var keyInt =  int.Parse(key.Substring(1, key.Length-1));

                inTxt = Regex.Replace(inTxt, matches[i].Value, MainWindow.instance.macroVariableManager.variables[keyInt].value.ToString());
            }

            try
            {
                outputTextBox.Text = evaluator.Evaluate(inTxt).ToString();

            }
            catch (System.Exception ex)
            {
                outputTextBox.Text = inTxt;
            }


        }
    }
}
