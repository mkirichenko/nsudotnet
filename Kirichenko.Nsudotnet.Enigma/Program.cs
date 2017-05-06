using System;
using System.IO;
using System.Security.Cryptography;

namespace Kirichenko.Nsudotnet.Enigma
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintHelp();
                return;
            }

            FileInfo inputFile;
            FileInfo outputFile;
            FileInfo keyFile;
            SymmetricAlgorithm algorithm;

            switch (args[0])
            {
                case "encrypt":
                    if (args.Length < 4)
                    {
                        PrintHelp();
                        return;
                    }
                    try
                    {
                        inputFile = new FileInfo(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("Unable to get file: {0}", args[1]);
                        return;
                    }
                    try
                    {
                        algorithm = Enigma.GetAlgotithm(args[2]);
                    }
                    catch (UnsupportedAlgorithmException)
                    {
                        Console.WriteLine("Unsupported algorithm: {0}", args[2]);
                        return;
                    }
                    try
                    {
                        outputFile = new FileInfo(args[3]);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unable to get file: {0}", args[3]);
                        return;
                    }
                    Enigma.Encrypt(inputFile, algorithm, outputFile);
                    break;
                case "decrypt":
                    if (args.Length < 5)
                    {
                        PrintHelp();
                        return;
                    }
                    try
                    {
                        inputFile = new FileInfo(args[1]);
                    }
                    catch
                    {
                        Console.WriteLine("Unable to get file: {0}", args[1]);
                        return; 
                    }
                    try
                    {
                        algorithm = Enigma.GetAlgotithm(args[2]);
                    }
                    catch (UnsupportedAlgorithmException)
                    {
                        Console.WriteLine("Unsupported algorithm: {0}", args[2]);
                        return;
                    }
                    try
                    {
                        keyFile = new FileInfo(args[3]);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unable to get file: {0}", args[3]);
                        return;
                    }
                    try
                    {
                        outputFile = new FileInfo(args[4]);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unable to get file: {0}", args[4]);
                        return;
                    }
                    Enigma.Decrypt(inputFile, algorithm, keyFile, outputFile);
                    break;
                default:
                    Console.WriteLine("Unsupported operation");
                    return;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Using:");
            Console.WriteLine("        encrypt <input file> <algorithm> <output file>");
            Console.WriteLine("        decrypt <input file> <algorithm> <key file> <output file>");
            Console.WriteLine("Supported algorithms: AES, DES, RC2, Rijndael");
        }
    }
}