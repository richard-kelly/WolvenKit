using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Splat;
using WolvenKit.Functionality.Commands;
using WolvenKit.Interfaces;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Dialogs;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.ViewModels.Red;

public class RedLegacySingleChannelCurveViewModel : ChunkViewModel, IRedCollectionViewModel
{
    public RedLegacySingleChannelCurveViewModel(IRedType data, ChunkViewModel parent, string name) : base(data, parent, name)
    {
        AddItemToCollectionCommand = new DelegateCommand(_ => ExecuteAddItemToCollection());
        DeleteAllFromCollectionCommand = new DelegateCommand(_ => ExecuteDeleteAllFromCollection());
    }

    public ICommand AddItemToCollectionCommand { get; }
    private void ExecuteAddItemToCollection()
    {
        if (Data == null)
        {
            Data = RedTypeManager.CreateRedType(PropertyType);
        }

        var curve = (IRedLegacySingleChannelCurve)Data;

        var type = curve.ElementType;
        var newItem = RedTypeManager.CreateRedType(type);
        InsertChild(-1, newItem);
    }

    public ICommand DeleteAllFromCollectionCommand { get; }
    private void ExecuteDeleteAllFromCollection()
    {
        var arr = (IRedLegacySingleChannelCurve)Data;
        if (arr.Count > 0)
        {
            arr.Clear();

            IsDeleteReady = false;
            Tab.File.SetIsDirty(true);
            RecalulateProperties();
        }
    }
}
