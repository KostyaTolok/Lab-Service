using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Lab2_service
{
    public class Service
    {
        private readonly FileSystemWatcher watcher;
        private bool isEnabled = true;
        private readonly Options options;
        private readonly Logger logger;

        public Service(Options options)
        {
            try
            {
                this.options = options;
                logger = new Logger();
                watcher = new FileSystemWatcher(this.options.PathOptions.SourcePath);
                watcher.Created += Watcher_Created;
                watcher.Deleted += Watcher_Deleted;
                watcher.Changed += Watcher_Changed;
                watcher.Renamed += Watcher_Renamed;
            }
            catch (Exception ex)
            {
                logger.RecordException(ex.Message);
            }
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (isEnabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            isEnabled = false;
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            logger.Record(e.OldFullPath, "переименован в " + e.FullPath);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            logger.Record(e.FullPath, "изменен");
            Watcher_Created(sender, e);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            logger.Record(e.FullPath, "удален");
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (Path.GetExtension(e.FullPath) == ".txt")
                {
                    FileInfo file = new FileInfo(e.FullPath);
                    string targetFile = Path.Combine(options.PathOptions.TargetPath, options.PathOptions.ClientDirectoryName, string.Format("{0:yyyy}\\{0:MM}\\{0:dd}", file.CreationTime));
                    if (!Directory.Exists(targetFile))
                    {
                        Directory.CreateDirectory(targetFile);
                    }
                    targetFile = Path.Combine(targetFile, string.Format(options.PathOptions.FileName + "_{0:yyyy}_{0:MM}_{0:dd}_{0:HH}_{0:mm}_{0:ss}.txt", file.CreationTime));
                    EncryptFile(e.FullPath, targetFile);
                    string newSource = Path.ChangeExtension(targetFile, ".gz");
                    CompressFile(targetFile, newSource);
                    File.Delete(targetFile);
                    string finalTarget = Path.Combine(Path.GetDirectoryName(targetFile), options.PathOptions.ArchiveName);
                    if (!Directory.Exists(finalTarget))
                    {
                        Directory.CreateDirectory(finalTarget);
                    }
                    finalTarget = Path.Combine(finalTarget, Path.ChangeExtension(Path.GetFileName(newSource), ".txt"));
                    DecompressFile(newSource, finalTarget);
                    DecryptFile(finalTarget, finalTarget);
                }
                logger.Record(e.FullPath, "создан");
            }
            catch (Exception ex)
            {
                logger.RecordException(ex.Message);
            }
        }

        private void EncryptFile(string sourcePath, string targetPath)
        {
            byte[] data;
            using (StreamReader reader = new StreamReader(sourcePath))
            {
                data = Encoding.ASCII.GetBytes(reader.ReadToEnd());
            }
            FileStream target;
            if (File.Exists(targetPath))
            {
                target = new FileStream(targetPath, FileMode.OpenOrCreate);
            }
            else
            {
                target = File.Create(targetPath);
            }
            using (target)
            {
                Aes aes = Aes.Create();
                byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                using (CryptoStream cryptoStream = new CryptoStream(target, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }
            }
        }

        private void CompressFile(string sourcePath, string targetPath)
        {
            CompressionLevel level;
            switch (options.ArchiveOptions.CompressionLevel)
            {
                case "Fastest":
                    level = CompressionLevel.Fastest;
                    break;
                case "Optimal":
                    level = CompressionLevel.Optimal;
                    break;
                case "NoCompression":
                    level = CompressionLevel.NoCompression;
                    break;
                default:
                    level = CompressionLevel.Fastest;
                    break;
            }
            using (FileStream source = new FileStream(sourcePath, FileMode.OpenOrCreate))
            {
                using (FileStream target = File.Create(targetPath))
                {
                    using (GZipStream compression = new GZipStream(target, level))
                    {
                        source.CopyTo(compression);
                    }
                }
            }
        }

        private void DecompressFile(string sourcePath, string targetPath)
        {
            using (FileStream source = new FileStream(sourcePath, FileMode.OpenOrCreate))
            {
                using (FileStream target = File.Create(targetPath))
                {
                    using (GZipStream decompressionStream = new GZipStream(source, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(target);
                    }
                }
            }
        }

        private void DecryptFile(string sourcePath, string targetPath)
        {
            using (FileStream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                if (!File.Exists(targetPath))
                {
                    File.Create(targetPath);
                }
                string data;
                Aes aes = Aes.Create();
                byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                using (CryptoStream cryptoStream = new CryptoStream(source, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        data = reader.ReadToEnd();
                    }
                }
                using (StreamWriter writer = new StreamWriter(targetPath, false))
                {
                    writer.Write(data);
                }
            }
        }

    }
}
