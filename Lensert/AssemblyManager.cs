using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lensert
{
    internal static class AssemblyManager
    {
        public static bool IsLensertAlreadyRunning()
        {
            try
            {
                var mutex = new Mutex(false, GetMutexName());
                return !mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch
            {
                return true;
            }
        }


        private static string GetMutexName()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                var guidAttribute = (GuidAttribute)assemblyAttributes.GetValue(0);
                return $"Global\\{{{guidAttribute.Value}}}";
            }
            catch
            {
                throw new InvalidOperationException(
                    "Ensure there is a Guid attribute defined for this assembly.");
            }
        }
    }
}