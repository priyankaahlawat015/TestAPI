﻿using Core.Constants;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Lib.Security;

/// <summary>
/// This class offer simple encryption and decryption
/// Ref: https://stackoverflow.com/a/27484425/5407188
/// </summary>
public class EncryptionHelper
{
    public EncryptionHelper()
    {
    }

    private Aes BuildAesEncryptor(string encryptionKey)
    {
        var aesEncryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        aesEncryptor.Key = pdb.GetBytes(32);
        aesEncryptor.IV = pdb.GetBytes(16);
        return aesEncryptor;
    }

    /// <summary>
    /// Encrypts the given string and return ciphertext
    /// </summary>
    public string EncryptString(string clearText, string encryptionKey)
    {
        var aesEncryptor = BuildAesEncryptor(encryptionKey);
        var clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, aesEncryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
            }
            var encryptedText = Convert.ToBase64String(ms.ToArray());
            return encryptedText;
        }
    }

    /// <summary>
    /// Decrypts the given string and returns plain text
    /// </summary>
    public string DecryptString(string cipherText, string encryptionKey)
    {
        var aesEncryptor = BuildAesEncryptor(encryptionKey);
        cipherText = cipherText.Replace(" ", "+");
        var cipherBytes = Convert.FromBase64String(cipherText);
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, aesEncryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
            }
            var clearText = Encoding.Unicode.GetString(ms.ToArray());
            return clearText;
        }
    }
}
