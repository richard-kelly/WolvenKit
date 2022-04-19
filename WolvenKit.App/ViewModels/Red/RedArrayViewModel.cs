using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.IconPacks;
using Splat;
using WolvenKit.Functionality.Commands;
using WolvenKit.Interfaces;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Dialogs;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.ViewModels.Red;

public class RedArrayViewModel : ChunkViewModel, IRedCollectionViewModel
{
    public RedArrayViewModel(IRedType data, ChunkViewModel parent, string name) : base(data, parent, name)
    {
        AddItemToCollectionCommand = new DelegateCommand(_ => ExecuteAddItemToCollection());
        DeleteAllFromCollectionCommand = new DelegateCommand(_ => ExecuteDeleteAllFromCollection());

        SetMenuItems();
    }

    private void SetMenuItems()
    {
        MenuItems.Add(new MenuItem
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
            // TODO: Need info for CStatic, ...
            return;
        }

        var arr = (IRedArray)Data;

        var innerType = arr.InnerType;
        var pointer = false;
        if (innerType.IsAssignableTo(typeof(IRedBaseHandle)))
        {
            pointer = true;
            innerType = innerType.GenericTypeArguments[0];
        }
        var existing = new ObservableCollection<string>(AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => innerType.IsAssignableFrom(p) && p.IsClass).Select(x => x.Name));

        // no inheritable
        if (existing.Count == 1)
        {
            var type = arr.InnerType;
            var newItem = RedTypeManager.CreateRedType(type);
            if (newItem is IRedBaseHandle handle)
            {
                var pointee = RedTypeManager.CreateRedType(handle.InnerType);
                handle.SetValue((RedBaseClass)pointee);
            }
            InsertChild(-1, newItem);
        }
        else
        {
            var app = Locator.Current.GetService<AppViewModel>();
            app.SetActiveDialog(new CreateClassDialogViewModel(existing, true)
            {
                DialogHandler = pointer ? HandleChunkPointer : HandleChunk
            });
        }
    }

    public ICommand DeleteAllFromCollectionCommand { get; }
    private void ExecuteDeleteAllFromCollection()
    {
        var arr = (IRedArray)Data;
        if (arr.Count > 0)
        {
            arr.Clear();

            IsDeleteReady = false;
            Tab.File.SetIsDirty(true);
            RecalulateProperties();
        }
    }
}
