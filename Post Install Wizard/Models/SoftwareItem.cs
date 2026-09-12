using CommunityToolkit.Mvvm.ComponentModel;

namespace Post_Install_Wizard.Models;

public partial class SoftwareItem : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isSelected;
}