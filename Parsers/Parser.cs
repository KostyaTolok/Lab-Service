using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_service
{
    public abstract class Parser
    {
        protected string documentText;
        protected string path;
        private readonly Logger logger = new Logger();

        public TOptions ReadOptions<TOptions>(string path) where TOptions : class, new()
        {
            this.path = path;
            TOptions optionsObject = new TOptions();
            documentText = ReadDocument();
            optionsObject = SetProperties(optionsObject);
            return optionsObject;
        }

        private TOptions SetProperties<TOptions>(TOptions optionsObject) where TOptions : class, new()
        {
            Type optionsObjectType = optionsObject.GetType();
            foreach (PropertyInfo property in optionsObjectType.GetProperties())
            {
                try
                {
                    string attributeName = property.GetCustomAttribute<NameAttribute>(true).Name;
                    string sourceProperty = GetProperty(attributeName);
                    var newProperty = ConvertToProperty(sourceProperty, property.PropertyType);
                    property.SetValue(optionsObject, newProperty);
                    List<ValidationResult> results = new List<ValidationResult>();
                    ValidationContext context = new ValidationContext(optionsObject) { MemberName = property.Name };
                    if (!Validator.TryValidateProperty(newProperty, context, results))
                    {
                        foreach (ValidationResult error in results)
                        {
                            throw new Exception(error.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.RecordException(ex.Message);
                    DefaultValueAttribute defAttribute = property.GetCustomAttribute<DefaultValueAttribute>(true);
                    property.SetValue(optionsObject, defAttribute.Value);
                }
            }
            return optionsObject;
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
