using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;
using Attributes;
using ApplicationInsights;

namespace ServiceConfigurationManager
{
    public abstract class Parser
    {
        protected string documentText;
        protected string path;
        private readonly AppInsights insights = new AppInsights();

        public TOptions ReadOptions<TOptions>(string path) where TOptions : class, new()
        {
            this.path = path;
            TOptions optionsObject = new TOptions();            //Создадим объект настроек
            documentText = ReadDocument();                      //Считаем текст из файла настроек для последующего использования в парсере
            optionsObject = SetProperties(optionsObject);       //Установим свойства объекту настроек
            return optionsObject;                               //Вернем объект настроек
        }

        private TOptions SetProperties<TOptions>(TOptions optionsObject) where TOptions : class
        {
            Type optionsObjectType = optionsObject.GetType();                       //Получим тип объекта настроек
            foreach (PropertyInfo property in optionsObjectType.GetProperties())
            {
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    try
                    {
                        string attributeName = property.GetCustomAttribute<NameAttribute>(true).Name;   //Получим имя свойства по которому будем выполнять поиск в файле настроек
                        string sourceProperty = GetProperty(attributeName);                             //Получим значение свойства в формате строки
                        var newProperty = ConvertToProperty(sourceProperty, property.PropertyType);     //Переведем в формат исходного свойства
                        List<ValidationResult> results = new List<ValidationResult>();
                        ValidationContext context = new ValidationContext(optionsObject) { MemberName = property.Name };
                        if (!Validator.TryValidateProperty(newProperty, context, results))              //Проверим значение свойства на соответсвие заданным атрибутам
                        {
                            foreach (ValidationResult error in results)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                        property.SetValue(optionsObject, newProperty);      //Установим значение исходному свойству
                    }
                    catch (Exception ex)
                    {
                        //В случае возникновения исключительной ситуации запишем ее в лог и установим стандартное значение свойству
                        insights.InsertInsight(ex.Message);
                        DefaultValueAttribute defAttribute = property.GetCustomAttribute<DefaultValueAttribute>(true);
                        property.SetValue(optionsObject, defAttribute.Value);
                    }
                }
                else
                {
                    object propertyObject= Activator.CreateInstance(property.PropertyType);
                    propertyObject = SetProperties(propertyObject);
                    property.SetValue(optionsObject ,propertyObject);
                }
            }
            return optionsObject;          //Вернем объект настроек с установленными свойствами
        }

        private object ConvertToProperty(string property, Type type)
        {
            return Convert.ChangeType(property, type);
        }

        private string ReadDocument()
        {
            using (StreamReader reader = new StreamReader(path, Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }

        protected abstract string GetProperty(string name);
    }
}
