using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSALab1
{
    public class Program
    {
        public static void Main()
        {
            var keys = RSA.GetKeys(new ByteNumber(17), new ByteNumber(19)); // 17 и 19
            Console.WriteLine("Keys!");
            var codedMsg = RSA.Encrypt("my name is vlad", keys.Item1);

            Console.WriteLine(RSA.MsgToString(codedMsg));
            var decodedMsg = RSA.Decode(codedMsg, keys.Item2);


            Console.WriteLine(RSA.MsgToString(decodedMsg));


            Console.ReadLine();
        }
    }
}
