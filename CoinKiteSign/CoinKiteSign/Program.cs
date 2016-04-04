using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinKiteSignTest;

namespace CoinKiteSign
{
    class Program
    {
        static void Main(string[] args)
        {

            test();

            Console.ReadLine();


        }

        static void test()
        {

            CoinKiteSignTest.ConkiteClient.setSSLCertificate();

            string s = ConkiteClient.CallApiGet("/v1/my/self", "key", "key");
            Console.WriteLine(s);

            //string s2 = ConkiteClient.RecieveFunds("test receive + " + System.DateTime.Now.ToString(), "0.01", true, true, true, "----");
            //Console.WriteLine(s2);

            Console.ReadLine();


        }
    }
}
