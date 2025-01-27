﻿using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MANAGERS
{
    public class EncryptionController : MonoBehaviour
    {
        public static readonly string _encryptionKey = "nhatquanglova12344321";
        //OLD KEY : KEY_FOR_ENCRYPTION = "nhatquanglova12344321";
        public static  string Encrypt(string toEncrypt)
        {
#if UNITY_WP8
            return toEncrypt;
#else
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = CreateRijndael();
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
#endif
        }

        public static  string Decrypt(string toDecrypt)
        {
      
#if UNITY_WP8
            return toDecrypt;
#else
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = CreateRijndael();
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
#endif
        }

#if !UNITY_WP8
        private static RijndaelManaged CreateRijndael()
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(_encryptionKey);
            var result = new RijndaelManaged();

            var newKeysArray = new byte[16];
            Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }
#endif
   
    }
}
