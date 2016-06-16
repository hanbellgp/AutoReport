using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Hanbell.AutoReport.Core
{
    public class Security
    {
        public Security() { }

        private static byte[] DecodeHexStringToByte(string hexstr)
        {
            byte[] buffer = new byte[((((int)Math.Round(Math.Round((double)((((double)hexstr.Length) / 2.0) - 1.0)))) + 1) - 1) + 1];
            int num2 = (int)Math.Round(Math.Round((double)((((double)hexstr.Length) / 2.0) - 1.0)));
            for (int i = 0; i <= num2; i++)
            {
                buffer[i] = Convert.ToByte(hexstr.Substring(i * 2, 2), 0x10);
            }
            return buffer;

        }

        public static string DecryptPassword(string secpwd)
        {
            string pwd;
            if (secpwd == "")
            {
                return "";
            }
            try
            {
                UnicodeEncoding encoding = new UnicodeEncoding();
                byte[] bytes = RSADecrypt(secpwd);
                if (bytes != null)
                {
                    return encoding.GetString(bytes);
                }
                pwd = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pwd;

        }

        private static string EncodeByteToHexString(byte[] bytedata)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytedata)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();

        }

        public static string EncryptPassword(string pwd)
        {
            string secpwd;
            if (pwd == null)
            {
                return "";
            }
            try
            {
                byte[] bytedata = RSAEncrypt(pwd);
                if (bytedata != null)
                {
                    return EncodeByteToHexString(bytedata);
                }
                secpwd = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return secpwd;

        }

        public static string EncryptWithMD5(string str)
        {
            if (str == null)
            {
                return "";
            }
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
            string str2 = "";
            short num2 = (short)(buffer.Length - 1);
            for (short i = 0; i <= num2; i = (short)(i + 1))
            {
                str2 = str2 + buffer[i].ToString("x2");
            }
            return str2;
        }

        private static void LoadRSAKey(ref RSACryptoServiceProvider rsa)
        {
            FileStream stream = new FileStream("key.xml", FileMode.Open, FileAccess.Read, FileShare.None);
            if (!stream.CanRead)
            {
                throw new FileNotFoundException();
            }
            StreamReader reader = new StreamReader(stream);
            string xmlString = reader.ReadToEnd();
            reader.Close();
            try
            {
                rsa.FromXmlString(xmlString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static byte[] RSADecrypt(string str)
        {

            byte[] buffer;
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                LoadRSAKey(ref rsa);
                buffer = rsa.Decrypt(DecodeHexStringToByte(str), false);
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
            return buffer;

        }

        private static byte[] RSAEncrypt(string str)
        {
            byte[] buffer;
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            try
            {
                CspParameters parameters;
                parameters = new CspParameters();
                parameters.KeyContainerName = "cn.sunshinning";
                parameters.Flags = parameters.Flags | CspProviderFlags.UseMachineKeyStore;

                byte[] bytes = new UnicodeEncoding().GetBytes(str);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(parameters);
                SaveRSAKey(rsa);
                buffer = rsa.Encrypt(bytes, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;

        }

        private static void SaveRSAKey(RSACryptoServiceProvider rsa)
        {
            string str = rsa.ToXmlString(true);
            FileStream stream = new FileStream("key.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            if (!stream.CanWrite)
            {
                throw new FileNotFoundException();
            }
            StreamWriter writer = new StreamWriter(stream);
            writer.Flush();
            writer.Write(str);
            writer.Flush();
            writer.Close();

        }

    }
}
