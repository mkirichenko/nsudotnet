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
            using (var inputStream = inputFile.OpenRead())
            {
                using (var outputStream = outputFile.OpenWrite())
                {
                    var keyFile = new FileInfo(String.Concat(outputFile.FullName, ".key.txt"));
                    if (keyFile.Exists)
                    {
                        keyFile.Delete();
                    }
                    using (var keyWriter = new StreamWriter(keyFile.Create()))
                    {
                        keyWriter.WriteLine(Convert.ToBase64String(algorithm.Key));
                        keyWriter.WriteLine(Convert.ToBase64String(algorithm.IV));
                    }
                    using (var cryptoTransform = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
                    {
                        using (var cryptoStream =
                            new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            inputStream.CopyTo(cryptoStream);
                        }
                    }
                }
            }
        }

        public static void Decrypt(FileInfo inputFile, SymmetricAlgorithm algorithm, FileInfo keyFile, FileInfo outputFile)
        {
            using (var inputStream = inputFile.OpenRead())
            {
                using (var outputStream = outputFile.OpenWrite())
                {
                    using (var keyReader = new StreamReader(keyFile.OpenRead()))
                    {
                        algorithm.Key = Convert.FromBase64String(keyReader.ReadLine());
                        algorithm.IV = Convert.FromBase64String(keyReader.ReadLine());
                    }
                    using (var cryptoTransform = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
                    {
                        using (var cryptoStream = new CryptoStream(inputStream, cryptoTransform, CryptoStreamMode.Read))
                        {
                            cryptoStream.CopyTo(outputStream);
                        }
                    }
                }
            }
        }
    }

    public class UnsupportedAlgorithmException : Exception
    {
    }
}