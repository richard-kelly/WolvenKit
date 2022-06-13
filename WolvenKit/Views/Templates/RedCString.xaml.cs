using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using WolvenKit.RED4.Types;

namespace WolvenKit.Views.Editors
{
    /// <summary>
    /// Interaction logic for RedStringEditor.xaml
    /// </summary>
    public partial class RedCStringEditor : UserControl
    {
        public RedCStringEditor()
        {
            InitializeComponent();
            //TextBox.TextChanged += TextBox_TextChanged;

            // causes things to be redrawn :/
            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                handler => TextBox.TextChanged += handler,
                handler => TextBox.TextChanged -= handler)
                .Throttle(TimeSpan.FromSeconds(.5))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    SetRedValue(TextBox.Text);
                });

        }

        public CString RedCString
        {
            get => (CString)GetValue(RedCStringProperty);
            set => SetValue(RedCStringProperty, value);
        }
        public static readonly DependencyProperty RedCStringProperty = DependencyProperty.Register(
            nameof(RedCString), typeof(CString), typeof(RedCStringEditor), new PropertyMetadata(default(CString)));


        public string Text
        {
            get => GetValueFromRedValue();
            set => SetRedValue(value);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => SetRedValue(TextBox.Text);

        private void SetRedValue(string value) => SetCurrentValue(RedCStringProperty, (CString)value);

        private string GetValueFromRedValue()
        {
            var redValue = (string)RedCString;
            if (redValue is { } redString)
            {
                return redString;
            }

            if (redValue is null)
            {
                return "";
            }

            throw new ArgumentException(nameof(redValue));
        }
    }
}
