using System;
using System.IO;

namespace CpuMonitor.Common
{
    /// <summary>
    /// Contains relevant information about names, folder, and files.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
    public static class FolderNames
  {
    #region Fields

    private static readonly string appDataApplicationFolder;

    #endregion


    #region Constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    static FolderNames()
    {
      string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      string appDataVendorFolder = Path.Combine(appDataFolder, Titles.VendorName);
      FolderNames.appDataApplicationFolder = Path.Combine(appDataVendorFolder, Files.AppDataFolderName);

      string myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string myDocumentsVendorFolder = Path.Combine(myDocumentsFolder, Titles.VendorName);

      // Check if folder exist and create them this is not yet the case.
      FolderNames.CreateFoldersIfNotExist(appDataVendorFolder,
                                       FolderNames.AppDataDirectory,
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
        return FolderNames.appDataApplicationFolder;
      }
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Checks each directory if it exists and creates it, if not.
    /// </summary>
    /// <param name="folderList">The list of folder names that might need to be created.</param>
    private static void CreateFoldersIfNotExist(params string[] folderList)
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
