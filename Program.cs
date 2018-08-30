using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CpuMonitor
{
  class Program
  {
    /// <summary>
    /// The entry point of the appliation.
    /// </summary>
    [STAThread]
    static void Main()
    {
      //Application.EnableVisualStyles();
      Application.Run(new MainWindow());
    }
  }
}
