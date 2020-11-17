using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_service
{
    public class Logger
    {
        private readonly object obj = new object();

        public void RecordException(string ex)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("C:\\services\\sourcelog.txt", true))
                {
                    writer.WriteLine("Возникло исключение: " + ex);
                    writer.Flush();
                }
            }
        }

        public void Record(string filePath, string fileEvent)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("C:\\services\\sourcelog.txt", true))
                {
                    writer.WriteLine(string.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
            }
        }

    }
}
