using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CpuMonitor.Common;
using CpuMonitor.Extensions;
using CpuMonitor.Model;

namespace CpuMonitor
{
  /// <summary>
  /// Zusammenfassung für MainWindow.
  /// </summary>
  public class MainWindow : Form
  {
    #region Fields

    private readonly Timer mTimer;
    private readonly Monitor mCpuMonitor;
    private readonly IContainer components = null;
    private readonly CpuPaint mCpuPaint;
    private Label mLabCpu;
    private Panel mPnlDisplayGraph;
    private Panel mPnlLabel;

    /// <summary>
    /// Saves current property values of the main window.
    /// </summary>
    private WindowSettings windowSettings = new WindowSettings();

    #endregion Fields

    #region Constructor

    /// <inheritdoc />
    public MainWindow()
    {
      //
      // Erforderlich für die Windows Form-Designerunterstützung
      //
      this.InitializeComponent();

      this.Text = $"{Titles.ApplicationTitle} - {Titles.VendorName}";

            this.mCpuMonitor = new Monitor();
      this.mCpuPaint = new CpuPaint(this.mPnlDisplayGraph, new Pen(Color.Red, 1));

      this.FormClosing += this.MainWindowOnFormClosing;

        this.mTimer = new Timer
        {
            Interval = 1000,
            Enabled = true
        };

        this.mTimer.Tick += this.TimerTick;

    }

    #endregion Constructor

    #region IDisposable Member

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
    protected override void Dispose(bool disposing)
    {
      // Prüfen, ob Dispose() bereits aufgerufen wurde.
      if (!this.Disposing)
      {
        // Wenn disposing == TRUE, gib alle managed und unmanaged Resourcen frei.
        if (disposing)
        {
          if (this.components != null)
          {
            this.components.Dispose();
          }

          if (this.mTimer != null)
          {
            this.mTimer.Stop();
            this.mTimer.Enabled = false;
            this.mTimer.Tick -= this.TimerTick;
            this.mTimer.Dispose();
          }

          if (this.mCpuMonitor != null)
          {
            this.mCpuMonitor.Dispose();
          }

          if (this.mLabCpu != null)
          {
            this.mLabCpu.Dispose();
          }

          if (this.mPnlDisplayGraph != null)
          {
            this.mPnlDisplayGraph.Dispose();
          }

          if (this.mPnlLabel != null)
          {
            this.mPnlLabel.Dispose();
          }

          if (this.mCpuPaint != null)
          {
            this.mCpuPaint.Dispose();
          }
        }

        base.Dispose( disposing );

        // Hier UNMANAGED Resourcen freigeben.
        // Wenn disposing == false, wird nur der folgende Code ausgeführt.
      }
    }

    /// <summary>
    /// Verwende die C# Destruktor-Syntax anstelle des Finalizers.
    /// Der Destruktor wird nur aufgerufen, wenn die Dispose()-Methode NICHT aufgerufen wird.
    /// Das gibt der Basisklasse die Möglichkeit zum Aufräumen.
    /// Abgeleitete Klassen dürfen keine Destruktoren implementieren.
    /// </summary>
    ~MainWindow()
    {
      // Hier keine Code-Duplizierung zum Aufräumen verwenden.
      // Für bessere Lesbarkeit und Wartbarkeit soll hier stattdessen
      // nur Dispose(false) aufgerufen werden.
      this.Dispose(false);
    }

    #endregion


    #region Vom Windows Form-Designer generierter Code
    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.mLabCpu = new System.Windows.Forms.Label();
      this.mPnlDisplayGraph = new System.Windows.Forms.Panel();
      this.mPnlLabel = new System.Windows.Forms.Panel();
      this.mPnlDisplayGraph.SuspendLayout();
      this.mPnlLabel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mLabCpu
      // 
      this.mLabCpu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mLabCpu.BackColor = System.Drawing.Color.Transparent;
      this.mLabCpu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mLabCpu.ForeColor = System.Drawing.Color.Green;
      this.mLabCpu.Location = new System.Drawing.Point(0, 0);
      this.mLabCpu.Name = "mLabCpu";
      this.mLabCpu.Size = new System.Drawing.Size(29, 32);
      this.mLabCpu.TabIndex = 0;
      this.mLabCpu.Text = "0%";
      this.mLabCpu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // mPnlDisplayGraph
      // 
      this.mPnlDisplayGraph.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPnlDisplayGraph.BackgroundImage")));
      this.mPnlDisplayGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.mPnlDisplayGraph.Controls.Add(this.mPnlLabel);
      this.mPnlDisplayGraph.Location = new System.Drawing.Point(8, 7);
      this.mPnlDisplayGraph.Name = "mPnlDisplayGraph";
      this.mPnlDisplayGraph.Size = new System.Drawing.Size(194, 77);
      this.mPnlDisplayGraph.TabIndex = 1;
      // 
      // mPnlLabel
      // 
      this.mPnlLabel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPnlLabel.BackgroundImage")));
      this.mPnlLabel.Controls.Add(this.mLabCpu);
      this.mPnlLabel.Location = new System.Drawing.Point(1, 1);
      this.mPnlLabel.Name = "mPnlLabel";
      this.mPnlLabel.Size = new System.Drawing.Size(32, 32);
      this.mPnlLabel.TabIndex = 1;
      // 
      // MainWindow
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.Color.Black;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(210, 90);
      this.Controls.Add(this.mPnlDisplayGraph);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Location = new System.Drawing.Point(1050, 870);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MainWindow";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Planet-Sensei.de - CPU Monitor";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.MainWindowOnLoad);
      this.LocationChanged += new System.EventHandler(this.MainWindowOnLocationChanged);
      this.mPnlDisplayGraph.ResumeLayout(false);
      this.mPnlLabel.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    #region Event Handlers

    /// <summary>
    /// Updates the UI with the current values in the defined timer interval.
    /// </summary>
    private void TimerTick(object sender, EventArgs e)
    {			
      int lUsage = (int)this.mCpuMonitor.GetCurrentCpuUsage();

      this.mCpuPaint.DrawGraph( lUsage );
      this.mPnlDisplayGraph.Invalidate();
      this.mLabCpu.Text = lUsage.ToString() + "%";

      // Should not be needed anymore...?
      //Application.DoEvents();
      //GC.Collect();

      this.Invalidate(true);
    }

    /// <summary>
    /// Signals the application to close.
    /// </summary>
    private void BtnExitOnClick(object sender, EventArgs e)
    {
      Application.Exit();
    }

    /// <summary>
    /// Memorizes the new location if the windows was moved by the user.
    /// </summary>
    private void MainWindowOnLocationChanged(object sender, EventArgs e)
    {
      this.windowSettings.Location = this.Location;
    }

    /// <summary>
    /// Occurs when the form is closing, or, in this case, when the application is closing.
    /// </summary>
    private void MainWindowOnFormClosing(object sender, FormClosingEventArgs e)
    {
      this.mTimer.Stop();
      this.mTimer.Enabled = false;
      this.mTimer.Tick -= this.TimerTick;

      this.windowSettings.Save(Files.SettingsFile, BackupOption.None);
    }

    /// <summary>
    /// Occurs when the application has started and the form is displayed for the first time.
    /// </summary>
    private void MainWindowOnLoad(object sender, EventArgs e)
    {
      this.windowSettings.Load(out this.windowSettings, Files.SettingsFile);

      if (this.windowSettings == null)
      {
        this.windowSettings = new WindowSettings();
      }
      
      this.Location = this.windowSettings.Location;

      this.mTimer.Start();
    }

    #endregion Event Handlers
  }
}
