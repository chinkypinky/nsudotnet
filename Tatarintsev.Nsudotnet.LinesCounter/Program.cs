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
        
        static int WalkDirTree(DirectoryInfo dir)
        {
            int lines = 0;
           
                foreach (System.IO.FileInfo files in dir.GetFiles(extension))
                {
               
                    Console.WriteLine(files.Name);
                    lines += LinesCount(files);
               
              }
           
            foreach (DirectoryInfo dirInfo in dir.GetDirectories())
            {
               lines+= WalkDirTree(dirInfo);
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
                        if (!skipMode) {
                            line= new String(line
                                     .Where(x => x != ' ' && x != '\r' && x != '\n')
                                     .ToArray());
                            if (line.Length == 0)
                                continue;
                            if (line.First() == '/')
                            {
                                if (line.ElementAt(1) == '*')
                                    skipMode = true;
                                continue;
                            }
                            lines++;
                        }
                        else
                        {
                            if (line.Contains("*/"))
                                skipMode = false;
                        }
                    }
                        
                }
            }catch(IOException e) { Console.WriteLine(e); }
            return lines;
        }
    }
}
