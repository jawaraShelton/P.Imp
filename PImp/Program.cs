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
            Boolean done = false;

            while (!done)
            {
                String src = Properties.Settings.Default.ImageSource;
                String dst = Properties.Settings.Default.ImageDestination;

                Console.Clear();
                Console.WriteLine(@" __________ .___                ");
                Console.WriteLine(@" \______   \|   | _____ ______  ");
                Console.WriteLine(@"  |     ___/|   |/     \\____ \ ");
                Console.WriteLine(@"  |    |    |   |  Y Y  \  |_> >");
                Console.WriteLine(@"  |____| /\ |___|__|_|  /   __/ ");
                Console.WriteLine(@"         \/           \/|__|    ");
                Console.WriteLine("  Photo Importer");
                Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" Import From: {0}", src);
                Console.WriteLine("        To  : {0}", dst);
                Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" ");
                Console.WriteLine("     [1] Change Source");
                Console.WriteLine("     [2] Change Destination");
                Console.WriteLine("     [3] Import Photos");
                Console.WriteLine(" ");
                Console.WriteLine("     [X] Exit P.Imp");
                Console.WriteLine(" ");
                Console.Write(" Enter Selection >[ ");

                switch (Console.ReadLine().ToUpper())
                {
                    case "1":
                        Console.Write(" New Source? >[ ");
                        Properties.Settings.Default.ImageSource = Console.ReadLine();
                        Properties.Settings.Default.Save();
                        break;
                    case "2":
                        Console.Write(" New Destination? >[");
                        Properties.Settings.Default.ImageDestination = Console.ReadLine();
                        Properties.Settings.Default.Save();
                        break;
                    case "3":
                        PImp(src, dst);
                        break;
                    case "X":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("");
                        Console.WriteLine(" Option is not valid. Please select from menu.");
                        Console.WriteLine(" Press <ENTER> to retry...");
                        Console.ReadLine();
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

                    String cOut = (nbcLock + " => " + imageSortFolder + "\\" + Path.GetFileName(nbcLock));
                    if (cOut.Length > 118)
                        cOut = cOut.Substring(0, 118);

                    Console.WriteLine("{0}", cOut.PadRight(118 - cOut.Length));
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
            return((Directory.GetFiles(targetDirectory).Length + Directory.GetDirectories(targetDirectory).Length) == 0);
        }

        public static DateTime? DateTaken(String fNym)
        {
            FileInfo oFileInfo = new FileInfo(fNym);
            return oFileInfo.LastWriteTime;
        }
    }
}
