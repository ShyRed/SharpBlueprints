using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SharpBlueprints.Graph;

public partial class Node : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private double _positionX;

    [ObservableProperty]
    private double _positionY;

    [ObservableProperty]
    private ObservableCollection<Pin> _incomingPins = new();

    [ObservableProperty]
    private ObservableCollection<Pin> _outgoingPins = new();
}