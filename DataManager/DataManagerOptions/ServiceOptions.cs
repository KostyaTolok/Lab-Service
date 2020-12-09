using Attributes;
using System.ComponentModel;

namespace DataManager.DataManagerOptions
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
