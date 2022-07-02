using System;
using System.Collections.Generic;
using SharpBlueprints.WPF.ViewModels;

namespace SharpBlueprints.WPF.Messages;

public sealed class NodeSelectionMessage
{
    public static readonly NodeSelectionMessage DeselectAll = 
        new (Array.Empty<NodeViewModel>(), true); 
    
    public IEnumerable<NodeViewModel> NodeViewModels { get; }
    public bool DeselectPrevious { get; }

    public NodeSelectionMessage(NodeViewModel nodeViewModel)
    {
        NodeViewModels = new[] { nodeViewModel };
        DeselectPrevious = true;
    }

    public NodeSelectionMessage(IEnumerable<NodeViewModel> nodeViewModels, bool deselectPrevious)
    {
        NodeViewModels = nodeViewModels;
        DeselectPrevious = deselectPrevious;
    }
}