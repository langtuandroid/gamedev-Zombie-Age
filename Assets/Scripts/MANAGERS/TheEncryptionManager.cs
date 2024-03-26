using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TheEncryptionManager : MonoBehaviour
{
   
    public static readonly string KEY_FOR_ENCRYPTION = "nhatquanglova12344321";
    //OLD KEY : KEY_FOR_ENCRYPTION = "nhatquanglova12344321";

   
    public static  string EncryptData(string toEncrypt)
    {
#if UNITY_WP8
            return toEncrypt;
#else
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
        RijndaelManaged rDel = CreateRijndaelManaged();
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
#endif
    }

    public static  string DecryptData(string toDecrypt)
    {
      
#if UNITY_WP8
            return toDecrypt;
#else
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = CreateRijndaelManaged();
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Encoding.UTF8.GetString(resultArray);
#endif
    }

#if !UNITY_WP8
    private static RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(KEY_FOR_ENCRYPTION);
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
