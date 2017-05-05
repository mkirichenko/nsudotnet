using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Kirichenko.Nsudotnet.Enigma
{
    public class Enigma
    {
        private static readonly Dictionary<string, SymmetricAlgorithm> AvailableAlgorithms;

        static Enigma()
        {
            AvailableAlgorithms = new Dictionary<string, SymmetricAlgorithm>();
            AvailableAlgorithms.Add("AES", new AesCryptoServiceProvider());
            AvailableAlgorithms.Add("DES", new DESCryptoServiceProvider());
            AvailableAlgorithms.Add("RC2", new RC2CryptoServiceProvider());
            AvailableAlgorithms.Add("Rijndael", new RijndaelManaged());
        }

        public static SymmetricAlgorithm GetAlgotithm(string name)
        {
            try
            {
                return AvailableAlgorithms[name];
            }
            catch (KeyNotFoundException)
            {
                throw new UnsupportedAlgorithmException();
            }
        }

        public static void Encrypt(FileInfo inputFile, SymmetricAlgorithm algorithm, FileInfo outputFile)
        {

        }

        public static void Decrypt(FileInfo inputFile, SymmetricAlgorithm algorithm, FileInfo keyFile, FileInfo outputFile)
        {

        }
    }

    public class UnsupportedAlgorithmException : Exception
    {
    }
}