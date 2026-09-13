using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Post_Install_Wizard.Models;
using System.Collections.ObjectModel;

namespace Post_Install_Wizard.ViewModels;

using System.IO;
using System.Windows.Threading;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<SoftwareItem> _generalSoftware;

    [ObservableProperty]
    private string _timeLeftText = "0:45";

    [ObservableProperty]
    private double _progressValue = 0.0;

    private DispatcherTimer? _startupTimer;
    private int _secondsRemaining = 45;

    public MainViewModel()
    {
        var configItems = ConfigManager.LoadConfig();
        if (configItems.Count > 0)
        {
            GeneralSoftware = configItems;
        }
        else
        {
            // Default sample items if no config exists
            GeneralSoftware = new ObservableCollection<SoftwareItem>
            {
                new SoftwareItem { Name = "System Utilities Pack", IsSelected = false, FileName = "sysutil.msi", Arguments = "/quiet" },
                new SoftwareItem { Name = "Web Browser Suite", IsSelected = true, FileName = "browser.exe", Arguments = "/S" },
                new SoftwareItem { Name = "Media Player", IsSelected = false, FileName = "mediaplayer.exe", Arguments = "/silent" },
                new SoftwareItem { Name = "Office Software", IsSelected = true, FileName = "office.exe", Arguments = "/q" }
            };
        }

        StartTimer();
    }

    private void StartTimer()
    {
        _startupTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _startupTimer.Tick += StartupTimer_Tick;
        _startupTimer.Start();
        UpdateTimeLeftText();
    }

    private void StartupTimer_Tick(object? sender, EventArgs e)
    {
        if (_secondsRemaining > 0)
        {
            _secondsRemaining--;
            UpdateTimeLeftText();
        }
        else
        {
            StopTimer();
            _ = BeginInstallAsync();
        }
    }

    private void UpdateTimeLeftText()
    {
        TimeLeftText = $"0:{_secondsRemaining:D2}";
    }

    private void StopTimer()
    {
        if (_startupTimer != null)
        {
            _startupTimer.Stop();
            _startupTimer = null;
        }
    }

    [RelayCommand]
    private void CancelTimer()
    {
        StopTimer();
        TimeLeftText = "Stopped";
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
    private async Task BeginInstallAsync()
    {
        StopTimer();
        TimeLeftText = "Installing...";
        ProgressValue = 0;

        var selectedItems = GeneralSoftware.Where(x => x.IsSelected).ToList();
        if (selectedItems.Count == 0)
        {
            TimeLeftText = "Done";
            return;
        }

        var installersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Installers");
        var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install_errors.log");

        int completedCount = 0;

        foreach (var item in selectedItems)
        {
            TimeLeftText = $"Installing:\n{item.Name}";

            try
            {
                if (string.IsNullOrWhiteSpace(item.FileName))
                {
                    throw new Exception("File name is empty.");
                }

                var fullPath = Path.Combine(installersPath, item.FileName);
                if (!System.IO.File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"Installer not found: {fullPath}");
                }

                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fullPath,
                    Arguments = item.Arguments,
                    UseShellExecute = true, // Required for running .msi and other executables
                    CreateNoWindow = true
                };

                using var process = System.Diagnostics.Process.Start(processStartInfo);
                if (process != null)
                {
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Process exited with code {process.ExitCode}");
                    }
                }
                else
                {
                    throw new Exception("Failed to start process.");
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"[{DateTime.Now}] Error installing {item.Name} ({item.FileName}): {ex.Message}{Environment.NewLine}";
                System.IO.File.AppendAllText(logFilePath, errorMsg);
            }

            completedCount++;
            ProgressValue = (completedCount / (double)selectedItems.Count) * 100.0;
        }

        TimeLeftText = "Finished!";
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
        StopTimer();
        TimeLeftText = "Paused for Config";

        var configWindow = new ConfigWindow();
        configWindow.ShowDialog();

        // Reload the config after closing the window in case it was modified
        var configItems = ConfigManager.LoadConfig();
        if (configItems.Count > 0)
        {
            GeneralSoftware = configItems;
        }
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