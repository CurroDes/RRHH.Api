using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services
{
    public class UtilService
    {
        //Clase para controlar las contraseñas, descifrarlas, cifrarlas, etc..... :)

        byte[] keyByte = null;
        public UtilService(byte[] b)
        {
            keyByte = b;
        }

        public string Cifrar(string cadena, string clave)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(cadena);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(clave);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public string DesCifrar(string cadena, string clave)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(cadena);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(clave);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }



        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Generar un IV aleatorio y derivar la clave
                aes.GenerateIV();
                aes.Key = passwordBytes;

                // Cifrar los datos
                using (MemoryStream ms = new MemoryStream())
                {
                    // Escribir el IV al principio del stream
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.FlushFinalBlock();
                    }

                    // Devolver los datos cifrados (IV + datos cifrados)
                    return ms.ToArray();
                }
            }
        }

        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extraer el IV de los primeros 16 bytes
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(bytesToBeDecrypted, 0, iv, 0, iv.Length);

                // Extraer los datos cifrados
                byte[] actualBytesToDecrypt = new byte[bytesToBeDecrypted.Length - iv.Length];
                Array.Copy(bytesToBeDecrypted, iv.Length, actualBytesToDecrypt, 0, actualBytesToDecrypt.Length);

                aes.IV = iv;
                aes.Key = passwordBytes;

                // Descifrar los datos
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(actualBytesToDecrypt, 0, actualBytesToDecrypt.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }
    }
}
