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
    public class Solver
    {
        /// <summary> Список простых чисел до 1000  </summary>
        public static List<int> Primes { get; private set; }

        static Solver()
        {
            Primes = new List<int>();
            int start = 2;
            int end = 1000;
            for (int i = start; i <= end; i++)
            {
                bool isPrime = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0 )
                        isPrime = false;
                }
                if(isPrime) Primes.Add(i);
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

        public static void RSA()
        {
            
        }

        /// <summary>Получить ключи для RSA шифрования</summary>
        /// <param name="p">Простое число</param>
        /// <param name="q">Простое число</param>
        /// <returns>Две пары чисел. 1 - открытый ключ {e,n}. 2 - закрытый ключ {d,n}</returns>
        public static Tuple<Tuple<ByteNumber, ByteNumber>, Tuple<ByteNumber, ByteNumber>> GetKeys(ByteNumber p, ByteNumber q)
        {
            var n = p * q;
            if (n <= new ByteNumber(256)) throw new Exception("p*q должно быть строго больше 256!");
            var one = new ByteNumber(1);
            var e = new ByteNumber(0);

            var fi = (p - one) * (q - one); //функция Эйлера от n
            for(int i = Primes.Count; i>=0; i--)
            {
                e = new ByteNumber(Primes[i]); // e - открытая экспонента. е<fi, простое и взаимнопростое с fi
                var nod = Gcd(fi, e);
                if (nod == one && e < fi)
                    break;
            }

            var openKey = Tuple.Create(e,n);
            var d = e.GetInverseModule(n);
            var closedKey = Tuple.Create(d, n);

            return Tuple.Create(openKey, closedKey);
        }

        public static ByteNumber RSAEncryption(string msg, Tuple<ByteNumber, ByteNumber> openKey)
        {
            //E = msg^e (mod) n
            var newMsg = new List<ByteNumber>();
            var one = new ByteNumber(1);
            var e = openKey.Item1;
            var n = openKey.Item2;
            var maxSize = new ByteNumber(n - one);

            throw new NotImplementedException();
        }

        /// <summary> Разбивает сообщение на блоки и превращает блоки в числа < n </summary>
        /// <returns>Список чисел, полученных после разбиения строки на блоки и перевод в числа</returns>
        public static List<ByteNumber> ParseMessage(string msg, ByteNumber n)
        {
            var one = new ByteNumber(1);
            var byteCount = (n - one).ByteCount; //количество байт (символов) в одном блоке
            var res = new List<ByteNumber>();
            var msgBytes = Encoding.ASCII.GetBytes(msg);
            var buffer = new List<byte>();


            for (int i = 0; i< msgBytes.Length; i++)
            {
                buffer.Add(msgBytes[i]);
                if(((i+1)%byteCount == 0) || (i==msgBytes.Length-1))
                {
                    res.Add(new ByteNumber(false, buffer));
                    buffer = new List<byte>();
                }
            }

            return res;
        }
    }

}
