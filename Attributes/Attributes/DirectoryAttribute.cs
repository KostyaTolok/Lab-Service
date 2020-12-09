using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Attributes
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
