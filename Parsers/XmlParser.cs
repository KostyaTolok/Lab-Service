using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Lab2_service
{
    public class XmlParser : Parser
    {
        protected override string GetProperty(string name)
        {
            XmlReaderSettings settings = new XmlReaderSettings();                   //Создадим объект настроек XmlReader
            settings.Schemas.Add(null, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configValidation.xsd"));     //Добавим схему валидации   
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += SettingsValidationEventHandler;  //Добавим событие ошибки валидации
            using (XmlReader reader = XmlReader.Create(path))
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
