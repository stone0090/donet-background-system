using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Stonefw.Utility
{
    /// <summary>
    /// 连接字符串加密解密方法
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// 判断字符串是否经过加密
        /// </summary>
        /// <param name="connectString"></param>
        /// <returns></returns>
        public static bool IsEncrypted(string connectString)
        {
            if (!Regex.IsMatch(connectString, "^[0-9,a-f,A-F]{2,}$"))
                return false;
            if ((connectString.Length%2) != 0)
                return false;
            return true;
        }

        /// <summary>
        /// 加密配置信息
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Encrypt(string val)
        {
            if (string.IsNullOrEmpty(val))
                return "";
            byte[] srcData = System.Text.Encoding.UTF8.GetBytes(val);
            var destData = DesEncrypt(srcData, ConnectStringDesKey, ConnectStringDesIv);
            StringBuilder retBuffer = new StringBuilder();
            for (int i = 0; i < destData.Length; i++)
                retBuffer.Append(destData[i].ToString("x2"));
            return retBuffer.ToString();
        }

        /// <summary>
        /// 解密配置信息
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Decrypt(string val)
        {
            if (string.IsNullOrEmpty(val))
                return "";
            if (!Regex.IsMatch(val, "^[0-9,a-f,A-F]{2,}$"))
                return "";

            System.IO.MemoryStream ms = new MemoryStream(val.Length);
            int tmpIndex = 0;
            while (tmpIndex < (val.Length - 1))
            {
                ms.WriteByte(byte.Parse(val.Substring(tmpIndex, 2), System.Globalization.NumberStyles.HexNumber));
                tmpIndex += 2;
            }

            byte[] data = ms.ToArray();
            ms.Close();

            var destData = DesDecrypt(data, ConnectStringDesKey, ConnectStringDesIv);
            return System.Text.Encoding.UTF8.GetString(destData);
        }

        private static readonly byte[] ConnectStringDesKey = {0x2e, 0x3f, 0x83, 0xc9, 0x22, 0x8e, 0x92, 0x88};
        private static readonly byte[] ConnectStringDesIv = {0xc3, 0x22, 0x06, 0x9a, 0x3b, 0x52, 0x92, 0xf5};

        /// <summary>
        /// 使用DES加密
        /// </summary>
        /// <param name="srcData">源数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>密文</returns>
        private static byte[] DesEncrypt(byte[] srcData, byte[] key, byte[] iv)
        {
            //声明1个新的DES对象
            DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
            //开辟一块内存流
            MemoryStream msEncrypt = new MemoryStream();
            //把内存流对象包装成加密流对象
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(key, iv),
                CryptoStreamMode.Write);
            csEncrypt.Write(srcData, 0, srcData.Length);
            //加密流关闭
            csEncrypt.Close();
            //把内存流转换成字节数组，内存流现在已经是密文了
            byte[] bytesCipher = msEncrypt.ToArray();
            //内存流关闭
            msEncrypt.Close();

            return bytesCipher;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="cipherData">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <returns>明文</returns>
        private static byte[] DesDecrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            //声明1个新的DES对象
            DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
            //开辟一块内存流，并存放密文字节数组
            MemoryStream msCipherData = new MemoryStream(cipherData);
            MemoryStream msRawData = new MemoryStream();
            byte[] buffer = new byte[512];
            int readCount = 0;
            //把内存流对象包装成解密流对象
            CryptoStream csDecrypt = new CryptoStream(msCipherData, desDecrypt.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);
            while ((readCount = csDecrypt.Read(buffer, 0, 512)) > 0)
            {
                msRawData.Write(buffer, 0, readCount);
            }
            byte[] rawData = msRawData.ToArray();
            //解密流关闭
            csDecrypt.Close();
            //内存流关闭
            msRawData.Close();
            msCipherData.Close();

            return rawData;
        }
    }
}