using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SharpBlueprints.Graph;
using SharpBlueprints.WPF.Messages;

namespace SharpBlueprints.WPF.ViewModels;

public partial class NodeViewModel : ObservableObject
{
    [ObservableProperty] private Node _node;

    [ObservableProperty]
    private bool _isSelected;

    public delegate void NodeDragStartEventHandler(NodeViewModel nodeViewModel);

    public delegate void NodeDragEndEventHandler(NodeViewModel nodeViewModel);

    public delegate void NodeDragMoveEventHandler(NodeViewModel nodeViewModel, double deltaX, double deltaY);

    public event NodeDragStartEventHandler? OnDragStart;
    public event NodeDragEndEventHandler? OnDragEnd;
    public event NodeDragMoveEventHandler? OnDragMove;
    
    public NodeViewModel(Node node)
    {
        _node = node;
    }

    public NodeViewModel()
    {
        _node = new Node { Name = "Demo Node" };
    }

    public void DragStart() => OnDragStart?.Invoke(this);
    public void DragEnd() => OnDragEnd?.Invoke(this);
    public void DragMove(double deltaX, double deltaY) => OnDragMove?.Invoke(this, deltaX, deltaY);

    public void Select()
        => WeakReferenceMessenger.Default.Send(new NodeSelectionMessage(this));
}