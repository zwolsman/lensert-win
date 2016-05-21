using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Lensert
{
    internal static class Util
    {
        public static string GetLoggerPath()
        {
            var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();

            return rootAppender != null 
                ? rootAppender.File 
                : string.Empty;
        }
    }
}