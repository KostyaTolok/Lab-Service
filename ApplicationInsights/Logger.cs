using System;
using System.IO;

namespace ApplicationInsights
{
    public class Logger
    {
        private readonly object obj = new object();

        public void RecordException(string ex)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Service Controls", "ApplicationInsightsExceptions.txt"), true))
                {
                    writer.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} Exception: {ex}");
                    writer.Flush();
                }
            }
        }

        public void Record(string filePath, string fileEvent)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "sourcelog.txt"), true))
                {
                    writer.WriteLine(string.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
            }
        }


    }
}
