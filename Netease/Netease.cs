using MyClassLibrary.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Netease
{
    public class Netease
    {
        private JsonData header = new JsonData();
        private JsonData cookies = new JsonData();
        private JsonData playlist_class_dict = new JsonData();
        private string username;
        private string password;
        public Netease(string username, string password)
        {
            this.cookies["appver"] = "1.5.2";
            this.username = username;
            this.password = password;
        }

        private string CreateSecretKey()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        private string AESEncrypt(string text, string seckey)
        {
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();

            byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(seckey.PadRight(bKey.Length)), bKey, bKey.Length);

            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }
        private string RSAEncrypt(string text, string pubKey)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                pubKey = rsa.ToXmlString(false);
                rsa.FromXmlString(pubKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(text);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.Accept = "*/*";
                webReq.Headers.Add("Accept-Encoding", "gzip, deflate");
                webReq.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                webReq.KeepAlive = true;
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.Host = "music.163.com";
                webReq.ContentLength = byteArray.Length;
                webReq.Referer = "http://music.163.com";
                webReq.Headers.Add("Origin", "http://music.163.com");
                webReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(newStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(paramData);
                myStreamWriter.Close();
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch
            {
            }
            return ret;
        }

        public string Login()
        {
            string modulus = "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";
            string nonce = "0CoJUm6Qyw8W8jud";
            string pubkey = "010001";

            string action = "http://music.163.com/weapi/login/?csrf_token=4d436cf2e0fed77f85139c7e885b4c17";
            JsonData login_info = new JsonData();
            login_info["username"] = this.username;
            login_info["password"] = this.password;
            login_info["rememberLogin"] = true;
            string text = JsonMapper.ToJson(login_info);
            string seckey = CreateSecretKey();
            string enctext = AESEncrypt(AESEncrypt(text, nonce), seckey);
            string encsecKey = RSAEncrypt(seckey, pubkey);
            string content = PostWebRequest(action, "params=" + "SEjMyKBI26oagBrRLEH/EMw3GWx4R9+meE7Mv5BIF6zivdGzOz7eQ+3QOXDUuJHrO7uPs60kAAi/ZKjOyUXkAW74+5DTxhJE93QJzINgZ29O320z+Oz/pCufZbVdUiQVAKXedjUuW2CmOE8XmLDw5/x/DTbqOaIPgF1gbNW+AmAg1Vrm7JN+J5TaYpOXGdljdYeFxaf7Cjc+8H1bfWjIYz5v4Dnexqr7+DiU09j4d04amVR+f6+jSB1Z6u/6elFujpoZ9w0euwDOeFkhypI4uJKL9qL1hdSggV1lhS3Q+ik=" + "&encSecKey=" + "c1dc3d97f8a31eb84da9d27c74beb80c021a282c294a881b810b130bcb0b8efa1a7cc72414dfa33fdc537910433cd15a880273887e164ebbe974f69c702901507f42e5b2f81b99b4594db6864504425da94521cdadfbcb9fae235073aa822f2d68f7665317543f5ca549ae087642b0bb97234955e207bbb3ec9aa29485790cd1", Encoding.Default);
            return content;
        }
    }
}
