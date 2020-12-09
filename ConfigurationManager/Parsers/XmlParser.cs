using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ServiceConfigurationManager
{
    public class XmlParser : Parser
    {
        private readonly string configSchemaPath;
        public XmlParser(string configSchemaPath)
        {
            this.configSchemaPath = configSchemaPath;
        }
        protected override string GetProperty(string name)
        {
            XmlReaderSettings settings;
            if (File.Exists(configSchemaPath))
            {
                settings = new XmlReaderSettings();                   //Создадим объект настроек XmlReader
                settings.Schemas.Add(null, configSchemaPath);     //Добавим схему валидации   
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += SettingsValidationEventHandler;  //Добавим событие ошибки валидации
            }
            else
            { 
                settings = null; 
            }
            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                while (reader.Read())     //Считаем xml по узлу
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == name)
                        {
                            return reader.ReadString();
                        }
                    }
                }
            }
            throw new KeyNotFoundException("Свойства с именем " + name + " не существует");
        }

        private void SettingsValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            throw new Exception(e.Message);
        }

    }
}
