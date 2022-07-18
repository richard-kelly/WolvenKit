using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Splat;
using WolvenKit.Common.Services;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Save;
using WolvenKit.RED4.Save.IO;
using WolvenKit.RED4.Types;
using WolvenKit.ViewModels.Documents;

namespace WolvenKit.ViewModels.Documents;

public class SaveDocumentViewModel : DocumentViewModel
{
    protected readonly ILoggerService _loggerService;

    public CyberpunkSaveFile SaveFile;

    [Reactive] public List<SaveTreeViewItem> Nodes { get; set; }
    [Reactive] public SaveTreeViewItem SelectedNode { get; set; }

    public SaveDocumentViewModel(string path) : base(path)
    {
        _loggerService = Locator.Current.GetService<ILoggerService>();
    }

    public override Task<bool> OpenFileAsync(string path) => throw new System.NotImplementedException();

    public override bool OpenFile(string path)
    {
        _isInitialized = false;

        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var saveReader = new CyberpunkSaveReader(stream);

            if (saveReader.ReadFile(out SaveFile) == EFileReadErrorCodes.NoError)
            {
                FilePath = path;
                _isInitialized = false;

                PopulateData();

                return true;
            }

            _loggerService.Error($"Failed to read save file {path}");
        }
        catch (Exception e)
        {
            _loggerService.Error(e);
            // Not processing this catch in any other way than rejecting to initialize this
            _isInitialized = false;
        }

        return false;
    }

    public override Task OnSave(object parameter)
    {
        using var fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
        using var sw = new CyberpunkSaveWriter(fs);
        sw.WriteFile(SaveFile);

        _loggerService.Success($"Saved file {FilePath}");

        return Task.CompletedTask;
    }

    private void PopulateData()
    {
        Nodes = SaveFile.Nodes.Select(x => new SaveTreeViewItem(x)).ToList();
    }

    public class SaveTreeViewItem
    {
        public string DisplayName { get; set; }
        public List<object> Children { get; set; } = new();

        public object Value { get; set; }

        public SaveTreeViewItem(NodeEntry nodeEntry)
        {
            DisplayName = nodeEntry.Name;
            Value = nodeEntry.Value;

            if (nodeEntry.Value is Inventory inv)
            {
                foreach (var subInventory in inv.SubInventories)
                {
                    Children.Add(new SaveTreeViewItem(subInventory));
                }
            }
            else if (nodeEntry.Value is ItemDropStorage storage)
            {
                foreach (var itemData in storage.Items)
                {
                    Children.Add(new SaveTreeViewItem(itemData));
                }
            }
            else if (nodeEntry.Value is Package { Content: RedPackage pkg })
            {
                foreach (var chunk in pkg.Chunks)
                {
                    Children.Add(new SaveTreeViewItem(chunk));
                }
            }
            else
            {
                foreach (var child in nodeEntry.Children)
                {
                    Children.Add(new SaveTreeViewItem(child));
                }
            }
        }

        public SaveTreeViewItem(InventoryHelper.SubInventory subInventory)
        {
            DisplayName = $"Inventory {subInventory.InventoryId}";
            Value = subInventory;

            foreach (var itemData in subInventory.Items)
            {
                Children.Add(new SaveTreeViewItem(itemData));
            }
        }

        public SaveTreeViewItem(InventoryHelper.ItemData itemData)
        {
            DisplayName = $"<TweakDBID 0x{(ulong)itemData.ItemTdbId:X8}>";
            if (itemData.ItemTdbId.ResolvedText != null)
            {
                DisplayName = itemData.ItemTdbId.ResolvedText;
            }

            Value = itemData;
        }

        private SaveTreeViewItem(RedBaseClass cls)
        {
            DisplayName = cls.GetType().Name;
            Value = cls;
        }
    }
}
