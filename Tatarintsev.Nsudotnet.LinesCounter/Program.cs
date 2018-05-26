using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatarintsev.Nsudotnet.LinesCounter
{
    class Program
    {
        static string extension;

        static void Main(string[] args)
        {
            extension = args[0];
            DirectoryInfo directory = new DirectoryInfo(@AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine(WalkDirTree(directory));
        }

        static int WalkDirTree(DirectoryInfo root)
        {
            int lines = 0;

            foreach (FileInfo files in root.GetFiles(extension))
            {
                lines += LinesCount(files);
            }

            foreach (DirectoryInfo dir in root.GetDirectories())
            {
                lines += WalkDirTree(dir);
            }
            return lines;
        }

        static int LinesCount(FileInfo file)
        {
            bool skipMode = false;
            int lines = 0;
            try
            {
                using (StreamReader sr = new StreamReader(file.Open(FileMode.Open)))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!skipMode)
                        {
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (line[i] == '/' && i < line.Length - 1)
                                {
                                    if (line[i + 1] == '*')
                                    {
                                        skipMode = true;
                                        break;
                                    }
                                    if (line[i + 1] == '/')
                                        break;
                                }
                                if (line[i] != ' ' && line[i] != '\t' && line[i] != '\n')
                                {
                                    lines++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (line.Contains("*/"))
                            {
                                skipMode = false;
                                continue;
                            }
                        }
                    }

                }
            }
            catch (IOException e) { Console.WriteLine(e); }
            return lines;
        }
    }
}
