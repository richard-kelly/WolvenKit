using System;
using System.Windows.Input;
using WolvenKit.Functionality.Commands;
using WolvenKit.Interfaces;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Shell;

namespace WolvenKit.ViewModels.Red;

public class RedBufferPointerViewModel : ChunkViewModel, IRedCollectionViewModel
{
    private IRedBufferPointer _castedData => (IRedBufferPointer)Data;

    public RedBufferPointerViewModel(IRedType data, ChunkViewModel parent, string name) : base(data, parent, name)
    {
        AddItemToCollectionCommand = new DelegateCommand(_ => ExecuteAddItemToCollection(), _ => CanAddItemToCollection());
        DeleteAllFromCollectionCommand = new DelegateCommand(_ => ExecuteDeleteAllFromCollection(), _ => CanDeleteAllFromCollection());
    }

    public ICommand AddItemToCollectionCommand { get; }
    private bool CanAddItemToCollection() => false;
    private void ExecuteAddItemToCollection() => throw new NotImplementedException(nameof(ExecuteAddItemToCollection));

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
