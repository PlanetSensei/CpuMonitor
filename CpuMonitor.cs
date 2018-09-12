using System;
using System.Diagnostics;

namespace CpuMonitor
{
    /// <summary>
    /// Zusammenfassung für CPUMonitor.
    /// </summary>
    public class Monitor : IDisposable
  {
      private PerformanceCounter cpuCounter;

    public Monitor()
    {
        this.cpuCounter = new PerformanceCounter
        {
            CategoryName = "Processor",
            CounterName = "% Processor Time",
            InstanceName = "_Total"
        };
    }

    /// <summary>
    /// Call this method every time you need to know the current cpu usage.
    /// </summary>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cpu")]
    public float GetCurrentCpuUsage()
    {
      float currentValue = this.cpuCounter.NextValue();
      return currentValue;
    }

    #region IDisposable Member

    /// <summary>
    /// Gibt an, ob Dispose() vom User/ User-Code oder durch die Runtime (Destruktor) aufgerufen wurde.
    /// </summary>
    private bool m_Disposed;

    /// <summary>
    /// Implementiert IDisposable.
    /// Eine abgeleitete Klasse sollte diese Methode nicht überschreiben können
    /// und die Methode darf nicht als VIRTUAL deklariert werden.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);

      // Dieses Objekt wird durch die Dispose() Methode aufgeräumt.
      // Darum sollte GC.SupressFinalize aufgerufen werden, um dieses
      // Objekt aus der Finalization-Queue zu entfernen und zu
      // verhindern, dass der Finalization-Code für dieses Objekt ein
      // zweites Mal ausgeführt wird.
      GC.SuppressFinalize(this);
    }    

    /// <summary>
    /// Dispose(bool disposing) wird in zwei unterschiedlichen Szenarios ausgeführt.
    /// Wenn disposing == true, wurde die Methode direkt oder durch User-Code aufgerufen.
    /// Managed und unmanaged Resourcen werden freigegeben.
    /// Wenn disposing == false, wurde die Methode innerhalb des Finalizers durch die
    /// Runtime aufgerufen und sollte keine anderen Objekte mehr ansprechen.
    /// Nur unmanaged Resourcen werden freigegeben.
    /// </summary>
    /// <param name="disposing">TRUE, wenn die Methode durch User-Code aufgerufen wurde oder
    /// FALSE, wenn die Methode durch die Runtime aufgerufen wurde.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Prüfen, ob Dispose() bereits aufgerufen wurde.
      if (!this.m_Disposed)
      {
        // Wenn disposing == TRUE, gib alle managed und unmanaged Resourcen frei.
        if (disposing)
        {
            // Hier MANAGED Resourcen freigeben.
            this.cpuCounter?.Dispose();
        }

        // Hier UNMANAGED Resourcen freigeben.
        // Wenn disposing == false, wird nur der folgende Code ausgeführt.

        // Speichern, dass Dispose fertig abgearbeitet wurde.
        this.m_Disposed = true;
      }
    }

    /// <summary>
    /// Verwende die C# Destruktor-Syntax anstelle des Finalizers.
    /// Der Destruktor wird nur aufgerufen, wenn die Dispose()-Methode NICHT aufgerufen wird.
    /// Das gibt der Basisklasse die Möglichkeit zum Aufräumen.
    /// Abgeleitete Klassen dürfen keine Destruktoren implementieren.
    /// </summary>
    ~Monitor()
    {
      // Hier keine Code-Duplizierung zum Aufräumen verwenden.
      // Für bessere Lesbarkeit und Wartbarkeit soll hier stattdessen
      // nur Dispose(false) aufgerufen werden.
      this.Dispose(false);
    }

    #endregion
  }
}
