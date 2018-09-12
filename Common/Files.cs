using System.IO;

namespace CpuMonitor.Common
{
  /// <summary>
  /// Contains names relevant to file handling.
  /// </summary>
  public static class Files
  {
    #region Fields

    /// <summary>
    /// File extension for backup files WITHOUT the dot.
    /// </summary>
    public const string BackupFileExtension = "bak";

    /// <summary>
    /// The folder name of the application.
    /// </summary>
    public const string AppDataFolderName = "CpuMonitor";

    /// <summary>
    /// The settings file name.
    /// </summary>
    private const string SettingsFileName = "Settings.config";

    /// <summary>
    /// Contains the assembled path and file name for the settings file.
    /// </summary>
    private static string fullSettingFile;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets the combined path and file name for the settings file.
    /// </summary>
    public static string SettingsFile
    {
      get
      {
        if (string.IsNullOrEmpty(fullSettingFile))
        {
          fullSettingFile = Path.Combine(FolderNames.AppDataDirectory, SettingsFileName);
        }

        return fullSettingFile;
      }
    }

    #endregion Properties
  }
}