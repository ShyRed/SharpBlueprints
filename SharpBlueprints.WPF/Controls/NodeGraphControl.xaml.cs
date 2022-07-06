using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using SharpBlueprints.WPF.ViewModels;

namespace SharpBlueprints.WPF.Controls;

public partial class NodeGraphControl : UserControl
{
    public NodeGraphViewModel GraphDataContext
    {
        get => (NodeGraphViewModel)DataContext;
        set => DataContext = value;
    }

    private bool _isBoxSelecting;
    private Point _boxSelectStartingPoint;

    private bool _isCanvasDragging;
    private bool _isCanvasMovedDuringDrag;
    private Point _canvasDragStartPosition;
    private Vector _canvasDragStartOffset;
    
    public NodeGraphControl()
    {
        InitializeComponent();
    }

    private void OnNodeGraphCanvasRightMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isCanvasDragging = true;
        _canvasDragStartPosition = e.GetPosition(this);
        _canvasDragStartOffset = NodeGraphCanvas.RenderTransform is TranslateTransform currentTransform
            ? new Vector(currentTransform.X, currentTransform.Y)
            : new Vector();
        NodeGraphCanvas.CaptureMouse();
        e.Handled = true;
    }

    private void OnNodeGraphCanvasRightMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isCanvasDragging)
            return;

        _isCanvasDragging = false;
        NodeGraphCanvas.ReleaseMouseCapture();
        Mouse.OverrideCursor = null;
        e.Handled = true;
    }

    private void OnNodeGraphCanvasLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        GraphDataContext.DeselectAll();

        _isBoxSelecting = true;
        _boxSelectStartingPoint = e.GetPosition(NodeGraphCanvas);
        Canvas.SetLeft(BoxSelectionBorder, _boxSelectStartingPoint.X);
        Canvas.SetTop(BoxSelectionBorder, _boxSelectStartingPoint.Y);
        BoxSelectionBorder.Width = 0.0;
        BoxSelectionBorder.Height = 0.0;
        BoxSelectionBorder.Visibility = Visibility.Visible;
        NodeGraphCanvas.CaptureMouse();
        e.Handled = true;
    }
    
    private void OnNodeGraphCanvasLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isBoxSelecting)
            return;

        _isBoxSelecting = false;
        BoxSelectionBorder.Visibility = Visibility.Hidden;
        NodeGraphCanvas.ReleaseMouseCapture();
        SelectWithinRectangle(new Rect(
            Canvas.GetLeft(BoxSelectionBorder),
            Canvas.GetTop(BoxSelectionBorder),
            BoxSelectionBorder.Width,
            BoxSelectionBorder.Height));
        e.Handled = true;
    }

    private void OnNodeGraphCanvasMouseMove(object sender, MouseEventArgs e)
    {
        if (_isCanvasDragging)
        {
            if (e.RightButton == MouseButtonState.Released)
                _isCanvasDragging = false;
            else
            {
                var offset = _canvasDragStartOffset + e.GetPosition(this) - _canvasDragStartPosition;
                NodeGraphCanvas.RenderTransform = new TranslateTransform(
                    Math.Min(0.0, offset.X),
                    Math.Min(0.0, offset.Y));
                _isCanvasMovedDuringDrag = true;
                Mouse.OverrideCursor = Cursors.ScrollAll;
            }
            
            e.Handled = true;
            return;
        }
        
        if (!_isBoxSelecting)
            return;

        if (e.LeftButton == MouseButtonState.Released)
        {
            _isBoxSelecting = false;
            return;
        }

        var x = _boxSelectStartingPoint.X;
        var y = _boxSelectStartingPoint.Y;
        var mx = e.GetPosition(NodeGraphCanvas).X;
        var my = e.GetPosition(NodeGraphCanvas).Y;
        var rectX = Math.Min(x, mx);
        var rectY = Math.Min(y, my);
        var rectWidth = Math.Abs(x - mx);
        var rectHeight = Math.Abs(y - my);
        Canvas.SetLeft(BoxSelectionBorder, rectX);
        Canvas.SetTop(BoxSelectionBorder, rectY);
        BoxSelectionBorder.Width = rectWidth;
        BoxSelectionBorder.Height = rectHeight;
    }

    private void SelectWithinRectangle(Rect rect)
    {
        var selectedNodes = new List<NodeViewModel>();
        for (var i = 0; i < GraphDataContext.NodeViewModels.Count; i++)
        {
            var nodeControl = (ContentPresenter)Nodes.ItemContainerGenerator.ContainerFromIndex(i);
            var nodeViewModel = (NodeViewModel)nodeControl.Content;
            if (rect.IntersectsWith(new Rect(
                    nodeViewModel.Node.PositionX,
                    nodeViewModel.Node.PositionY,
                    nodeControl.ActualWidth,
                    nodeControl.ActualHeight)))
            {
                selectedNodes.Add(nodeViewModel);
            }
        }
        GraphDataContext.SelectNodes(selectedNodes);
    }
}