using System;
using System.IO;

namespace SPICA.Misc
{
    public class WriteManager
    {
        private static String getDesktopPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        public static StreamWriter CreateOutputFile(String OutputFileName)
        {
            String newFilePath = Path.Combine(getDesktopPath(), OutputFileName);
            return new StreamWriter(newFilePath);
        }
    }
}
