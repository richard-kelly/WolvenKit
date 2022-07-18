using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Splat;
using WolvenKit.Common.Services;
using WolvenKit.RED4.Save;
using WolvenKit.RED4.Save.IO;
using WolvenKit.ViewModels.Documents;

namespace WolvenKit.ViewModels.Documents;

public class SaveDocumentViewModel : DocumentViewModel
{
    protected readonly ILoggerService _loggerService;

    public CyberpunkSaveFile SaveFile;

    [Reactive] public List<NodeEntry> Nodes { get; set; }
    [Reactive] public NodeEntry SelectedNode { get; set; }

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
            var saveReader = new CyberpunkSaveReader(stream);

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

    public override Task OnSave(object parameter) => throw new System.NotImplementedException();

    private void PopulateData()
    {
        Nodes = SaveFile.Nodes;
    }
}
