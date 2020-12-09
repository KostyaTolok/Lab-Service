using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Attributes;

namespace FileManager
{
    public class PathOptions
    {
        [Name("ClientDirectoryName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое имя папки клиента. Установлено стандартное значение")]
        [DefaultValue("ClientDirectory")]
        public string ClientDirectoryName { get; set; }

        [Name("FileName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое имя файла. Установлено стандартное значение")]
        [DefaultValue("Sales")]
        public string FileName { get; set; }

        [Name("ArchiveName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустое имя архива. Установлено стандартное значение")]
        [DefaultValue("Archive")]
        public string ArchiveName { get; set; }

        [Name("SourcePath")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустой путь к папке source. Установлено стандартное значение")]
        [Directory(ErrorMessage = "Неверный путь к папке source. Установлено стандартное значение")]
        [DefaultValue("C:\\SourceDirectory")]
        public string SourcePath { get; set; }

        [Name("TargetPath")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Пустой путь к папке target. Установлено стандартное значение")]
        [Directory(ErrorMessage = "Неверный путь к папке target. Установлено стандартное значение")]
        [DefaultValue("C:\\TargetDirectory")]
        public string TargetPath { get; set; }

    }
}
