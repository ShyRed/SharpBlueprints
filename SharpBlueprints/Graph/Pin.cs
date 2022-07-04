using CommunityToolkit.Mvvm.ComponentModel;

namespace SharpBlueprints.Graph;

public partial class Pin : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private Pin? _connectedTo;
}