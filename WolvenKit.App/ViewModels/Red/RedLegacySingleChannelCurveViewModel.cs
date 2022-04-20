using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.IconPacks;
using Splat;
using WolvenKit.Functionality.Commands;
using WolvenKit.Functionality.Converters;
using WolvenKit.Interfaces;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Dialogs;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.ViewModels.Red;

public class RedLegacySingleChannelCurveViewModel : ChunkViewModel
{
    public RedLegacySingleChannelCurveViewModel(IRedType data, ChunkViewModel parent, string name) : base(data, parent, name)
    {
        AddItemToCollectionCommand = new DelegateCommand(_ => ExecuteAddItemToCollection());
        DeleteAllFromCollectionCommand = new DelegateCommand(_ => ExecuteDeleteAllFromCollection());

        SetMenuItems();
    }

    private void SetMenuItems()
    {
        HeaderButtons.Add(RedDataTemplateEnum.AddToArrayButton);
        HeaderButtons.Add(RedDataTemplateEnum.DeleteAllButton);

        ContextMenuItems.Add(new MenuItem
        {
            Command = AddItemToCollectionCommand,
            Header = "Add new Item",
            IsCheckable = false,
            Icon = new PackIconMaterial
            {
                Width = 13,
                Height = 13,
                Padding = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Kind = PackIconMaterialKind.ContentDuplicate
            }
        });
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
