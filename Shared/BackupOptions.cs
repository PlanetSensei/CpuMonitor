﻿namespace CpuMonitor.Shared
{
  public enum BackupOptions
  {
    /// <summary>
    /// No custom behavior defined.
    /// </summary>
    None,

    /// <summary>
    /// Defines if a backup file should be created if a file with the same name already exists.
    /// </summary>
    CreateBackup
  }
}