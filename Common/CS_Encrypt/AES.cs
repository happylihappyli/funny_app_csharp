
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

using System.IO;


namespace CS_Encrypt {

    public class AES {
        public static string iv = "www.google.com,www.youtube.com,www.facebook.com";
        //public static void Main() {
        //    //密码
        //    string password = "1234567890123456";
        //    //加密初始化向量
        //    string message = AESEncrypt("abcdefghigklmnopqrstuvwxyz0123456789", password, iv);
        //    Console.WriteLine(message);

        //    message = AESDecrypt("8Z3dZzqn05FmiuBLowExK0CAbs4TY2GorC2dDPVlsn/tP+VuJGePqIMv1uSaVErr", password, iv);

        //    Console.WriteLine(message);
        //}


        /// <summary>
        /// AES加密 
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string text, string password) {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            System.Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;
            //rijndaelCipher.GenerateIV();

            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            byte[] ivBytes2 = new byte[16];
            for (int i = 0; i < 16; i++) {
                ivBytes2[i] = ivBytes[i];
            }
            rijndaelCipher.IV = ivBytes2;// new byte[16];

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

            byte[] plainText = Encoding.UTF8.GetBytes(text);

            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            return Convert.ToBase64String(cipherBytes);

        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string text, string password) {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(text);

            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            System.Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            //byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            byte[] ivBytes2 = new byte[16];
            for (int i = 0; i < 16; i++) {
                ivBytes2[i] = ivBytes[i];
            }
            rijndaelCipher.IV = ivBytes2;

            //rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);

        }

    }
}