using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CpuMonitor.Model;

namespace CpuMonitor.Configuration
{
    internal static class Configurator
    {
        internal static void AssignSettings(MainWindow window, WindowSettings settings)
        {
            if (!IsOnScreen(window))
            {
                settings.Location = FindScreenCenter(window);
                window.Location = settings.Location;
            }
            else
            {
                window.Location = settings.Location;
            }
        }

        private static Point FindScreenCenter(Form window)
        {
            return new Point((Screen.PrimaryScreen.Bounds.Size.Width / 2) - (window.Size.Width / 2), (Screen.PrimaryScreen.Bounds.Size.Height / 2) - (window.Size.Height / 2));
        }

        private static bool IsOnScreen(Control form)
        {
            // Create rectangle
            var formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

            // Test
            return Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(formRectangle));
        }
    }
}