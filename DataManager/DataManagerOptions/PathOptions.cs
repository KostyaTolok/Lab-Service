using Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataManager.DataManagerOptions
{
    public class PathOptions
    {
        [Name("SourcePath")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустой путь к папке source. Установлено стандартное значение")]
        [Directory(ErrorMessage = "Неверный путь к папке source. Установлено стандартное значение")]
        [DefaultValue("C:\\SourceDirectory")]
        public string SourcePath { get; set; }

        [Name("XmlFileName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое имя файла xml. Установлено стандартное значение")]
        [DefaultValue("order")]
        public string XmlFileName { get; set; }

        [Name("XsdFileName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое имя файла xsd. Установлено стандартное значение")]
        [DefaultValue("orderSchema")]
        public string XsdFileName { get; set; }
    }
}
