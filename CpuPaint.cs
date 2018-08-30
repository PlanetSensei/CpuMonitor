using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CpuMonitor
{
  /// <summary>
  /// Zusammenfassung für CpuPaint.
  /// </summary>
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

    private Control mControl;

    private List<int> mPointList = new List<int>();
    private float mUsage = 0f;
    private Pen mGraphPen;
    private Pen mBackgroundLinesPen = new Pen(Color.DimGray, 1);

    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="pControl">Das Control, auf dem gezeichnet wird.</param>
    /// <param name="penColor">Der Pen, mit dem der Graph gezeichnet wird.</param>
    public CpuPaint(Control pControl, Pen graphPen)
    {
      mControl = pControl;
      mControl.Paint += new PaintEventHandler(mControl_Paint);

      mGraphPen = graphPen;
    }


    public void DrawGraph(float pUsage)
    {
      mUsage = pUsage;
    }


    private void drawLines(Graphics pGraphics, int pControlHeight)
    {
      float controlHeight = pGraphics.VisibleClipBounds.Height;
      float rowHeight = controlHeight / MAX_ROWS;

      Point lPointLeft;
      Point lPointRight;

      for (int i = 1; i < MAX_ROWS; i++)
      {
        int yOffset = (int)(rowHeight * i);

        lPointLeft = new Point(0, yOffset);
        lPointRight = new Point(mControl.Width, yOffset);

        pGraphics.DrawLine(mBackgroundLinesPen, lPointLeft, lPointRight);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pGraphics"></param>
    /// <param name="pPointList"></param>
    private void drawUsage(Graphics pGraphics, List<int> pPointList)
    {
      int lXCoordinate = pPointList.Count * PIX_DELTA_X_COORDS;

      if (lXCoordinate > mControl.ClientSize.Width)
      {
        pPointList.RemoveAt(0);
      }

      Point lStartPoint = new Point(0, (int)pPointList[0]);
      Point lNextPoint;

      for (int i = 0; i < mPointList.Count; i++)
      {
        lNextPoint = new Point(i * PIX_DELTA_X_COORDS, (int)pPointList[i]);
        pGraphics.DrawLine(mGraphPen, lStartPoint, lNextPoint);
        lStartPoint = lNextPoint;
      }
    }

    private void mControl_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      Region lRegion = g.Clip;
      int lHeight = mControl.ClientSize.Height;

      //g.FillRegion( new SolidBrush( Color.Black ), lRegion );

      this.drawLines(g, lHeight);

      if (lHeight != 0)
      {
        float lTempPercent = (float)lHeight / 100;
        lHeight = (int)(lTempPercent * mUsage);
      }

      int lYCoordinate = mControl.ClientSize.Height - lHeight;
      mPointList.Add(lYCoordinate);

      this.drawUsage(g, mPointList);

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
          if (this.mPointList != null)
          {
            this.mPointList.Clear();
          }

          if (this.mControl != null)
          {
            this.mControl.Dispose();
          }
          if (this.mBackgroundLinesPen != null)
          {
            this.mBackgroundLinesPen.Dispose();
          }

          if (this.mGraphPen != null)
          {
            this.mGraphPen.Dispose();
          }
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
      Dispose(false);
    }

    #endregion
  }
}
