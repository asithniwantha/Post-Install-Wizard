using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Post_Install_Wizard.Models;

namespace Post_Install_Wizard;

public static class ConfigManager
{
    private const string ConfigFileName = "config.json";

    public static string GetConfigFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
    }

    public static ObservableCollection<SoftwareItem> LoadConfig()
    {
        var filePath = GetConfigFilePath();

        if (File.Exists(filePath))
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var items = JsonSerializer.Deserialize<ObservableCollection<SoftwareItem>>(json);
                if (items != null)
                {
                    return items;
                }
            }
            catch
            {
                // If there's an error parsing, return default/empty and perhaps log it
            }
        }

        // Return an empty collection if config doesn't exist
        return new ObservableCollection<SoftwareItem>();
    }

    public static void SaveConfig(IEnumerable<SoftwareItem> items)
    {
        var filePath = GetConfigFilePath();
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}
