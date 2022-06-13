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
    public partial class RedCNameEditor : UserControl
    {
        public RedCNameEditor()
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

        public CName RedCName
        {
            get => (CName)GetValue(RedCNameProperty);
            set => SetValue(RedCNameProperty, value);
        }
        public static readonly DependencyProperty RedCNameProperty = DependencyProperty.Register(
            nameof(RedCName), typeof(CName), typeof(RedCNameEditor), new PropertyMetadata(default(CName)));


        public string Text
        {
            get => GetValueFromRedValue();
            set => SetRedValue(value);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => SetRedValue(TextBox.Text);

        private void SetRedValue(string value) => SetCurrentValue(RedCNameProperty, (CName)value);

        private string GetValueFromRedValue()
        {
            var redValue = (string)RedCName;
            if (redValue is { } redString)
            {
                return redString;
            }
            else if (redValue is null)
            {
                return "";
            }
            else
            {
                throw new ArgumentException(nameof(redValue));
            }
        }


    }
}
