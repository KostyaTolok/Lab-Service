using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_service
{
    public class ServiceOptions
    {
        [Name("CanStop")]
        [DefaultValue(true)]
        public bool CanStop { get; set; }
        [Name("CanPauseAndContinue")]
        [DefaultValue(true)]
        public bool CanPauseAndContinue { get; set; }
        [Name("AutoLog")]
        [DefaultValue(true)]
        public bool AutoLog { get; set; }
    }
}
