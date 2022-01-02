using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WolvenKit.RED4.Types;

namespace WolvenKit.Views.Editors
{
    /// <summary>
    /// Interaction logic for RedWorldPositionEditor.xaml
    /// </summary>
    public partial class RedWorldPositionEditor : UserControl
    {
        public RedWorldPositionEditor()
        {
            InitializeComponent();
        }

        public FixedPoint X
        {
            get => (FixedPoint)this.GetValue(XProperty);
            set => this.SetValue(XProperty, value);
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(FixedPoint), typeof(RedWorldPositionEditor), new PropertyMetadata(default(FixedPoint)));

        public FixedPoint Y
        {
            get => (FixedPoint)this.GetValue(YProperty);
            set => this.SetValue(YProperty, value);
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(FixedPoint), typeof(RedWorldPositionEditor), new PropertyMetadata(default(FixedPoint)));

        public FixedPoint Z
        {
            get => (FixedPoint)this.GetValue(ZProperty);
            set => this.SetValue(ZProperty, value);
        }
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z), typeof(FixedPoint), typeof(RedWorldPositionEditor), new PropertyMetadata(default(FixedPoint)));

        public string XText
        {
            get => GetValueFromXValue();
            set => SetXValue(value);
        }

        public string YText
        {
            get => GetValueFromYValue();
            set => SetYValue(value);
        }

        public string ZText
        {
            get => GetValueFromZValue();
            set => SetZValue(value);
        }

        private void SetXValue(string value) => SetCurrentValue(XProperty, (FixedPoint)float.Parse(value));
        private void SetYValue(string value) => SetCurrentValue(YProperty, (FixedPoint)float.Parse(value));
        private void SetZValue(string value) => SetCurrentValue(ZProperty, (FixedPoint)float.Parse(value));

        private string GetValueFromXValue() => ((float)X).ToString("R");
        private string GetValueFromYValue() => ((float)Y).ToString("R");
        private string GetValueFromZValue() => ((float)Z).ToString("R");


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}