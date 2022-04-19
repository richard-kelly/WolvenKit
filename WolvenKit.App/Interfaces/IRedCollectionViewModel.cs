using System.Windows.Input;

namespace WolvenKit.Interfaces;

public interface IRedCollectionViewModel
{
    public ICommand AddItemToCollectionCommand { get; }
    public ICommand DeleteAllFromCollectionCommand { get; }
}
