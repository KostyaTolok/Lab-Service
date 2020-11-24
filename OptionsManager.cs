using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_service
{
    public class OptionsManager
    {
        private readonly string configPath;
        private readonly Parser parser;

        public OptionsManager(string configPath)
        {
            this.configPath = configPath;
            if (Path.GetExtension(configPath) == ".xml")        //Определим парсер на основе переданного пути
            {
                parser = new XmlParser();
            }
            else if (Path.GetExtension(configPath) == ".json")
            {
                parser = new JsonParser();
            }
        }

        public TOptions GetOptions<TOptions>() where TOptions : class, new()
        {
            TOptions options = parser.ReadOptions<TOptions>(configPath);          //Получим модель настроек из парсера и вернем ее
            return options;
        }

    }
}
