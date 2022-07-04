using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SharpBlueprints.Graph;
using SharpBlueprints.WPF.Messages;

namespace SharpBlueprints.WPF.ViewModels;

public partial class NodeViewModel : ObservableObject
{
    [ObservableProperty] private Node _node;

    [ObservableProperty] private bool _isSelected;

    [ObservableProperty] private ObservableCollection<PinViewModel> _incomingPinViewModels = new();

    [ObservableProperty] private ObservableCollection<PinViewModel> _outgoingPinViewModels = new();

    public delegate void NodeDragStartEventHandler(NodeViewModel nodeViewModel);

    public delegate void NodeDragEndEventHandler(NodeViewModel nodeViewModel);

    public delegate void NodeDragMoveEventHandler(NodeViewModel nodeViewModel, double deltaX, double deltaY);

    public event NodeDragStartEventHandler? OnDragStart;
    public event NodeDragEndEventHandler? OnDragEnd;
    public event NodeDragMoveEventHandler? OnDragMove;
    
    public NodeViewModel(Node node)
    {
        _node = node;
        _incomingPinViewModels =
            new ObservableCollection<PinViewModel>(_node.IncomingPins.Select(pin => new PinViewModel(pin, true)));
        _outgoingPinViewModels =
            new ObservableCollection<PinViewModel>(_node.OutgoingPins.Select(pin => new PinViewModel(pin, false)));
    }

    public NodeViewModel()
    {
        _node = new Node { Name = "Demo Node" };
        _incomingPinViewModels.Add(new PinViewModel(new Pin() { Name = "Incoming Pin #1"}, true));
        _incomingPinViewModels.Add(new PinViewModel(new Pin() { Name = "Incoming Pin #2"}, true));
        _incomingPinViewModels.Add(new PinViewModel(new Pin() { Name = "Incoming Pin #3"}, true));
        _outgoingPinViewModels.Add(new PinViewModel(new Pin() { Name = "Outgoing Pin #1" }, false));
        _outgoingPinViewModels.Add(new PinViewModel(new Pin() { Name = "Outgoing Pin #2" }, false));
    }

    public void DragStart() => OnDragStart?.Invoke(this);
    public void DragEnd() => OnDragEnd?.Invoke(this);
    public void DragMove(double deltaX, double deltaY) => OnDragMove?.Invoke(this, deltaX, deltaY);

    public void Select()
        => WeakReferenceMessenger.Default.Send(new NodeSelectionMessage(this));
}