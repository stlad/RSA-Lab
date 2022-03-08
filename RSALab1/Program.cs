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
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Что делаем?");
                Console.WriteLine("[1] Ключи \n[2] Шифрование \n[3] Дешифрование \n\n\n\n[i] Информация \n[x] Выход");
                var c = Console.ReadLine();
                if (c[0] == 'x') break;
                switch (c[0])
                {
                    case '1':
                        Console.Clear();
                        KeysModule();
                        break;
                    case '2':
                        Console.Clear();
                        EncryptModule();
                        break;
                    case '3':
                        Console.Clear();
                        DecodeModule();
                        break;
                    case 'i':
                        Console.Clear();
                        Info();
                        break;
                    default:
                        Console.WriteLine("НЕВЕРНАЯ КОММАНДА!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }


        public static void KeysModule()
        {
            Console.WriteLine("Генерация открытого и закрытого ключа для алгоритма RSA\n\n");
            Console.WriteLine("\t\t\tp q e - простые. \n\t\t\tе - взаимно простое с (p-1)*(q-1)\n\t\t\tВ текущей реализации p*q > 256 !");
            Console.WriteLine("Введите p q e.\nРекомендуется: p = 17(37) q = 19(7) e = 5");

            Console.Write("p = ");
            int p =int.Parse(Console.ReadLine());

            Console.Write("q = ");
            int q = int.Parse(Console.ReadLine());
            
            Console.Write("e = ");
            int e = int.Parse(Console.ReadLine());

            Console.WriteLine("---------------");

            var keys = RSA.GetKeys(new ByteNumber(p), new ByteNumber(q), new ByteNumber(e));

            Console.WriteLine($"Открытый ключ: ({keys.Item1.Item1.ToInt()}, {keys.Item1.Item2.ToInt()})");
            Console.WriteLine($"Закрытый ключ: ({keys.Item2.Item1.ToInt()}, {keys.Item2.Item2.ToInt()})");




            Console.WriteLine("\n\n\n...Нажмите любую кнопку, чтобы вернуться...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void EncryptModule()
        {
            Console.WriteLine("Шифрование алгоритмом RSA");
            Console.WriteLine("Обратите внимание, что шифровние текта в однобайтной кодировке ASCII");

            var msg = "";





            Console.WriteLine("\n\nОткуда взять текст? [1] - Файл, [2] - Ручной ввод");
            var com = Console.ReadLine();
            if(com[0]=='1')
            {
                Console.WriteLine("Введите название файла: ");
                var textFileName = Console.ReadLine();
                var textFile = new System.IO.StreamReader(textFileName);
                msg = textFile.ReadLine();
                Console.WriteLine($"Текст: {msg}");
                textFile.Close();
            }
            else
            {
                Console.WriteLine("Введите текст для шифрования:");
                msg = Console.ReadLine();
            }


            Console.WriteLine("\nВведите открытый ключ:");
            Console.Write("e = ");
            int e = int.Parse(Console.ReadLine()); 
            Console.Write("n = ");
            int n = int.Parse(Console.ReadLine());

            var openKey = Tuple.Create(new ByteNumber(e), new ByteNumber(n));

            var codedMsg = RSA.Encrypt(msg, openKey);
            Console.WriteLine("\n\nЗакодированное сообщение: ");
            Console.WriteLine(RSA.MsgToString(codedMsg));

            Console.WriteLine("Введите название файла для записи сообщения (.xml):");
            var xmlFileName = Console.ReadLine();

            var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<ByteNumber>));
            var xmlFile= new System.IO.StreamWriter(xmlFileName);
            writer.Serialize(xmlFile, codedMsg);
            xmlFile.Close();


            Console.WriteLine("\n\n\n...Нажмите любую кнопку, чтобы вернуться...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void DecodeModule()
        {
            Console.WriteLine("Дешифрование алгоритмом RSA");
            Console.WriteLine("Обратите внимание, что шифровние текта в однобайтной кодировке ASCII");

            Console.WriteLine("Ведите название файла с зашифрованным текстом:");

            var fileName = Console.ReadLine();


            var reader = new System.Xml.Serialization.XmlSerializer(typeof(List<ByteNumber>));
            var file = new System.IO.StreamReader(fileName);
            var codedMsg = (List<ByteNumber>)reader.Deserialize(file);
            file.Close();
            Console.WriteLine($"\n\nЗашифрованное сообщение: {RSA.MsgToString(codedMsg)}");



            Console.WriteLine("\nВведите закрытый ключ:");
            Console.Write("d = ");
            int d = int.Parse(Console.ReadLine());
            Console.Write("n = ");
            int n = int.Parse(Console.ReadLine());

            var closedKey = Tuple.Create(new ByteNumber(d), new ByteNumber(n));


            Console.Write("\n\n\nрасшифровка....");
            var decodedMsg = RSA.Decode(codedMsg, closedKey);
            Console.Write("  Готово!\n");


            Console.WriteLine("Введите название файла, куда будет сдублирован декодированный текст:");
            var textFileName = Console.ReadLine();
            var textFile = new System.IO.StreamWriter(textFileName);
            textFile.Write(RSA.MsgToString(decodedMsg));
            textFile.Close();

            Console.WriteLine("\n\nРаскодированное сообщение: ");
            Console.WriteLine(RSA.MsgToString(decodedMsg));


            Console.WriteLine("\n\n\n...Нажмите любую кнопку, чтобы вернуться...");
            Console.ReadKey();
            Console.Clear();

        }

        public static void Info()
        {

            Console.WriteLine("\nАвтор: Ваганов Владислав Сергеевич");
            Console.WriteLine("Студент 2 курса ИРИТ-РТФ. \nГруппа: РИ-200022\n2022");
            Console.WriteLine("------------------------------------------");

            Console.WriteLine("Лаборатнорная работа 1. RSA.");
            Console.WriteLine("1) Создан класс ByteNumber - хранит целое знаковое число в виде массива однобайтных элементов");
            Console.WriteLine("2) Создан класс RSA - инструменты для осуществления кодирования алгоритмом RSA");
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Из-за особенностей реализации арифметики больших чисел и высокой сложности алгоритмов вычисления" +
                "кодирование происходит посимвольно в кодировке ASCII. Кроме того в методах шифрования в качестве рекомендации предложены несколько возможных пригодных" +
                "для шифрования чисел, чтобы вычисления занимали не очень много времени");
            Console.ReadKey();
        }
    }
}
