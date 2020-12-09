using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.DataManagerOptions
{
    public class Options
    {
        public ServiceOptions ServiceOptions { get; set; }
        public DataAccessOptions DataAccessOptions { get; set; }
        public PathOptions PathOptions { get; set; }
    }
}
