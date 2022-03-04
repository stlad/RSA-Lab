using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace RSALab1
{
    /// <summary>
    /// Хранит целое знаковое число в виде списка однобайтовых элементов
    /// </summary>
    public class ByteNumber
    {
        public List<byte> Number { get; set; }
        public bool IsNegative { get; private set; }
        public byte this[int index]
        {
            get
            {
                if (index >= Number.Count || index < 0) throw new IndexOutOfRangeException();
                return Number[index];
            }
        }

        public int ByteCount => Number.Count;

        public ByteNumber(int num)
        {
            IsNegative = num < 0 ? true : false;
            Number = new List<byte>();
            if (num == 0)
            {
                Number.Add(0);
                return;
            }
            num = Math.Abs(num);
            while (num > 0)
            {
                Number.Add((byte)(num % 256));
                num = num / 256;
            }

        }
        public ByteNumber(long num)
        {
            IsNegative = num < 0 ? true : false;
            Number = new List<byte>();
            if (num == 0)
            {
                Number.Add(0);
                return;
            }
            num = Math.Abs(num);
            while (num > 0)
            {
                Number.Add((byte)(num % 256));
                num = num / 256;
            }

        }
        public ByteNumber(bool isNegative, List<byte> num)
        {
            IsNegative = isNegative;
            Number = num;
        }
        public ByteNumber(ByteNumber num)
        {
            IsNegative = num.IsNegative;
            Number = new List<byte>();
            foreach (var b in num.Number)
                Number.Add(b);
        }
        public ByteNumber()
        {
            IsNegative = false;
            Number = new List<byte>();
        }

        /// <summary>
        ///  Конвертация в int. Работает только с числом до 4х байт. В противном случае исключение.
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            if (this.ByteCount > 4) throw new Exception("Невозможно конвертировать в int32, из-за переполнения");
            int num = 0;
            for (int i = 0; i < Number.Count; i++)
            {
                num += (int)(Number[i] * Math.Pow(256, i));
            };

            if (IsNegative) num *= (-1);
            return num;
        }


        /// <summary> Получить число, обратное по модулю </summary>
        /// <param name="module"> Модуль</param>
        /// <returns></returns>
        public ByteNumber GetInverseModule(ByteNumber module)
        {
            var x = new ByteNumber(0);
            var y = new ByteNumber(0);
            var nod = RSA.gcdExtended(this, module, ref x, ref y);

            if (x.IsNegative)
            {
                var res = x + module;
                res.ClearEmptyBytes();
                return res;
            }
            return x;
        }

        /// <summary>Возведение в положительную степень</summary>
        public ByteNumber Power(ByteNumber power)
        {
            var res = new ByteNumber(1);
            var one = new ByteNumber(1);
            if (power < new ByteNumber(0)) throw new Exception("Только положительная степень!");
            for (var i = new ByteNumber(0); i < power; i = i + one)
            {
                var buffer = res * this; 
                res = buffer;
            }

            return res;
        }

        public ByteNumber LeftBitShift()
        {
            var num = new ByteNumber();
            num.IsNegative = this.IsNegative;
            var buffer = 0;
            for (int i = 0; i <= this.ByteCount; i++)
            {
                var newNum = i == this.ByteCount ? 0 : (this[i] << 1);
                num.Number.Add((byte)((newNum+buffer)));
                buffer = newNum / 256;
            }
            num.ClearEmptyBytes();
            return num;
        }
        public ByteNumber RightBitShift()
        {
            var num = new ByteNumber();
            num.IsNegative = this.IsNegative;
            var buffer = 0;
            for (int i = 0; i < this.ByteCount; i++)
            {
                var newNum = this.Number[i] >> 1;
                buffer = i==this.ByteCount-1 ? 0: (this.Number[i + 1] % 2)*128;
                num.Number.Add((byte)(newNum + buffer));

            }
            num.ClearEmptyBytes();

            if (num < new ByteNumber(0))
                num = num - new ByteNumber(1);
            return num;
        }

        public static ByteNumber operator +(ByteNumber a, ByteNumber b)
        {
            var res = new ByteNumber();
            var compRes = ModuleComparision(a, b);
            if (a.IsNegative == b.IsNegative)
            {
                res.IsNegative = a.IsNegative;
                res.Number = ModuleSum(a, b);
            }
            else
            {
                res.IsNegative = compRes >= 0 ? a.IsNegative : b.IsNegative;
                res.Number = ModuleSubs(a, b);
            }
            return res;
        }
        public static ByteNumber operator -(ByteNumber a, ByteNumber b)
        {
            var oppositeNum = new ByteNumber(b);
            oppositeNum.IsNegative = !oppositeNum.IsNegative;
            var res = a + oppositeNum;
            res.ClearEmptyBytes();
            return res;
        }
        public static ByteNumber operator *(ByteNumber a, ByteNumber b)
        {
            var res = new ByteNumber(0);
            var newA = new ByteNumber(a);
            newA.IsNegative = false;
            
            var newB = new ByteNumber(b);
            newB.IsNegative = false;
            var zero = new ByteNumber(0);

            var bit = 0;
            while(newB > zero)
            {
                bit = newB.Number[0] % 2; // все равно, что остаток от деления всего числа
                if (bit == 1)
                    res = res + newA;
                newB = newB.RightBitShift();
                newA = newA.LeftBitShift();
            }
            res.IsNegative = a.IsNegative == b.IsNegative ? false: true;
            return res;
        }
        public static ByteNumber operator /(ByteNumber a, ByteNumber b)
        {
            var zero = new ByteNumber(0);
            if (b == zero) throw new DivideByZeroException();

            var one = new ByteNumber(1);
            var res = new ByteNumber(0);
            var startB = new ByteNumber(b);
            startB.IsNegative = false;

            var remainder = new ByteNumber(a); //здесь будет остаток от деления
            remainder.IsNegative = false;

            while(remainder - startB >=zero)
            {
                var counter = new ByteNumber(0);
                var prev = new ByteNumber(b);
                prev.IsNegative = false;
                var next = prev.LeftBitShift();
                while(next <= remainder)
                {
                    prev = next;
                    next = next.LeftBitShift();
                    counter = counter + one;
                }
                remainder = remainder - prev;
                res = res + new ByteNumber(new ByteNumber(2).Power(counter));
            }

            res.IsNegative = a.IsNegative == b.IsNegative ? false: true;
            return res;
        }
        public static ByteNumber operator %(ByteNumber a, ByteNumber b)
        {
            //var zero = new ByteNumber(0);
            //if (b == zero) throw new DivideByZeroException();
            ////a = bx + r

            var div = a / b;
            var res = a - (b * div);
            return res;
            //var zero = new ByteNumber(0);
            //if (b == zero) throw new DivideByZeroException();

            //var counter = new ByteNumber(0);                    //станет целой частью деления. 
            //var one = new ByteNumber(1);
            //var res = new ByteNumber(0);
            //var startB = new ByteNumber(b);
            //startB.IsNegative = false;
            //var remainder = new ByteNumber(a); //здесь будет остаток от деления
            //remainder.IsNegative = false;

            //while (remainder - startB >= zero)
            //{
            //    counter = new ByteNumber(0);
            //    var prev = new ByteNumber(startB);
            //    var next = prev.LeftBitShift();
            //    while (next <= remainder)
            //    {
            //        prev = next;
            //        next = next.LeftBitShift();
            //        counter = counter + one;
            //    }
            //    remainder = remainder - prev;
            //    res = res + new ByteNumber(new ByteNumber(2).Power(counter));
            //}

            //remainder.IsNegative = a.IsNegative;
            //return remainder;
        }
        public static bool operator >(ByteNumber a, ByteNumber b) //=> a.ToInt() > b.ToInt();
        {
            if (a.IsNegative && !b.IsNegative) return false;
            if(!a.IsNegative && b.IsNegative) return true;
            var modComp = ModuleComparision(a, b);

            if(a.IsNegative && modComp == 1) return false;
            if (a.IsNegative && modComp == -1) return true;
            if (!a.IsNegative && modComp == 1) return true;
            if (!a.IsNegative && modComp == -1) return false ;
            return false;
        }
        public static bool operator <(ByteNumber a, ByteNumber b) //=> a.ToInt() < b.ToInt(); 
        {
            if (a.IsNegative && !b.IsNegative) return true;
            if (!a.IsNegative && b.IsNegative) return false;
            var modComp = ModuleComparision(a, b);

            if (a.IsNegative && modComp == 1) return true;
            if (a.IsNegative && modComp == -1) return false;
            if (!a.IsNegative && modComp == 1) return false;
            if (!a.IsNegative && modComp == -1) return true;
            return false;
        }
        public static bool operator >=(ByteNumber a, ByteNumber b) => a > b || a == b;
        public static bool operator <=(ByteNumber a, ByteNumber b) => a < b || a == b;
        public static bool operator==(ByteNumber a, ByteNumber b) => a.IsNegative == b.IsNegative && ModuleComparision(a, b) == 0; //=> a.ToInt() == b.ToInt();
        public static bool operator!=(ByteNumber a, ByteNumber b) => !(a == b); //=> a.ToInt() != b.ToInt();

        /// <summary>
        /// Вспомогательный метод сравнения без учета знака
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>1 - первый больше второго. -1 - первый меньше второго. 0 - операнды равны</returns>
        private static int ModuleComparision(ByteNumber a, ByteNumber b)
        {
            if (a.ByteCount > b.ByteCount) return 1;
            if (a.ByteCount < b.ByteCount) return -1;

            for(int i=a.ByteCount-1;i>=0;i--)
            {
                if (a.Number[i] > b.Number[i]) return 1;
                if (a.Number[i] < b.Number[i]) return -1;
            }
            return 0;
        }

        /// <summary>
        /// Модульное сложение без учета знака
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Список байтов нового числа после сложения</returns>
        private static List<byte>ModuleSum(ByteNumber a, ByteNumber b)
        {
            var res = new List<byte>();
            var maxModuleNum = ModuleComparision(a, b) >= 0 ? a : b;

            for(int i=0; i< maxModuleNum.ByteCount;i++)
            {
                var num1 = a.ByteCount > i ? a.Number[i] : (byte)0;
                var num2 = b.ByteCount > i ? b.Number[i] : (byte)0;
                
                if (res.Count == i) res.Add((byte)0);

                var resCell = res[i] + num1 + num2;
                res[i]+= (byte)((num1+ num2) % 256);

                if(resCell >=256) res.Add((byte)1);
            }
            return res;
        }

        /// <summary>
        /// Модульное вычитание без учета знака
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Список байтов нового числа после вычитания</returns>
        private static List<byte> ModuleSubs(ByteNumber a, ByteNumber b)
        {
            var res = new List<byte>();                                 //из большего будем вычитать меньшее
            var compareRes = ModuleComparision(a, b);
            var maxModuleNum = compareRes >= 0 ? a : b;


            byte temp = 0; // флаг заема с след разряда
            for (int i = 0; i < maxModuleNum.ByteCount; i++)
            {
                var num1 = a.ByteCount > i ? a.Number[i] : (byte)0;
                var num2 = b.ByteCount > i ? b.Number[i] : (byte)0;

                if (compareRes == -1)
                {                           //меняем местами числа, если 2 больше 1 по модулю
                    (num1, num2) = (num2, num1);        //сомнительная реализация. Возможно, стоит использовать закоментированную версию                           
                    /*var buffer = num2;
                    num2 = num1;
                    num1 = num2;*/
                }

                byte currentByteResullt = 0;
                if(num1 - num2 - temp  >=0)
                {
                    currentByteResullt = (byte)(num1-num2- temp);
                    temp = 0;
                }
                else
                {
                    currentByteResullt = (byte)(256+num1-num2 - temp);
                    temp = 1;
                }
                res.Add(currentByteResullt);
            }
            return res;
        }

        /// <summary>
        /// Удаление незначащих нулевых байтов
        /// </summary>
        public void ClearEmptyBytes()
        {
            for(int i=ByteCount-1;i>0;i--)
            {
                if (Number[i] == 0)
                    Number.RemoveAt(i);
                else
                    break;
            }
        }

        public byte[] ToByteArray() => Number.ToArray();
    }
}
