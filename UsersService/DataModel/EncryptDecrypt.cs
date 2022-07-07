using System.Security.Cryptography;
using System.Text;

namespace UsersService.Api.DataModel
{
    public static class EncryptDecrypt
    {
        public static bool isTesting = false;

         public static string Decrypt(string cipherText, string password)
        {
            if(isTesting)
                return cipherText;
            string DecryptText;
            byte[] numArray = Convert.FromBase64String(cipherText);
            Aes ae = Aes.Create();
            try
            {
                byte[] array = Enumerable.ToArray<byte>(Enumerable.Take<byte>(numArray, 16));
                byte[] array1 = Enumerable.ToArray<byte>(Enumerable.Take<byte>(Enumerable.Skip<byte>(numArray, 16), 16));
                byte[] numArray1 = Enumerable.ToArray<byte>(Enumerable.Skip<byte>(numArray, 32));
                Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes(password, array, 100);
                ae.Key=(rfc2898DeriveByte.GetBytes(32));
                ae.Padding=PaddingMode.PKCS7;
                ae.Mode=CipherMode.CBC;
                ae.IV=(array1);
                MemoryStream memoryStream = new MemoryStream(numArray1);
                try
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, ae.CreateDecryptor(), 0);
                    try
                    {
                        StreamReader streamReader = new StreamReader(cryptoStream, Encoding.UTF8);
                        try
                        {
                            DecryptText = streamReader.ReadToEnd();
                        }
                        finally
                        {
                            if (streamReader != null)
                            {
                                streamReader.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        if (cryptoStream != null)
                        {
                            cryptoStream.Dispose();
                        }
                    }
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                    }
                }
            }
            finally
            {
                if (ae != null)
                {
                    ae.Dispose();
                }
            }
            return DecryptText;
        }

    }
}
