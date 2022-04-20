using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WolvenKit.Functionality.Commands;

namespace WolvenKit.Views.Elements
{
    /// <summary>
    /// Interaktionslogik für ConfirmButton.xaml
    /// </summary>
    public partial class ConfirmButton : UserControl
    {
        public ConfirmButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsButtonReadyProperty =
            DependencyProperty.Register(nameof(IsButtonReady), typeof(bool), typeof(ConfirmButton));

        public bool IsButtonReady
        {
            get { return (bool)GetValue(IsButtonReadyProperty); }
            set { SetValue(IsButtonReadyProperty, value); }
        }

        public static readonly DependencyProperty IsArrayElementProperty =
            DependencyProperty.Register(nameof(IsArrayElement), typeof(bool), typeof(ConfirmButton));

        public bool IsArrayElement
        {
            get { return (bool)GetValue(IsArrayElementProperty); }
            set { SetValue(IsArrayElementProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ConfirmButton));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private void ExecuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            Command?.SafeExecute();

            SetCurrentValue(IsButtonReadyProperty, false);
        }
    }
}
