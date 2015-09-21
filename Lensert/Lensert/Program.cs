using System;
using System.Collections.Generic;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool IsInstanceRunning()
        {
            try
            {
                var mutex = new Mutex(false, GenerateMutexName());
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
            }
        }

        private static string GenerateMutexName()
        {
            return String.Format("Global\\{{{0}}}", ResolveAssemblyGuid());
        }

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
