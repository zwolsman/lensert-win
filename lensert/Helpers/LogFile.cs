using System;
using System.Diagnostics;
using System.IO;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Lensert.Helpers
{
    internal static class LogFile
    {
        private const string LOG_TARGET = "logfile";

        public static void Open()
        {
            var path = GetLogFileName(LOG_TARGET);
            if (!string.IsNullOrEmpty(path))
                Process.Start(path);
        }

        private static string GetLogFileName(string targetName)
        {
            if (LogManager.Configuration == null || LogManager.Configuration.ConfiguredNamedTargets.Count == 0)
                throw new Exception("LogManager contains no Configuration or there are no named targets");

            var target = LogManager.Configuration.FindTargetByName(targetName);
            if (target == null)
                throw new Exception($"Could not find target named: {targetName}");

            var wrapperTarget = target as WrapperTargetBase;

            // Unwrap the target if necessary.
            var fileTarget = wrapperTarget == null
                ? target as FileTarget
                : wrapperTarget.WrappedTarget as FileTarget;

            if (fileTarget == null)
                throw new Exception($"Could not get a FileTarget from {target.GetType()}");

            var logEventInfo = new LogEventInfo {TimeStamp = DateTime.Now};
            var fileName = fileTarget.FileName.Render(logEventInfo);

            return File.Exists(fileName)
                ? fileName
                : string.Empty;
        }
    }
}
