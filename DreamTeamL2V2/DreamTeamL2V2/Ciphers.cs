using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace DreamTeamL2V2
{
    public enum CipherType { AES, DES };

    public interface ICrypto
    {
        string Encrypt(string plainText, string key);
        string Decrypt(string cipherText, string key);
    }

    public class CryptorAES : ICrypto
    {
        public string Encrypt(string plainText, string key)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);
            byte[] keyArray;

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            Aes aesAlg = Aes.Create();
            aesAlg.BlockSize = 128;
            aesAlg.Key = keyArray;
            aesAlg.IV = keyArray;
            aesAlg.Padding = PaddingMode.PKCS7;
            
            
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(toEncryptArray, 0, toEncryptArray.Length);
            csEncrypt.Close();
                        
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        public string Decrypt(string cipherText, string key)
        {
            
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherText);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            Aes aesAlg = Aes.Create();
            aesAlg.BlockSize = 128;
            aesAlg.Key = keyArray;
            aesAlg.IV = keyArray;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var msDecrypt = new MemoryStream();
            var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write);
            csDecrypt.Write(toEncryptArray, 0, toEncryptArray.Length);
            csDecrypt.Close();
            return UTF8Encoding.UTF8.GetString(msDecrypt.ToArray());

        }
    }

    public class CryptorDES : ICrypto
    {
        public string Encrypt(string plainText, string key)
        {

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);


            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }    

        public string Decrypt(string cipherText, string key)
        {

            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherText);            

            
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
           

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }

    public class ParallelCipher
    {
        public static string Encrypt(string plainText, string key, CipherType cipherType, int n)
        {
            try
            {
                int amount;
                if (plainText.Length <= 8)
                    amount = 1;
                else
                    amount = n;
                string[] textArray = new string[amount];
                int chunkSize = plainText.Length / amount;
                int pos = 0;
                for (int i = 0; i < amount - 1; ++i)
                {
                    textArray[i] = plainText.Substring(pos, chunkSize);
                    pos += chunkSize;
                }
                textArray[amount - 1] = plainText.Substring(pos);

                string[] resArray = new string[amount];
                ICrypto cryptor;
                if (cipherType == CipherType.AES)
                    cryptor = new CryptorAES();
                else
                    cryptor = new CryptorDES();

                for (int i = 0; i < amount; ++i)
                    resArray[i] = cryptor.Encrypt(textArray[i], key);
                return String.Join("", resArray);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected error: " + e.Message);
                return "";
            }
            
        }
        public static string Decrypt(string cipherText, string key, CipherType cipherType, int n)
        {
            try
            {
                int amount;
                if (cipherText.Length <= 8)
                    amount = 1;
                else
                    amount = n;
                string[] textArray = new string[amount];
                int chunkSize = cipherText.Length / amount;
                int pos = 0;
                for (int i = 0; i < amount - 1; ++i)
                {
                    textArray[i] = cipherText.Substring(pos, chunkSize);
                    pos += chunkSize;
                }
                textArray[amount - 1] = cipherText.Substring(pos);

                string[] resArray = new string[amount];
                ICrypto cryptor;
                if (cipherType == CipherType.AES)
                    cryptor = new CryptorAES();
                else
                    cryptor = new CryptorDES();

                for (int i = 0; i < amount; ++i)
                    resArray[i] = cryptor.Decrypt(textArray[i], key);
                return String.Join("", resArray);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected error: " + e.Message);
                return "";
            }
        }
    }

    public class FileCryptor
    {
        public static void EncryptFile(string fileName, string key, CipherType cipherType, int n)
        {
            try
            {
                string plainText = File.ReadAllText(fileName);
                string cipherText = ParallelCipher.Encrypt(plainText, key, cipherType, n);
                File.WriteAllText(fileName, cipherText);
                MessageBox.Show("The file has been encrypted");
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Access error");
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Didectory not found");
            }
            catch(FileNotFoundException e)
            {
                MessageBox.Show("File not found");
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected error: " + e.Message);
            }
        }
        public static void DecryptFile(string fileName, string key, CipherType cipherType, int n)
        {
            try
            {
                string cipherText = File.ReadAllText(fileName);
                string originalText = ParallelCipher.Decrypt(cipherText, key, cipherType, n);
                File.WriteAllText(fileName, originalText);
                MessageBox.Show("The file has been decrypted");
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Access error");
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Didectory not found");
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("File not found");
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected error: " + e.Message);
            }
        }
    }
}
