using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CpuMonitor.Interfaces;
using CpuMonitor.Shared;

namespace CpuMonitor.Extensions
{
  /// <summary>
  /// Handles deep cloning on objects that implement the required interface.
  /// </summary>
  public static class DeepCloneExtension
  {
    #region Public Methods

    /// <summary>
    /// Creates a new copied instance of the given object with the same values.
    /// </summary>
    /// <param name="objectToBeCloned">The object that will be cloned.</param>
    /// <returns>Returns the cloned object.</returns>
    public static object DeepClone(this IDeepCloneable objectToBeCloned)
    {
      byte[] serializedData = DeepCloneExtension.getSerializedData(objectToBeCloned);

      // Now create the cloned object.
      object clonedObject = deserializeData(serializedData);
      return clonedObject;
    }

    /// <summary>
    /// Saves the serialized data into the given file name. If the file already exists the file will be overridden.
    /// </summary>
    /// <param name="objectToBeSaved">The object to be saved.</param>
    /// <param name="fileName">Path and file name to define the saving location.</param>
    /// <param name="saveOptions">Defines if a backup file should be created if a file with the same name already exists.</param>
    public static void Save(this IDeepCloneable objectToBeSaved, string fileName, BackupOptions saveOptions = BackupOptions.CreateBackup)
    {
      byte[] serializedData = DeepCloneExtension.getSerializedData(objectToBeSaved);

      DeepCloneExtension.backupFileIfExists(fileName, saveOptions);

      using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
      {
        fileStream.Write(serializedData, 0, serializedData.Length);
      }
    }

    /// <summary>
    /// Saves the serialized data into the given file name. If the file already exists the file will be overridden.
    /// </summary>
    /// <param name="callingInstance">The object that will be assigned the loaded values.</param>
    /// <param name="loadedInstance">Returns the object that was created using the data of the serialized file.</param>
    /// <param name="fileName">Path and file name to define the saving location.</param>
    public static void Load<T>(this IDeepCloneable callingInstance, out T loadedInstance, string fileName)
    {
      if (!(File.Exists(fileName)))
      {
        loadedInstance = default(T);
        return;
      }

      byte[] serializedData = null;
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        int length = (int)fileStream.Length;
        serializedData = new byte[length];

        fileStream.Read(serializedData, 0, length);
      }

      object objectInstance = DeepCloneExtension.deserializeData(serializedData);
      loadedInstance = (T)objectInstance;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// If the file with the given name already exists it is renamed with a ".bak" extension.
    /// </summary>
    /// <param name="fileName">Path and file name to define the saving location.</param>
    /// <param name="saveOptions">Defines if a backup file should be created if a file with the same name already exists.</param>
    private static void backupFileIfExists(string fileName, BackupOptions saveOptions)
    {
      if (saveOptions != BackupOptions.CreateBackup)
      {
        return;
      }

      if (!(File.Exists(fileName)))
      {
        return;
      }

      // Get file name
      string backupFile = string.Format("{0}.{1}", fileName, Files.BackupFileExtension);

      // Old backup file is not needed anymore.
      if (File.Exists(backupFile))
      {
        File.Delete(backupFile);
      }

      // Rename is a MOVE in Windows API
      File.Move(fileName, backupFile);
    }

    /// <summary>
    /// Deserializes the raw bytes into a new object instance.
    /// </summary>
    /// <param name="serializedData">The serialized object in raw byte form.</param>
    /// <returns>Returns the created object.</returns>
    private static object deserializeData(byte[] serializedData)
    {
      using (MemoryStream memoryStream = new MemoryStream(serializedData))
      {
        StreamingContext streamingContext = new StreamingContext(StreamingContextStates.Remoting);
        BinaryFormatter binaryFormatter = new BinaryFormatter(null, streamingContext);
        object cloneInstance = binaryFormatter.Deserialize(memoryStream);

        return cloneInstance;
      }
    }

    /// <summary>
    /// Serializes the given object.
    /// </summary>
    /// <param name="objectToBeSerialized">The object to be serialized.</param>
    /// <returns>Returns the serialized data.</returns>
    private static byte[] getSerializedData(IDeepCloneable objectToBeSerialized)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        StreamingContext streamingContext = new StreamingContext(StreamingContextStates.Remoting);
        BinaryFormatter binaryFormatter = new BinaryFormatter(null, streamingContext);

        binaryFormatter.Serialize(memoryStream, objectToBeSerialized);

        memoryStream.Seek(0, SeekOrigin.Begin);
        byte[] serializedData = memoryStream.ToArray();
        return serializedData;
      }
    }

    #endregion Private Methods
  }
}
