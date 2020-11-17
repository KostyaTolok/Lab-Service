using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_service
{
    public class DirectoryAttribute : ValidationAttribute
    {
        public override bool IsValid(object valueObj)
        {
            string path = valueObj as string;
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
