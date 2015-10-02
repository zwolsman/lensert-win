using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            if (IsInstanceRunning())
                return;

            if (Preferences.Default.StartupOnLogon)
            {   //REFACTOR to settings UI
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                var location = Assembly.GetExecutingAssembly().Location;

                using (var streamWriter = new StreamWriter(Path.Combine(directory, "Lensert.url")))
                {
                    streamWriter.WriteLine("[InternetShortcut]");
                    streamWriter.WriteLine("URL=file:///" + location);
                    streamWriter.WriteLine("IconIndex=0");
                    streamWriter.WriteLine("IconFile=" + location);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PreferencesForm());
        }

        private static bool IsInstanceRunning()
        {
            try
            {
                var mutex = new Mutex(false, MutexName());
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
            }
        }

        private static string MutexName() => $"Global\\{{{ResolveAssemblyGuid()}}}";

        private static string ResolveAssemblyGuid()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                var guidAttribute = (GuidAttribute)assemblyAttributes.GetValue(0);
                return guidAttribute.Value;
            }
            catch
            {
                throw new InvalidOperationException(
                    "Ensure there is a Guid attribute defined for this assembly.");
            }
        }
    }
}
