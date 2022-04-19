using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WolvenKit.Common.Services;
using WolvenKit.Interfaces;

namespace WolvenKit.ViewModels.Shell;

public class GroupedChunkViewModel : ReactiveObject, ISelectableTreeViewItemModel
{
    public string Name { get; }
    public ObservableCollectionExtended<ChunkViewModel> TVProperties { get; }

    public GroupedChunkViewModel(string range, IEnumerable<ChunkViewModel> chunkViewModels)
    {
        Name = range;
        TVProperties = new ObservableCollectionExtended<ChunkViewModel>(chunkViewModels);
    }

    [Reactive] public bool IsExpanded { get; set; }
    [Reactive] public bool IsSelected { get; set; }
}
