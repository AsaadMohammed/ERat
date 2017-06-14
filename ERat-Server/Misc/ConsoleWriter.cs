using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ERat.Misc.Configurations;

namespace ERat.Misc {
    /// <summary>
    /// Custom implementation that respect the "Configurations.LogLevel"
    /// </summary>
    public static class ConsoleWriter {
        public enum LogLevels {
            Normal,
            Debug,
            Error,
            Info
        }

        const ConsoleColor NormalForeground = ConsoleColor.Gray;
        const ConsoleColor DebugForeground = ConsoleColor.Green;
        const ConsoleColor ErrorForeground = ConsoleColor.Red;
        const ConsoleColor InfoForeground = ConsoleColor.Yellow;

        static void Write(string text, LogLevels level) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(DateTime.Now.ToString("[HH:mm:ss] "));

            switch (level) {
                case LogLevels.Normal:
                    Console.ForegroundColor = NormalForeground;
                    Console.WriteLine(text);
                    break;
                case LogLevels.Debug:
                    Console.ForegroundColor = DebugForeground;
                    Console.WriteLine(text);
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ErrorForeground;
                    Console.WriteLine(text);
                    break;
                case LogLevels.Info:
                    Console.ForegroundColor = InfoForeground;
                    Console.WriteLine(text);
                    break;
                default:
                    break;
            }

            Console.ResetColor();
        }

        public static void WriteNormal(string text) {
            Write(string.Format(text), LogLevels.Normal);
        }

        public static void WriteNormal(string text, object arg0) {
            Write(string.Format(text, arg0), LogLevels.Normal);
        }

        public static void WriteNormal(string text, object arg0, object arg1) {
            Write(string.Format(text, arg0, arg1), LogLevels.Normal);
        }

        public static void WriteNormal(string text, object arg0, object arg1, object arg2) {
            Write(string.Format(text, arg0, arg1, arg2), LogLevels.Normal);
        }

        public static void WriteDebug(string text) {
            Write(text, LogLevels.Debug);
        }

        public static void WriteDebug(string text, object arg0) {
            Write(string.Format(text, arg0), LogLevels.Debug);
        }

        public static void WriteDebug(string text, object arg0, object arg1) {
            Write(string.Format(text, arg0, arg1), LogLevels.Debug);
        }

        public static void WriteDebug(string text, object arg0, object arg1, object arg2) {
            Write(string.Format(text, arg0, arg1, arg2), LogLevels.Debug);
        }

        public static void WriteError(string text) {
            Write(string.Format(text), LogLevels.Error);
        }

        public static void WriteError(string text, object arg0) {
            Write(string.Format(text, arg0), LogLevels.Error);
        }

        public static void WriteError(string text, object arg0, object arg1) {
            Write(string.Format(text, arg0, arg1), LogLevels.Error);
        }

        public static void WriteError(string text, object arg0, object arg1, object arg2) {
            Write(string.Format(text, arg0, arg1, arg2), LogLevels.Error);
        }

        public static void WriteInfo(string text) {
            Write(string.Format(text), LogLevels.Info);
        }

        public static void WriteInfo(string text, object arg0) {
            Write(string.Format(text, arg0), LogLevels.Info);
        }

        public static void WriteInfo(string text, object arg0, object arg1) {
            Write(string.Format(text, arg0, arg1), LogLevels.Info);
        }

        public static void WriteInfo(string text, object arg0, object arg1, object arg2) {
            Write(string.Format(text, arg0, arg1, arg2), LogLevels.Info);
        }
    }
}
