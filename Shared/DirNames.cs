using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CpuMonitor.Shared
{
  /// <summary>
  /// Contains relevant information about names, folder, and files.
  /// </summary>
  public static class DirNames
  {
    #region Fields

    private static readonly string appDataApplicationFolder;

    #endregion


    #region Constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    static DirNames()
    {
      string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      string appDataVendorFolder = Path.Combine(appDataFolder, Titles.VendorName);
      DirNames.appDataApplicationFolder = Path.Combine(appDataVendorFolder, Files.AppDataDirName);

      string myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string myDocumentsVendorFolder = Path.Combine(myDocumentsFolder, Titles.VendorName);

      // Check if folder exist and create them this is not yet the case.
      DirNames.createFoldersIfNotExist(appDataVendorFolder,
                                       DirNames.AppDataDirectory,
                                       myDocumentsVendorFolder);
    }

    #endregion


    #region Properties


    /// <summary>
    /// Returns the name and path of the directory that contains the user specific application data files.
    /// </summary>
    public static string AppDataDirectory
    {
      get
      {
        return DirNames.appDataApplicationFolder;
      }
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Checks each directory if it exists and creates it, if not.
    /// </summary>
    /// <param name="folderList">The list of folder names that might need to be created.</param>
    private static void createFoldersIfNotExist(params string[] folderList)
    {
      foreach (string dirName in folderList)
      {
        if (!Directory.Exists(dirName))
        {
          Directory.CreateDirectory(dirName);
        }
      }
    }

    #endregion
  }
}
