using System.Net;
using Task_2;
using Task_2.Models;

namespace Task2_Tests
{

    public class IP_Filtering_Tests
    {

        public List<IPRange> IpRanges = new();

        [SetUp]
        public void Setup()
        {

            IpRanges.Add(
                new IPRange
                {
                    Start = IPAddress.Parse("192.168.1.0"),
                    End = IPAddress.Parse("192.168.1.255")
                }
            );
        }

        [Test]
        [TestCase("192.168.1.0 - 192.168.1.255", true)]
        [TestCase("192.168.1.0 192.168.1.255", false)]
        [TestCase("192.168.1.0/24", true)]
        [TestCase("192.168.1.0", true)]
        [TestCase("192.168.2.0", false)]
        public void Test1(string ip, bool expectedResult)
        {

            IPRangeChecker rangeChecker = new(IpRanges);

            bool x = rangeChecker.Execute(ip);

            if (expectedResult == x)
            {
                Assert.Pass();
            }
            Assert.Fail();

        }
    }
}