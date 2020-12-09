using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    public class CompressionLevelAttribute : ValidationAttribute
    {
        public override bool IsValid(object valueObj)
        {
            string value = valueObj as string;
            switch(value)
            {
                case "Fastest":
                    break;
                case "Optimal":
                    break;
                case "NoCompression":
                    break;
                default:
                    ErrorMessage = "Неверный уровень компрессии. Установлено стандартое значение";
                    return false;
            }
            return true;
        }
    }
}
