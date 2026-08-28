using System;
using System.Globalization;

namespace Task3
{
    public static class LogProcessor
    {
        public static string? ProcessLine(string line, out string? problem)
        {
            problem = null;

            if (string.IsNullOrWhiteSpace(line))
            {
                problem = line;
                return null;
            }

            if (line.Contains('|'))
            {
                // Format 2: 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства...
                string[] parts = line.Split('|', 5);
                if (parts.Length == 5)
                {
                    string dateTimePart = parts[0].Trim();
                    string levelPart = parts[1].Trim();
                    string caller = parts[3].Trim();
                    string message = parts[4].Trim();

                    string[] dt = dateTimePart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (dt.Length == 2)
                    {
                        string dateStr = dt[0];
                        string timeStr = dt[1];

                        if (DateTime.TryParseExact(dateTimePart, "yyyy-MM-dd HH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            string? level = MapLogLevel(levelPart);
                            if (level != null)
                            {
                                string callerMethod = string.IsNullOrEmpty(caller) ? "DEFAULT" : caller;
                                return $"{date:dd-MM-yyyy}\t{timeStr}\t{level}\t{callerMethod}\t{message}";
                            }
                        }
                    }
                }
            }
            else
            {
                // Format 1: 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
                string[] parts = line.Split(new[] { ' ' }, 4, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4)
                {
                    string dateStr = parts[0];
                    string timeStr = parts[1];
                    string levelStr = parts[2];
                    string message = parts[3];

                    string fullDateTimeStr = $"{dateStr} {timeStr}";
                    if (DateTime.TryParseExact(fullDateTimeStr, "dd.MM.yyyy HH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        string? level = MapLogLevel(levelStr);
                        if (level != null)
                        {
                            return $"{date:dd-MM-yyyy}\t{timeStr}\t{level}\tDEFAULT\t{message}";
                        }
                    }
                }
            }

            problem = line;
            return null;
        }

        private static string? MapLogLevel(string level)
        {
            switch (level.Trim().ToUpperInvariant())
            {
                case "INFO":
                case "INFORMATION":
                    return "INFO";
                case "WARN":
                case "WARNING":
                    return "WARN";
                case "ERROR":
                    return "ERROR";
                case "DEBUG":
                    return "DEBUG";
                default:
                    return null;
            }
        }
    }
}
