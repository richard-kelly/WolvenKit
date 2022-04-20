using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WolvenKit.Functionality.Converters;

public enum RedDataTemplateEnum
{
    AddHandleButton,
    AddToCompiledDataButton,
    AddToArrayButton,
    AddToBufferButton,
    DeleteButton,
    DeleteAllButton
}

public class EnumToDataTemplateConverter : IValueConverter
{
    public FrameworkElement FrameworkElement { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (FrameworkElement != null && value is RedDataTemplateEnum enm)
        {
            switch (enm)
            {
                case RedDataTemplateEnum.AddHandleButton:
                    return FrameworkElement.FindResource("AddHandleButton") as DataTemplate;

                case RedDataTemplateEnum.AddToCompiledDataButton:
                    return FrameworkElement.FindResource("AddToCompiledDataButton") as DataTemplate;

                case RedDataTemplateEnum.AddToArrayButton:
                    return FrameworkElement.FindResource("AddToArrayButton") as DataTemplate;

                case RedDataTemplateEnum.AddToBufferButton:
                    return FrameworkElement.FindResource("AddToBufferButton") as DataTemplate;

                case RedDataTemplateEnum.DeleteButton:
                    return FrameworkElement.FindResource("DeleteButton") as DataTemplate;

                case RedDataTemplateEnum.DeleteAllButton:
                    return FrameworkElement.FindResource("DeleteAllButton") as DataTemplate;
            }
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
