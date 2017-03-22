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
            UI();
        }

        static void UI()
        {
            String src = @"N:\Recovered Files\My Photos";
            String dst = @"G:\My Photography";

            Boolean done = false;

            while (!done)
            {
                Console.WriteLine("1. Edit Import File Source");
                Console.WriteLine("2. Edit Import File Destination");
                Console.WriteLine("3. Import Files from Source");
                Console.WriteLine("");
                Console.WriteLine("X. Exit Program");
                Console.WriteLine("");

                switch (Console.ReadLine().ToUpper())
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        PImp(src, dst);
                        break;
                    case "X":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Option is not valid. Please select from menu.");
                        break;
                }
            }
        }

        private static void PImp(String src, String dst)
        {
            String exts = "AVI BMP CR2 DNG GIF JPG MOV MP4 MPO PNG PSD TIF WAV XCF";
            String[] folders = Directory.GetDirectories(src);

            foreach (string nbcLock in folders)
                PImp(nbcLock, dst);

            Console.WriteLine("Reading in file list for {0}...", src);
            String[] files = Directory.GetFiles(src);

            Console.WriteLine("Importing Images...");
            foreach (string nbcLock in files)
                if (exts.Contains(nbcLock.Substring(nbcLock.Length - 3, 3).ToUpper()))
                {
                    DateTime? d = DateTaken(nbcLock);

                    String imageSortFolder = dst + "\\" + d.Value.Year.ToString() + "." + d.Value.Month.ToString("00") + "." + d.Value.Day.ToString("00");
                    if (!Directory.Exists(imageSortFolder))
                        Directory.CreateDirectory(imageSortFolder);

                    Console.WriteLine("{0} => {1}", nbcLock, imageSortFolder + "\\" + Path.GetFileName(nbcLock));
                    try
                    {
                        File.Move(nbcLock, imageSortFolder + "\\" + Path.GetFileName(nbcLock));
                    }
                    catch
                    {
                        File.Move(nbcLock, imageSortFolder + "\\" + Renamed(nbcLock));
                    }
                }

            Console.WriteLine("Checking to see if Folder is now empty...");
            if (IsEmpty(src))
            {
                Console.WriteLine("Folder is empty. Deleting.");
                Directory.Delete(src);
            }
            else
                Console.WriteLine("Folder is not empty. Leaving alone.");
        }

        private static String Renamed(String nbcLock)
        {
            int fLen = Path.GetFileName(nbcLock).Length;
            String fExt = Path.GetFileName(nbcLock).Substring(fLen - 3, 3);
            String fNym = Path.GetFileName(nbcLock).Substring(0, fLen - 5);
            String fDst = fNym + DateTime.Now.ToString("msf") + "." + fExt;

            return(fDst);
        }

        private static Boolean IsEmpty(String targetDirectory)
        {
            String[] fEmpty = Directory.GetFiles(targetDirectory);
            String[] dEmpty = Directory.GetDirectories(targetDirectory);

            return((fEmpty.Length + dEmpty.Length) == 0);
        }

        public static DateTime? DateTaken(String fNym)
        {
            FileInfo oFileInfo = new FileInfo(fNym);
            return oFileInfo.LastWriteTime;
        }
    }
}
