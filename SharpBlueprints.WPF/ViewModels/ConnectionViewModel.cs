using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SharpBlueprints.WPF.ViewModels;

public partial class ConnectionViewModel : ObservableObject
{
    private const double CheckpointOffset = 50.0;

    public PinViewModel PinStart
    {
        get => _pinStart;
        set
        {
            var oldPin = _pinStart;
            if (!SetProperty(ref _pinStart, value)) return;
            OnPropertyChanged(nameof(CheckpointStart));
            oldPin.PropertyChanged -= PinStartOnPropertyChanged;
        }
    }
    private PinViewModel _pinStart;

    public PinViewModel PinEnd
    {
        get => _pinEnd;
        set
        {
            var oldPin = _pinEnd;
            if (!SetProperty(ref _pinEnd, value)) return;
            OnPropertyChanged(nameof(CheckpointEnd));
            oldPin.PropertyChanged -= PinEndOnPropertyChanged;
        }
    }
    private PinViewModel _pinEnd;

    public Point CheckpointStart => new Point(PinStart.AbsolutePosition.X + CheckpointOffset, PinStart.AbsolutePosition.Y);
    public Point CheckpointEnd => new Point(PinEnd.AbsolutePosition.X - CheckpointOffset, PinEnd.AbsolutePosition.Y);

    public ConnectionViewModel(PinViewModel pinStart, PinViewModel pinEnd)
    {
        _pinStart = pinStart;
        _pinEnd = pinEnd;
        
        _pinStart.PropertyChanged += PinStartOnPropertyChanged;
        _pinEnd.PropertyChanged += PinEndOnPropertyChanged;
    }

    private void PinStartOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PinViewModel.AbsolutePosition))
            OnPropertyChanged(nameof(CheckpointStart));
    }
    
    private void PinEndOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PinViewModel.AbsolutePosition))
            OnPropertyChanged(nameof(CheckpointEnd));
    }

    public ConnectionViewModel()
    {
        _pinStart = new PinViewModel() { AbsolutePosition = new Point(10, 10)};
        _pinEnd = new PinViewModel() { AbsolutePosition = new Point(100, 150)};
    }
}