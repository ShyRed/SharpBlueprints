using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SharpBlueprints.Graph;

namespace SharpBlueprints.WPF.ViewModels;

public partial class PinViewModel : ObservableObject
{
    [ObservableProperty]
    private Pin _pin;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOutgoingPin))]
    private bool _isIncomingPin;

    [ObservableProperty]
    private Point _absolutePosition;
    
    public bool IsOutgoingPin => !IsIncomingPin;

    public PinViewModel()
    {
        _pin = new Pin() { Name = "Demo Pin"};
        _isIncomingPin = true;
    }

    public PinViewModel(Pin pin, bool isIncomingPin)
    {
        _pin = pin;
        _isIncomingPin = isIncomingPin;
    }
}