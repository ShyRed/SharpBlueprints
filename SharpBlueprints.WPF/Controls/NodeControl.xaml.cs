using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SharpBlueprints.WPF.ViewModels;

namespace SharpBlueprints.WPF.Controls;

public partial class NodeControl : UserControl
{
    public NodeViewModel NodeDataContext
    {
        get => (NodeViewModel)DataContext;
        set => DataContext = value;
    }

    private bool _isDragging;
    private bool _isDragged;
    private Point _lastMousePosition;

    public NodeControl()
    {
        InitializeComponent();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        _isDragged = false;
        _isDragging = true;
        if (!NodeDataContext.IsSelected)
        {
            NodeDataContext.Select();
        }
        NodeDataContext.DragStart();

        _lastMousePosition = PointToScreen(e.GetPosition(this));
        CaptureMouse();
        e.Handled = true;
    }

    protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonUp(e);

        if (_isDragging)
        {
            _isDragging = false;
            NodeDataContext.DragEnd();
            ReleaseMouseCapture();
        }

        if (!_isDragged)
        {
            NodeDataContext.Select();
        }

        e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (_isDragging)
        {
            var mousePosition = PointToScreen(e.GetPosition(this));
            var offset = mousePosition - _lastMousePosition;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _lastMousePosition = mousePosition;
                NodeDataContext.DragMove(offset.X, offset.Y);
                if (offset.X != 0.0 && offset.Y != 0.0)
                    _isDragged = true;
            }
            else
            {
                _isDragging = false;
                NodeDataContext.DragEnd();
                ReleaseMouseCapture();
            }

            e.Handled = true;
        }
    }
}