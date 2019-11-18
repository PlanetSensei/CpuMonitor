using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
        private Label labelProcessor;
        private Panel panelDisplayGraph;
        private Panel panelLabel;

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
            this.mCpuPaint = new CpuPaint(this.panelDisplayGraph, new Pen(Color.Red, 1));

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
                    this.components?.Dispose();

                    if (this.mTimer != null)
                    {
                        this.mTimer.Stop();
                        this.mTimer.Enabled = false;
                        this.mTimer.Tick -= this.TimerTick;
                        this.mTimer.Dispose();
                    }

                    this.mCpuMonitor?.Dispose();
                    this.labelProcessor?.Dispose();
                    this.panelDisplayGraph?.Dispose();
                    this.panelLabel?.Dispose();
                    this.mCpuPaint?.Dispose();
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
            this.labelProcessor = new Label();
            this.panelDisplayGraph = new Panel();
            this.panelLabel = new Panel();
            this.panelDisplayGraph.SuspendLayout();
            this.panelLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProcessor
            // 
            this.labelProcessor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                         | AnchorStyles.Left
                                         | AnchorStyles.Right;
            this.labelProcessor.BackColor = Color.Transparent;
            this.labelProcessor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelProcessor.ForeColor = Color.Green;
            this.labelProcessor.Location = new Point(0, 0);
            this.labelProcessor.Name = "labelProcessor";
            this.labelProcessor.Size = new Size(29, 32);
            this.labelProcessor.TabIndex = 0;
            this.labelProcessor.Text = "0%";
            this.labelProcessor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelDisplayGraph
            // 
            this.panelDisplayGraph.BackgroundImage = ((Image)(resources.GetObject("panelDisplayGraph.BackgroundImage")));
            this.panelDisplayGraph.BackgroundImageLayout = ImageLayout.Stretch;
            this.panelDisplayGraph.Controls.Add(this.panelLabel);
            this.panelDisplayGraph.Location = new Point(8, 7);
            this.panelDisplayGraph.Name = "panelDisplayGraph";
            this.panelDisplayGraph.Size = new Size(194, 77);
            this.panelDisplayGraph.TabIndex = 1;
            // 
            // panelLabel
            // 
            this.panelLabel.BackgroundImage = ((Image)(resources.GetObject("panelLabel.BackgroundImage")));
            this.panelLabel.Controls.Add(this.labelProcessor);
            this.panelLabel.Location = new Point(1, 1);
            this.panelLabel.Name = "panelLabel";
            this.panelLabel.Size = new Size(32, 32);
            this.panelLabel.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = Color.Black;
            this.BackgroundImage = ((Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(210, 90);
            this.Controls.Add(this.panelDisplayGraph);
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
            this.panelDisplayGraph.ResumeLayout(false);
            this.panelLabel.ResumeLayout(false);
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

            this.mCpuPaint.DrawGraph(lUsage);
            this.panelDisplayGraph.Invalidate();
            this.labelProcessor.Text = lUsage + "%";

            this.Invalidate(true);
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

            if (this.windowSettings == null || !IsOnScreen(this))
            {
                this.windowSettings = new WindowSettings();
        }

            this.Location = this.windowSettings.Location;

            this.mTimer.Start();
        }

        #endregion Event Handlers

        #region Private Methods

        private static bool IsOnScreen(Form form)
        {
            // Create rectangle
            var formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

            // Test
            return Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));
        }

        #endregion Private Methods
    }
}
