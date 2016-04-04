using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoinKiteSignTest
{

    class ConkiteClient
    {

        // Certificate to verify 
        private static X509Certificate2 verifyCert = null;

        public static void setSSLCertificate()
        {

              //verifyCert = new X509Certificate2("ca-certificates.crt");
              ServicePointManager.ServerCertificateValidationCallback += new
              System.Net.Security.RemoteCertificateValidationCallback(
              customCertificateValidation);
              
        }

        public static bool customCertificateValidation(Object sender,
               X509Certificate certificate, X509Chain chain,
               SslPolicyErrors sslPolicyErrors)
        {
            //switch (sslPolicyErrors.ToString())
            //{
            //    case "RemoteCertificateChainErrors": break;
            //    //case RemoteCertificateNameMismatch: break;
            //    //case RemoteCertificateNotAvailable:
                                  
            //}
            // With this we have accepted the server certificate
            // Now, VERIFY the certificate against verifyCert
            // If details of both the certificates matches, then returns true, else false 
            //return verifyCert.Verify();
            return true;

        }



        public static async Task<WebResponse> HttpClientCallAsync(string endpoint, string pubKey, string privateKey)
        {

            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");

            string data = endpoint + "|" + time;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(privateKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(data);

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            string sign = ByteToString(hashmessage);

            sign = sign.ToLower();
 
                       
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://api.coinkite.com" + endpoint);

            myReq.Headers.Add("X-CK-Sign", sign);
            myReq.Headers.Add("X-CK-Timestamp", time);
            myReq.Headers.Add("X-CK-Key", pubKey);

            myReq.KeepAlive = true;
            myReq.Method = "GET";
            myReq.ProtocolVersion = HttpVersion.Version11;
            
            
            return await myReq.GetResponseAsync();
       


        }


        public static string CallApiGet(string endpoint, string pubKey, string privateKey)
        {

            if (endpoint == null || endpoint.Length < 3) return "";

            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");

            string data = endpoint + "|" + time;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(privateKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(data);

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            string sign = ByteToString(hashmessage);

            sign = sign.ToLower();


            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://api.coinkite.com" + endpoint);

            myReq.Headers.Add("X-CK-Sign", sign);
            myReq.Headers.Add("X-CK-Timestamp", time);
            myReq.Headers.Add("X-CK-Key", pubKey);

            myReq.KeepAlive = true;
            myReq.Method = "GET";
            myReq.ProtocolVersion = HttpVersion.Version11;
            myReq.Timeout = 15000;
            myReq.ReadWriteTimeout = 1000;
            //myReq.AllowReadStreamBuffering = true;
            //myReq.AllowWriteStreamBuffering = true;
            myReq.ContinueTimeout = 1000;
            

            try
            {
                string responce = "";

                WebResponse wr = myReq.GetResponse();

                using (StreamReader str = new StreamReader(wr.GetResponseStream()))
                {
                    responce = str.ReadToEnd();
                }

                return responce;

            }
            catch (Exception ex)
            {
                return "Communication error: " + ex.Message;
            }
            

        }


        public static string CallApiPut(string endpoint, string pubKey, string privateKey)
        {

            if (endpoint == null || endpoint.Length < 3) return "";

            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");

            string data = endpoint + "|" + time;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(privateKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(data);

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            string sign = ByteToString(hashmessage);

            sign = sign.ToLower(); //Significant 

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://api.coinkite.com" + endpoint);

            myReq.Headers.Add("X-CK-Sign", sign);
            myReq.Headers.Add("X-CK-Timestamp", time);
            myReq.Headers.Add("X-CK-Key", pubKey);

            myReq.KeepAlive = true;
            myReq.Method = "PUT";
            myReq.ProtocolVersion = HttpVersion.Version11;
            myReq.Timeout = 15000;
            myReq.ReadWriteTimeout = 1000;
            myReq.ContinueTimeout = 1000;


            try
            {
                string responce = "";

                WebResponse wr = myReq.GetResponse();

                using (StreamReader str = new StreamReader(wr.GetResponseStream()))
                {
                    responce = str.ReadToEnd();
                }

                return responce;

            }
            catch (Exception ex)
            {
                return "Communication error: " + ex.Message;
            }


        }


      

        public static string RecieveFunds(string memo, string amount, bool showPublic, bool showMemo, bool showUsername, string account,  string pubKey, string privateKey)
        {
            
            string endpoint = "/v1/new/receive";

            string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");

            string data = endpoint + "|" + time;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(privateKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(data);

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            string sign = ByteToString(hashmessage);

            sign = sign.ToLower();

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://api.coinkite.com" + endpoint);

            myReq.Headers.Add("X-CK-Sign", sign);
            myReq.Headers.Add("X-CK-Timestamp", time);
            myReq.Headers.Add("X-CK-Key", pubKey);

            myReq.KeepAlive = true;
            myReq.Method = "PUT";
            myReq.ProtocolVersion = HttpVersion.Version11;
            myReq.ContentType = "application/json";
            
            // ------

            StringBuilder strBldr = new StringBuilder();

            strBldr.Append("{\"account\": \""); strBldr.Append(account.ToLower()); strBldr.Append("\",");
            strBldr.Append("\"memo\": \""); strBldr.Append(memo); strBldr.Append("\",");
            strBldr.Append("\"amount\": \""); strBldr.Append(amount); strBldr.Append("\",");
            strBldr.Append("\"show_public\": \""); strBldr.Append(showPublic.ToString()); strBldr.Append("\",");
            strBldr.Append("\"show_memo\": \""); strBldr.Append(showMemo.ToString()); strBldr.Append("\",");
            strBldr.Append("\"show_username\": \""); strBldr.Append(showUsername.ToString()); strBldr.Append("\"}");
            
            byte[] byte1 = encoding.GetBytes(strBldr.ToString());

            myReq.ContentLength = byte1.Length;

            Stream newStream = myReq.GetRequestStream();

            newStream.Write(byte1, 0, byte1.Length);
            newStream.Close();

            // ---------------------------------------------------------------------------------------------------------------------
            
            string responce = "";

            WebResponse wr = myReq.GetResponse();

            using (StreamReader str = new StreamReader(wr.GetResponseStream()))
            {
                responce = str.ReadToEnd();
            }

            return responce;

        }


        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

       

    }
}
