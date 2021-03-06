using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace RSALab1
{
    /// <summary>
    /// Инструменты и функции для RSA
    /// </summary>
    public class RSA
    {
        /// <summary> Список простых чисел до 1000  </summary>
        public static List<int> Primes { get; private set; }

        static RSA(){Primes = new List<int>();}

        /// <summary>
        /// Вспомогательный метод для установки простых чисел до 1000. Нужен для отбора е. В реализации текущей НЕ ИСПОЛЬЗУЕТСЯ!!
        /// </summary>
        public static void SetPrimes()
        {
            Primes = new List<int>();
            int start = 2;
            int end = 1000;
            for (int i = start; i <= end; i++)
            {
                bool isPrime = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                        isPrime = false;
                }
                if (isPrime) Primes.Add(i);
            }
        }

        /// <summary> Аогоритм Евлида. Реализация через остатки от деления. </summary>
        /// <returns>Наибольший общий делитель a и b</returns>
        public static ByteNumber Gcd(ByteNumber fn, ByteNumber sn)
        {
            var a = new ByteNumber(fn);
            var b = new ByteNumber(sn);
            var zero = new ByteNumber(0);
            while (a != zero && b != zero)
            {
                if (a > b) a = a % b;
                else b = b % a;
            }
            //while (b != zero)
            //    b = a % (a = b);
            return a+b;
        }

        /// <summary> Расширенный алгоритм Евклида.</summary>
        /// <param name="fn"> число a</param>
        /// <param name="sn"> число b</param>
        /// <param name="x"> Коэффициент Безу - возвращается по ссылке</param>
        /// <param name="y"> Коэффициент Безу - возвращается по ссылке</param>
        /// <returns>НОД fn и sn. Коэффициенты Безу x, y из ax+by = gcd(a,b) возвращаются по ссылке</returns>
        public static ByteNumber gcdExtended(ByteNumber fn, ByteNumber sn, ref ByteNumber x, ref ByteNumber y)
        {
            var a = new ByteNumber(fn);
            var b = new ByteNumber(sn);
            var zero = new ByteNumber(0);
            if (a == zero)
            {
                x = new ByteNumber(0); 
                y = new ByteNumber(1);
                return b;
            }
            var x1 = new ByteNumber(0);
            var y1 = new ByteNumber(0);

            var d = gcdExtended(b % a, a, ref x1, ref y1);

            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }


        /// <summary>Получить ключи для RSA шифрования</summary>
        /// <param name="p">Простое число</param>
        /// <param name="q">Простое число</param>
        /// <returns>Две пары чисел. 1 - открытый ключ {e,n}. 2 - закрытый ключ {d,n}</returns>
        public static Tuple<Tuple<ByteNumber, ByteNumber>, Tuple<ByteNumber, ByteNumber>> GetKeys(ByteNumber p, ByteNumber q, ByteNumber e)
        {
            var n = p * q;
            if (n <= new ByteNumber(256)) throw new Exception("p*q должно быть строго больше 256!");
            var one = new ByteNumber(1);

            var fi = (p - one) * (q - one); //функция Эйлера от n

            var openKey = Tuple.Create(new ByteNumber(e),n);
            var d = e.GetInverseModule(fi);

            var closedKey = Tuple.Create(d, n);

            return Tuple.Create(openKey, closedKey);
        }

        /// <summary>Метод Шифрования сообщения RSA</summary>
        /// <param name="msg"> Сообщение для шифрования</param>
        /// <param name="openKey">Откртый RSA ключ</param>
        public static List<ByteNumber> Encrypt(string msg, Tuple<ByteNumber, ByteNumber> openKey)
        {
            //E = msg^e (mod) n
            var one = new ByteNumber(1);
            var e = openKey.Item1;
            var n = openKey.Item2;

            var parsedMsg = ParseMessage(msg);
            var newMsg = new List<ByteNumber>();

            for(int i = 0; i< parsedMsg.Count; i++)
            {
                var P = (parsedMsg[i].Power(e));
                P = P % n;
                newMsg.Add(P);
            }

            return newMsg;
        }


        /// <summary>
        /// Декодировать сообщения по закрытому ключу
        /// </summary>
        /// <param name="codedMsg"></param>
        /// <param name="secretKey"></param>
        public static List<ByteNumber> Decode(List<ByteNumber> codedMsg, Tuple<ByteNumber, ByteNumber> secretKey)
        {
            //P = E^d(mod n)
            var d = secretKey.Item1;
            var n = secretKey.Item2;

            //var codedMsg = ParseMessage(msg);

            var decodedNum = new List<ByteNumber>();
            for(int i =0;i<codedMsg.Count;i++)
            {
                var P = (codedMsg[i].Power(d));// % n;
                P = P % n;
                
                decodedNum.Add(P);
            }

            return decodedNum;
        }


        /// <summary> Разбивает сообщение на блоки и превращает блоки в числа. 
        /// В ТЕКУЩЕЙ РЕАЛИЗАЦИИ БЛОКИ ПО ОДНОМУ БАЙТУ (кодировка ASCII)</summary>
        /// <returns>Список чисел, полученных после разбиения строки на блоки и перевод в числа</returns>
        public static List<ByteNumber> ParseMessage(string msg)
        {
            var msgBytes =  Encoding.ASCII.GetBytes(msg);
            var res = new List<ByteNumber>();
            for(int i = 0; i< msgBytes.Length; i++)
            {
                var buffer = new List<byte>() { msgBytes[i]};
                var btNum = new ByteNumber(false, buffer) ;
                res.Add(btNum);
            }
            return res;
        }

        /// <summary> Преобразует массив чисел как строку. В ТЕКУЩЕЙ РЕАЛИЗАЦИИ ASCII </summary>
        public static string MsgToString(List<ByteNumber> msg)
        {
            var decodedMsg = new StringBuilder();
            foreach(var msgItem in msg)
            {
                var bufferWord = Encoding.ASCII.GetString(msgItem.ToByteArray());
                decodedMsg.Append(bufferWord);
            }
            return decodedMsg.ToString();
        }

    }

}
