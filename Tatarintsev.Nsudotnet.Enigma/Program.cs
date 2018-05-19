using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Tatarintsev.Nsudotnet.Enigma
{
    class Program
    {
        delegate void Algorithm();
        static string inFile;
        static string outFile;
        static void Main(string[] args)
        {
            if (args[0].Equals("encrypt"))
            {
                inFile = args[1];
                outFile = args[3];
                algChooser(args[2]);
            }
            else if (args[0].Equals("decrypt"))
            {
                inFile = args[1];
                outFile = args[4];
                algChooser(args[2], args[3]);
            }
            else
                Console.WriteLine("Wrong 1st argument. Should be encrypt or decrypt.");
        }

        static void algChooser(string algorithm)
        {
            switch (algorithm)
            {
                case ("rc2"): { var Alg = RC2.Create(); encrypt(Alg); break; }
                case ("aes"): { var Alg = Aes.Create(); encrypt(Alg); break; }
                case ("des"): { var Alg = DES.Create(); encrypt(Alg); break; }
                case ("rijndael"): { var Alg = Rijndael.Create(); encrypt(Alg); break; }
                default: { Console.WriteLine("Wrong 2nd argument. Should be rc2 or aes or des or rijndael."); break; }
            }
        }

        static void algChooser(string algorithm,string keyFile)
        {
            switch (algorithm)
            {
                case ("rc2"): { var Alg = RC2.Create(); decrypt(Alg, keyFile); break; }
                case ("aes"): { var Alg = Aes.Create(); decrypt(Alg, keyFile); break; }
                case ("des"): { var Alg = DES.Create(); decrypt(Alg, keyFile); break; }
                case ("rijndael"): { var Alg = Rijndael.Create(); decrypt(Alg, keyFile); break; }
                default: { Console.WriteLine("Wrong 2nd argument. Should be rc2 or aes or des or rijndael."); break; }
            }
        }

        static void encrypt(dynamic alg)
        {
            try
            {
                using (StreamReader sr = new StreamReader(File.Open(inFile, FileMode.Open, FileAccess.Read)))
                {
                    using (FileStream fs = File.Open(outFile, FileMode.Create))
                    {
                        using (StreamWriter keyFS = new StreamWriter(File.Create("file.key.txt")))
                        {
                            using (CryptoStream cs = new CryptoStream(fs, alg.CreateEncryptor(alg.Key, alg.IV), CryptoStreamMode.Write))
                            {
                                using (BinaryWriter bw = new BinaryWriter(cs))
                                {
                                    
                                    String line;
                                    while ((line = sr.ReadLine()) != null)
                                        bw.Write(line);
                                    keyFS.WriteLine(Convert.ToBase64String(alg.IV));
                                    keyFS.WriteLine(Convert.ToBase64String(alg.Key));
                                }
                            }
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
            }
        }

        static void decrypt(dynamic alg, string keyFile)
        {
            try
            {
                int strCount = 0;
                using (StreamReader sr = new StreamReader(File.Open(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    while (sr.ReadLine() != null)
                        strCount++;
                }

                using (FileStream fs = File.Open(inFile, FileMode.Open, FileAccess.Read))
                {
                    using (StreamWriter sw = new StreamWriter(File.Open(outFile, FileMode.Create, FileAccess.Write)))
                    {
                        using (StreamReader keyFR = new StreamReader(File.Open("file.key.txt", FileMode.Open, FileAccess.Read)))
                        {
                            byte[] IV = Convert.FromBase64String(keyFR.ReadLine());
                            byte[] key = Convert.FromBase64String(keyFR.ReadLine());
                            using (CryptoStream cs = new CryptoStream(fs, alg.CreateDecryptor(key, IV), CryptoStreamMode.Read))
                            {
                                using (BinaryReader br = new BinaryReader(cs))
                                {
                                    for(;strCount>0;strCount--)
                                        sw.WriteLine(br.ReadString());
                                }
                            }
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
            }
        }
    }
}
