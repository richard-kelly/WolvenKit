using ReactiveUI;
using WolvenKit.Functionality.Converters;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.Views.Editors
{
    /// <summary>
    /// Interaction logic for RedTypeView.xaml
    /// </summary>
    public partial class RedTypeView : ReactiveUserControl<ChunkViewModel>
    {
        public RedTypeView()
        {
            Resources.Add("EnumToDataTemplateConverter", new EnumToDataTemplateConverter { FrameworkElement = this });

            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                if (DataContext is ChunkViewModel vm)
                {
                    SetCurrentValue(ViewModelProperty, vm);
                }
            });
        }
    }
}
