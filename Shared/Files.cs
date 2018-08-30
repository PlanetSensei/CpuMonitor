using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CpuMonitor.Shared
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
    public const string AppDataDirName = "CpuMonitor";

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
          Files.fullSettingFile = Path.Combine(DirNames.AppDataDirectory, Files.SettingsFileName);
        }

        return Files.fullSettingFile;
      }
    }

    #endregion Properties
  }
}