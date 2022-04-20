using System;
using System.Collections.Generic;
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
using WolvenKit.RED4;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Dialogs;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.ViewModels.Red;

public class RedBufferPointerViewModel : ChunkViewModel
{
    private IRedBufferPointer _castedData => (IRedBufferPointer)Data;

    public RedBufferPointerViewModel(IRedType data, ChunkViewModel parent, string name) : base(data, parent, name)
    {
        AddItemToCompiledDataCommand = new DelegateCommand(_ => ExecuteAddItemToCompiledData(), _ => CanAddItemToCompiledData());
        DeleteAllFromCollectionCommand = new DelegateCommand(_ => ExecuteDeleteAllFromCollection(), _ => CanDeleteAllFromCollection());

        SetMenuItems();
    }

    private void SetMenuItems()
    {
        HeaderButtons.Add(RedDataTemplateEnum.AddToCompiledDataButton);
        HeaderButtons.Add(RedDataTemplateEnum.DeleteAllButton);

        ContextMenuItems.Add(new MenuItem
        {
            Command = AddItemToCompiledDataCommand,
            Header = "Add new Element",
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

    public ICommand AddItemToCompiledDataCommand { get; }
    private bool CanAddItemToCompiledData() => true;
    private void ExecuteAddItemToCompiledData()
    {
        if (Data == null)
        {
            Data = RedTypeManager.CreateRedType(ResolvedPropertyType);
            _castedData.SetValue(new RedBuffer()
            {
                Data = new Package04()
                {
                    Chunks = new List<RedBaseClass>()
                }
            });
        }
        if (Data is DataBuffer db2)
        {
            if (Name == "rawData" && db2.Data is null)
            {
                db2.Buffer = RedBuffer.CreateBuffer(0, new byte[] { 0 });
                db2.Data = new CR2WList();
            }
        }
        ObservableCollection<string> existing = null;
        if (_castedData.GetValue().Data is Package04 pkg)
        {
            existing = new ObservableCollection<string>(pkg.Chunks.Select(t => t.GetType().Name).Distinct());
        }
        var app = Locator.Current.GetService<AppViewModel>();
        app.SetActiveDialog(new CreateClassDialogViewModel(existing, true)
        {
            DialogHandler = HandleChunk
        });
    }

    public ICommand DeleteAllFromCollectionCommand { get; }
    private bool CanDeleteAllFromCollection() => _castedData.GetValue().Data is Package04 or CR2WList;
    private void ExecuteDeleteAllFromCollection()
    {
        if (_castedData.GetValue().Data is Package04 pkg)
        {
            pkg.Chunks.Clear();

            IsDeleteReady = false;
            Tab.File.SetIsDirty(true);
            RecalulateProperties();
        }

        if (_castedData.GetValue().Data is CR2WList lst)
        {
            lst.Files.Clear();

            IsDeleteReady = false;
            Tab.File.SetIsDirty(true);
            RecalulateProperties();
        }
    }
}
