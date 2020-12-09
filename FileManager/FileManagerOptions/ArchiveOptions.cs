using System.ComponentModel;
using Attributes;

namespace FileManager
{
    public class ArchiveOptions
    {
        [Name("CompressionLevel")]
        [CompressionLevel]
        [DefaultValue("Fastest")]
        public string CompressionLevel { get; set; }
    }
}
