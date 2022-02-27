using NUnit.Framework;
using RSALab1;
using System;

namespace ByteNumTests
{
    public class NumberTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(256)]
        [TestCase(-256)]
        [TestCase(-450)]
        [TestCase(450)]
        [TestCase(28)]
        [TestCase(-28)]
        public void CreateToIntTest(int num)
        {
            var number = new ByteNumber(num);
            Assert.AreEqual(num, number.ToInt());
        }

       

        [TestCase(1,1)]
        [TestCase(-1,1)]
        [TestCase(1,-1)]
        [TestCase(0,0)]
        [TestCase(0,650)]
        [TestCase(0, -650)]
        [TestCase(300, 450)]
        [TestCase(650, 105)]
        [TestCase(-650, 105)]
        [TestCase(-105, -650)]
        [TestCase(-105, 650)]
        [TestCase(-256, 256)]

        public void OperatorSumTest(int a, int b)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);
            var res = fb + sb;
            Assert.AreEqual(a + b, res.ToInt());
        }

        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 650)]
        [TestCase(0, -650)]
        [TestCase(300, 450)]
        [TestCase(650, 105)]
        [TestCase(-650, 105)]
        [TestCase(-105, -650)]
        [TestCase(-105, 650)]
        [TestCase(-256, 256)]
        [TestCase(256, 50)]

        public void OperatorMinusTest(int a, int b)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);
            var res = fb - sb;
            Assert.AreEqual(a - b, res.ToInt());
        }


        [TestCase(0,0, false)]
        [TestCase(10, 0,true )]
        [TestCase(10, 10, false)]
        [TestCase(-10, 0, false)]
        [TestCase(10, 0, true)]
        [TestCase(-10,-5, false)]
        [TestCase(555810, -356, true)]
        [TestCase(689, 1281058, false)]
        public void MoreTest(int a, int b, bool res)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);

            Assert.AreEqual(res, fb>sb);
        }


        [TestCase(0, 0, false)]
        [TestCase(10, 0, false)]
        [TestCase(10, 10, false)]
        [TestCase(-10, 0, true)]
        [TestCase(10, 0, false)]
        [TestCase(-10, -5, true)]
        [TestCase(555810, -356, false)]
        [TestCase(689, 1281058, true)]
        public void LessTest(int a, int b, bool res)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);

            Assert.AreEqual(res, fb<sb);
        }

        [TestCase(0, 0, true)]
        [TestCase(10, 0, false)]
        [TestCase(10, 10, true)]
        [TestCase(-10, -10, true)]
        [TestCase(10, 0, false)]
        [TestCase(-10, -5, false)]
        [TestCase(6258, 6258, true)]
        public void EqualTest(int a, int b, bool res)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);

            Assert.AreEqual(res, fb==sb);
        }


        [TestCase(0, 0)]
        [TestCase(10, 0)]
        [TestCase(0, 10)]
        [TestCase(-10, -10)]
        [TestCase(10, 0)]
        [TestCase(-10, -5)]
        [TestCase(-1, -5)]
        [TestCase(256, 4)]
        public void MultiTest(int a, int b)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);
            var res = fb * sb;
            Assert.AreEqual(a*b, res.ToInt());
        }



        [TestCase(0, 1)]
        [TestCase(10, -1)]
        [TestCase(0, 10)]
        [TestCase(-10, -10)]
        [TestCase(-10, 20)]
        [TestCase(10, 20)]
        [TestCase(10, 5)]
        [TestCase(654, 34)]
        [TestCase(-654, -34)]
        [TestCase(-256, 452)]
        public void ModTest(int a, int b)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);
            var res = fb % sb;
            Assert.AreEqual(a % b, res.ToInt());
        }


        [TestCase(0, 1)]
        [TestCase(10, -1)]
        [TestCase(0, 10)]
        [TestCase(-10, -10)]
        [TestCase(-10, 20)]
        [TestCase(10, 20)]
        [TestCase(10, 5)]
        [TestCase(654, 34)]
        [TestCase(-654, -34)]
        [TestCase(-256, 452)]
        public void DivideTest(int a, int b)
        {
            var fb = new ByteNumber(a);
            var sb = new ByteNumber(b);
            var res = fb / sb;
            Assert.AreEqual(a /b, res.ToInt());
        }

        [TestCase(1,1)]
        [TestCase(0,1)]
        [TestCase(10, 0)]
        [TestCase(5, 3)]
        [TestCase(2, 10)]
        public void PowerTest(int n, int pow)
        {
            var num = new ByteNumber(n);
            var res = num.Power(pow);

            Assert.AreEqual(Math.Pow((double)n, (double)pow),(double)res.ToInt());
        }
    }

    public class SolverTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(1,1,1)]
        [TestCase(10, 5, 5)]
        [TestCase(64, 48, 16)]
        [TestCase(75, 100, 25)]
        public void gcdTest(int a, int b, int res)
        {
            var f = new ByteNumber(a);
            var s = new ByteNumber(b);
            Assert.AreEqual(res, Solver.Gcd(f,s).ToInt());
        }

        [TestCase("mama papa", 600,2)]
        public void MsgParserTest(string msg, int n, int blockSize)
        {
            var n1 = new ByteNumber(n);
            var res = Solver.ParseMessage(msg, n1);
            Assert.AreEqual(blockSize, res[0].ByteCount);
        }
    }

}