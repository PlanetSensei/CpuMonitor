using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CpuMonitor.Common;
using CpuMonitor.Configuration;
using CpuMonitor.Model;

namespace CpuMonitor
{
    /// <summary>
    /// Zusammenfassung für MainWindow.
    /// </summary>
    public class MainWindow : Form
    {
        #region Fields

        private readonly Timer _timer;
        private readonly Monitor _monitor;
        private readonly IContainer components = null;
        private readonly CpuPaint _cpuPaint;
        private Label _labelProcessor;
        private Panel _panelDisplayGraph;
        private Panel _panelLabel;

        /// <summary>
        /// Saves current property values of the main window.
        /// </summary>
        private WindowSettings _windowSettings = new WindowSettings();

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

            this._monitor = new Monitor();
            this._cpuPaint = new CpuPaint(this._panelDisplayGraph, new Pen(Color.Red, 1));

            this.FormClosing += this.MainWindowOnFormClosing;

            this._timer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };

            this._timer.Tick += this.TimerTick;
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
                    this.components?.Dispose();

                    if (this._timer != null)
                    {
                        this._timer.Stop();
                        this._timer.Enabled = false;
                        this._timer.Tick -= this.TimerTick;
                        this._timer.Dispose();
                    }

                    this._monitor?.Dispose();
                    this._labelProcessor?.Dispose();
                    this._panelDisplayGraph?.Dispose();
                    this._panelLabel?.Dispose();
                    this._cpuPaint?.Dispose();
                }

                base.Dispose(disposing);

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
            // für bessere Lesbarkeit und Wartbarkeit soll hier stattdessen
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            this._labelProcessor = new Label();
            this._panelDisplayGraph = new Panel();
            this._panelLabel = new Panel();
            this._panelDisplayGraph.SuspendLayout();
            this._panelLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _labelProcessor
            // 
            this._labelProcessor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                         | AnchorStyles.Left
                                         | AnchorStyles.Right;
            this._labelProcessor.BackColor = Color.Transparent;
            this._labelProcessor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._labelProcessor.ForeColor = Color.Green;
            this._labelProcessor.Location = new Point(0, 0);
            this._labelProcessor.Name = "_labelProcessor";
            this._labelProcessor.Size = new Size(29, 32);
            this._labelProcessor.TabIndex = 0;
            this._labelProcessor.Text = "0%";
            this._labelProcessor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _panelDisplayGraph
            // 
            this._panelDisplayGraph.BackgroundImage = ((Image)(resources.GetObject("_panelDisplayGraph.BackgroundImage")));
            this._panelDisplayGraph.BackgroundImageLayout = ImageLayout.Stretch;
            this._panelDisplayGraph.Controls.Add(this._panelLabel);
            this._panelDisplayGraph.Location = new Point(8, 7);
            this._panelDisplayGraph.Name = "_panelDisplayGraph";
            this._panelDisplayGraph.Size = new Size(194, 77);
            this._panelDisplayGraph.TabIndex = 1;
            // 
            // _panelLabel
            // 
            this._panelLabel.BackgroundImage = ((Image)(resources.GetObject("_panelLabel.BackgroundImage")));
            this._panelLabel.Controls.Add(this._labelProcessor);
            this._panelLabel.Location = new Point(1, 1);
            this._panelLabel.Name = "_panelLabel";
            this._panelLabel.Size = new Size(32, 32);
            this._panelLabel.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = Color.Black;
            this.BackgroundImage = ((Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(210, 90);
            this.Controls.Add(this._panelDisplayGraph);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Location = new Point(1050, 870);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Planet-Sensei.de - CPU Monitor";
            this.TopMost = true;
            this.Load += this.MainWindowOnLoad;
            this.LocationChanged += this.MainWindowOnLocationChanged;
            this._panelDisplayGraph.ResumeLayout(false);
            this._panelLabel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Updates the UI with the current values in the defined timer interval.
        /// </summary>
        private void TimerTick(object sender, EventArgs e)
        {
            int lUsage = (int)this._monitor.GetCurrentCpuUsage();

            this._cpuPaint.DrawGraph(lUsage);
            this._panelDisplayGraph.Invalidate();
            this._labelProcessor.Text = lUsage + "%";

            this.Invalidate(true);
        }

        /// <summary>
        /// Memorizes the new location if the windows was moved by the user.
        /// </summary>
        private void MainWindowOnLocationChanged(object sender, EventArgs e)
        {
            this._windowSettings.Location = this.Location;
        }

        /// <summary>
        /// Occurs when the form is closing, or, in this case, when the application is closing.
        /// </summary>
        private void MainWindowOnFormClosing(object sender, FormClosingEventArgs e)
        {
            this._timer.Stop();
            this._timer.Enabled = false;
            this._timer.Tick -= this.TimerTick;

            SettingsWriter.Save(_windowSettings, Files.SettingsFile, BackupOption.None);
        }

        /// <summary>
        /// Occurs when the application has started and the form is displayed for the first time.
        /// </summary>
        private void MainWindowOnLoad(object sender, EventArgs e)
        {
            this._windowSettings = SettingsReader.Read();

            Configurator.AssignSettings(this, _windowSettings);

            this._timer.Start();
        }

        #endregion Event Handlers
    }
}
