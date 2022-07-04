using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SharpBlueprints.WPF.ViewModels;

namespace SharpBlueprints.WPF.Controls;

public partial class PinControl : UserControl
{
    public PinViewModel PinDataContext
    {
        get => (PinViewModel)DataContext;
        set => DataContext = value;
    }

    public PinControl()
    {
        InitializeComponent();
        LayoutUpdated += OnLayoutUpdated;
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        var nodeGraphControl = FindGraphControl();
        if (nodeGraphControl is null)
            return;
        
        var context = PinDataContext;
        context.AbsolutePosition = Connector.TranslatePoint(
            new Point(Connector.ActualWidth * 0.5, Connector.ActualHeight * 0.5),
            nodeGraphControl);
    }

    private NodeGraphControl? FindGraphControl()
    {
        var parent = VisualTreeHelper.GetParent(this);
        while (parent is not null && parent is not NodeGraphControl)
            parent = VisualTreeHelper.GetParent(parent);
        return parent as NodeGraphControl;
    }
}