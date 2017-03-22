using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PImp
{
    class Program
    {
        static void Main(string[] args)
        {
            String targetDirectory = @"N:\Unsorted\_Images\New Folder";
            String[] folders = Directory.GetDirectories(targetDirectory);

            foreach (string nbcLock in folders)
                sortImages(nbcLock);

            sortImages(targetDirectory);
            Console.WriteLine("Waiting for you...");
            Console.ReadLine();
        }

        private static void sortImages(String targetDirectory)
        {
            String dest = @"G:\My Photography";

            Console.WriteLine("Reading in the list of files...");
            String[] files = Directory.GetFiles(targetDirectory);

            Console.WriteLine("Writing out the file list...");
            foreach (string nbcLock in files)
                if (
                    nbcLock.Substring(nbcLock.Length - 3, 3).Equals("CR2")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("AVI")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("dng")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("bmp")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("gif")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("MOV")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("JPG")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("jpg")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("MPO")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("mp4")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("xcf")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("psd") 
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("png")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("tif")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("wav")
                    || nbcLock.Substring(nbcLock.Length - 3, 3).Equals("WAV")
                    )
                {
                    DateTime? d = DateTaken(nbcLock);

                    String imageSortFolder = dest + "\\" + d.Value.Year.ToString() + "." + d.Value.Month.ToString("00") + "." + d.Value.Day.ToString("00");
                    if (!Directory.Exists(imageSortFolder))
                        Directory.CreateDirectory(imageSortFolder);

                    Console.WriteLine("{0} => {1}", nbcLock, imageSortFolder + "\\" + Path.GetFileName(nbcLock));
                    try
                    {
                        File.Move(nbcLock, imageSortFolder + "\\" + Path.GetFileName(nbcLock));
                    }
                    catch
                    {
                        // For now if the file exists (or other problem occurs) skip the file.
                    }
                }
        }

        public static DateTime? DateTaken(String fNym)
        {
            FileInfo oFileInfo = new FileInfo(fNym);
            return oFileInfo.LastWriteTime;
        }
    }
}
