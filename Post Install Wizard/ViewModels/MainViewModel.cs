using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Post_Install_Wizard.Models;
using System.Collections.ObjectModel;

namespace Post_Install_Wizard.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<SoftwareItem> _generalSoftware;

    [ObservableProperty]
    private string _timeLeftText = "0:45";

    [ObservableProperty]
    private double _progressValue = 20.0; // Sample progress

    public MainViewModel()
    {
        GeneralSoftware = new ObservableCollection<SoftwareItem>
        {
            new SoftwareItem { Name = "System Utilities Pack", IsSelected = false },
            new SoftwareItem { Name = "Web Browser Suite", IsSelected = true },
            new SoftwareItem { Name = "Media Player", IsSelected = false },
            new SoftwareItem { Name = "Office Software", IsSelected = true },
            new SoftwareItem { Name = "Archive Manager", IsSelected = false },
            new SoftwareItem { Name = "Developer Tools", IsSelected = true },
            new SoftwareItem { Name = "Antivirus", IsSelected = false }
        };
    }

    [RelayCommand]
    private void SelectAll()
    {
        foreach (var item in GeneralSoftware)
        {
            item.IsSelected = true;
        }
    }

    [RelayCommand]
    private void SelectNone()
    {
        foreach (var item in GeneralSoftware)
        {
            item.IsSelected = false;
        }
    }

    [RelayCommand]
    private void BeginInstall()
    {
        // Placeholder for begin install logic
    }

    [RelayCommand]
    private void SelectDefaults()
    {
        // Placeholder
    }

    [RelayCommand]
    private void Options()
    {
        // Placeholder
    }

    [RelayCommand]
    private void Config()
    {
        // Placeholder
    }

    [RelayCommand]
    private void Manual()
    {
        // Placeholder
    }

    [RelayCommand]
    private void AboutWPI()
    {
        // Placeholder
    }
}