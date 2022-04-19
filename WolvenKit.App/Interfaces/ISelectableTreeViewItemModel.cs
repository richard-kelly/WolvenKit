using DynamicData.Binding;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.Interfaces;

public interface ISelectableTreeViewItemModel
{
    public ObservableCollectionExtended<ChunkViewModel> TVProperties { get; }
    public bool IsExpanded { get; set; }
    public bool IsSelected { get; set; }
}
