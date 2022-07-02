using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SharpBlueprints.Graph;

public partial class NodeGraph : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Node> _nodes = new();
}