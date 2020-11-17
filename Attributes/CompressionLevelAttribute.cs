using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lab2_service
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
