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

        public static StreamWriter CreateoutputFile(String outputFileName)
        {
            String newFilePath = Path.Combine(getDesktopPath(), outputFileName);
            return new StreamWriter(newFilePath);
        }
    }
}
