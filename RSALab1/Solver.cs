using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSALab1
{
    //http://e-maxx.ru/algo/export_extended_euclid_algorithm

    /// <summary>
    /// Инструменты и функции для RSA
    /// </summary>
    public class Solver
    {
        /// <summary>
        /// Аогоритм Евлида. Реализация через остатки от деления.
        /// </summary>
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
            var p = new ByteNumber(3);
            var q = new ByteNumber(7);
            var n = p * q;
            var one = new ByteNumber(1);

            var fi = (p - one) * (q - one);
            var s = fi.ToInt();
        }
        //http://fb3809fm.bget.ru/_csharp/157.php - нахождение простых 
    }

}
