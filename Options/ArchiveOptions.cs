using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab2_service
{
    public class ArchiveOptions
    {
        [Name("CompressionLevel")]
        [CompressionLevel]
        [DefaultValue("Fastest")]
        public string CompressionLevel { get; set; }
    }
}
