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
    public class CpuPaint : IDisposable
  {
    /// <summary>
    /// Anzahl der angezeigten Maßeinheiten-Zeilen.
    /// </summary>
    private const int MAX_ROWS = 10;

    /// <summary>
    /// Anzahl der Pixel zwischen den Messpunkten im Diagrammfenster.
    /// </summary>
    private const int PIX_DELTA_X_COORDS = 10;

    private readonly Control mControl;
    private readonly List<int> pointList = new List<int>();
    private readonly Pen mGraphPen;
    private readonly Pen mBackgroundLinesPen = new Pen(Color.DimGray, 1);

    private float mUsage = 0f;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="pControl">Das Control, auf dem gezeichnet wird.</param>
    /// <param name="penColor">Der Pen, mit dem der Graph gezeichnet wird.</param>
    public CpuPaint(Control pControl, Pen graphPen)
    {
      this.mControl = pControl;
      this.mControl.Paint += new PaintEventHandler(this.ControlPaint);

      this.mGraphPen = graphPen;
    }


    public void DrawGraph(float pUsage)
    {
      this.mUsage = pUsage;
    }


    private void DrawLines(Graphics pGraphics, int pControlHeight)
    {
      float controlHeight = pGraphics.VisibleClipBounds.Height;
      float rowHeight = controlHeight / MAX_ROWS;

      Point lPointLeft;
      Point lPointRight;

      for (int i = 1; i < MAX_ROWS; i++)
      {
        int yOffset = (int)(rowHeight * i);

        lPointLeft = new Point(0, yOffset);
        lPointRight = new Point(this.mControl.Width, yOffset);

        pGraphics.DrawLine(this.mBackgroundLinesPen, lPointLeft, lPointRight);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pGraphics"></param>
    /// <param name="pPointList"></param>
    private void DrawUsage(Graphics pGraphics, List<int> pPointList)
    {
      int lXCoordinate = pPointList.Count * PIX_DELTA_X_COORDS;

      if (lXCoordinate > this.mControl.ClientSize.Width)
      {
        pPointList.RemoveAt(0);
      }

      Point lStartPoint = new Point(0, (int)pPointList[0]);
      Point lNextPoint;

      for (int i = 0; i < this.pointList.Count; i++)
      {
        lNextPoint = new Point(i * PIX_DELTA_X_COORDS, (int)pPointList[i]);
        pGraphics.DrawLine(this.mGraphPen, lStartPoint, lNextPoint);
        lStartPoint = lNextPoint;
      }
    }

    private void ControlPaint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      Region lRegion = g.Clip;
      int lHeight = this.mControl.ClientSize.Height;

      //g.FillRegion( new SolidBrush( Color.Black ), lRegion );

      this.DrawLines(g, lHeight);

      if (lHeight != 0)
      {
        float lTempPercent = (float)lHeight / 100;
        lHeight = (int)(lTempPercent * this.mUsage);
      }

      int lYCoordinate = this.mControl.ClientSize.Height - lHeight;
      this.pointList.Add(lYCoordinate);

      this.DrawUsage(g, this.pointList);

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
    protected void Dispose(bool disposing)
    {
      // Prüfen, ob Dispose() bereits aufgerufen wurde.
      if (!this.m_Disposed)
      {
        // Wenn disposing == TRUE, gib alle managed und unmanaged Resourcen frei.
        if (disposing)
        {
          // Hier MANAGED Resourcen freigeben.
          this.pointList?.Clear();

          this.mControl?.Dispose();
          this.mBackgroundLinesPen?.Dispose();
          this.mGraphPen?.Dispose();
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
