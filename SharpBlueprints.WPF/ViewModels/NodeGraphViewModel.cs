using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SharpBlueprints.Graph;
using SharpBlueprints.WPF.Messages;

namespace SharpBlueprints.WPF.ViewModels;

public partial class NodeGraphViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<NodeViewModel> _nodeViewModels = new();

    [ObservableProperty] private double _nodeDragScale = 1.0;

    public NodeGraph Graph
    {
        get => _graph;
        set
        {
            if (_graph == value)
                return;

            NodeViewModels.CollectionChanged -= OnNodeViewModelCollectionChanged;
            foreach (var nodeViewModel in NodeViewModels)
                SetupNodeEvents(nodeViewModel, false);

            _graph = value;
            NodeViewModels =
                new ObservableCollection<NodeViewModel>(_graph.Nodes.Select(node => new NodeViewModel(node)));

            NodeViewModels.CollectionChanged += OnNodeViewModelCollectionChanged;
            foreach (var nodeViewModel in NodeViewModels)
                SetupNodeEvents(nodeViewModel, true);
        }
    }

    private NodeGraph _graph = new();

    public IEnumerable<NodeViewModel> SelectedNodeViewModels
    {
        get => _selectedNodeViewModels;
        private set
        {
            if (_selectedNodeViewModels != value)
            {
                foreach (var nodeViewModel in _selectedNodeViewModels)
                    nodeViewModel.IsSelected = false;
            }

            _selectedNodeViewModels = value;

            foreach (var nodeViewModel in _selectedNodeViewModels)
                nodeViewModel.IsSelected = true;
        }
    }

    private IEnumerable<NodeViewModel> _selectedNodeViewModels = Array.Empty<NodeViewModel>();

    private Point _nodeDragAccum;
    private readonly Dictionary<NodeViewModel, Point> _nodeStartPositions = new();

    public NodeGraphViewModel(NodeGraph nodeGraph)
    {
        Graph = nodeGraph;
        
        WeakReferenceMessenger.Default.Register<NodeSelectionMessage>(this, OnNodeSelectionReceived);
    }

    public NodeGraphViewModel()
    {
        _nodeViewModels.Add(new NodeViewModel(new Node() { Name = "Start", PositionX = 10, PositionY = 10 }));
        _nodeViewModels.Add(new NodeViewModel(new Node() { Name = "Do Stuff", PositionX = 210, PositionY = 110 }));
        _nodeViewModels.Add(new NodeViewModel(new Node() { Name = "End", PositionX = 410, PositionY = 50 }));
    }

    private void OnNodeSelectionReceived(object sender, NodeSelectionMessage msg)
    {
        if (msg.DeselectPrevious)
            SelectedNodeViewModels = Array.Empty<NodeViewModel>();
        SelectedNodeViewModels = SelectedNodeViewModels.Concat(msg.NodeViewModels).Distinct();
    }

    private void SetupNodeEvents(NodeViewModel nodeViewModel, bool add)
    {
        if (add)
        {
            nodeViewModel.OnDragStart += NodeViewModelOnOnDragStart;
            nodeViewModel.OnDragMove += NodeViewModelOnOnDragMove;
            nodeViewModel.OnDragEnd += NodeViewModelOnOnDragEnd;
        }
        else
        {
            nodeViewModel.OnDragStart -= NodeViewModelOnOnDragStart;
            nodeViewModel.OnDragMove -= NodeViewModelOnOnDragMove;
            nodeViewModel.OnDragEnd -= NodeViewModelOnOnDragEnd;
        }
    }

    private void NodeViewModelOnOnDragEnd(NodeViewModel nodeviewmodel)
    {
        foreach (var selectedNodeViewModel in SelectedNodeViewModels)
        {
            selectedNodeViewModel.Node.PositionX -= selectedNodeViewModel.Node.PositionX % 28;
            selectedNodeViewModel.Node.PositionY -= selectedNodeViewModel.Node.PositionY % 28;
        }
    }

    private void NodeViewModelOnOnDragMove(NodeViewModel nodeviewmodel, double deltaX, double deltaY)
    {
        _nodeDragAccum.X += deltaX * NodeDragScale;
        _nodeDragAccum.Y += deltaY * NodeDragScale;
        foreach (var nodeViewModel in SelectedNodeViewModels)
        {
            nodeViewModel.Node.PositionX =
                _nodeStartPositions[nodeViewModel].X + _nodeDragAccum.X - _nodeDragAccum.X % 28;
            nodeViewModel.Node.PositionY =
                _nodeStartPositions[nodeViewModel].Y + _nodeDragAccum.Y - _nodeDragAccum.Y % 28;
        }
    }

    private void NodeViewModelOnOnDragStart(NodeViewModel nodeviewmodel)
    {
        _nodeDragAccum.X = 0.0;
        _nodeDragAccum.Y = 0.0;
        _nodeStartPositions.Clear();
        foreach (var selectedNodeViewModel in SelectedNodeViewModels)
            _nodeStartPositions.Add(selectedNodeViewModel,
                new Point(selectedNodeViewModel.Node.PositionX, selectedNodeViewModel.Node.PositionY));
    }

    private void OnNodeViewModelCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var removedNodeViewModel in e.OldItems.Cast<NodeViewModel>())
                SetupNodeEvents(removedNodeViewModel, false);
        }

        if (e.NewItems != null)
        {
            foreach (var removedNodeViewModel in e.NewItems.Cast<NodeViewModel>())
                SetupNodeEvents(removedNodeViewModel, true);
        }
    }
}