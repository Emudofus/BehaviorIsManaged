#region License GNU GPL
// Cryptography.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace BiM.Core.Cryptography
{
    public static class Cryptography
    {
        #region MD5

        /// <summary>
        ///   Get the md5 from a string
        /// </summary>
        /// <param name = "input">String input</param>
        /// <returns>MD5 Hash</returns>
        public static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        ///   Check if the given hash equals to the hash of the given string
        /// </summary>
        /// <param name = "chaine">String</param>
        /// <param name = "hash">MD5 hash to check</param>
        /// <returns></returns>
        public static bool VerifyMD5Hash(string chaine, string hash)
        {
            string hashOfInput = GetMD5Hash(chaine);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        #endregion

        #region RSA

        public static string EncryptRSA(string encryptValue, RSAParameters parameters)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(encryptValue);
            byte[] bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);

            string encryptedValue = Convert.ToBase64String(bytesEncrypted);


            return encryptedValue;
        }

        public static string DecryptRSA(byte[] encryptedValue, RSAParameters parameters)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            string decryptedValue = Encoding.UTF8.GetString(rsa.Decrypt(encryptedValue, false));

            return decryptedValue;
        }
        #endregion
    }
}