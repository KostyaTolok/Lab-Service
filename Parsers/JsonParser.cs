using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace Lab2_service
{
    public class JsonParser : Parser
    {
        protected override string GetProperty(string name)
        {
            JsonElement root = JsonDocument.Parse(documentText).RootElement;
            string property = root.GetProperty(name).GetRawText().Trim('"');
            return property;
        }
    }
}
