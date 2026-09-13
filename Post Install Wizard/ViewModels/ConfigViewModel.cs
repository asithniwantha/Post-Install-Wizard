using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Post_Install_Wizard.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace Post_Install_Wizard.ViewModels;

public partial class ConfigViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsItemSelected))]
    private SoftwareItem? _selectedSoftware;

    public bool IsItemSelected => SelectedSoftware != null;

    [ObservableProperty]
    private ObservableCollection<SoftwareItem> _softwareList;

    public ConfigViewModel()
    {
        SoftwareList = ConfigManager.LoadConfig();
    }

    [RelayCommand]
    private void AddItem()
    {
        var newItem = new SoftwareItem
        {
            Name = "New Application",
            FileName = "installer.exe",
            Arguments = "/S"
        };
        SoftwareList.Add(newItem);
        SelectedSoftware = newItem;
    }

    [RelayCommand]
    private void DeleteItem()
    {
        if (SelectedSoftware != null)
        {
            SoftwareList.Remove(SelectedSoftware);
            SelectedSoftware = null;
        }
    }

    [RelayCommand]
    private void Save()
    {
        ConfigManager.SaveConfig(SoftwareList);
        MessageBox.Show("Configuration saved.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}