using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace CpuMonitor.Common
{
    /// <summary>
    /// Contains relevant information about names, folder, and files.
    /// </summary>
    public static class FolderNames
  {
      /// <summary>
    /// Constructor.
    /// </summary>
    static FolderNames()
    {
      var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var appDataVendorFolder = Path.Combine(appDataFolder, Titles.VendorName);
      AppDataDirectory = Path.Combine(appDataVendorFolder, Files.AppDataFolderName);

      var myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      var myDocumentsVendorFolder = Path.Combine(myDocumentsFolder, Titles.VendorName);

      // Check if folder exist and create them this is not yet the case.
      CreateFoldersIfNotExist(appDataVendorFolder,
                                       AppDataDirectory,
                                       myDocumentsVendorFolder);
    }

      /// <summary>
      /// Returns the name and path of the directory that contains the user specific application data files.
      /// </summary>
      public static string AppDataDirectory { get; }

      /// <summary>
      /// Checks each directory if it exists and creates it, if not.
      /// </summary>
      /// <param name="folderList">The list of folder names that might need to be created.</param>
      private static void CreateFoldersIfNotExist(params string[] folderList)
    {
      foreach (var dirName in folderList)
      {
        if (!Directory.Exists(dirName))
        {
          Directory.CreateDirectory(dirName);
        }
      }
    }
  }
}
