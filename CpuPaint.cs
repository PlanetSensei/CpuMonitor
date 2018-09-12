using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CpuMonitor
{
  /// <summary>
  /// Zusammenfassung für CpuPaint.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cpu")]
  public sealed class CpuPaint : IDisposable
  {
    /// <summary>
    /// Anzahl der angezeigten Maßeinheiten-Zeilen.
    /// </summary>
    private const int MaxRows = 10;

    /// <summary>
    /// Anzahl der Pixel zwischen den Messpunkten im Diagrammfenster.
    /// </summary>
    private const int PixDeltaXCoords = 10;

    private readonly Control mControl;
    private readonly List<int> pointList = new List<int>();
    private readonly Pen graphPen;
    private readonly Pen mBackgroundLinesPen = new Pen(Color.DimGray, 1);

    private float currentProcessorUsage;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="hostControl">Das Control, auf dem gezeichnet wird.</param>
    /// <param name="graphPen">Der Pen, mit dem der Graph gezeichnet wird.</param>
    public CpuPaint(Control hostControl, Pen graphPen)
    {
      this.mControl = hostControl;
      this.mControl.Paint += this.ControlPaint;

      this.graphPen = graphPen;
    }


    public void DrawGraph(float pUsage)
    {
      this.currentProcessorUsage = pUsage;
    }


    private void DrawLines(Graphics graphics)
    {
      float controlHeight = graphics.VisibleClipBounds.Height;
      float rowHeight = controlHeight / MaxRows;

        for (int i = 1; i < MaxRows; i++)
      {
        int yOffset = (int)(rowHeight * i);

        var pointLeft = new Point(0, yOffset);
        var pointRight = new Point(this.mControl.Width, yOffset);

          graphics.DrawLine(this.mBackgroundLinesPen, pointLeft, pointRight);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pGraphics"></param>
    /// <param name="pPointList"></param>
    private void DrawUsage(Graphics pGraphics, List<int> pPointList)
    {
      int lXCoordinate = pPointList.Count * PixDeltaXCoords;

      if (lXCoordinate > this.mControl.ClientSize.Width)
      {
        pPointList.RemoveAt(0);
      }

      Point startPoint = new Point(0, pPointList[0]);

      for (int i = 0; i < this.pointList.Count; i++)
      {
        var nextPoint = new Point(i * PixDeltaXCoords, pPointList[i]);
        pGraphics.DrawLine(this.graphPen, startPoint, nextPoint);
        startPoint = nextPoint;
      }
    }

    private void ControlPaint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;

      var lHeight = this.mControl.ClientSize.Height;

      this.DrawLines(g);

      if (lHeight != 0)
      {
        float lTempPercent = (float)lHeight / 100;
        lHeight = (int)(lTempPercent * this.currentProcessorUsage);
      }

      int lYCoordinate = this.mControl.ClientSize.Height - lHeight;
      this.pointList.Add(lYCoordinate);

      this.DrawUsage(g, this.pointList);

    }

    #region IDisposable Member

    /// <summary>
    /// Gibt an, ob Dispose() vom User/ User-Code oder durch die Runtime (Destruktor) aufgerufen wurde.
    /// </summary>
    private bool disposed;

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
    public void Dispose(bool disposing)
    {
      // Prüfen, ob Dispose() bereits aufgerufen wurde.
      if (!this.disposed)
      {
        // Wenn disposing == TRUE, gib alle managed und unmanaged Resourcen frei.
        if (disposing)
        {
          // Hier MANAGED Resourcen freigeben.
          this.pointList?.Clear();

          this.mControl?.Dispose();
          this.mBackgroundLinesPen?.Dispose();
          this.graphPen?.Dispose();
        }

        // Hier UNMANAGED Resourcen freigeben.
        // Wenn disposing == false, wird nur der folgende Code ausgeführt.

        // Speichern, dass Dispose fertig abgearbeitet wurde.
        this.disposed = true;
      }
    }

    /// <summary>
    /// Verwende die C# Destruktor-Syntax anstelle des Finalizers.
    /// Der Destruktor wird nur aufgerufen, wenn die Dispose()-Methode NICHT aufgerufen wird.
    /// Das gibt der Basisklasse die Möglichkeit zum Aufräumen.
    /// Abgeleitete Klassen dürfen keine Destruktoren implementieren.
    /// </summary>
    ~CpuPaint()
    {
      // Hier keine Code-Duplizierung zum Aufräumen verwenden.
      // Für bessere Lesbarkeit und Wartbarkeit soll hier stattdessen
      // nur Dispose(false) aufgerufen werden.
      this.Dispose(false);
    }

    #endregion
  }
}
