using Microsoft.EntityFrameworkCore.DataEncryption;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.DataSecurity
{
    public class Encryption : IEncryptionProvider // DB Encryption
    {
        public string Decrypt(string cipherText)
        {
            return CryptoManager.DecrypteAES(cipherText);
        }
        public string Encrypt(string cipherText)
        {
            return CryptoManager.EncryptAES(cipherText);
        }

    }

    public static class CryptoManager
    {
        private readonly static string key = "MAKV2SPBNI992121"; //Same as in Angular (16 Char)
        private readonly static byte[] keybytes= Encoding.UTF8.GetBytes(key);
        private readonly static byte[] iv= Encoding.UTF8.GetBytes(key);
        public static string DecrypteAES(string cipherText)
        {
            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            if (decriptedFromJavascript.Contains(@"[{"))
                return decriptedFromJavascript;
            return string.Format(decriptedFromJavascript);
        }
        public static string EncryptAES(string cipherText)
        {
            var encryptedFromJavascript = EncryptStringToBytes(cipherText, keybytes, iv);
            return string.Format(encryptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        private static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.  
            return Convert.ToBase64String(encrypted);
        }


        /*   
            --------------------------------------------------------------- DBCONTEXT CONFIGURATION ---------------------------------------------------------------
         
                public class CryptedDataContext : DataContext
                {
                    private readonly IEncryptionProvider _provider;
                    public CryptedDataContext(DbContextOptions<DataContext> options) : base(options)
                    {
                        this._provider = new Encryption();
                    }
                    protected override void OnModelCreating(ModelBuilder modelBuilder)
                    {
                        modelBuilder.UseEncryption(this._provider);
                        base.OnModelCreating(modelBuilder);
                    }

                }
         
         */


        /*   
            --------------------------------------------------------------- ANGULAR SIDE ---------------------------------------------------------------

            npm i crypto-js

            import* as CryptoJS from 'crypto-js';


            var key = CryptoJS.enc.Utf8.parse('MAKV2SPBNI992121');
            var iv = CryptoJS.enc.Utf8.parse('MAKV2SPBNI992121');
            var encryptedData = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse("hasan test"), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            }).toString();

            var decrypteData = CryptoJS.AES.decrypte(CryptoJS.enc.Utf8.parse("hasan test"), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
            if (decrypteData.toString())
                JSON.parse(decrypteData.toString(CryptoJS.enc.Utf8))
        */
    }
}
