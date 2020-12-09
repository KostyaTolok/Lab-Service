using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Attributes;

namespace DataManager.DataManagerOptions
{
    public class DataAccessOptions
    {
        [Name("StoredProcedure")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое название хранимой процедуры. Установлено стандартное значение")]
        [DefaultValue("GetShippingData")]
        public string StoredProcedure { get; set; }

        [Name("ConnectionString")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустая строка подключения. Установлено стандартное значение")]
        [DefaultValue(@"Data Source =.\SQLEXPRESS;Initial Catalog = Northwind;Integrated Security = True")]
        public string ConnectionString { get; set; } 
    }
}
